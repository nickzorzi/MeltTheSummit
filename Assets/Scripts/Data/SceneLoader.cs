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

            SceneManager.LoadScene(loadScene.ToString());
        }
    }
}
