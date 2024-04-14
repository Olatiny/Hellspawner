using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool DemonDefeated = false;

    public bool FrostWardenDefeated = false;

    public bool LichDefeated = false;

    public static GameManager Instance;
    [SerializeField]
    private Canvas mainMenuCanv;

    [SerializeField]
    private Canvas bossSelectCanv;

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


    private void Awake()
    {
        bossSelectCanv.enabled = false;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
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

    public void setHealth(int newhealth)
    {
        playerHealth = newhealth;
    }

    public int getHealth()
    {
        return playerHealth;
    }

    public void PlayerTakeDamage(int damage)
    {
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

    public void setEasyDifficulty()
    {
        levelDifficulty = Difficulty.Easy;
    }

    public void setHardDifficulty()
    {
        levelDifficulty = Difficulty.Hard;
    }

    public void setExtremeDifficulty()
    {
        levelDifficulty = Difficulty.Extreme;
    }

    public void LoseGame()
    {
        //lose game means reset
        bossSelectCanv.enabled = false;
        mainMenuCanv.enabled = true;
        SceneManager.LoadScene("MainMenu");
    }

    public bool lichStatus()
    {
        return LichDefeated;
    }

    public bool frostWardenStatus()
    {
        return FrostWardenDefeated;
    }

    public bool demonStatus()
    {
        return DemonDefeated;
    }

    public void beatLich()
    {
        LichDefeated = true;
    }

    public void beatfrostWarden()
    {
        FrostWardenDefeated = true;
    }

    public void BeatDemon()
    {
        DemonDefeated = true;
    }

    public void startEasyGame()
    {
        setEasyDifficulty();
        bossSelectCanv.enabled = true;
        mainMenuCanv.enabled = false;
        SceneManager.LoadScene("BossSelect");
    }

    public void startHardGame()
    {
        setHardDifficulty();
        mainMenuCanv.enabled = false;
        bossSelectCanv.enabled = true;
        SceneManager.LoadScene("BossSelect");
    }

    public void startExtremeGame()
    {
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
        if (DemonDefeated && FrostWardenDefeated && LichDefeated){
            //SceneManager.LoadScene("hiddenBoss");
        }

    }

    public void exitGame()
    {
        Application.Quit();
    }


}
