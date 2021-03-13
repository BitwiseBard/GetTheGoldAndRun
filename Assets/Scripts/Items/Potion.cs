using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, Item {
    private Inventory inventory;
    private PlayerHealth ph;

    [SerializeField]
    private string name;    

    [SerializeField]
    private int spawnChance;

    void Start() {
        inventory = GameObject.FindObjectOfType<Inventory>();
        ph = GameObject.FindObjectOfType<PlayerHealth>();
    }

    public void Equip() {}

    /*Add to player's health then return true to show it was used.*/
    public bool Use() {
        ph.ChangeHealth(20);
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
