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
        Kingdom,
        Town,
        Forest,
        Snow,
        Mountain,
        Summit,
        Puzzle,
        Shop
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
