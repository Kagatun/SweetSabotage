using Figure;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using YG;

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
        private IControllerPaint _controller;

        private float _timeCooldown = 10;
        private float _elapsedTime;
        private float _offsetX;
        private float _offsetY;

        private Vector3 _offsetPosition;

        private bool _isFound = true;
        private bool _isReady = true;
        private bool _isCooldownActive;

        private Color _color;
        private Color _originalColor;

        private void Start()
        {
            _timeCooldown -= YandexGame.savesData.CooldownPaint;
            _mainCamera = Camera.main;

            SetNewColor();
            DefineController();
            SetOffsetPosition();
            ColorStartCooldown();
        }

        private void DefineController()
        {
            if (YandexGame.savesData.IsDesktop)
            {
                _controller = new ControllerPaintDesktop(_cursor);
            }
            else
            {
                _controller = new ControllerPaintMobile(_mainCamera, _cursorRoller, _button);
                _inputDetector.SecondTouchPressed += OnSecondTouchPressed;
            }
        }

        private void Update()
        {
            if (_isCooldownActive == false)
                return;

            _elapsedTime += Time.deltaTime;
            _fill.fillAmount = _elapsedTime / _timeCooldown;

            if (_elapsedTime >= _timeCooldown)
                CompleteCooldown();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isReady == false)
                return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                SetOffsetPosition();
                _controller.ActivateCursor(eventData.position);
                PerformSphereCast(eventData.position);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                StartCooldown();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isReady)
                return;

            _controller.DragCursor(eventData.position, _offsetPosition);
            PerformSphereCast(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _controller.DeactivateCursor();

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

        private void PerformSphereCast(Vector3 screenPosition)
        {
            Vector3 screenPoint = _controller.DragCursor(screenPosition, _offsetPosition);

            Ray ray = _mainCamera.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if (Physics.SphereCast(ray, _controller.GuidanceRadius, out hit, Mathf.Infinity, _interactionLayerMask))
            {
                if (hit.transform.TryGetComponent(out TeleporterFigure figure) && figure.IsInstall)
                    return;

                if (_currentFigure != figure)
                {
                    if (_currentFigure != null)
                        _currentFigure.SetColor(_originalColor);

                    _currentFigure = figure;
                    _originalColor = figure.Color;
                }

                figure.SetColor(_color);
            }
            else
            {
                if (_currentFigure == null)
                    return;

                _currentFigure.SetColor(_originalColor);
                _currentFigure = null;
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

            _controller.DeactivateCursor();
        }

        private void CompleteCooldown()
        {
            _isCooldownActive = false;
            _isReady = true;

            Color previousColor = _color;

            do
            {
                SetNewColor();
            } while (_color == previousColor);

            _imageColor.gameObject.SetActive(true);

            Color color = _color;
            color.a = 1;
            _imageColor.color = color;
        }

        private void OnSecondTouchPressed()
        {
            if (_isReady == false)
                return;

            if (_controller.IsSecondTouchOnButton() == false)
                return;

            StartCooldown();
        }
    }
}