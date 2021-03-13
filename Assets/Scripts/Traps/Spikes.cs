using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    public Trap trap;

    private Collider2D collider;
    private Renderer rend;

    private bool isUp;

    [SerializeField]
    private int damage;

    void Start() {
        collider = GetComponent<Collider2D>();
        rend = GetComponent<Renderer>();

        Lower();
    }

    void Update() {
        if(trap.GetTurn()) {
            if(isUp) {
                if(Random.Range(0, 2) == 0) {
                    Lower();
                }
            }     
            else {
                if(Random.Range(0, 3) == 0) {
                    PopUp();
                }
            }   
            trap.FinishTurn();
        }
    }

    private void PopUp() {
        isUp = true;
        
        rend.enabled = true;
        collider.enabled = true;
    }

    private void Lower() {
        isUp = false;

        rend.enabled = false;
        collider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Wedge") {
            Lower();
        }        
        else if(col.tag == "Enemy") {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        else if(col.tag == "Player") {
            col.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }        
    } 
}
