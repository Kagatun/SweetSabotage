using UnityEngine;

public class MoverConveyorMaterial : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Material _material;

    private float _speed = 2f;
    private float _offsetY = -0.02f;

    private void FixedUpdate()
    {
        _material.mainTextureOffset += new Vector2(Time.fixedDeltaTime, _offsetY);
        Vector3 position = _rigidbody.position;
        _rigidbody.position += Vector3.forward * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(position);
    }
}
