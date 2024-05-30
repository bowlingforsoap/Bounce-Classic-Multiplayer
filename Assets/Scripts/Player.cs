using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private const string GROUND_TAG = "Ground";

    [SerializeField] private float _jumpImpulse;
    [SerializeField] private float _moveAccelerationAmount;
    [SerializeField] private float _maxMoveSpeed;


    private Rigidbody _rigidbody;
    private bool _shouldJump;
    private bool _shouldMove;
    private bool _isGrounded;
    private float _acceleration;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxLinearVelocity = _maxMoveSpeed;
    }

    public void PlayerInput_Moved(CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            //Debug.Log("Move started");
            _shouldMove = true;

            var valueX = callbackContext.ReadValue<Vector2>().x;
            _acceleration = valueX * _moveAccelerationAmount;
        }
        else if (callbackContext.canceled)
        {
            //Debug.Log("Move canceled");
            _shouldMove = false;
        }
    }

    public void PlayerInput_Jumped(CallbackContext callbackContext)
    {
        var isStarted = callbackContext.started; 
        // TODO: should allow jumping in a close proximity to the ground (i.e. 0.04m)? Otherwise very restrictive...
        if (isStarted && _isGrounded)
        {
            //Debug.Log("Jump requested");
            _shouldJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_shouldJump)
        {
            _shouldJump = false;

            _rigidbody.AddForce(Vector3.up * _jumpImpulse, ForceMode.Impulse);
            //Debug.Log($"Velocity after jump start: {_rigidbody.velocity}");
        }

        if (_shouldMove)
        {
            _rigidbody.velocity += new Vector3(_acceleration, 0f, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == GROUND_TAG)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == GROUND_TAG)
        {
            _isGrounded = false;
        }
    }
}
