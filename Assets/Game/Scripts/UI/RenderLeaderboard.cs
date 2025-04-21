using UnityEngine;

namespace InterfaceUI
{
    public class RenderLeaderboard : ButtonHandler
    {
        [SerializeField] private GameObject _dropdownPanel;

        protected override void OnButtonClick()
        {
            _dropdownPanel.SetActive(!_dropdownPanel.activeSelf);
        }
    }
}
