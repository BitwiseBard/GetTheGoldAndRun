using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipAttack : MonoBehaviour {
    private Rigidbody2D rb;

    [SerializeField]
    private int maxDamage;
    [SerializeField]
    private int damageCount;    
    [SerializeField]
    private int damageModifier;

    private int x;
    private int y;
    private bool dirSet;
    
    [SerializeField]
    private float speed;
    [SerializeField]
    private int range;

    private Vector3 moveTo;
    private Vector3 returnPoint;
    private bool returning;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(dirSet) {
            if(returning) {
                transform.position = Vector3.MoveTowards(transform.position, returnPoint, (speed * Time.deltaTime));

                if(transform.position == returnPoint) {
                    Destroy(gameObject);
                }
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, moveTo, (speed * Time.deltaTime));

                if(transform.position == moveTo) {
                    returning = true;
                }
            }
        }
    }

    public void SetDirection(int tempX, int tempY, int returnX, int returnY) {
        x = tempX;
        y = tempY;

        /*Get start and end points for whip.*/
        if(x != 0) {
            moveTo = new Vector3((transform.position.x + (x * range)), transform.position.y, transform.position.z);
        }
        else {
            moveTo = new Vector3(transform.position.x, (transform.position.y + (y * range)), transform.position.z);
        }       
        returnPoint = new Vector3(returnX, returnY, transform.position.z); 

        dirSet = true; 
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(!col.isTrigger ) {
            if(col.tag == "Enemy") {
                int damage = 0;
                
                /*Roll each damage roll.*/
                for(int x = 0; x < damageCount; ++x) {
                    damage += Random.Range(1, maxDamage);
                }

                col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage + damageModifier);
            }
            Destroy(gameObject);   
        }
    } 
}
