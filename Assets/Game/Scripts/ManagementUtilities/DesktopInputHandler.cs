using System;
using UnityEngine;

namespace ManagementUtilities
{
    public class DesktopInputHandler : IInputHandler
    {
        public event Action LeftMouseButtonPressed;
        public event Action LeftMouseButtonReleased;
        public event Action SecondTouchPressed;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
                LeftMouseButtonPressed?.Invoke();

            if (Input.GetMouseButtonUp(0))
                LeftMouseButtonReleased?.Invoke();

            if (Input.GetMouseButtonDown(1))
                SecondTouchPressed?.Invoke();
        }
    }
}
