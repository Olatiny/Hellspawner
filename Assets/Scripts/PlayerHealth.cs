using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject happyHeart;
    [SerializeField] private GameObject sadHeart;
    [SerializeField] private float spriteWidth;
    [SerializeField] private Transform startHeartsHere;

    GameManager gameManager;
    List<GameObject> hearts = new();

    // private void Start()
    // {
    //     gameManager = GameManager.Instance;
    // }

    //using instead of start because it was null reffing
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void UpdateHearts()
    {
        int currHealth = gameManager.playerHealth;
        int maxHealth = gameManager.maxPlayerHealth;

        foreach(GameObject heart in hearts)
            Destroy(heart);

        for (int i = 0; i < maxHealth; i++)
        {
            Vector3 pos = startHeartsHere.position + new Vector3(i * spriteWidth, 0);
            hearts.Add(Instantiate(i < currHealth ? happyHeart : sadHeart, pos, transform.rotation, transform));
        }
    }
}
