using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ManagementUtilities
{
    public class ControllerPaintMobile : IControllerPaint
    {
        private Camera _camera;
        private int _indexSecondTouch = 1;
        private Image _cursorRoller;
        private Button _button;

        public ControllerPaintMobile(Camera camera, Image cursorRoller, Button button)
        {
            _camera = camera;
            _cursorRoller = cursorRoller;
            _button = button;
        }

        public float GuidanceRadius { get; set; } = 0.5f;

        public Vector3 DragCursor(Vector3 position, Vector3 offsetPosition)
        {
            UpdateMobileCursorPosition(position, offsetPosition);
            
            Vector3 screenPoint = new Vector3(position.x, position.y, _camera.nearClipPlane);
            Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPoint);
            Vector3 offsetWorldPoint = worldPoint + offsetPosition;
            
            return _camera.WorldToScreenPoint(offsetWorldPoint);
        }

        public void ActivateCursor(Vector3 screenPosition)
        {
            _cursorRoller.gameObject.SetActive(true);
            UpdateMobileCursorPosition(screenPosition, Vector3.zero);
        }

        public void DeactivateCursor() =>
            _cursorRoller.gameObject.SetActive(false);

        public bool IsSecondTouchOnButton()
        {
            if (Input.touchCount > _indexSecondTouch)
            {
                Touch secondTouch = Input.GetTouch(_indexSecondTouch);
                Vector2 touchPosition = secondTouch.position;

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touchPosition;

                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (var result in results)
                {
                    if (result.gameObject == _button.gameObject)
                        return true;
                }
            }

            return false;
        }
        
        private void UpdateMobileCursorPosition(Vector2 screenPosition, Vector3 offsetPosition)
        {
            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _camera.nearClipPlane + GuidanceRadius));
            _cursorRoller.transform.position = worldPosition + offsetPosition;
        }
    }
}