using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    private GameController gc;
    
    private bool isTurn;
    
    [SerializeField]
    private int likeliness;

    void Start() {
        gc = GameObject.FindObjectOfType<GameController>();
    }

    public void StartTurn() {
        isTurn = true;
    }
    public void FinishTurn() {
        gc.NextTurn();
    }
    public void RemoveItem(bool destroy) {
        gc.RemoveTrap(this, destroy);
    }
    public void EndTurn() {
        isTurn = false;
    }
    public bool GetTurn() {
        return isTurn;
    }

    public int GetLikeliness() {
        return likeliness;
    }
}
