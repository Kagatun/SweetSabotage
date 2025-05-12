using UnityEngine;

    public interface IControllerPaint
    {
        float GuidanceRadius { get; set; }
        
        Vector3 DragCursor(Vector3 position, Vector3 offsetPosition);
        void ActivateCursor(Vector3 screenPosition);
        void DeactivateCursor();
        bool IsSecondTouchOnButton();
    }