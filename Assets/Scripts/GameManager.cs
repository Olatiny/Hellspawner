using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool DemonDefeated = false;
    public bool FrostWardenDefeated = false;
    public bool LichDefeated = false;

    [Header("Menu Canvases")]
    [SerializeField] private Canvas mainMenuCanv;
    [SerializeField] private Canvas bossSelectCanv;
    [SerializeField] private Canvas gameOverCanv;
    [SerializeField] private Canvas battleUI;
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private GameObject playerHealthUI;
    public int playerHealth { get; private set; } = 10;
    public int maxPlayerHealth { get; private set; } = 10;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string bossSelectScene;
    [SerializeField] private string gameOverScene;
    [SerializeField] private string demonScene;
    [SerializeField] private string frostScene;
    [SerializeField] private string lichScene;
    [SerializeField] private string finalScene;

    enum Difficulty
    {
        Easy,
        Hard,
        Extreme
    }

    [SerializeField]
    Difficulty levelDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        //if easy mode set health to maxhealth
        if (levelDifficulty == Difficulty.Easy)
        {
            playerHealth = maxPlayerHealth;
        }
        //normal mode don't update health
        //hard mode die on hit

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Handles which canvases should be displayed each time a scene is loaded.
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (levelDifficulty == Difficulty.Easy)
            playerHealth = maxPlayerHealth;

        if (scene.name == mainMenuScene)
        {
            mainMenuCanv.gameObject.SetActive(true);
            bossSelectCanv.gameObject.SetActive(false);
            gameOverCanv.gameObject.SetActive(false);
            battleUI.gameObject.SetActive(false);
        }
        else if (scene.name == bossSelectScene)
        {
            mainMenuCanv.gameObject.SetActive(false);
            bossSelectCanv.gameObject.SetActive(true);
            gameOverCanv.gameObject.SetActive(false);
            battleUI.gameObject.SetActive(false);
        }
        else if (scene.name == gameOverScene)
        {
            mainMenuCanv.gameObject.SetActive(false);
            bossSelectCanv.gameObject.SetActive(false);
            gameOverCanv.gameObject.SetActive(true);
            battleUI.gameObject.SetActive(false);
        }
        else
        {
            mainMenuCanv.gameObject.SetActive(false);
            bossSelectCanv.gameObject.SetActive(false);
            gameOverCanv.gameObject.SetActive(false);
            battleUI.gameObject.SetActive(true);
            playerHealthUI.GetComponent<PlayerHealth>().UpdateHearts();
        }
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
        playerHealthUI.GetComponent<PlayerHealth>().UpdateHearts();
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
        SceneManager.LoadScene(gameOverScene);
    }

    public void UpdateBossHealthBar(Boss boss)
    {
        if (battleUI != null && bossHealthBar != null && battleUI.enabled)
        {
            bossHealthBar.fillAmount = boss.bossCurrentHealth / boss.bossMaxHealth;
            bossName.text = boss.name;
        }
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
        SceneManager.LoadScene(bossSelectScene);
    }

    public void startHardGame()
    {
        setHardDifficulty();
        mainMenuCanv.enabled = false;
        bossSelectCanv.enabled = true;
        SceneManager.LoadScene(bossSelectScene);
    }

    public void startExtremeGame()
    {
        setExtremeDifficulty();
        mainMenuCanv.enabled = false;
        bossSelectCanv.enabled = true;
        SceneManager.LoadScene(bossSelectScene);
    }

    public void startDemonBossFight(){
        bossSelectCanv.enabled = false;
        SceneManager.LoadScene(demonScene);
    }

    public void startLichBossFight(){
        bossSelectCanv.enabled = false;
        SceneManager.LoadScene(lichScene);
    }

    public void startFrostWardenBossFight(){
        bossSelectCanv.enabled = false;
        SceneManager.LoadScene(frostScene);
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
