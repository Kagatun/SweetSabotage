using UnityEngine;
using UnityEngine.UI;

namespace InterfaceUI
{
    public class SwitcherUI : ButtonHandler
    {
        [SerializeField] private Image _panelOpen;
        [SerializeField] private Image _panelClose;

        protected override void OnButtonClick()
        {
            _panelOpen.gameObject.SetActive(true);
            _panelClose.gameObject.SetActive(false);
        }
    }
}
