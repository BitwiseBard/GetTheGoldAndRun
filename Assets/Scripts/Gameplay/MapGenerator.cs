using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GameObject player;

    public GameObject wall;
    public GameObject floor;
    public GameObject stairsUp;
    public GameObject stairsDown;

    public List<EnemyController> enemies;
    public List<Trap> traps;
    public List<Treasure> treasures;
    public List<GameObject> items;

    [SerializeField]
    private int maxX;
    [SerializeField]
    private int maxY;
    // [SerializeField]
    // private int maxCount;
    // [SerializeField]
    // private int maxRoomSize;

    [SerializeField]
    private int trapMax;    
    [SerializeField]
    private int treasureMax;    
    [SerializeField]
    private int enemyMax;    
    [SerializeField]
    private int itemMax;
    [SerializeField]
    private int trapMin;    
    [SerializeField]
    private int treasureMin;    
    [SerializeField]
    private int enemyMin;    
    [SerializeField]
    private int itemMin;

    [SerializeField]
    private int shiftWallChance;    
    [SerializeField]
    private int wallFrequency;

    private List<List<bool>> occupied;

    private GameController gc;

    void Start() {
        gc = GameObject.FindObjectOfType<GameController>();

        occupied = new List<List<bool>>();

        Setup();
    }

    private void Setup() {
        DrawFloor();
        AddPlayer();
        AddStairs();
        AddTraps();
        AddEnemies();
        AddTreasures();
        AddItems();
        ShiftWalls();
    }

    public void ResetMap() {
        occupied.Clear();

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Wall")) {
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Stairs")) {
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Floor")) {
            Destroy(go);
        }                

        Setup();
    }

    /*Draws a floor layout along with walls around the perimiter.*/
    private void DrawFloor() {
        for(int x = 0; x < maxX; ++x) {
            occupied.Add(new List<bool>());
            for(int y = 0; y < maxY; ++y) {
                if(x == 0 || y == 0 || (x == (maxX - 1)) || (y == (maxY - 1))) {
                    Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity); 
                    occupied[x].Add(true);
                }
                else {
                    Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity); 
                    occupied[x].Add(false);
                }
            }
        }
    }

    private void AddPlayer() {
        /*Pick a random spot not on the perimiter.*/
        int x = Random.Range(1, (maxX - 1));
        int y = Random.Range(1, (maxY - 1));

        /*Move player and set occupied flag.*/
        player.transform.position = new Vector3(x, y, 1);
        occupied[x][y] = true;
    }

    /*Add up and down stairs to map.*/
    private void AddStairs() {
        bool downAdded = false;
        bool upAdded = false;

        /*Loop picking random X and Y until available location is found.*/
        while(!downAdded) {
            int x = Random.Range(1, (maxX - 1));
            int y = Random.Range(1, (maxY - 1));

            /*If available create stairs down.*/
            if(!occupied[x][y]) {
                Instantiate(stairsDown, new Vector3(x, y, 0), Quaternion.identity);
                downAdded = true;
                /*Prevent from drawing other objects on top.*/
                occupied[x][y] = true;
            }
        }       

        /*Loop picking random X and Y until available location is found.*/ 
        while(!upAdded) {
            int x = Random.Range(1, (maxX - 1));
            int y = Random.Range(1, (maxY - 1));

            /*If available create stairs up.*/
            if(!occupied[x][y]) {
                Instantiate(stairsUp, new Vector3(x, y, 0), Quaternion.identity);
                upAdded = true;
                /*Prevent from drawing other objects on top.*/
                occupied[x][y] = true;
            }
        }
    }

    private void AddTreasures() {
        /*Get the total likeliness for all combined traps for future use.*/
        int treasureLikeliness = 0;
        foreach(Treasure t in treasures) {
            treasureLikeliness += t.GetSpawnChance();
        }

        /*Determine count of  to place.*/
        int treasureCount = Random.Range(treasureMin, treasureMax);
        for(int i = 0; i < treasureCount; ++i) {
            /*Reset flag and get a new index to determine what trap will be added.*/
            bool treasureAdded = false;
            int treasureIndex = 0;
            int treasureNumber = Random.Range(0, treasureLikeliness);
            while(treasureNumber > treasures[treasureIndex].GetSpawnChance()) {
                treasureNumber -= treasures[treasureIndex].GetSpawnChance();
                ++treasureIndex;
            }            
            
            /*Loop until trap has found a place to be added.*/
            while(!treasureAdded) {
                int x = Random.Range(0, maxX);
                int y = Random.Range(0, maxY);     

                /*If space is free add.*/
                if(!occupied[x][y]) {
                    Instantiate(treasures[treasureIndex], new Vector3(x, y, 0), Quaternion.identity);
                    treasureAdded = true;
                    /*Prevent from drawing other objects on top.*/
                    occupied[x][y] = false;
                }              
            }
        }
    }
    
    private void AddItems() {
        /*Get the total likeliness for all combined items for future use.*/
        int itemLikeliness = 0;
        foreach(GameObject i in items) {
            itemLikeliness += i.GetComponent<Item>().GetSpawnChance();
        }

        /*Determine count of  to place.*/
        int itemCount = Random.Range(itemMin, itemMax);
        for(int i = 0; i < itemCount; ++i) {
            /*Reset flag and get a new index to determine what trap will be added.*/
            bool itemAdded = false;
            int itemIndex = 0;
            int itemNumber = Random.Range(0, itemLikeliness);
            while(itemNumber > items[itemIndex].GetComponent<Item>().GetSpawnChance()) {
                itemNumber -= items[itemIndex].GetComponent<Item>().GetSpawnChance();
                ++itemIndex;
            }            
            
            /*Loop until trap has found a place to be added.*/
            while(!itemAdded) {
                int x = Random.Range(0, maxX);
                int y = Random.Range(0, maxY);     

                /*If space is free add.*/
                if(!occupied[x][y]) {
                    Instantiate(items[itemIndex], new Vector3(x, y, 0), Quaternion.identity);
                    itemAdded = true;
                    /*Prevent from drawing other objects on top.*/
                    occupied[x][y] = false;
                }              
            }
        }
    }

    /*Add all enemies to map.*/
    private void AddEnemies() {
        /*Get the total likeliness for all combined traps for future use.*/
        int enemyLikeliness = 0;
        foreach(EnemyController ec in enemies) {
            enemyLikeliness += ec.GetSpawnChance();
        }

        /*Add how many enemies to place.*/
        int enemyCount = Random.Range(enemyMin, enemyMax);
        for(int i = 0; i < enemyCount; ++i) {
            /*Reset flag and get a new index to determine what trap will be added.*/
            bool enemyAdded = false;
            int enemyIndex = 0;
            int enemyNumber = Random.Range(0, enemyLikeliness);
            while(enemyNumber > enemies[enemyIndex].GetSpawnChance()) {
                enemyNumber -= enemies[enemyIndex].GetSpawnChance();
                ++enemyIndex;
            }
            
            /*Loop until trap has found a place to be added.*/
            while(!enemyAdded) {
                int x = Random.Range(0, maxX);
                int y = Random.Range(0, maxY);     

                /*If space is free add.*/
                if(!occupied[x][y]) {
                    EnemyController tempEnemy = Instantiate(enemies[enemyIndex], new Vector3(x, y, 0), Quaternion.identity);
                    gc.AddEnemy(tempEnemy);
                    enemyAdded = true;
                }              
            }
        }
    }  

    /*Add all the traps to the map.*/
    private void AddTraps() {
        /*Get the total likeliness for all combined traps for future use.*/
        int trapLikeliness = 0;
        foreach(Trap t in traps) {
            trapLikeliness += t.GetLikeliness();
        }

        /*Add a trap for each in count.*/
        int trapCount = Random.Range(trapMin, trapMax);
        for(int i = 0; i < trapCount; ++i) {
            /*Reset flag and get a new index to determine what trap will be added.*/
            bool trapAdded = false;

            /*Determine which trap based on likeliness.*/
            int trapIndex = 0;
            int trapNumber = Random.Range(0, trapLikeliness);
            while(trapNumber > traps[trapIndex].GetLikeliness()) {
                trapNumber -= traps[trapIndex].GetLikeliness();
                ++trapIndex;
            }
            
            /*Loop until trap has found a place to be added.*/
            while(!trapAdded) {
                int x = Random.Range(1, (maxX - 1));
                int y = Random.Range(1, (maxY - 1));     

                /*If space is free add.*/
                if(!occupied[x][y]) {
                    Trap tempTrap = Instantiate(traps[trapIndex], new Vector3(x, y, 0), Quaternion.identity);
                    gc.AddTrap(tempTrap);
                    trapAdded = true;
                }              
            }
        }
    }    

    public void CheckWallShift() {
        if(Random.Range(0, shiftWallChance) == 0) {
            /*Play here so it doesn't happen on startup or new level.*/
            SoundManager.PlaySound("tileMove");
            ShiftWalls();
        }
    }

    private void ResetWalls() {
        for(int x = 0; x < maxX; ++x) {
            for(int y = 0; y < maxY; ++y) {
                occupied[x][y] = false;
            }
        }        
    }

    private void ShiftWalls() {
        /*Remove current walls.*/
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Wall")) {
            Destroy(go);
        }

        /*Add new walls.*/
        for(int x = 0; x < maxX; ++x) {
            for(int y = 0; y < maxY; ++y) {
                /*Add the perimiter.*/
                if(x == 0 || y == 0 || (x == (maxX - 1)) || (y == (maxY - 1))) {
                    Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity); 
                }                
                /*Randomly place wall. Don't place around player and check for collisions.*/
                else if(!occupied[x][y] && (Random.Range(0, wallFrequency) == 0) && !CheckCollision((float)x, (float)y) &&
                        !GetAdjacentPlayer((float)x, (float)y)) {
                    Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity); 
                }
            }
        }
    }

    /*Check if wall is going to hit another collider.*/
    private bool CheckCollision(float x, float y) {
        /*Get all colliders to check if they are just triggers.*/
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.1f);

        foreach(Collider2D col in colliders) {
            if(!col.isTrigger) {
                return true;
            }
        }

        return false;
    }    

    private bool GetAdjacentPlayer(float x, float y) {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        if((playerX > (x - 1.1)) && (playerX < (x + 1.1)) && (playerY > (y - 0.1)) && (playerY < (y + 0.1)) || 
            ((playerX > (x - 0.1)) && (playerX < (x + 0.1)) && (playerY > (y - 1.1)) && (playerY < (y + 1.1)))) {
            return true;
        }
        return false;
    }
}
