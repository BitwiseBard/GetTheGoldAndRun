using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreasureType {
    Gold,
    Ruby,
    DrGoldmen,
    GoldenTurtle,
    BigGold
}

public class Treasure : MonoBehaviour {
    private GameController gc;

    [SerializeField]
    private TreasureType type;
    [SerializeField]
    private int spawnChance;

    void Start() {
        gc = GameObject.FindObjectOfType<GameController>();
    }

    public int GetSpawnChance() {
        return spawnChance;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            SoundManager.PlaySound("treasure");
            gc.CollectTreasure(type);
            Destroy(gameObject);
        }
    }
}
