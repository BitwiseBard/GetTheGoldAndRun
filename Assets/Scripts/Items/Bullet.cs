using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rb;

    [SerializeField]
    private int maxDamage;
    [SerializeField]
    private int damageCount;    
    [SerializeField]
    private int damageModifier;

    private bool isMoving;
    private Vector3 moveTo;
    
    [SerializeField]
    private float speed;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(isMoving) {
            if(transform.position != moveTo) {
                transform.position = Vector3.MoveTowards(transform.position, moveTo, (speed * Time.deltaTime));
            }
            else {
                Destroy(gameObject);
            }     
        }   
    }

    public void SetDirection(int x, int y) {
        moveTo = new Vector2((x == 0 ? transform.position.x : (x * 100)), (y == 0 ? transform.position.y : (y * 100)));
        isMoving = true;
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
