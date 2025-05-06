using Figure;
using System;
using UnityEngine;
using YG;

namespace ManagementUtilities
{
    public class RemoverFigures : MonoBehaviour
    {
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private AudioSource _soundRemove;

        private Camera _mainCamera;
        private float _radius = 0.25f;

        public event Action<Color> Fined;

        private void Start()
        {
            _mainCamera = Camera.main;

            if (YandexGame.savesData.IsDesktop == false)
            {
                float radiusMobile = 0.5f;
                _radius = radiusMobile;
            }
        }

        private void OnEnable()
        {
            _inputDetector.SecondTouchPressed += RemoveFigure;
        }

        private void OnDisable()
        {
            _inputDetector.SecondTouchPressed -= RemoveFigure;
        }

        private void RemoveFigure()
        {
            Vector3 inputPosition = YandexGame.savesData.IsDesktop ? (Vector3)Input.mousePosition : (Vector3)Input.GetTouch(1).position;
            Ray ray = _mainCamera.ScreenPointToRay(inputPosition);
            RaycastHit hit;

            if (Physics.SphereCast(ray, _radius, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.TryGetComponent(out TeleporterFigure figure))
                {
                    if (figure.IsInstall == false && _inputDetector.IsActive)
                    {
                        figure.Use();
                        figure.Remove();
                        _soundRemove.Play();
                        Fined?.Invoke(figure.Color);
                    }
                }
            }
        }
    }
}