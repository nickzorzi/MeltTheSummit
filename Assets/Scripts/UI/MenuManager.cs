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

    [Header("Finder")]
    [SerializeField] private GameObject playerController;
    public PlayerController player;

    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        if (!inMainMenu && !inCreditsMenu && !inOtherMenu)
        {
            playerController = GameObject.FindGameObjectWithTag("Player");
            player = FindObjectOfType<PlayerController>();

            masterSlider.value = VolumeData.instance.masterLevel;
            sfxSlider.value = VolumeData.instance.sfxLevel;
            musicSlider.value = VolumeData.instance.musicLevel;
        }

        if (inMainMenu)
        {
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);

            //Destroy(GameObject.Find("DontDestroy"));
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
        if (InputManager.isPauseTriggered && inMainMenu)
        {
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }

        if (InputManager.isPauseTriggered && !inMainMenu && !inOtherMenu && !inCreditsMenu && !player._inDialogue)
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

        //if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad1) && Input.GetKey(KeyCode.Keypad2))
        //{
           //Application.Quit();
        //}
    }

    public void OpenPause()
    {
        inPauseMenu = true;

        player._inPause = true;

        Time.timeScale = 0;
        playerController.SetActive(false);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);

        pauseMenu.SetActive(true);
    }

    public void ClosePause()
    {
        inPauseMenu = false;

        player._inPause = false;

        Time.timeScale = 1;
        playerController.SetActive(true);

        pauseMenu.SetActive(false);
    }

    public void PlayGame()
    {
        //Destroy(GameObject.Find("DontDestroy"));

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.name == "DontDestroy")
            {
                Destroy(obj);
            }
        }

        if (SpawnData.Instance != null)
        {
            SpawnData.Instance.enemies.Clear();
            SpawnData.Instance.items.Clear();
            SpawnData.Instance.npcs.Clear();
        }
        Collected.currencyValue = 0;
        Collected.flowerValue = 0;
        SceneManager.LoadScene("Tutorial");
        //MusicManager.instance.SetMusicTrack(1);
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
        //Destroy(GameObject.Find("DontDestroy"));

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.name == "DontDestroy")
            {
                Destroy(obj);
            }
        }

        Time.timeScale = 1;
    }
}
