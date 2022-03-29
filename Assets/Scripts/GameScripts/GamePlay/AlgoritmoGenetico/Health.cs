using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth = 200f;
    public RectTransform healthBar;
   public void UpdateHealth(float amount)
    {
        float currentValue=   (100 * amount) / maxHealth;

        currentHealth = amount;
        healthBar.sizeDelta = new Vector2(currentValue*2, healthBar.sizeDelta.y);
    }
    public void calc()
    {
      
    }
}
