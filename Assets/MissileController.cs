using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {

    public GameObject explosionParticle;
    public int explosionDamage = 100;

    GameObject player;
    PlayerHealth playerHealth;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(explosionParticle, this.transform.position, this.transform.rotation);

            if (playerHealth.currentHealth > 0)
            {
                playerHealth.TakeDamage(explosionDamage);
            }

            Destroy(gameObject);
        }
    }
}