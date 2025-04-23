using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Checks")]
    [SerializeField] private bool inMainMenu;
    [SerializeField] private bool inCreditsMenu;
    [SerializeField] private bool inPauseMenu;
    [SerializeField] private bool inOtherMenu;

    [Header("First Selected Options")]
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject creditsMenuFirst;
    [SerializeField] private GameObject pauseMenuFirst;
    [SerializeField] private GameObject otherMenuFirst;

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
        if (!inMainMenu && !inCreditsMenu && !inOtherMenu)
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

        if (inOtherMenu)
        {
            EventSystem.current.SetSelectedGameObject(otherMenuFirst);
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

        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad1) && Input.GetKey(KeyCode.Keypad2))
        {
            Application.Quit();
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
        if (SpawnData.Instance != null)
        {
            SpawnData.Instance.enemies.Clear();
            SpawnData.Instance.items.Clear();
            SpawnData.Instance.npcs.Clear();
        }
        Collected.currencyValue = 0;
        Collected.flowerValue = 0;
        SceneManager.LoadScene("Tutorial");
        Time.timeScale = 1;
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
        Time.timeScale = 1;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
