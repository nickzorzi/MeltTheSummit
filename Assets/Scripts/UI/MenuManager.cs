using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("Checks")]
    [SerializeField] private bool inMainMenu;
    [SerializeField] private bool inCreditsMenu;
    [SerializeField] private bool inPauseMenu;

    [Header("First Selected Options")]
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject creditsMenuFirst;
    [SerializeField] private GameObject pauseMenuFirst;

    private void Start()
    {
        if (inMainMenu)
        {
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }

        if (inCreditsMenu)
        {
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }

        if (inPauseMenu)
        {
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }
    }

    private void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
