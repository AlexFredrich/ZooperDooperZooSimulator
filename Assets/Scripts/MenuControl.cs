using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class MenuControl : MonoBehaviour {

    [SerializeField]
    GameObject startGameButton, creditsButton, exitButton, returnButton;
    [SerializeField]
    GameObject creditsMenu, mainMenu;

    private void Awake()
    {
        Button startGame = startGameButton.GetComponent<Button>();
        startGame.onClick.AddListener(StartGame);
        Button credits = creditsButton.GetComponent<Button>();
        credits.onClick.AddListener(CreditOpen);
        Button Return = returnButton.GetComponent<Button>();
        Return.onClick.AddListener(CreditClose);
        Button exit = exitButton.GetComponent<Button>();
        exit.onClick.AddListener(ExitGame);
    }

    // Use this for initialization
    void Start ()
    {
        creditsMenu.SetActive(false);
	}
	
    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void CreditOpen()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CreditClose()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
