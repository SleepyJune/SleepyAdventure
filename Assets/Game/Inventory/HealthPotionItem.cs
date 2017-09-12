using UnityEngine;

public class HealthPotionItem : Item
{
    public int healthAmount = 0;

    public override bool Use()
    {
        GameManager.instance.player.TakeDamage(null, -healthAmount);
            //Mathf.Min(GameManager.instance.player.maxHealth, GameManager.instance.player.health + healthAmount);

        return true;
    }
}