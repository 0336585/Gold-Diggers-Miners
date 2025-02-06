using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseMovement : MonoBehaviour
{
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;

    public void SetVelocity(float _x, float _y, Rigidbody2D _rb)
    {
        _rb.linearVelocity = new Vector2(_x * xSpeed, _y * ySpeed);
    }
}
