using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Text currentItemText;

    private List<Item> items;
    private int currentItem;

    void Start() {
        items = new List<Item>();
    }

    public bool ItemNeedsDirection() {
        if(items.Count > 0 && items[currentItem] != null) {
            return items[currentItem].NeedDirection();
        }
        else {
            return false;
        }
    }

    public void SetItemDirection(int x, int y) {
        items[currentItem].SetDirection(x, y);
    }

    public bool UseItem() {
        /*Check there is an item to use.*/
        if(items.Count > 0 && items[currentItem] != null) {
            /*Use returns true if item was used.*/
            if(items[currentItem].Use()) {
                /*Remove the item if it only can be used once.*/
                if(items[currentItem].SingleUse()) {
                   RemoveItem(); 
                }

                return true;
            }
        }
        return false;
    }

    public void AddItem(Item item) {
        bool addItem = true;

        if(item.GetSingleSlot()) {
            foreach(Item i in items) {
                if(i.GetName() == item.GetName()) {
                   addItem = false; 
                }
            }
        }

        if(addItem) {
            items.Add(item);

            /*If the only item equip it.*/
            if(items.Count == 1) {
                items[currentItem].Equip();
                ShowItem();
            }
        }
    }

    private void RemoveItem() {
        items.Remove(items[currentItem]);

        /*If number is greater than size of list loop back to 0.*/
        if(currentItem >= items.Count) {
            currentItem = 0;
            ShowItem();
        }
        if(items.Count > 0) {
            items[currentItem].Equip();
            ShowItem();
        }
    }

    public void NextItem() {
        ++currentItem;
        /*If number is greater than size of list loop back to 0.*/
        if(currentItem >= items.Count) {
            currentItem = 0;
        }

        /*Run any functionality that needs to be done when first used.*/
        items[currentItem].Equip();
        ShowItem();
    }

    /*Displays an item in the inventory.*/
    private void ShowItem() {
        if(items.Count > 0 && items[currentItem] != null) {
            currentItemText.text = items[currentItem].GetName();
        }
        else {
            currentItemText.text = "Fists";
        }
    }
}
