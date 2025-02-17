using System;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    private MeshRenderer _render;
    private IPoolAdder<Cookie> _poolAdder;
    private Rigidbody _rigidbody;

    public event Action Cleaned;

    public Color Color => _render.material.color;
    public bool IsReady { get; private set; }

    private void Awake()
    {
        _render = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetColor(Color color) =>
        _render.material.color = color;

    public void Remove() =>
        _poolAdder.AddToPool(this);

    public void RemoveFromAssemblyLine() =>
        Cleaned?.Invoke();

    public void Init(IPoolAdder<Cookie> poolAdder) =>
        _poolAdder = poolAdder;

    public void SetReady() =>
        IsReady = true;

    public void SetStartReady() =>
        IsReady = false;

    public void PushStart()
    {
        int force = 3;
        _rigidbody.AddForce(-transform.forward * force, ForceMode.Impulse);
    }
}