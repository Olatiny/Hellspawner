using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject happyHeart;
    [SerializeField] private GameObject sadHeart;
    [SerializeField] private float spriteWidth;
    [SerializeField] private Transform startHeartsHere;
    [SerializeField] private Transform startAbilitiesHere;
    [SerializeField] private GameObject demonAbility;
    [SerializeField] private GameObject frostAbility;
    [SerializeField] private GameObject lichAbility;
    GameManager gameManager;
    List<GameObject> hearts = new();
    List<GameObject> abilityIndicators = new();

    // private void Start()
    // {
    //     gameManager = GameManager.Instance;
    // }

    //using instead of start because it was null reffing
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        updateAbilityGUI();
    }

    public void UpdateHearts()
    {
        if (!gameManager)
            gameManager = GameManager.Instance;

        int currHealth = gameManager.playerHealth;
        int maxHealth = gameManager.maxPlayerHealth;

        foreach (GameObject heart in hearts)
            Destroy(heart);

        for (int i = 0; i < maxHealth; i++)
        {
            Vector3 pos = startHeartsHere.position + new Vector3(i * spriteWidth, 0);
            hearts.Add(Instantiate(i < currHealth ? happyHeart : sadHeart, pos, transform.rotation, transform));
        }
    }

    public void AddAbilityGUI()
    {
        Debug.Log("abilityUISet");
        if (!gameManager){
            gameManager = GameManager.Instance;
        }

        foreach (GameObject abilityGUIElement in abilityIndicators){
            Destroy(abilityGUIElement);
        }
            
        int index = 0;
        Vector3 pos = startAbilitiesHere.position + new Vector3(index * spriteWidth, 0);
        if (gameManager.lichStatus()){
            Debug.Log("lichUISet");
            abilityIndicators.Add(Instantiate(lichAbility, pos, transform.rotation, transform));
            index++; //increment index so stuff doesn't overlap
        }
        pos = startAbilitiesHere.position + new Vector3(index * spriteWidth, 0);
        if (gameManager.frostWardenStatus()){
            Debug.Log("frostUISet");
            abilityIndicators.Add(Instantiate(frostAbility, pos, transform.rotation, transform));
            index++; //increment index so stuff doesn't overlap
        }
        pos = startAbilitiesHere.position + new Vector3(index * spriteWidth, 0);
        if (gameManager.demonStatus()){
            Debug.Log("demonUISet"); 
            //instantiate with icon for always ready/active
            abilityIndicators.Add(Instantiate(demonAbility, pos, transform.rotation, transform));
            index++; //increment index so stuff doesn't overlap
        }
            
        
    }

    public void updateAbilityGUI()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player){
            if (gameManager.lichStatus()){
                
                if (player.canDoDash()){
                    Debug.Log("lichUIUpdatePos");
                    //set ability to full opaq/active
                    //lichAbility.GetComponent<Renderer>().GetComponent<Renderer>().material.color = new Color(
                    //    lichAbility.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, transparency);
                    // Color color = lichAbility.GetComponent<Renderer>().material.color;
                    // color.a = transparency;
                    // gameObject.GetComponent<Renderer>().material.color = color;
                    
                }
                else{
                    Debug.Log("lichUIUpdateneg");
                    //set ability to half opaq/inactive
                    //lichAbility.GetComponent<Renderer>().material.color.a = 0.5f;
                }
                
            }
            if (gameManager.frostWardenStatus()){
                if (player.canDoDash()){
                    Debug.Log("frostUIUpdatePos");
                    //set ability to full opaq/active
                    //frostAbility;
                }
                else{
                    Debug.Log("frostUIUpdateneg");
                    //set ability to half opaq/inactive
                    //frostAbility;
                }
            }
        }


    }
}
