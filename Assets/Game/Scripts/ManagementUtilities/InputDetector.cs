using System;
using UnityEngine;
using YG;

namespace ManagementUtilities
{
    public class InputDetector : MonoBehaviour
    {
        protected IInputHandler _inputHandler;
        private bool _isTabPressed;

        public event Action LeftMouseButtonPressed;
        public event Action LeftMouseButtonReleased;
        public event Action SecondTouchPressed;
        public event Action TabPressed;

        public bool IsActive { get; set; } = true;

        private void Start()
        {
            _inputHandler = YandexGame.savesData.IsDesktop ? new DesktopInputHandler() : new MobileInputHandler();

            _inputHandler.LeftMouseButtonPressed += () =>
                LeftMouseButtonPressed?.Invoke();
            _inputHandler.LeftMouseButtonReleased += () =>
                LeftMouseButtonReleased?.Invoke();
            _inputHandler.SecondTouchPressed += () =>
                SecondTouchPressed?.Invoke();
        }

        private void Update()
        {
            if (_isTabPressed == false)
                _inputHandler.Update();
            
            if (Input.GetKeyDown(KeyCode.Tab))
                if (IsActive)
                    TabPressed?.Invoke();
        }

        public void SetStatusPressed() =>
            _isTabPressed = true;

        public void SetStatusNotPressed() =>
            _isTabPressed = false;

        public void SetActivity() =>
            IsActive = true;

        public void SetBlockInput() =>
            IsActive = false;
    }
}