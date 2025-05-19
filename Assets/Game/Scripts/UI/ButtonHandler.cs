using UnityEngine;
using UnityEngine.UI;

namespace InterfaceUI
{
    public abstract class ButtonHandler : MonoBehaviour
    {
        [SerializeField] protected Button ActionButton;

        private void OnEnable()
        {
            ActionButton.onClick.AddListener(OnButtonClick);
            OnEnableAction();
        }

        private void OnDisable()
        {
            ActionButton.onClick.RemoveListener(OnButtonClick);
            OnDisableAction();
        }
        
        protected abstract void OnButtonClick();

        protected virtual void OnEnableAction()
        {
        }
        
        protected virtual void OnDisableAction()
        {
        }
    }
}
