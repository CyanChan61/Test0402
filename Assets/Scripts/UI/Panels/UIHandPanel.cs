using UnityEngine;
using RogueCard.Cards;

namespace RogueCard.UI.Panels
{
    public class UIHandPanel : UIPanel
    {
        [SerializeField] private Transform cardContainer;
        [SerializeField] private Components.UICardView cardViewPrefab;

        public void Refresh(Hand hand)
        {
            if (cardContainer == null) return;

            // Clear existing
            foreach (Transform child in cardContainer)
                Destroy(child.gameObject);

            // Spawn a card view for each hand card
            foreach (var card in hand.Cards)
            {
                if (cardViewPrefab == null) continue;
                var view = Instantiate(cardViewPrefab, cardContainer);
                view.Setup(card);
            }
        }
    }
}
