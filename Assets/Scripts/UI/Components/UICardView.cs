using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Cards;

namespace RogueCard.UI.Components
{
    /// <summary>
    /// Visual representation of a single PlaceCardInstance in the hand or on the map.
    /// </summary>
    public class UICardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private Image artwork;
        [SerializeField] private Image background;
        [SerializeField] private Button selectButton;

        public PlaceCardInstance Card { get; private set; }
        public event Action<PlaceCardInstance> OnSelected;

        private void Start()
        {
            selectButton?.onClick.AddListener(() => OnSelected?.Invoke(Card));
        }

        public void Setup(PlaceCardInstance card)
        {
            Card = card;
            Refresh();
        }

        private void Refresh()
        {
            if (Card == null) return;

            bool revealed = Card.IsRevealed;

            if (nameText) nameText.text = revealed ? Card.DisplayName : "???";
            if (typeText) typeText.text = revealed ? Card.PlaceType.ToString() : "?";
            if (artwork && Card.Definition.artwork)
                artwork.sprite = Card.Definition.artwork;

            // Color-code by type
            if (background)
                background.color = GetColorForType(Card.PlaceType);
        }

        private static Color GetColorForType(PlaceType type) => type switch
        {
            PlaceType.Combat   => new Color(0.8f, 0.2f, 0.2f),
            PlaceType.Elite    => new Color(0.6f, 0.1f, 0.6f),
            PlaceType.Boss     => new Color(0.9f, 0.0f, 0.0f),
            PlaceType.Shop     => new Color(0.9f, 0.8f, 0.1f),
            PlaceType.Rest     => new Color(0.2f, 0.8f, 0.3f),
            PlaceType.Event    => new Color(0.2f, 0.5f, 0.9f),
            PlaceType.Treasure => new Color(0.9f, 0.7f, 0.1f),
            PlaceType.Start    => new Color(0.5f, 0.5f, 0.5f),
            _                  => Color.white
        };
    }
}
