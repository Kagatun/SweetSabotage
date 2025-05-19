using System;

public interface IInputHandler
{
    event Action LeftMouseButtonPressed;
    event Action LeftMouseButtonReleased;
    event Action SecondTouchPressed;
    
    void Update();
}