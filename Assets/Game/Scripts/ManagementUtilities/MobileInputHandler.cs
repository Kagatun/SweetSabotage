using System;
using UnityEngine;

namespace ManagementUtilities
{
    public class MobileInputHandler : IInputHandler
    {
        public event Action LeftMouseButtonPressed;
        public event Action LeftMouseButtonReleased;
        public event Action SecondTouchPressed;

        public void Update()
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
                }
            }
        }
    }
}