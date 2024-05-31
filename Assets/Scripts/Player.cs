using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private const string SPIKE_TAG = "Spike";
    private const string GROUND_TAG = "Ground";

    [SerializeField] private float _jumpImpulse;
    [SerializeField] private float _moveAccelerationAmount;
    [SerializeField] private float _maxMoveSpeed;

    [Header("GameOver")]
    [SerializeField] private Animation _gameOverAnimation;
    [SerializeField] private AudioSource _gameOverSource;
    [SerializeField] private GuiController _guiController;


    private Rigidbody _rigidbody;
    private bool _shouldJump;
    private bool _shouldMove;
    private bool _isGrounded;
    private float _acceleration;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_rigidbody.maxLinearVelocity = _maxMoveSpeed; // this would also clamps jump height
    }

    // TODO: this should not be in the player
    public void GameOver()
    {
        _rigidbody.maxLinearVelocity = 0f;
        _rigidbody.Sleep();
        _gameOverSource.Play();
        _gameOverAnimation.Play();
        _guiController.ShowGameOverScreen();
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
            float newXVelocity = Mathf.Clamp(_rigidbody.velocity.x + _acceleration, -_maxMoveSpeed, _maxMoveSpeed);
            _rigidbody.velocity = new Vector3(newXVelocity, _rigidbody.velocity.y, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == GROUND_TAG)
        {
            _isGrounded = true;
        }

        if (collision.gameObject.tag == SPIKE_TAG)
        {
            // Game Over
            Debug.Log("Game Over!");
            GameOver();
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
