using UnityEngine;

namespace RogueCard.Cards
{
    public enum PlaceType
    {
        Start,
        Combat,
        Elite,
        Boss,
        Shop,
        Rest,
        Event,
        Treasure
    }

    [CreateAssetMenu(menuName = "RogueCard/PlaceCard", fileName = "NewPlaceCard")]
    public class PlaceCard : ScriptableObject
    {
        [Header("Identity")]
        public string cardId;
        public string displayName;
        public Sprite artwork;
        [TextArea] public string flavourText;

        [Header("Classification")]
        public PlaceType placeType;

        [Header("Spawn Weighting")]
        [Tooltip("X = tree depth (0-1 normalized), Y = spawn weight")]
        public AnimationCurve spawnWeightCurve = AnimationCurve.Constant(0, 1, 1f);

        public PlaceCardInstance CreateInstance()
        {
            return new PlaceCardInstance(this);
        }
    }
}
