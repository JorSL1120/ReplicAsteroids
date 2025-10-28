using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Fields

    public float lifeTime = 5f;
    #endregion

    #region OnEnable
    
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }
    #endregion

    #region Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Asteroids>(out var a))
        {
            a.Hit();
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        CancelInvoke();
        BulletPoolManager.Instance.ReturnBullet(this.gameObject);
    }
    #endregion
}
