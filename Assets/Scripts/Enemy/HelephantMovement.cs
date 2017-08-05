using UnityEngine;
using System.Collections;

public class HelephantMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;

    public GameObject chargeParticle;

    bool charging = false;
    bool chargedStrike = false;

    public float chargingTime = 2.0f;
    float chargeTimer = 0;

    public int range = 20;
    public int timeBetweenAttacks = 10;

    float timer;

    GameObject currentChargeParticle;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }
    
    void Update ()
    {
        timer += Time.deltaTime;

        /*var distance = Vector3.Distance(this.transform.position, player.transform.position);
        bool playerInRange = distance <= range;

        if (charging && Time.time >= chargeTimer)
        {            
            charging = false;
            chargedStrike = true;
        }

        if (chargedStrike)
        {
            nav.enabled = false;
            //Debug.Log(Time.time);
            transform.position += new Vector3(0, 1, 0);
            GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
            Destroy(currentChargeParticle);
            //nav.speed = 100;
            chargedStrike = false;
        }

        if (timer >= timeBetweenAttacks && playerInRange 
            && enemyHealth.currentHealth > 0
            && playerHealth.currentHealth > 0
            && charging == false)
        {
            charging = true;
            timer = 0f;
            chargeTimer = Time.time + chargingTime;           
            currentChargeParticle = Instantiate(chargeParticle, transform);
            //nav.enabled = false;
        }*/
        
                
        if (charging == false)
        {
            if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                nav.SetDestination(player.position);
            }
            else
            {
                nav.enabled = false;
            }
        }
    }
}
