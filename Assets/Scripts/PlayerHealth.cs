using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                    // Get the Image component
                    Image image = lichAbility.GetComponent<Image>();

                    // Get the material of the Image
                    Material material = image.material;

                    // Get the current color
                    Color color = material.color;

                    // Update the alpha value
                    color.a = 1.0f;

                    // Assign the modified color back to the material
                    material.color = color;
                    
                }
                else{
                    Debug.Log("lichUIUpdateneg");
                    Image image = lichAbility.GetComponent<Image>();

                    // Get the material of the Image
                    Material material = image.material;

                    // Get the current color
                    Color color = material.color;

                    // Update the alpha value
                    color.a = 0.25f;

                    // Assign the modified color back to the material
                    material.color = color;
                }
                
            }
            if (gameManager.frostWardenStatus()){
                
                if (player.canDoFrost()){
                    Debug.Log("frostUIUpdatePos");
                    //set ability to full opaq/active
                    // Get the Image component
                    Image image = frostAbility.GetComponent<Image>();

                    // Get the material of the Image
                    Material material = image.material;

                    // Get the current color
                    Color color = material.color;

                    // Update the alpha value
                    color.a = 1.0f;

                    // Assign the modified color back to the material
                    material.color = color;
                    
                }
                else{
                    Debug.Log("frostUIUpdateneg");
                    Image image = frostAbility.GetComponent<Image>();

                    // Get the material of the Image
                    Material material = image.material;

                    // Get the current color
                    Color color = material.color;

                    // Update the alpha value
                    color.a = 0.25f;

                    // Assign the modified color back to the material
                    material.color = color;
                }
                
            }
        }


    }
}
