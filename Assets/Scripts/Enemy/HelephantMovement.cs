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

    Rigidbody rb;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        rb = GetComponent<Rigidbody>();
    }
    
    void Update ()
    {
        timer += Time.deltaTime;

        var distance = Vector3.Distance(this.transform.position, player.transform.position);
        bool playerInRange = distance <= range;

        if (charging && Time.time >= chargeTimer)
        {            
            charging = false;

            Debug.Log("strike");
            //nav.enabled = false;
            //Debug.Log(Time.time);
            //transform.position += new Vector3(0, 1, 0);
            rb.AddForce(transform.forward * 500, ForceMode.Impulse);
            //GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 1000, ForceMode.Impulse);
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
            nav.isStopped = true;
        }

        if (charging)
        {
            Vector3 unitToPlayer = player.transform.position - transform.position;
            unitToPlayer.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(unitToPlayer);
            rb.MoveRotation(newRotation);
        }

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
