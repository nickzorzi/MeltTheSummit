using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SceneNames
    {
        Test,
        Test2,
        Tutorial,
        Kingdom,
        Town,
        Forest,
        Snow,
        Mountain,
        Summit,
        Puzzle,
        Shop,
        Cave,
        Tutorial2,
        Tutorial3,
        Tutorial4,
        Kingdom1,
        Kingdom2,
        Town1,
        Town2,
        Town3,
        Town4,
        Town5,
        Town6,
        House1,
        House2,
        House3,
        Forest1,
        Forest2,
        Forest3,
        Forest4,
        Forest5,
        Forest6,
        Forest7,
        Forest8,
        Snow1,
        Snow2,
        Snow3,
        ShopLodge,
        Mountain1,
        Mountain2,
        Mountain3,
        Mountain4,
        Mountain5,
        Mountain6,
        Mountain7,
        Mountain8,
        Puzzle1,
        Puzzle2,
        Puzzle3,
        Cave1,
        Cave2,
        Cave3,
        Hike,
        SummitRuins
    }

    [SerializeField] private SceneNames loadScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerData.Instance.lastScene = SceneManager.GetActiveScene().name;

            TrackChange();

            SceneManager.LoadScene(loadScene.ToString());
        }
    }

    public void TrackChange()
    {
        if (PlayerData.Instance.lastScene == "Tutorial4" && loadScene == SceneNames.Kingdom1)
        {
            MusicManager.instance.SetMusicTrack(0);
        }

        if (PlayerData.Instance.lastScene == "Forest4" && loadScene == SceneNames.Snow1)
        {
            MusicManager.instance.SetMusicTrack(1);
        }

        if (PlayerData.Instance.lastScene == "Snow3" && loadScene == SceneNames.Mountain1 || PlayerData.Instance.lastScene == "Cave1" && loadScene == SceneNames.Mountain2 || PlayerData.Instance.lastScene == "Cave2" && loadScene == SceneNames.Mountain4 || PlayerData.Instance.lastScene == "Cave3" && loadScene == SceneNames.Mountain6 || PlayerData.Instance.lastScene == "Puzzle1" && loadScene == SceneNames.Mountain2 || PlayerData.Instance.lastScene == "Puzzle2" && loadScene == SceneNames.Mountain4 || PlayerData.Instance.lastScene == "Puzzle3" && loadScene == SceneNames.Mountain6)
        {
            MusicManager.instance.SetMusicTrack(2);
        }

        if (PlayerData.Instance.lastScene == "Mountain2" && loadScene == SceneNames.Puzzle1 || PlayerData.Instance.lastScene == "Mountain4" && loadScene == SceneNames.Puzzle2 || PlayerData.Instance.lastScene == "Mountain6" && loadScene == SceneNames.Puzzle3)
        {
            MusicManager.instance.SetMusicTrack(3);
        }

        if (PlayerData.Instance.lastScene == "Hike" && loadScene == SceneNames.SummitRuins)
        {
            MusicManager.instance.SetMusicTrack(4);
        }
    }
}
