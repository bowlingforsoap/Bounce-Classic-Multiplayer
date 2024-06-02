using UnityEngine;

public class Ground : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            GroundManager.Instance.AddActiveGround(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            GroundManager.Instance.RemoveActiveGround(this);
        }
    }
}
