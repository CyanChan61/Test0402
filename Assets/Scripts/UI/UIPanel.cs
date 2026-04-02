using UnityEngine;

namespace RogueCard.UI
{
    /// <summary>
    /// Base class for all UI panels. Panels self-manage show/hide state.
    /// </summary>
    public abstract class UIPanel : MonoBehaviour
    {
        public bool IsVisible { get; private set; }

        public virtual void Show()
        {
            IsVisible = true;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            IsVisible = false;
            gameObject.SetActive(false);
        }
    }
}
