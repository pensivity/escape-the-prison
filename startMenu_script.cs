using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu_script : MonoBehaviour
{
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject instructions;


    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;

        startMenu = GameObject.Find("StartMenu");
        instructions = GameObject.Find("HowToPlay");
        instructions.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("01_FPS_FirstLevel");
    }

    public void QuitGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Application.Quit();
    }

    public void HowToPlay()
    {
        // Show the game instructions for how to play
        startMenu.SetActive(false);
        instructions.SetActive(true);
    }

    public void BackToMain()
    {
        startMenu.SetActive(true);
        instructions.SetActive(false);
    }
}
