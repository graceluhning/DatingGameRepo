using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DatingGame.Data;

namespace DatingGame.Core
{
    public class ProfileDisplay : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI bioText;
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private Image portraitImage;

        public void DisplayProfile(GeneratedProfile profile)
        {
            if (profile == null) return;

            if (bioText != null)
                bioText.text = profile.bio;

            if (distanceText != null)
                distanceText.text = $"{profile.distance:F1} miles away";

            if (portraitImage != null && profile.portrait != null)
                portraitImage.sprite = profile.portrait;
        }
    }
}
