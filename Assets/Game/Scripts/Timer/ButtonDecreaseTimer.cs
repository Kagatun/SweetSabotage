using InterfaceUI;
using System;

namespace Timer
{
    public class ButtonDecreaseTimer : ButtonHandler
    {
        public event Action Clicked;

        protected override void OnButtonClick()
        {
            Clicked?.Invoke();
        }
        
        protected override void OnEnableAction(){}
        
        protected override void OnDisableAction(){}
    }
}