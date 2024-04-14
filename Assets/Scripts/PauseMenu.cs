using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;

    void Awake()
    {
        pauseMenuUI.SetActive( false );
    }


    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape ) && GameManager.fighting == true )
        {
            if(paused)
                Resume();
            else
            {
                Pause();
                Debug.Log("Paused");
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive( false );
        Time.timeScale = 1f;
        paused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive( true );
        Time.timeScale = 0f;
        paused = true;
    }
}
