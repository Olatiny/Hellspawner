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
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(SceneManager.GetActiveScene(), new());
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

    [Header("Player Health Stuff")]
    [SerializeField] private int defaultMaxHealth = 10;
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

    public enum Difficulty
    {
        Normal,
        Hard,
        Extreme
    }

    [SerializeField]
    Difficulty levelDifficulty = Difficulty.Normal;

    // Start is called before the first frame update
    void Start()
    {
        //if easy mode set health to maxhealth
        if (levelDifficulty == Difficulty.Normal)
        {
            playerHealth = maxPlayerHealth;
        }
        //normal mode don't update health
        //hard mode die on hit

    }

    // Handles which canvases should be displayed each time a scene is loaded.
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (levelDifficulty == Difficulty.Normal)
            UpdatePlayerHealthAfterLoad();

        if (scene.name == mainMenuScene)
        {
            AudioManager.Instance?.TransitionMusicDefault();

            mainMenuCanv.gameObject.SetActive(true);
            bossSelectCanv.gameObject.SetActive(false);
            gameOverCanv.gameObject.SetActive(false);
            battleUI.gameObject.SetActive(false);
        }
        else if (scene.name == bossSelectScene)
        {
            AudioManager.Instance?.TransitionMusicDefault();

            mainMenuCanv.gameObject.SetActive(false);
            bossSelectCanv.gameObject.SetActive(true);
            gameOverCanv.gameObject.SetActive(false);
            battleUI.gameObject.SetActive(false);
        }
        else if (scene.name == gameOverScene)
        {
            AudioManager.Instance?.TransitionMusicDefault();

            mainMenuCanv.gameObject.SetActive(false);
            bossSelectCanv.gameObject.SetActive(false);
            gameOverCanv.gameObject.SetActive(true);
            battleUI.gameObject.SetActive(false);
        }
        else if (scene.name != finalScene)
        {
            if (scene.name == demonScene)
                AudioManager.Instance?.TransitionMusicFire();
            else if (scene.name == frostScene)
                AudioManager.Instance?.TransitionMusicIce();
            else if (scene.name == lichScene)
                AudioManager.Instance?.TransitionMusicLich();

            mainMenuCanv.gameObject.SetActive(false);
            bossSelectCanv.gameObject.SetActive(false);
            gameOverCanv.gameObject.SetActive(false);
            battleUI.gameObject.SetActive(true);
            playerHealthUI.GetComponent<PlayerHealth>().UpdateHearts();
        }
        else
        {
            mainMenuCanv.gameObject.SetActive(false);
            bossSelectCanv.gameObject.SetActive(false);
            gameOverCanv.gameObject.SetActive(false);
            battleUI.gameObject.SetActive(false);
        }
    }

    public void ResetPlayerProgress()
    {
        LichDefeated = DemonDefeated = FrostWardenDefeated = false;
    }

    public void SetDifficulty(GameManager.Difficulty difficulty)
    {
        this.levelDifficulty = difficulty;

        UpdatePlayerHealthAfterLoad();
    }

    public void UpdatePlayerHealthAfterLoad()
    {
        if (levelDifficulty == Difficulty.Extreme)
            maxPlayerHealth = 1;
        else
            maxPlayerHealth = defaultMaxHealth;

        playerHealth = maxPlayerHealth;
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

    public void setNormalDifficulty()
    {
        SetDifficulty(Difficulty.Normal);
    }

    public void FXButtonClick()
    {
        AudioManager.Instance.ClickSFX();
    }

    public void FXExtreme()
    {
        AudioManager.Instance.ExtremeModeSFX();
    }

    public void setHardDifficulty()
    {
        SetDifficulty(Difficulty.Hard);
    }

    public void setExtremeDifficulty()
    {
        SetDifficulty(Difficulty.Extreme);
    }

    public void LoseGame()
    {
        //lose game means reset
        FindAnyObjectByType<PlayerController>().Kill();
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

    public void Victory()
    {
        FindAnyObjectByType<PlayerController>()?.Kill();
        LoadBossSelect();
    }

    public void startEasyGame()
    {
        setNormalDifficulty();
        SceneManager.LoadScene(bossSelectScene);
    }

    public void startHardGame()
    {
        setHardDifficulty();
        SceneManager.LoadScene(bossSelectScene);
    }

    public void LoadSecret()
    {
        SceneManager.LoadScene(finalScene);
    }

    public void startExtremeGame()
    {
        setExtremeDifficulty();
        SceneManager.LoadScene(bossSelectScene);
    }

    public void startDemonBossFight()
    {
        SceneManager.LoadScene(demonScene);
        //show ability ui
        playerHealthUI.GetComponent<PlayerHealth>().AddAbilityGUI();
    }

    public void startLichBossFight()
    {
        SceneManager.LoadScene(lichScene);
        //show ability ui
        playerHealthUI.GetComponent<PlayerHealth>().AddAbilityGUI();
    }

    public void startFrostWardenBossFight()
    {
        SceneManager.LoadScene(frostScene);
        //show ability ui
        playerHealthUI.GetComponent<PlayerHealth>().AddAbilityGUI();
    }

    //method for hidden boss fight button
    public void startHiddenBossFight()
    {
        if (DemonDefeated && FrostWardenDefeated && LichDefeated)
        {
            //SceneManager.LoadScene("hiddenBoss");
        }

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadBossSelect()
    {
        SceneManager.LoadScene(bossSelectScene);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
