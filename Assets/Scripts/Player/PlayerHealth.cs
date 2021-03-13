using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public GameObject gameOverPanel;
    public Text healthText;
    public GameObject damageNumber;

    [SerializeField]
    private int maxHealth;
    private int currentHealth;

    void Start() {
        currentHealth = maxHealth;

        SetHealthText();
    }

    /*Receives a positive number for gaining health or negative when taking damage.*/
    public void ChangeHealth(int health) {
        /*Reduce health and check death.*/
        currentHealth += health;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        if(currentHealth <= 0) {
            Die();
        }
        else {
            /*Display UI damage.*/
            var clone = (GameObject)Instantiate(damageNumber, transform.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingNumbers>().SetDisplayText(health);

            /*If gaining health change to green.*/
            if(health > 0) {
                clone.GetComponent<FloatingNumbers>().SetHealColor();
            }
            /*If losing health change to red.*/
            else if(health < 0) {
                clone.GetComponent<FloatingNumbers>().SetDamageColor();
            }
            else {
                clone.GetComponent<FloatingNumbers>().SetNoColor();
            }
            
            /*Change health on screen.*/
            SetHealthText();
        }
    }

    private void Die() {
        gameOverPanel.SetActive(true);
    }

    private void SetHealthText() {
        healthText.text = currentHealth.ToString();
    }
}
