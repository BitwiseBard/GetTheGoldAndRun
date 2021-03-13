using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {
    public EnemyController ec;

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            ec.SetSeePlayer(true);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.tag == "Player") {
            ec.SetSeePlayer(false);
        }
    }
}
