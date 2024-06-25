using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private const string SPIKE_TAG = "Spike";
    
    private static readonly int AnimatorGameOverTrigger = Animator.StringToHash("GameOver");
    private static readonly int AnimatorNewGameTrigger = Animator.StringToHash("NewGame");

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _jumpImpulse;
    [SerializeField] private float _moveAccelerationAmount;
    [SerializeField] private float _maxMoveSpeed;
    [Tooltip("How much air under the player to also consider as ground.")]
    [SerializeField] private float _jumpLeeway;

    [Header("GameOver")] 
    [SerializeField] private Animator _animator;
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
    }

    // TODO: this should not be in the player
    public IEnumerator GameOver()
    {
        _rigidbody.maxLinearVelocity = 0f;
        _rigidbody.Sleep();
        _gameOverSource.Play();
        _animator.SetTrigger(AnimatorGameOverTrigger);
        _guiController.ShowGameOverScreen();

        yield return new WaitForSeconds(3f);
        
        _animator.SetTrigger(AnimatorNewGameTrigger);
        _rigidbody.maxLinearVelocity = float.MaxValue;
        _rigidbody.WakeUp();
        transform.position = new Vector3(0.4f, 1.03299999f, 0);
        _guiController.HideGameOverScreen();
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
        if (isStarted && _isGrounded)
        {
            //Debug.Log("Jump requested");
            _shouldJump = true;
        }
    }

    private void Update()
    {
        // Check if player is on the ground
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.lossyScale.y / 2f + _jumpLeeway, _groundLayerMask);
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
        if (collision.gameObject.tag == SPIKE_TAG)
        {
            // Game Over
            Debug.Log("Game Over!");
            StartCoroutine(GameOver());
        }
    }
}
