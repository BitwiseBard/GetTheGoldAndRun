using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    /*Turn based objects.*/
    public PlayerController player;
    public List<EnemyController> enemies;
    public List<Trap> traps;

    public FadeIn fadeIn;
    
    public MapGenerator mg;

    public GameObject winPanel;
    public GameObject quitPanel;

    public Text scoreText;
    public Text levelText;

    /*Treasures*/
    public Text goldLabel;
    public Text rubyLabel;
    public Text drGoldmenLabel;
    public Text goldenTurtleLabel;
    public Text bigGoldLabel;
    public List<Treasure> treasures;
    private int goldCount;
    private int rubyCount;
    private int drGoldmenCount;
    private int goldenTurtleCount;
    private int bigGoldCount;  

    bool isPlayerTurn;
    bool isEnemyTurn;
    bool isTrapTurn;

    private int currentLevel;

    void Start() {
        player = GameObject.FindObjectOfType<PlayerController>();
        if(player != null) {
            player.StartTurn();
            isPlayerTurn = true;
        }

        currentLevel = 1;

        SoundManager.PlaySound("start");
    }

    /*Skip to the next turn in the list.*/
    public void NextTurn() {
        /*If it is the players turn end their turn and move on to enemies.*/
        if(isPlayerTurn) {
            /*End turn.*/
            player.EndTurn();

            /*Set flags.*/
            isPlayerTurn = false;
            
            /*Start first enemy turn or trap turn if either exist. IF they don't restart player turn.*/
            if(enemies.Count > 0) {
                enemies[0].StartTurn();
                isEnemyTurn = true;
            }
            else if(traps.Count > 0) {
                traps[0].StartTurn();
                isTrapTurn = true;
            }
            else {
                isPlayerTurn = true;
                player.StartTurn();
                mg.CheckWallShift();
            }
        }
        else if(isEnemyTurn) {
            /*Loop through enemies looking for the next enemy.*/
            for(int x = 0; x < enemies.Count; ++x) {
                if(enemies[x].GetTurn()) {
                    enemies[x].EndTurn();

                    /*If there is another enemy after move to them.*/
                    if(x < (enemies.Count - 1)) {
                        enemies[x + 1].StartTurn();
                    }
                    else {
                        isEnemyTurn = false;

                        /*If there is no enemy after start the traps turn.*/
                        if(traps.Count > 0) {
                            traps[0].StartTurn();
                            isTrapTurn = true;
                        }
                        /*If there are no traps start the player turn.*/
                        else {
                            player.StartTurn();
                            isPlayerTurn = true;
                            mg.CheckWallShift();
                        }                        
                    }
                    break;
                }
            }
        }
        else if(isTrapTurn) {
            for(int x = 0; x < traps.Count; ++x) {
                if(traps[x].GetTurn()) {
                    traps[x].EndTurn();

                    /*If there is another trap after move to them.*/
                    if(x < (traps.Count - 1)) {
                        traps[x + 1].StartTurn();
                    }
                    /*Otherwise restart player turn.*/
                    else {
                        isTrapTurn = false;
                        isPlayerTurn = true;
                        player.StartTurn();
                        mg.CheckWallShift();
                    }
                    break;
                }
            }
        }
        //Error checking. Should never happen.
        else {
            isPlayerTurn = true;
            player.StartTurn();
            mg.CheckWallShift();
        }
    }

    /*Add a treasures for the end of the game.*/
    /**
        TODO: MOVE TO A PLAYER SCRIPT.
    */
    public void CollectTreasure(TreasureType type) {
        switch(type) {
            case TreasureType.Gold: 
                ++goldCount;
                goldLabel.text = "Gold: " + goldCount;
                break;            
            case TreasureType.Ruby: 
                ++rubyCount;
                rubyLabel.text = "Ruby: " + rubyCount;
                break;            
            case TreasureType.DrGoldmen: 
                ++drGoldmenCount;
                drGoldmenLabel.text = "Dr Goldmen: " + drGoldmenCount;
                break;            
            case TreasureType.GoldenTurtle: 
                ++goldenTurtleCount;
                goldenTurtleLabel.text = "Golden Turtle: " + goldenTurtleCount;
                break;            
            case TreasureType.BigGold: 
                ++bigGoldCount;
                bigGoldLabel.text = "Big Gold: " + bigGoldCount;
                break;
            default:
                break;
        }
    }  

    /**/
    public void AddTrap(Trap t) {
        traps.Add(t);
    }
    public void RemoveTrap(Trap t) {
        if(t.GetTurn()) {
            NextTurn();
        }
        traps.Remove(t);
        Destroy(t.gameObject);
    }

    public void AddEnemy(EnemyController ec) {
        enemies.Add(ec);
    }
    /*Remove enemies from the game when killed.*/
    public void RemoveEnemy(EnemyController ec) {
        if(ec.GetTurn()) {
            NextTurn();
        }
        enemies.Remove(ec);
        Destroy(ec.gameObject);
    }  

    /*Remove traps from the game when destroyed.*/
    public void RemoveTrap(Trap t, bool destroy) {
        if(t.GetTurn()) {
            NextTurn();
        }        
        traps.Remove(t);
        if(destroy) {
            Destroy(t.gameObject);
        }
    }
    
    /*Reload the game scene.*/
    public void ReloadGame() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    /*Exit the game.*/
    public void QuitGame() {
        Application.Quit();
    }

    public void ChangeLevel(int level) {
        currentLevel += level;

        if(level > 0) {
            SoundManager.PlaySound("downStairs");
        }
        else {
            SoundManager.PlaySound("upStairs");
        }

        /*Check if game is over.*/
        if(currentLevel <= 0) {
            /*Each gold worth 1 point.*/
            int score = goldCount;

            /*Each ruby worth 3 points.*/
            score += (rubyCount * 3);

            /*Each golden turtle is worth 5.*/
            score += (goldenTurtleCount * 5);

            /*Each pair of goldmen are worth 10 points.*/
            score += ((drGoldmenCount / 2) * 5);

            /*Big gold treasures are exponential.*/
            if(bigGoldCount > 0) {
                int bigGoldScore = 1;
                while(--bigGoldCount > 0) {
                    bigGoldScore *= 2;
                }
                score += bigGoldScore;
            }

            SetScoreText(score);
            Win();
        }
        else {
            fadeIn.Fade(true);

            /*Otherwise reset map.*/
            player.EndTurn();

            foreach(EnemyController ec in enemies) {
                Destroy(ec.gameObject);
            }
            enemies.Clear();

            foreach(Trap t in traps) {
                Destroy(t.gameObject);
            }
            traps.Clear();

            mg.ResetMap();

            player.StartTurn();
            isPlayerTurn = true; 

            levelText.text = currentLevel.ToString();
        }       
    }

    private void Win() {
        winPanel.SetActive(true);
    }

    private void SetScoreText(int value) {
        scoreText.text = ("Score: " + value);
    }    

    public void PauseGame() {
        quitPanel.SetActive(true);
    }
}
