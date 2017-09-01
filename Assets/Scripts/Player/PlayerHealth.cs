using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth = 100;
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
        currentHealth = startingHealth;

        var healthBar = GameObject.Find("Canvas").transform.Find("PlayerHealthBar").gameObject;

        healthSlider = healthBar.GetComponentInChildren<Slider>();

        healthBar.SetActive(true);
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

        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
        else if(currentHealth > 0)
        {
            GameManager.instance.CreateDamageText(-amount, this.transform.position);
            //GetComponent<PlayerMovement>().LookAt(source);
            //anim.SetTrigger("Hurt");
        }
    }

    public void GainHealth(int amount)
    {
        currentHealth += amount;

        healthSlider.value = currentHealth;
        
        if (currentHealth >= startingHealth)
        {
            currentHealth = startingHealth;
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
                
        GameManager.instance.SetSceneWithWait("LevelFailed",2);
    }
}
