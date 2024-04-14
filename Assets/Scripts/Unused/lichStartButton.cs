using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lichStartButton : MonoBehaviour
{

    private GameManager gameManagerRef;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerRef = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startFight(){
        gameManagerRef.startLichBossFight();
    }
}
