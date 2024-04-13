using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool DemonDefeatedBeatBool = false;

    public bool FrostWardenBeatBool = false;

    public bool LichBeatBool = false;

    public static GameManager _instance;
    [SerializeField]
    private int playerHealth = 5;
    [SerializeField]
    private int maxPlayerHealth = 5;

    enum Difficulty 
    {
        Easy,
        Medium,
        Hard
    }

    [SerializeField]
    Difficulty levelDifficulty;


    private void Awake(){
        
	    if (_instance != null && _instance != this){
		    Destroy(this.gameObject);
	    }
	    else{
		    _instance = this;
	    }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //if easy mode set health to maxhealth
        if (levelDifficulty == Difficulty.Easy)
        {
            playerHealth = maxPlayerHealth;
        }
        //normal mode don't update health
        //hard mode die on hit
    }

    public void setHealth(int newhealth){
        playerHealth = newhealth;
    }

    public int getHealth(){
        return playerHealth;
    }

    public void playerTakeDamage(){
        //Cam player take damage sound guh
        //if hard then lose on hit
        if (levelDifficulty == Difficulty.Hard)
        {
            LoseGame();
        }
        if (playerHealth == 1)
        {
            LoseGame();
        }
        //player can take a damage and lose
        playerHealth--;
        //update ui

    }

    public void setEasyDifficulty(){
        levelDifficulty = Difficulty.Easy;
    }

    public void setMediumDifficulty(){
        levelDifficulty = Difficulty.Medium;
    }

    public void setHardDifficulty(){
        levelDifficulty = Difficulty.Hard;
    }

    public void LoseGame(){
        //lose game means reset
        //SceneManager.LoadScene("mainmenu");
    }

    public bool lichStatus(){
        return LichBeatBool;
    }

    public bool frostWardenStatus(){
        return FrostWardenBeatBool;
    }

    public bool demonStatus(){
        return DemonDefeatedBeatBool;
    }

    public void beatLich(){
        LichBeatBool = true;
    }

    public void beatfrostWarden(){
        FrostWardenBeatBool = true;
    }

    public void beatdemon(){
        DemonDefeatedBeatBool = true;
    }


}
