using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireMissile : MonoBehaviour {

    public GameObject missile;
    public float startSpeed = 10;
    public float accel = 1;

    public int range = 20;
    public int timeBetweenAttacks = 10;

    float currentSpeed = 10;

    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;

    float timer;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        var distance = Vector3.Distance(this.transform.position, player.transform.position);
        bool playerInRange = distance <= range;
                
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
        }

        //currentSpeed += accel * Time.deltaTime;
	}

    void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {            
            var newMissile = Instantiate(missile, this.transform.position + new Vector3(0,0.5f,0), this.transform.rotation);
            newMissile.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000);
        }
    }
}
