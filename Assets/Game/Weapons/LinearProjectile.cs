using UnityEngine;

public class LinearProjectile : Projectile
{
    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if (Vector3.Distance(start, transform.position) > maxDistance)
        {
            Destroy(transform.gameObject);
        }
    }
}
