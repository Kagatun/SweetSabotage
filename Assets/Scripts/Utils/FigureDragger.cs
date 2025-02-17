using System;
using UnityEngine;

public class FigureDragger : MonoBehaviour
{
    [SerializeField] private InputDetector _inputDetector;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _layerMask;

    private TeleporterFigure _figure;
    private bool _isDragging = false;
    private int _offsetY = 1;
    private Vector3 _offset;

    public event Action Activated;

    private void Awake()
    {
        _offset = new Vector3(0, _offsetY, 0);
    }

    private void Update()
    {
        if (_isDragging && _figure != null)
            DragFigure();
    }

    private void DragFigure()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (_figure != null && _figure.isInstall == false)
            {
                Vector3 newPosition = hit.point + _offset;
                newPosition.y = _offsetY;
                _figure.transform.position = newPosition;
                _inputDetector.SetBlockInput();
            }
        }
    }

    private void OnStartDrag()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            if (hit.transform.TryGetComponent(out TeleporterFigure figure))
            {
                _figure = figure;
                _isDragging = true;
                _figure.SetStandardSize();
                _figure.EnableDetector();
                _offset = _figure.transform.position - hit.point;
            }
        }
    }

    private void OnEndDrag()
    {
        if (_isDragging && _figure != null)
        {
            _figure.InstallPanelInCells();

            if (_figure.isInstall == false)
            {
                _figure.SetSmallSize();
                _figure.ResetPosition();
            }

            _isDragging = false;
            _figure.DisableDetector();
            _figure = null;
            _inputDetector.SetActivity();
        }
    }

    protected virtual void OnEnable()
    {
        _inputDetector.LeftMouseButtonPressed += OnStartDrag;
        _inputDetector.LeftMouseButtonReleased += OnEndDrag;
        _inputDetector.RightClicked += OnRotateFigure;
    }

    protected virtual void OnDisable()
    {
        _inputDetector.LeftMouseButtonPressed -= OnStartDrag;
        _inputDetector.LeftMouseButtonReleased -= OnEndDrag;
        _inputDetector.RightClicked -= OnRotateFigure;
    }

    private void OnRotateFigure()
    {
        if (_isDragging && _figure != null)
            _figure.Rotation();
    }
}