using UnityEngine;

public class FollowSolver : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - Target.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + _offset, Speed * Time.deltaTime);
    }
}
