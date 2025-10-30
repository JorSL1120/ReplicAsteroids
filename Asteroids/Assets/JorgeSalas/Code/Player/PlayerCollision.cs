using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Asteroids>(out var asteroids))
        {
            GameEvents.PlayerHit();
            asteroids.Hit();
        }
    }
}
