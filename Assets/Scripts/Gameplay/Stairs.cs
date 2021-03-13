using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {
    private GameController gc;

    [SerializeField]
    private int level;

    void Start() {
        gc = GameObject.FindObjectOfType<GameController>();
    }
    
    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            gc.ChangeLevel(level);
        }
    }  
}
