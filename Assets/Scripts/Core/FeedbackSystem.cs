using UnityEngine;
using TMPro;
using DatingGame.Data;

namespace DatingGame.Core
{
    public class FeedbackSystem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hintText;
        [SerializeField] private float displayDuration = 3f;

        private float timer;

        private void Start()
        {
            if (hintText != null)
                hintText.text = "Guess their attributes and find your match!";
        }

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    hintText.text = "";
                }
            }
        }

        public void ShowFeedback(bool liked, bool wasCorrect, bool isPerfectMatch)
        {
            if (hintText == null) return;

            string message = "";

            if (wasCorrect)
            {
                if (liked)
                    message = "Great choice! They seem perfect for you.";
                else
                    message = "Good call, they weren't your type.";
            }
            else
            {
                if (liked)
                    message = "Oops! That person didn't actually match your traits.";
                else
                    message = "Missed one! They actually shared all your interests.";
            }

            hintText.text = message;
            timer = displayDuration;
        }

        public void ShowPerfectMatch(GeneratedProfile profile)
        {
            if (hintText == null) return;
            hintText.text = "YOU FOUND THEM! YOUR PERFECT MATCH!\nTap to play again.";
            timer = 0; // Setting timer to 0 ensures it won't be cleared in Update
        }

        public void ShowGameOver()
        {
            if (hintText == null) return;
            hintText.text = "GAME OVER! You made too many mistakes.\nTap to try again.";
            timer = 0; // Setting timer to 0 ensures it won't be cleared in Update
        }
    }
}
