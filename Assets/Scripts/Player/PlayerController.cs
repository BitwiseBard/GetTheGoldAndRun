using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private GameController gc;
    private Inventory inventory;
    private PlayerMovement pm;
    
    public GameObject garlicStink;
    [SerializeField]
    private int garlicStinkMax;
    [SerializeField]
    private int stinkRadius;
    private int garlicStinkCount;
    private bool garlicOn;

    private bool isTurn;
    private bool waitForDirection;
    private bool waitForRelease;

    void Start() {
        gc = GameObject.FindObjectOfType<GameController>();
        pm = GetComponent<PlayerMovement>();
        inventory = GetComponent<Inventory>();
        SetGarlicOn(false);
    }

    void Update() {
        if(isTurn) {
            /*Use an item when space is clicked.*/
            if(Input.GetKeyDown("space") && !pm.GetMoving()) {
                /*Some items require a direction to use.*/
                if(inventory.ItemNeedsDirection()) {
                    waitForDirection = true;
                }
                else if(inventory.UseItem()) {
                    FinishTurn();
                }
            }

            /*If the item needs a direction wait for input from user.*/
            if(waitForDirection) {
                float x = Input.GetAxisRaw("Horizontal");
                float y = Input.GetAxisRaw("Vertical");

                /*If input was received add direction.*/
                if(x != 0 || y != 0) {
                    inventory.SetItemDirection((int)x, (int)y);
                    waitForRelease = true;                 
                }
            }

            /*Wait for release of input keys or else the player will just move along with using the item.*/
            if(waitForRelease && (Input.GetAxisRaw("Horizontal") == 0) && (Input.GetAxisRaw("Vertical") == 0)) {
                waitForDirection = false;
                if(inventory.UseItem()) {
                    FinishTurn();
                }              
                waitForRelease = false;
            }               

            /*Swap items.*/
            if(Input.GetKeyDown("e")) {
                pm.ResetAttack();
                inventory.NextItem();
            }

            if(Input.GetKeyDown("q")) {
                gc.PauseGame();
            }            
        }     
    }

    public void SetGarlicOn(bool garlic) {
        garlicOn = garlic;
        if(garlic) {
            garlicStinkCount = garlicStinkMax;
            garlicStink.GetComponent<CircleCollider2D>().radius = stinkRadius;
        }
        else {
            garlicStink.GetComponent<CircleCollider2D>().radius = 0.01f;
        }
    }

    public void StartTurn() {
        isTurn = true;

        /**
            TODO: If start turn in water increase drowning.
        */
        if(garlicOn) {
            /*If garlic stink is active lower it.*/
            if(garlicStink.active) {
                --garlicStinkCount;
            }
            /*If garlic stink counter has hit 0 disable the gameobject and reset the counter.*/
            if(garlicStinkCount <= 0) {
                // garlicStink.SetActive(false);
                
                SetGarlicOn(false);
            }
        }
    }
    public void FinishTurn() {
        gc.NextTurn();
    }
    public void EndTurn() {
        isTurn = false;
        pm.StopMoving();
    }
    public bool GetTurn() {
        return isTurn;
    }

    public bool GetWaiting() {
        return waitForDirection;
    }
}
