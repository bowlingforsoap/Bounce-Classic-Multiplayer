using UnityEngine;

public class Spike : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            // Game Over
            Debug.Log("Game Over!");
        }
    }
}
