using System;
using UnityEngine;
using YG;

namespace ManagementUtilities
{
    public class InputDetector : MonoBehaviour
    {
        public event Action LeftMouseButtonPressed;
        public event Action LeftMouseButtonReleased;
        public event Action SecondTouchPressed;
        public event Action SecondTouchReleased;
        public event Action EscPressed;

        public bool IsActive { get; private set; } = true;
        public bool IsEscPressed { get; private set; }

        private void Update()
        {
            if (IsEscPressed == false)
            {
                if (YandexGame.savesData.IsDesktop)
                {
                    if (Input.GetMouseButtonDown(0))
                        LeftMouseButtonPressed?.Invoke();

                    if (Input.GetMouseButtonUp(0))
                        LeftMouseButtonReleased?.Invoke();

                    if (Input.GetMouseButtonDown(1))
                        SecondTouchPressed?.Invoke();
                }
                else
                {
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);

                        if (touch.phase == TouchPhase.Began)
                            LeftMouseButtonPressed?.Invoke();

                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            LeftMouseButtonReleased?.Invoke();

                        if (Input.touchCount > 1)
                        {
                            Touch secondTouch = Input.GetTouch(1);

                            if (secondTouch.phase == TouchPhase.Began)
                                SecondTouchPressed?.Invoke();

                            if (secondTouch.phase == TouchPhase.Ended || secondTouch.phase == TouchPhase.Canceled)
                                SecondTouchReleased?.Invoke();
                        }
                    }
                }
            }

            if (IsActive)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    EscPressed?.Invoke();
            }
        }

        public void SetStatusPressed() =>
            IsEscPressed = true;

        public void SetStatusNotPressed() =>
            IsEscPressed = false;

        public void SetActivity() =>
            IsActive = true;

        public void SetBlockInput() =>
            IsActive = false;
    }
}