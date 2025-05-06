using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using YG;
using Figure;
using Utility;

namespace ManagementUtilities
{
    public class Paint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private Image _imageColor;
        [SerializeField] private Image _fill;
        [SerializeField] private Image _cursorRoller;
        [SerializeField] private Texture2D _cursor;
        [SerializeField] private Button _button;
        [SerializeField] private LayerMask _interactionLayerMask;
        [SerializeField] private AudioSource _sound;
        [SerializeField] private bool _isRight;

        private Camera _mainCamera;
        private TeleporterFigure _currentFigure;
        private Texture2D _defaultCursorTexture;

        private float _timeCooldown = 10;
        private float _elapsedTime;
        private float _offsetX;
        private float _offsetY;
        private float _radius = 0.35f;
        private float _radiusMobile = 0.5f;

        private Vector3 _offsetPosition;

        private bool _isFound = true;
        private bool _isReady = true;
        private bool _isCooldownActive;

        private Color _color;
        private Color _originalColor;

        private bool _isMobile => YandexGame.savesData.IsDesktop == false;

        private void Start()
        {
            _timeCooldown -= YandexGame.savesData.CooldownPaint;
            _mainCamera = Camera.main;

            if (_isMobile)
                _radius = _radiusMobile;

            SetNewColor();
            ColorStartCooldown();
            SetOffsetPosition();
        }

        private void OnEnable()
        {
            _inputDetector.SecondTouchPressed += OnSecondTouchPressed;
        }

        private void OnDisable()
        {
            _inputDetector.SecondTouchPressed -= OnSecondTouchPressed;
            ResetCursor();
        }

        private void Update()
        {
            if (_isCooldownActive)
            {
                _elapsedTime += Time.deltaTime;
                _fill.fillAmount = _elapsedTime / _timeCooldown;

                if (_elapsedTime >= _timeCooldown)
                    CompleteCooldown();
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && _isReady)
            {
                int divider = 2;
                Vector2 hotSpot = new Vector2(_cursor.width / divider, _cursor.height / divider);
                Cursor.SetCursor(_cursor, hotSpot, CursorMode.Auto);
                
                SetOffsetPosition();
                ActiveCursorMobile();
                
                PerformSphereCast(eventData.position);
            }
            else if (eventData.button == PointerEventData.InputButton.Right && _isReady)
            {
                StartCooldown();
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!_isReady) return;
            
            ActiveCursorMobile();
            PerformSphereCast(eventData.position);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                ResetCursor();
                _cursorRoller.gameObject.SetActive(false);

                if (_currentFigure != null)
                {
                    _currentFigure.SetColor(_color);
                    _sound.Play();
                    _isFound = true;
                    ColorStartCooldown();
                }
                else
                {
                    if (_currentFigure != null)
                        _currentFigure.SetColor(_originalColor);
                }

                _currentFigure = null;
            }
        }
        
        private void ColorStartCooldown()
        {
            if (_isReady && _isFound)
                StartCooldown();
        }

        private void SetOffsetPosition()
        {
            if (_isRight == false)
            {
                _offsetX = YandexGame.savesData.OffsetPaintLeftHorizontal;
                _offsetY = YandexGame.savesData.OffsetPaintLeftVertical;
            }
            else
            {
                _offsetX = YandexGame.savesData.OffsetPaintRightHorizontal;
                _offsetY = YandexGame.savesData.OffsetPaintRightVertical;
            }

            _offsetPosition = new Vector3(_offsetX, _offsetY, 0);
        }

        private void ActiveCursorMobile()
        {
            _cursorRoller.gameObject.SetActive(_isMobile);

            if (_isMobile && Input.touchCount > 0)
            {
                Vector3 inputPosition = Input.GetTouch(0).position;
                Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, _mainCamera.nearClipPlane + _radiusMobile));

                _cursorRoller.transform.position = worldPosition + _offsetPosition;
            }
        }

        private void PerformSphereCast(Vector2 screenPosition)
        {
            Vector3 inputPosition = _isMobile ? (Vector3)Input.GetTouch(0).position : (Vector3)screenPosition;

            if (_isMobile)
            {
                Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(
                    new Vector3(inputPosition.x, inputPosition.y, _mainCamera.nearClipPlane + 0.5f));
                _cursorRoller.transform.position = worldPosition + _offsetPosition;
            }

            Vector3 screenPoint = new Vector3(inputPosition.x, inputPosition.y, _mainCamera.nearClipPlane);

            if (_isMobile)
            {
                Vector3 offsetScreen = _mainCamera.WorldToScreenPoint(_mainCamera.ScreenToWorldPoint(screenPoint) + _offsetPosition) - screenPoint;
                screenPoint += offsetScreen;
            }

            Ray ray = _mainCamera.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if (Physics.SphereCast(ray, _radius, out hit, Mathf.Infinity, _interactionLayerMask))
            {
                if (hit.transform.TryGetComponent(out TeleporterFigure figure) && figure.IsInstall == false)
                {
                    if (_currentFigure != figure)
                    {
                        if (_currentFigure != null)
                            _currentFigure.SetColor(_originalColor);

                        _currentFigure = figure;
                        _originalColor = figure.Color;
                    }

                    figure.SetColor(_color);
                }
            }
            else
            {
                if (_currentFigure != null)
                {
                    _currentFigure.SetColor(_originalColor);
                    _currentFigure = null;
                }
            }
        }

        private void SetNewColor() =>
            _color = ColorPalette.GetRandomActiveColor();

        private void StartCooldown()
        {
            _isReady = false;
            _isFound = false;
            _imageColor.gameObject.SetActive(false);

            _elapsedTime = 0;
            _isCooldownActive = true;
        }

        private void CompleteCooldown()
        {
            _isCooldownActive = false;
            _isReady = true;

            Color previousColor = _color;

            do
            {
                SetNewColor();
            }
            while (_color == previousColor);

            _imageColor.gameObject.SetActive(true);

            Color color = _color;
            color.a = 1;
            _imageColor.color = color;
        }

        private void ResetCursor() =>
            Cursor.SetCursor(_defaultCursorTexture ?? null, Vector2.zero, CursorMode.Auto);

        private void OnSecondTouchPressed()
        {
            if (_isReady == false)
                return;

            if (IsSecondTouchOnButton())
            {
                SetNewColor();
                _imageColor.gameObject.SetActive(true);
                _imageColor.color = _color;

                StartCooldown();
            }
        }

        private bool IsSecondTouchOnButton()
        {
            if (Input.touchCount > 1)
            {
                Touch secondTouch = Input.GetTouch(1);
                Vector2 touchPosition = secondTouch.position;

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touchPosition;

                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (var result in results)
                {
                    if (result.gameObject == _button.gameObject)
                        return true;
                }
            }

            return false;
        }
    }
}

