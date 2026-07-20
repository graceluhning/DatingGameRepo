using UnityEngine;
using System.Collections.Generic;

namespace DatingGame.Data
{
    [CreateAssetMenu(fileName = "NewProfilePool", menuName = "DatingGame/Profile Pool")]
    public class ProfilePool : ScriptableObject
    {
        public List<GeneratedProfile> profiles = new List<GeneratedProfile>();
    }
}
