using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemenu : MonoBehaviour
{
    public GameObject Pausepannel;
    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Pausepannel.SetActive(false);
                isPaused = false;
                Time.timeScale = 1;
                
            }

            else if (!isPaused && Time.timeScale > 0)
            {
                Pausepannel.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
                
            }
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void Lmainmenu()
    {
        SceneManager.LoadScene(0);
    }
}
