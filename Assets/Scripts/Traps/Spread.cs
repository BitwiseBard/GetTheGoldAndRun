using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : MonoBehaviour {
    public Trap trap;
    public GameObject spreadGo;

    [SerializeField]
    private int spreadChance;

    void Update() {
        if(trap.GetTurn()) {

            AttemptSpread();

            trap.FinishTurn();
        }        
    }

    /*Checks if spread will happen this turn then finds a suitable location.*/
    /**
        TODO: Currently being near walls will be less likely to build.
    */
    private void AttemptSpread() {
        /*Check if expanding will happen this turn.*/
        if(Random.Range(0, spreadChance) == 0) {
            bool finished = false;

            Vector2 spreadLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

            while(!finished) {
                /*Get direction to expand.*/
                switch(Random.Range(0, 8)) {
                    /*Up and left.*/
                    case 0:
                        spreadLocation.x -= 1;
                        spreadLocation.y += 1;
                        break;  
                    /*Up.*/                  
                    case 1:
                        spreadLocation.y += 1;
                        break;
                    /*Up and right.*/
                    case 2:
                        spreadLocation.x += 1;
                        spreadLocation.y += 1;
                        break;     
                    /*Left.*/               
                    case 3:
                        spreadLocation.x -= 1;
                        break;
                    /*Right.*/
                    case 4:
                        spreadLocation.x += 1;
                        break;
                    /*Down and left.*/                    
                    case 5:
                        spreadLocation.x -= 1;
                        spreadLocation.y -= 1;
                        break;      
                    /*Down.*/              
                    case 6:
                        spreadLocation.y -= 1;
                        break;
                    /*Down and right.*/                       
                    default:
                        spreadLocation.x += 1;
                        spreadLocation.y -= 1;
                        break;
                }

                /*Now check for collisions.*/
                switch (CheckCollision(spreadLocation.x, spreadLocation.y)) {
                    /*If wall exit.*/
                    case -1:
                        finished = true;
                        break;
                    /*If no collision found add */
                    case 0:
                        Instantiate(spreadGo, new Vector3(spreadLocation.x, spreadLocation.y, 0), Quaternion.identity);
                        finished = true;
                        break;
                    /*If same object type continue.*/
                    case 1:
                        break;
                    /*Error check. Should not happen.*/
                    default:
                        finished = true;
                        break;
                }
            }
        }        
    }

    /*Check for a collision at the current point. Returns -1 if colliding with a wall. Returns 0 if no collision. Returns 1 if collision with this object type.*/
    private int CheckCollision(float x, float y) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.1f);
        bool hitSomething = false;
        foreach(Collider2D col in colliders) {
            /*If non trigger collision check if either wall or this object type.*/
            if(!col.isTrigger) {
                // if(col.tag == "Wall") {
                if(col.tag == gameObject.tag) {
                    return 1;
                } 
                else if(col.tag != "Player" && col.tag != "Enemy") {
                    return -1;
                }
            }
        }
        return 0;
    }
}
