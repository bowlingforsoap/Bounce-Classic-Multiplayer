using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private const string GROUND_TAG = "Ground";

    //[SerializeField] private InputAction _jumpAction;
    [SerializeField] private float _jumpHeight = .5f; // default - 50cm

    private Rigidbody _rigidbody;
    private bool _shouldJump;
    private bool _isGrounded;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        //_jumpAction.performed += Input_Jumped;
    }

    public void PlayerInput_Jumped(CallbackContext callbackContext)
    {
        var isPerformed = callbackContext.performed; 
        if (!isPerformed && _isGrounded)
        {
            Debug.Log("Jump requested");
            _shouldJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_shouldJump/* && _isGrounded*/)
        {
            _shouldJump = false;

            _rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
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
        if (collision.collider.tag == "Ground")
        {
            _isGrounded = false;
        }
    }
}
