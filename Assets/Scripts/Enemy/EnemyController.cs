using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    private GameController gc;
    
    private bool seePlayer;
    private bool isTurn;

    [SerializeField]
    private int spawnChance;

    void Start() {
        gc = GameObject.FindObjectOfType<GameController>();
    }

    public bool GetSeePlayer() {
        return seePlayer;
    }
    public void SetSeePlayer(bool see) {
        seePlayer = see;
    }

    public void StartTurn() {
        isTurn = true;
    }
    public void FinishTurn() {
        gc.NextTurn();
    }
    public void EndTurn() {
        isTurn = false;
    }
    public bool GetTurn() {
        return isTurn;
    }

    public void Remove() {
        gc.RemoveEnemy(this);
    }

    public int GetSpawnChance() {
        return spawnChance;
    }
}
