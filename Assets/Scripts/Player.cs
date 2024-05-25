using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    //[SerializeField] private InputAction _jumpAction;
    [SerializeField] private float _jumpHeight = .5f; // default - 50cm

    private Rigidbody _rigidbody;
    private bool _jumping;
    private bool _grounded;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        //_jumpAction.performed += Input_Jumped;
    }

    public void PlayerInput_Jumped(CallbackContext callbackContext)
    {
        var value = callbackContext.ReadValueAsButton(); 
        if (value)
        {
            Debug.Log("Jumped!");
            _jumping = true;
        }
    }

    private void FixedUpdate()
    {
        if (_jumping && _grounded)
        {
            _jumping = false;

            _rigidbody.AddForce(Vector3.up * _jumpHeight);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _grounded = false;
        }
    }
}
