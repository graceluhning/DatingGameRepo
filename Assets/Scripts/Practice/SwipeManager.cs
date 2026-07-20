using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using DatingGame.Data;

namespace DatingGame.Core
{
    public class SwipeManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [Header("Settings")]
        [SerializeField] private RectTransform cardTransform;
        [SerializeField] private float swipeThreshold = 100f;
        [SerializeField] private float rotationModifier = 0.1f;

        [Header("References")]
        [SerializeField] private ProfileGenerator generator;
        [SerializeField] private ProfileDisplay display;
        [SerializeField] private FeedbackSystem feedback;
        [SerializeField] private TextMeshProUGUI playerAttributesText;

        private Vector2 startPosition;
        private GeneratedProfile currentProfile;
        
        // Player State
        private RelationshipType playerRelationship;
        private SmokerStatus playerSmoker;
        private PetStatus playerPets;

        private int mistakesCount = 0;
        private int correctSwipesCount = 0;
        private bool isGameOver = false;
        private const int MaxMistakes = 3;
        private const int SwipesRequiredForPerfectMatch = 15;

        private void Start()
        {
            startPosition = cardTransform.anchoredPosition;
            InitializePlayer();
            LoadNextProfile();
        }

        private void InitializePlayer()
        {
            playerRelationship = (RelationshipType)Random.Range(0, 3);
            playerSmoker = (SmokerStatus)Random.Range(0, 2);
            playerPets = (PetStatus)Random.Range(0, 2);

            Debug.Log($"[PLAYER INFO] Looking for: {playerRelationship}, {playerSmoker}, {playerPets}");
            
            if (playerAttributesText != null)
            {
                playerAttributesText.text = $"YOUR GOAL:\n" +
                                            $"- {playerRelationship}\n" +
                                            $"- {playerSmoker}\n" +
                                            $"- {playerPets}";
            }
        }

        private void LoadNextProfile()
        {
            if (isGameOver) return;

            if (generator == null || display == null)
            {
                Debug.LogError("References missing on SwipeManager!");
                return;
            }
            currentProfile = generator.GetRandomProfile();
            display.DisplayProfile(currentProfile);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isGameOver) return;
            startPosition = cardTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isGameOver) return;
            cardTransform.anchoredPosition += eventData.delta;
            float xOffset = cardTransform.anchoredPosition.x - startPosition.x;
            cardTransform.rotation = Quaternion.Euler(0, 0, -xOffset * rotationModifier);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isGameOver) return;
            float xOffset = cardTransform.anchoredPosition.x - startPosition.x;

            if (Mathf.Abs(xOffset) > swipeThreshold)
            {
                if (xOffset > 0)
                    SwipeRight();
                else
                    SwipeLeft();
            }
            else
            {
                ResetCard();
            }
        }

        private void SwipeRight()
        {
            ProcessSwipe(true);
        }

        private void SwipeLeft()
        {
            ProcessSwipe(false);
        }

        private void ProcessSwipe(bool liked)
        {
            if (currentProfile != null)
            {
                bool isMatch = CheckIfMatch(currentProfile);
                bool wasCorrect = (liked == isMatch);

                if (wasCorrect)
                {
                    correctSwipesCount++;
                    
                    if (liked && correctSwipesCount > SwipesRequiredForPerfectMatch)
                    {
                        feedback.ShowPerfectMatch(currentProfile);
                        isGameOver = true;
                    }
                    else
                    {
                        feedback.ShowFeedback(liked, true, false);
                    }
                }
                else
                {
                    mistakesCount++;
                    correctSwipesCount = 0; // Reset streak as per "without failing" rule
                    if (mistakesCount >= MaxMistakes)
                    {
                        feedback.ShowGameOver();
                        isGameOver = true;
                    }
                    else
                    {
                        feedback.ShowFeedback(liked, false, false);
                    }
                }
            }

            ResetCard();
            if (!isGameOver)
            {
                LoadNextProfile();
            }
        }

        private bool CheckIfMatch(GeneratedProfile profile)
        {
            return profile.relationshipType == playerRelationship &&
                   profile.smokerStatus == playerSmoker &&
                   profile.petStatus == playerPets;
        }

        private void ResetCard()
        {
            cardTransform.anchoredPosition = startPosition;
            cardTransform.rotation = Quaternion.identity;
        }
    }
}
