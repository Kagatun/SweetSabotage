using UnityEngine;
using UnityEngine.UI;

namespace InterfaceUI
{
    public abstract class ButtonHandler : MonoBehaviour
    {
        [SerializeField] protected Button ActionButton;

        protected virtual void OnEnable()
        {
            ActionButton.onClick.AddListener(OnButtonClick);
        }

        protected virtual void OnDisable()
        {
            ActionButton.onClick.RemoveListener(OnButtonClick);
        }

        protected abstract void OnButtonClick();
    }
}
