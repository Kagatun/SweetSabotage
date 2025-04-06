using System;

public class ButtonDecreaseTimer : ButtonHandler
{
    public event Action Clicked;

    protected override void OnButtonClick()
    {
        Clicked?.Invoke();
    }
}