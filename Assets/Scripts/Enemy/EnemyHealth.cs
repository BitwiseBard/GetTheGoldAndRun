using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {
    public EnemyController ec;
    public GameObject damageNumber;

    [SerializeField]
    private int maxHealth;
    private int currentHealth;  

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        /*Display UI damage.*/
        var clone = (GameObject)Instantiate(damageNumber, transform.position, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingNumbers>().SetDisplayText(damage);

        /*Reduce health and check death.*/
        currentHealth -= damage;
        if(currentHealth <= 0) {
            ec.Remove();
            SoundManager.PlaySound("monster");
        }
    }
}
