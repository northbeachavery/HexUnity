using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
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
    }
    public void Heal(int healAmount)
    {
        currHealth += Mathf.Clamp(healAmount, minHealth, maxHealth);
    }
}
