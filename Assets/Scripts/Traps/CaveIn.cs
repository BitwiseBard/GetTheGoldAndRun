using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveIn : MonoBehaviour {
    public Trap trap;
    public GameObject rocksGo;

    /**
        TODO: Get map size in Start instead of default values for the below values.
    */
    [SerializeField]
    private int minX;
    [SerializeField]
    private int maxX;    
    [SerializeField]
    private int minY;
    [SerializeField]
    private int maxY;

    [SerializeField]
    private int chance;

    void Update() {
        if(trap.GetTurn()) {
            if(Random.Range(0, chance) == 0) {
                /*Determine X and Y. Add .5 to center the sprite.*/
                float x = ((float)Random.Range(minX, maxX));
                float y = ((float)Random.Range(minY, maxY));
                /**
                    TODO: Never block stair path.
                */
                if(!CheckCollision(x, y)) {
                    Instantiate(rocksGo, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
            
            trap.FinishTurn();
        }        
    }

    /*Check for a collision. Do not cave in if it is not */
    private bool CheckCollision(float x, float y) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.1f);
        foreach(Collider2D col in colliders) {
            if(!col.isTrigger || col.tag == "Stairs") {
                return true;
            }
        }
        return false;
    }    
}
