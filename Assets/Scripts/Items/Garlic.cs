using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : MonoBehaviour, Item {
    private Inventory inventory;
    private GameObject stink;
    private PlayerController pc;

    [SerializeField]
    private string name;

    [SerializeField]
    private int spawnChance;    

    public void Equip() {}

    void Start() {
        inventory = GameObject.FindObjectOfType<Inventory>();
        pc = GameObject.FindObjectOfType<PlayerController>();
        stink = GameObject.FindWithTag("GarlicStink");
    }

    /*Add to player's health then return true to show it was used.*/
    public bool Use() {
        // stink.SetActive(true);
        pc.SetGarlicOn(true);
        return true;
    }

    public bool SingleUse() {
        return true;
    }    

    public bool NeedDirection() {
        return false;
    }
    public void SetDirection(int x, int y) {} 

    public string GetName() {
        return name;
    }    

    public int GetSpawnChance() {
        return spawnChance;
    }

    public bool GetSingleSlot() {
        return false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            inventory.AddItem(GetComponent<Item>());
            Destroy(gameObject);
        }
    }
}
