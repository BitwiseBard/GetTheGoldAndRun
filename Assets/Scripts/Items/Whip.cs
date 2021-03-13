using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour, Item {
    private Inventory inventory;
    public WhipAttack whipAttack;

    private float dirX;
    private float dirY;

    [SerializeField]
    private string name;

    [SerializeField]
    private int spawnChance;

    void Start() {
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    public void Equip() {}

    /*Drops the wedge at the current location.*/
    public bool Use() {
        WhipAttack clone = Instantiate(whipAttack, new Vector3((inventory.gameObject.transform.position.x + dirX), (inventory.gameObject.transform.position.y + dirY), 0), Quaternion.identity);
        clone.SetDirection((int)dirX, (int)dirY, (int)inventory.gameObject.transform.position.x, (int)inventory.gameObject.transform.position.y);
        return true;
    }

    public bool SingleUse() {
        return false;
    }    

    public bool NeedDirection() {
        return true;
    }

    public void SetDirection(int x, int y) {
        dirX = x;
        dirY = y;
    }   

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
