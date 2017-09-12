using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;    
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    bool isDead;
    bool damaged;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        //currentHealth = startingHealth;

        var healthBar = GameManager.instance.hud.Find("PlayerHealthBar").gameObject;

        healthSlider = healthBar.GetComponentInChildren<Slider>();
    }


    void Update ()
    {                        
        if(damaged)
        {
            //damageImage.color = flashColour;
        }
        else
        {
            //damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
    }

    public void TakeDamage (Unit source, int amount)
    {
        //Debug.Log(currentHealth);        

        playerMovement.health -= amount;
        healthSlider.value = playerMovement.health;


        if (amount > 0)
        {
            damaged = true;
            //playerAudio.Play ();

            if (playerMovement.health <= 0 && !isDead)
            {
                Death();
            }
            else if (playerMovement.health > 0)
            {
                GameManager.instance.CreateDamageText(playerMovement, -amount);
                //GetComponent<PlayerMovement>().LookAt(source);
                //anim.SetTrigger("Hurt");
            }
        }
        else
        {
            if (playerMovement.health >= playerMovement.maxHealth)
            {
                playerMovement.health = playerMovement.maxHealth;
            }
        }
    }

    void Death ()
    {
        isDead = true;
        
        anim.SetTrigger("Die");
        anim.SetBool("isDead", true);
        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

        playerMovement.enabled = false;

        //return null;

        GameManager.instance.GameOver();
        GameManager.instance.SetSceneWithWait("LevelFailed",2);
    }
}
