using UnityEngine;

public class Cleaver : Projectile
{
    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if(Vector3.Distance(start, transform.position) > maxDistance)
        {
            Destroy(transform.gameObject);
        }
    }


}
