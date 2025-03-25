using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

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

    [Header("Menu")]
    [SerializeField] private GameObject pauseMenu;

    [Header("GameObject Finder")]
    [SerializeField] private GameObject playerController;

    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        if (!inMainMenu && !inCreditsMenu)
        {
            playerController = GameObject.FindGameObjectWithTag("Player");

            masterSlider.value = VolumeData.instance.masterLevel;
            sfxSlider.value = VolumeData.instance.sfxLevel;
            musicSlider.value = VolumeData.instance.musicLevel;
        }

        if (inMainMenu)
        {
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);

            Destroy(GameObject.Find("DontDestroy"));
        }

        if (inCreditsMenu)
        {
            EventSystem.current.SetSelectedGameObject(creditsMenuFirst);
        }

        if (inPauseMenu)
        {
            EventSystem.current.SetSelectedGameObject(pauseMenuFirst);
        }
    }

    private void Update()
    {
        if (InputManager.isPauseTriggered)
        {
            if (!inPauseMenu)
            {
                OpenPause();
            }
            else
            {
                ClosePause();
            }
        }
    }

    public void OpenPause()
    {
        inPauseMenu = true;

        Time.timeScale = 0;
        playerController.SetActive(false);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);

        pauseMenu.SetActive(true);
    }

    public void ClosePause()
    {
        inPauseMenu = false;

        Time.timeScale = 1;
        playerController.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);

        pauseMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
        Time.timeScale = 1;
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
