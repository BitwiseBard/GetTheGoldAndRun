using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour {

    public Trap trap;

    private Animator anim;

    enum Direction {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }

    private Direction dir;
    private bool hasDir;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int moveDistance;
    private int moveCounter;
    private Vector3 moveTo;
    private bool isMoving;

    [SerializeField]
    private int damage;

    void Start() {
        anim = gameObject.GetComponent<Animator>();

        moveCounter = 0;
    }

    void Update() {
        if(moveDistance > 0) {
            if(trap.GetTurn()) {
                /*If the boulder is currently moving continue.*/
                if(isMoving) {
                    Move();
                }
                /*If the boulder has a direction continue that way.*/
                else if(hasDir) {
                    switch(dir) {
                        case Direction.LEFT:
                            moveTo = new Vector3((gameObject.transform.position.x - 1), gameObject.transform.position.y, 0);
                            GetMoveCollision(moveTo.x, moveTo.y);
                            break;
                        case Direction.UP:
                            moveTo = new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y + 1), 0);
                            GetMoveCollision(moveTo.x, moveTo.y);
                            break;
                        case Direction.RIGHT:
                            moveTo = new Vector3((gameObject.transform.position.x + 1), gameObject.transform.position.y, 0);
                            GetMoveCollision(moveTo.x, moveTo.y);
                            break;
                        case Direction.DOWN:
                            moveTo = new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y - 1), 0);
                            GetMoveCollision(moveTo.x, moveTo.y);
                            break; 
                        default:
                            break;
                    }
                }
                /*If the boulder does not have a location get one.*/
                else {
                    dir = (Direction)Random.Range(0, 3);
                    hasDir = true;
                }
            }
        }
        else {
            trap.FinishTurn();
        }
    }

    /*Check each point from the current location to its endpoint for collisions.*/
    private void GetMoveCollision(float x, float y) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.1f);
        bool hitSomething = false;
        foreach(Collider2D col in colliders) {
            /*If it is not a trigger do not move. Able to roll over players or enemies.*/
            // if(col.tag == "Wall") {
            if(!col.isTrigger && col.tag != "Player" && col.tag != "Enemy") {
                anim.SetBool("IsMoving", false);
                hitSomething = true;
                break;
            }
        }
        if(!hitSomething) { 
            anim.SetBool("IsMoving", true);
            isMoving = true;       
        }    
        else {
            hasDir = false;
            anim.SetBool("IsMoving", false);
            moveCounter = 0;
            trap.FinishTurn();
        }     
    }

    /*Move the player sprite.*/
    private void Move() {
        if(transform.position != moveTo) {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, (speed * Time.deltaTime));
        }
        else {
            isMoving = false;

            /*Check if the boualder has moved its entire distance.*/
            ++moveCounter;
            if(moveCounter == moveDistance) {
                anim.SetBool("IsMoving", false);
                moveCounter = 0;
                trap.FinishTurn();
            }
        }
    }    

    /*Damage player or enemy if they are hit by the boulder.*/
    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Wedge") {
            trap.RemoveItem(false);
        }
        else if(col.tag == "Enemy") {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        else if(col.tag == "Player") {
            col.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
    }
}
