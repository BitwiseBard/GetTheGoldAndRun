using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {
    [SerializeField]
    private int damage;

    /*Damage player or enemy if they are hit by the boulder.*/
    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Enemy") {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        else if(col.tag == "Player") {
            col.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
    }
}
