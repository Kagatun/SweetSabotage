using System;

public interface IInputHandler
{
    void Update();
    event Action LeftMouseButtonPressed;
    event Action LeftMouseButtonReleased;
    event Action SecondTouchPressed;
}