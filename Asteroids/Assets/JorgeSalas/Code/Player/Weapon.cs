using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Fields

    [Header("Shoot conf")]
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    
    private float nextFire = 0.0f;
    #endregion

    #region Methods

    public void TryFire()
    {
        if (Time.time > nextFire)
        {
            Shoot();
            nextFire = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (BulletPoolManager.Instance == null) return;

        GameObject bullet = BulletPoolManager.Instance.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = transform.up * bulletSpeed;
        }
    }
    #endregion
}
