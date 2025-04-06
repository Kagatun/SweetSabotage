using System;
using UnityEngine;

public class DeafeningBeam : MonoBehaviour
{
    [SerializeField] private InputDetector _inputDetector;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private AudioSource _soundStun;

    private Camera _mainCamera;
    private float _radiusClick = 0.35f;

    public event Action Stuned;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputDetector.LeftMouseButtonPressed += CheckFigures;
    }

    private void OnDisable()
    {
        _inputDetector.LeftMouseButtonPressed -= CheckFigures;
    }

    private void CheckFigures()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.SphereCastAll(ray, _radiusClick, Mathf.Infinity, _layerMask);

        if (hits.Length > 0)
        {
            bool stunnedAny = false;

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Goose goose))
                {
                    goose.TakeStun();
                    stunnedAny = true;
                }
            }

            if (stunnedAny)
            {
                _soundStun.Play();
                Stuned?.Invoke();
            }
        }
    }
}