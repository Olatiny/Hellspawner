using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool DemonDefeatedBeatBool = false;

    public bool FrostWardenBeatBool = false;

    public bool LichBeatBool = false;

    [SerializeField]
    private Canvas mainMenuCanv;

    [SerializeField]
    private Canvas bossSelectCanv;

    public static GameManager _instance;
    [SerializeField]
    private int playerHealth = 5;
    [SerializeField]
    private int maxPlayerHealth = 5;

    enum Difficulty 
    {
        Easy,
        Hard,
        Extreme
    }

    [SerializeField]
    Difficulty levelDifficulty;


    private void Awake(){
        bossSelectCanv.enabled = false;
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

    public void playerTakeDamage(int damage){
        //Cam player take damage sound guh
        //if hard then lose on hit
        if (levelDifficulty == Difficulty.Extreme)
        {
            LoseGame();
        }
        if ((playerHealth - damage) <= 0)
        {
            LoseGame();
        }
        //player can take a damage and not lose
        playerHealth -= damage;
        //update ui

    }

    public void setEasyDifficulty(){
        levelDifficulty = Difficulty.Easy;
    }

    public void setHardDifficulty(){
        levelDifficulty = Difficulty.Hard;
    }

    public void setExtremeDifficulty(){
        levelDifficulty = Difficulty.Extreme;
    }

    public void LoseGame(){
        //lose game means reset
        bossSelectCanv.enabled = false;
        mainMenuCanv.enabled = true;
        SceneManager.LoadScene("MainMenu");
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

    public void startEasyGame(){
        setEasyDifficulty();
        mainMenuCanv.enabled = false;
        bossSelectCanv.enabled = true;
        SceneManager.LoadScene("BossSelect");
    }

    public void startHardGame(){
        setHardDifficulty();
        mainMenuCanv.enabled = false;
        bossSelectCanv.enabled = true;
        SceneManager.LoadScene("BossSelect");
    }

    public void startExtremeGame(){
        setExtremeDifficulty();
        mainMenuCanv.enabled = false;
        bossSelectCanv.enabled = true;
        SceneManager.LoadScene("BossSelect");
    }

    public void startDemonBossFight(){
        bossSelectCanv.enabled = false;
        SceneManager.LoadScene("grantBossTestScene");
    }

    public void startLichBossFight(){
        bossSelectCanv.enabled = false;
        SceneManager.LoadScene("lichScene");
    }

    public void startFrostWardenBossFight(){
        bossSelectCanv.enabled = false;
        SceneManager.LoadScene("frostwardenScene");
    }

    //method for hidden boss fight button
    public void startHiddenBossFight(){
        bossSelectCanv.enabled = false;
        if (DemonDefeatedBeatBool && FrostWardenBeatBool && LichBeatBool){
            //SceneManager.LoadScene("hiddenBoss");
        }
        
    }

    public void exitGame(){
        Application.Quit();
    }


}