using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCapsuleScript : MonoBehaviour
{
    public int health_gained = 30;

    PlayerHealth playerHealth;
    GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 100);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.GainHealth(health_gained);
            Destroy(gameObject);
        }
    }
}
