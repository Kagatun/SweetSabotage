using UnityEngine;

namespace ManagementUtilities
{
    public class ControllerPaintDesktop : IControllerPaint
    {
        private Texture2D _cursor;
        private int _dividerOffset = 2;

        public ControllerPaintDesktop(Texture2D cursor)
        {
            _cursor = cursor;
        }

        public float GuidanceRadius { get; set; } = 0.35f;

        public Vector3 DragCursor(Vector3 position, Vector3 offsetPosition) =>
            position;

        public void ActivateCursor(Vector3 screenPosition)
        {
            Vector2 hotSpot = new Vector2(_cursor.width / _dividerOffset, _cursor.height / _dividerOffset);
            Cursor.SetCursor(_cursor, hotSpot, CursorMode.Auto);
        }

        public void DeactivateCursor() =>
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        public bool IsSecondTouchOnButton() =>
            false;
    }
}