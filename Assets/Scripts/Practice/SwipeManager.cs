using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using DatingGame.Data;

namespace DatingGame.Core
{
    public class SwipeManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
    {
        [Header("Settings")]
        [SerializeField] private RectTransform cardTransform;
        [SerializeField] private float swipeThreshold = 100f; // threshold for swipe
        [SerializeField] private float rotationModifier = 0.1f;

        [Header("References")]
        [SerializeField] private ProfileGenerator generator;
        [SerializeField] private ProfileDisplay display;
        [SerializeField] private FeedbackSystem feedback;
        [SerializeField] private TextMeshProUGUI playerAttributesText;

        private Vector2 startPosition;
        private GeneratedProfile currentProfile;
        
        
        private RelationshipType playerRelationship;
        private SmokerStatus playerSmoker;
        private PetStatus playerPets;

        private int mistakesCount = 0;
        private int totalSwipesCount = 0;
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
            MusicManager.instance.StopSFX();
            MusicManager.instance.SwipingSFX();
            ProcessSwipe(true);
        }

        private void SwipeLeft()
        {
            MusicManager.instance.StopSFX();
            MusicManager.instance.SwipingSFX();
            ProcessSwipe(false);
        }

        private void ProcessSwipe(bool liked)
        {
            if (currentProfile != null)
            {
                totalSwipesCount++;
                int matchCount = GetMatchCount(currentProfile);
                bool relationshipMatches = currentProfile.relationshipType == playerRelationship;
                int requiredMatches = (totalSwipesCount > SwipesRequiredForPerfectMatch) ? 1 : 2;
                bool isMatch = relationshipMatches && (matchCount >= requiredMatches);
                bool wasCorrect = (liked == isMatch);

                if (wasCorrect)
                {
                    if (liked && totalSwipesCount > SwipesRequiredForPerfectMatch)
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

        private int GetMatchCount(GeneratedProfile profile)
        {
            int count = 0;
            if (profile.relationshipType == playerRelationship) count++;
            if (profile.smokerStatus == playerSmoker) count++;
            if (profile.petStatus == playerPets) count++;
            return count;
        }

        private bool CheckIfMatch(GeneratedProfile profile)
        {
            return GetMatchCount(profile) == 3;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isGameOver)
            {
                RestartGame();
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ResetCard()
        {
            cardTransform.anchoredPosition = startPosition;
            cardTransform.rotation = Quaternion.identity;
        }
    }
}
