using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public PlayerController pc;

    private Animator anim;

    private Rigidbody2D rb;
    [SerializeField]
    private float speed;
    private bool isMoving;

    private Vector3 moveTo;

    //TODO: MOVE TO CONTROLLER
    [SerializeField]
    private int defaultMaxDamage;
    [SerializeField]
    private int defaultDamageCount;
    [SerializeField]
    private int defaultDamageModifier;    
    private int maxDamage;
    private int damageCount;
    private int damageModifier;   

    private bool axisInUse; 

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ResetAttack();
    }

    void Update() {
        /*If player is moving call move function.*/
        if(pc.GetTurn() && !pc.GetWaiting()) {
            if(isMoving) {
                MovePlayer();
            }
            /*If player is not moving check user input.*/
            else {
                CheckMove();
            }
        }
    }

    /*Check for arrow button presses to setup movement.*/
    private void CheckMove() {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");
        
        /*If either */
        if((xMove != 0) || (yMove != 0)) {
            /*Do not run the script multiple times when user presses key once.*/
            if(!axisInUse) {
                axisInUse = true;

                /*Set the position to move the player to.*/
                moveTo = new Vector3((gameObject.transform.position.x + xMove), (gameObject.transform.position.y + yMove), 0);
                
                /*Check if the player is moving into a collider to prevent them from moving onto an enemy, wall, etc.*/
                /*Get all colliders to check if they are just triggers.*/
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(moveTo.x, moveTo.y), 0.1f);
                bool hitSomething = false;
                foreach(Collider2D col in colliders) {
                    /*If it as enemy hit it.*/
                    if(col.tag == "Enemy") {
                        Attack(col.gameObject.GetComponent<EnemyHealth>());
                        hitSomething = true;
                        break;                    
                    }
                    /*If it is not a trigger do not move.*/
                    if(!col.isTrigger) {
                        hitSomething = true;
                        break;
                    }
                }
                if(!hitSomething) { 
                    isMoving = true;  
                    anim.SetBool("IsMoving", true);
                } 
                else {
                    anim.SetBool("IsMoving", false);
                }
            }
            anim.SetFloat("LastX", xMove);
            anim.SetFloat("LastY", yMove);
        }
        /*When user lets go of key reset to allow for another key press.*/
        else {
            axisInUse = false;
        }
    }

    /*Move the player sprite.*/
    private void MovePlayer() {
        if(transform.position != moveTo) {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, (speed * Time.deltaTime));
        }
        else {
            isMoving = false;
            anim.SetBool("IsMoving", false);
            pc.FinishTurn();
        }
    }

    /*Allow other scripts to check if player is moving.*/
    public bool GetMoving() {
        return isMoving;
    }

    public void StopMoving() {
        isMoving = false;
    }

    //TODO: MOVE TO CONTROLLER
    /*Attacks a given enemy.*/
    private void Attack(EnemyHealth eh) {
        int damage = 0;
        
        /*Roll each damage roll.*/
        for(int x = 0; x < damageCount; ++x) {
            damage += Random.Range(1, maxDamage);
        }
        
        /*Add any modifier for damage.*/
        damage += damageModifier;
        
        /*Damage the enemy.*/
        eh.TakeDamage(damage);
        anim.SetBool("IsMoving", false);
        pc.FinishTurn();
    }

    public void SetAttack(int md, int dc, int dm) {
        maxDamage = md;
        damageCount = dc;
        damageModifier = dm; 
    }

    public void ResetAttack() {
        maxDamage = defaultMaxDamage;
        damageCount = defaultDamageCount;
        damageModifier = defaultDamageModifier; 
    }    
}
