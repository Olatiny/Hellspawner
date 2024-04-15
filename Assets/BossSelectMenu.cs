using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossSelectMenu : MonoBehaviour
{
    public string SelectedBoss { get; private set; } = "";

    public string DemonName = "Hell Scourge";
    [TextAreaAttribute] public string DemonDescription;

    public string LichName = "Lord of Secrets";
    [TextAreaAttribute] public string LichDescription;

    public string FrostName = "Frost Warden";
    [TextAreaAttribute] public string FrostDescription;

    [SerializeField]
    TextMeshProUGUI bossName;

    [SerializeField]
    TextMeshProUGUI bossDescription;

    [SerializeField]
    Image background;

    [SerializeField]
    Sprite noSelectedSprite;

    [SerializeField]
    Sprite selectedSprite;

    [SerializeField]
    Button summonBtn;

    [SerializeField]
    Button demonBtn;

    [SerializeField]
    Button lichBtn;

    [SerializeField]
    Button frostBtn;

    private void Start()
    {
        SetActive(false);
    }

    private void OnEnable()
    {
        GameManager gameManager = GameManager.Instance;

        demonBtn.interactable = !gameManager.DemonDefeated;
        lichBtn.interactable = !gameManager.LichDefeated;
        frostBtn.interactable = !gameManager.FrostWardenDefeated;

        SetActive(false);
    }

    public void SelectDemon()
    {
        SelectedBoss = "Demon";

        bossName.text = DemonName;
        bossDescription.text = DemonDescription;

        SetActive(true);
    }

    public void SelectLich()
    {
        SelectedBoss = "Lich";

        bossName.text = LichName;
        bossDescription.text = LichDescription;

        SetActive(true);
    }

    public void SelectFrost()
    {
        SelectedBoss = "Frost";

        bossName.text = FrostName;
        bossDescription.text = FrostDescription;

        SetActive(true);
    }

    public void CancelSelection()
    {
        SelectedBoss = "";
        SetActive(false);
    }

    public void Summon()
    {
        switch (SelectedBoss)
        {
            case "Demon":
                GameManager.Instance.startDemonBossFight();
                break;
            case "Lich":
                GameManager.Instance.startLichBossFight();
                break;
            case "Frost":
                GameManager.Instance.startFrostWardenBossFight();
                break;
            default:
                break;
        }
    }

    private void SetActive(bool active)
    {
        if (active)
            background.sprite = selectedSprite;
        else
            background.sprite = noSelectedSprite;

        bossName.gameObject.SetActive(active);
        bossDescription.gameObject.SetActive(active);
        summonBtn.gameObject.SetActive(active);
    }
}
