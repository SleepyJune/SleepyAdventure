using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{

    public GameObject explosionParticle;
    public int explosionDamage = 10;

    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(explosionParticle, this.transform);

            enemyHealth.TakeDamage(100, this.transform.position);

            if (playerHealth.currentHealth > 0)
            {
                playerHealth.TakeDamage(explosionDamage);
            }
        }        
    }
}
