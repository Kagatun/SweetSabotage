using System;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
    private int _mouseButtonLeft = 0;
    private int _mouseButtonRight = 1;

    public event Action LeftMouseButtonPressed;
    public event Action LeftMouseButtonReleased;
    public event Action RightClicked;
    public event Action QPressed;
    public event Action EPressed;
    public event Action RPressed;
    public event Action FPressed;
    public event Action EscPressed;

    public bool IsActive { get; private set; } = true;
    public bool IsPressed { get; private set; }

    private void Update()
    {
        if (Input.GetMouseButtonDown(_mouseButtonLeft))
            LeftMouseButtonPressed?.Invoke();

        if (Input.GetMouseButtonUp(_mouseButtonLeft))
            LeftMouseButtonReleased?.Invoke();

        if (Input.GetMouseButtonDown(_mouseButtonRight))
            RightClicked?.Invoke();

        if (IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                QPressed?.Invoke();

            if (Input.GetKeyDown(KeyCode.E))
                EPressed?.Invoke();

            if (Input.GetKeyDown(KeyCode.R))
                RPressed?.Invoke();

            if (Input.GetKeyDown(KeyCode.F))
                FPressed?.Invoke();

            if (Input.GetKeyDown(KeyCode.Escape))
                Debug.Log("bc");
            EscPressed?.Invoke();
        }
    }

    public void SetActivity() =>
        IsActive = true;

    public void SetBlockInput() =>
        IsActive = false;
}

