using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public Image healthBar;
    [SerializeField] private int maxHealth;
    [SerializeField] private int minHealth;
    [SerializeField] private int currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    public void Damage(int damageAmount)
    {
        currHealth -= damageAmount;
        if (currHealth < minHealth)
        {
            Destroy(gameObject);
        }
        if (healthBar != null)
        {
            healthBar.fillAmount = currHealth / maxHealth;
        }
    }
    public void Heal(int healAmount)
    {
        int potentialHealth = currHealth + healAmount;
        if(potentialHealth <= maxHealth)
        {
            currHealth =potentialHealth;
        }
        if(potentialHealth > maxHealth)
        {
            currHealth = maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.fillAmount = currHealth / maxHealth;
        }

    }
}
