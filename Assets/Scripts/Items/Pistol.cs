using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, Item {
    private Inventory inventory;
    public Bullet bullet;

    private float dirX;
    private float dirY;

    [SerializeField]
    private string name;

    [SerializeField]
    private int spawnChance;

    [SerializeField]
    private int ammo;

    void Start() {
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    public void Equip() {}

    /*Drops the wedge at the current location.*/
    public bool Use() {
        SoundManager.PlaySound("gunShot");
        Bullet clone = Instantiate(bullet, new Vector3((inventory.gameObject.transform.position.x + dirX), (inventory.gameObject.transform.position.y + dirY), 0), Quaternion.identity);
        // Instantiate(bullet, new Vector3((inventory.gameObject.transform.position.x), (inventory.gameObject.transform.position.y), 0), Quaternion.identity);
        clone.SetDirection((int)dirX, (int)dirY);

        --ammo;

        return true;
    }

    public bool SingleUse() {
        return ((ammo > 0) ? false : true);
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
        return false;
    }    

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            inventory.AddItem(GetComponent<Item>());
            Destroy(gameObject);
        }
    }  
}
