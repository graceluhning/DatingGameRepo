using UnityEngine;

namespace DatingGame.Data
{
    public enum RelationshipType { LongTerm, Casual, Friendship }
    public enum SmokerStatus { Smoker, NonSmoker }
    public enum PetStatus { HasPets, NoPets }

    [System.Serializable]
    public class GeneratedProfile
    {
        [TextArea(3, 10)]
        public string bio;
        public float distance;
        public Sprite portrait;
        
        public RelationshipType relationshipType;
        public SmokerStatus smokerStatus;
        public PetStatus petStatus;
    }
}
