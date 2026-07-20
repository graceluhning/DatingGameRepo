using UnityEngine;
using DatingGame.Data;

namespace DatingGame.Core
{
    public class ProfileGenerator : MonoBehaviour
    {
        [SerializeField] private ProfilePool pool;

        public GeneratedProfile GetRandomProfile()
        {
            if (pool == null || pool.profiles.Count == 0)
            {
                Debug.LogError("ProfilePool is not assigned or empty!");
                return null;
            }

            return pool.profiles[Random.Range(0, pool.profiles.Count)];
        }
    }
}
