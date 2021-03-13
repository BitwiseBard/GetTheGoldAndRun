using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public EnemyController ec;
    public PlayerController pc;

    private Animator anim;

    [SerializeField]
    private float speed;

    private bool isMoving;
    private Vector3 moveTo;

    //TODO: MOVE TO CONTROLLER
    [SerializeField]
    private int maxDamage;
    [SerializeField]
    private int damageCount;
    [SerializeField]
    private int damageModifier;

    void Start() {
        pc = GameObject.FindObjectOfType<PlayerController>();
        anim = anim = GetComponent<Animator>();
    }

    void Update() {
        /*Only act if it is not the players turn.*/
        if(ec.GetTurn()) {
            /*If enemy is moving */
            if(isMoving) {
                MoveEnemy();
            }
            /*If not moving attack player if adjacent. If no player is near by get the best move location.*/
            else if(ec.GetSeePlayer()) {
                GetMove();
            }
            else {
                ec.FinishTurn();
            }
        }
    }

    /*Check for arrow button presses to setup movement.*/
    private void GetMove() {
        /**
            TODO: This should be improved to A*.        
        */
        /*Check all four directions to move closer to the player.*/
        if((pc.gameObject.transform.position.x > gameObject.transform.position.x) &&
            GetMoveCollision((gameObject.transform.position.x + 1), gameObject.transform.position.y)) {
            moveTo = new Vector3((gameObject.transform.position.x + 1), gameObject.transform.position.y, 0);  
            isMoving = true;
            anim.SetFloat("LastX", 1);
            anim.SetFloat("LastY", 0);            
        }
        else if((pc.gameObject.transform.position.x < gameObject.transform.position.x) &&
                 GetMoveCollision((gameObject.transform.position.x - 1), gameObject.transform.position.y)) {
            moveTo = new Vector3((gameObject.transform.position.x - 1), gameObject.transform.position.y, 0);  
            isMoving = true;
            anim.SetFloat("LastX", -1);
            anim.SetFloat("LastY", 0);            
        }
        else if((pc.gameObject.transform.position.y > gameObject.transform.position.y) &&
                 GetMoveCollision(gameObject.transform.position.x, (gameObject.transform.position.y + 1))) {
            moveTo = new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y + 1), 0);  
            isMoving = true;
            anim.SetFloat("LastX", 0);
            anim.SetFloat("LastY", 1);            
        }
        else if((pc.gameObject.transform.position.y < gameObject.transform.position.y) &&
                 GetMoveCollision(gameObject.transform.position.x, (gameObject.transform.position.y - 1))) {
            moveTo = new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y - 1), 0);  
            isMoving = true;
            anim.SetFloat("LastX", 0);
            anim.SetFloat("LastY", -1);                
        }
        else {
            isMoving = false;
            ec.FinishTurn();
        }
        anim.SetBool("IsMoving", isMoving);
    }

    /*Check if the enemy is moving into a collider to prevent them from moving onto player, wall, etc.*/
    private bool GetMoveCollision(float x, float y) {
        /*Get all colliders to check if they are just triggers.*/
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.1f);

        foreach(Collider2D col in colliders) {
            if(!col.isTrigger || col.tag == "GarlicStink") {
                /*Attack player if they are adjacent.*/
                if(col.tag == "Player") {
                    Attack(col.gameObject.GetComponent<PlayerHealth>());
                }
                return false;
                anim.SetBool("IsMoving", false);
            }
        }

        return true;
    }

    /*Move enemy sprite towards player.*/
    private void MoveEnemy() {
        if(transform.position != moveTo) {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, (speed * Time.deltaTime));
        }
        else {
            isMoving = false;
            ec.FinishTurn();
            anim.SetBool("IsMoving", false);
        }
    }    

    //TODO: MOVE TO CONTROLLER
    /*Attack the player.*/
    private void Attack(PlayerHealth ph) {
        int damage = 0;
        
        /*Roll each damage roll.*/
        for(int x = 0; x < damageCount; ++x) {
            damage += Random.Range(1, maxDamage);
        }
        
        /*Add any modifier for damage.*/
        damage += damageModifier;
        
        /*Damage the enemy.*/
        ph.ChangeHealth(-damage);
        ec.FinishTurn();
        anim.SetBool("IsMoving", false);
    }        
}
