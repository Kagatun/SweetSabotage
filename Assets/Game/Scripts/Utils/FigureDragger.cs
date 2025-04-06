using UnityEngine;
using YG;

public class FigureDragger : MonoBehaviour
{
    [SerializeField] private InputDetector _inputDetector;
    [SerializeField] private LayerMask _layerMask;

    private Camera _mainCamera;
    private TeleporterFigure _figure;
    private bool _isDragging = false;
    private float _radius = 0.5f;
    private float _offsetX;
    private float _offsetY = 1;
    private float _offsetZ;
    private Vector3 _offsetFly;
    private Vector3 _offsetPosition;

    private void Awake()
    {
        _offsetFly = new Vector3(0, _offsetY, 0);
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        SetOffsetPosition();
    }

    private void Update()
    {
        if (_isDragging && _figure != null)
            DragFigure();
    }

    private void OnEnable()
    {
        _inputDetector.LeftMouseButtonPressed += OnStartDrag;
        _inputDetector.LeftMouseButtonReleased += OnEndDrag;
        _inputDetector.SecondTouchPressed += OnRotateFigure;
    }

    private void OnDisable()
    {
        _inputDetector.LeftMouseButtonPressed -= OnStartDrag;
        _inputDetector.LeftMouseButtonReleased -= OnEndDrag;
        _inputDetector.SecondTouchPressed -= OnRotateFigure;
    }

    public void ClearFigure()
    {
        if (_figure != null)
        {
            _figure.ResetPosition();
            _figure.SetSmallSize();
            _figure = null;
            Cursor.visible = true;
        }
    }

    private void SetOffsetPosition()
    {
        if (YandexGame.savesData.IsDesktop == false)
        {
            _offsetX = YandexGame.savesData.OffsetFigureHorizontal;
            _offsetZ = YandexGame.savesData.OffsetFigureVertical;

            _offsetPosition = new Vector3(_offsetX, 0, _offsetZ);
        }
    }

    private void DragFigure()
    {
        Vector3 inputPosition = YandexGame.savesData.IsDesktop ? (Vector3)Input.mousePosition : (Vector3)Input.GetTouch(0).position;
        Ray ray = _mainCamera.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        int layerMask = ~LayerMask.GetMask("Default", "Goose", "Cookie", "Figure");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (_figure != null && _figure.IsInstall == false)
            {
                Vector3 newPosition = hit.point + _offsetFly + _offsetPosition;
                newPosition.y = _offsetY;
                _figure.transform.position = newPosition;
                _inputDetector.SetBlockInput();
            }
        }
    }

    private void OnStartDrag()
    {
        SetOffsetPosition();

        Vector3 inputPosition = YandexGame.savesData.IsDesktop ? (Vector3)Input.mousePosition : (Vector3)Input.GetTouch(0).position;
        Ray ray = _mainCamera.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.SphereCast(ray, _radius, out hit, Mathf.Infinity, _layerMask))
        {
            if (hit.transform.TryGetComponent(out TeleporterFigure figure))
            {
                if (figure.IsInstall == false)
                {
                    _figure = figure;
                    _isDragging = true;
                    _figure.SetStandardSize();
                    _figure.EnableDetector();
                    _offsetFly = _figure.transform.position - hit.point;

                    if (YandexGame.savesData.IsDesktop)
                        Cursor.visible = false;
                }
            }
        }
    }

    private void OnEndDrag()
    {
        if (_isDragging && _figure != null)
        {
            _figure.InstallPanelInCells();

            if (_figure.IsInstall == false && _figure != null)
            {
                _figure.SetSmallSize();
                _figure.ResetPosition();
            }

            _isDragging = false;
            _figure.DisableDetector();
            _figure = null;
            _inputDetector.SetActivity();

            if (YandexGame.savesData.IsDesktop)
                Cursor.visible = true;
        }
    }

    private void OnRotateFigure()
    {
        if (_isDragging && _figure != null)
            _figure.Rotation();
    }
}