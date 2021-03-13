using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, Item {
    private Inventory inventory;
    private PlayerMovement pm;

    [SerializeField]
    private int maxDamage;
    [SerializeField]
    private int damageCount;
    [SerializeField]
    private int damageModifier; 

    [SerializeField]
    private int spawnChance;

    [SerializeField]
    private string name;    

    void Start() {
        inventory = GameObject.FindObjectOfType<Inventory>();
        pm = GameObject.FindObjectOfType<PlayerMovement>();
    }

    public void Equip() {
        pm.SetAttack(maxDamage, damageCount, damageModifier);
    }

    /*This item is equipped not used. Return false to avoid skipping turn.*/
    public bool Use() {
        return false;
    }

    public bool SingleUse() {
        return false;
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
        return true;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            inventory.AddItem(GetComponent<Item>());
            Destroy(gameObject);
        }
    }
}
