using UnityEngine;
using System.Collections.Generic;
using DatingGame.Data;

namespace DatingGame.Core
{
    public class ProfileGenerator : MonoBehaviour
    {
        [SerializeField] private ProfilePool pool;
        private int lastIndex = -1;
        private List<int> availableIndices = new List<int>();

        public GeneratedProfile GetRandomProfile()
        {
            if (pool == null || pool.profiles.Count == 0)
            {
                Debug.LogError("ProfilePool is not assigned or empty!");
                return null;
            }

            // If we've run out of unique profiles, refill the pool
            if (availableIndices == null || availableIndices.Count == 0)
            {
                RefillAvailableIndices();
            }

            // If still empty (no valid profiles in pool), return first as fallback
            if (availableIndices.Count == 0)
            {
                return pool.profiles[0];
            }

            int listIndex;
            int pickedIndex;

            // If we have more than one option, avoid repeating the last one shown
            if (availableIndices.Count > 1)
            {
                do
                {
                    listIndex = Random.Range(0, availableIndices.Count);
                    pickedIndex = availableIndices[listIndex];
                } while (pickedIndex == lastIndex);
            }
            else
            {
                listIndex = 0;
                pickedIndex = availableIndices[0];
            }

            // Remove the picked index so it's not shown again until the pool is refilled
            availableIndices.RemoveAt(listIndex);
            lastIndex = pickedIndex;

            return pool.profiles[pickedIndex];
        }

        private void RefillAvailableIndices()
        {
            if (availableIndices == null) availableIndices = new List<int>();
            else availableIndices.Clear();

            for (int i = 0; i < pool.profiles.Count; i++)
            {
                // Only consider profiles that exist and have bios (are completed)
                if (pool.profiles[i] != null && !string.IsNullOrWhiteSpace(pool.profiles[i].bio))
                {
                    availableIndices.Add(i);
                }
            }
        }
    }
}
