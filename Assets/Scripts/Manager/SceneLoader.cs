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
        Shop
    }

    [SerializeField] private SceneNames loadScene;

    [SerializeField] private bool updatePlayerData = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var temp = collision.GetComponent<PlayerController>();

            temp._canAttack = true;

            if (updatePlayerData)
            {
                PlayerData.Instance.health = temp.health;
                PlayerData.Instance.coolCost = temp.coolCost;
                PlayerData.Instance.currency = temp.currency;
                PlayerData.Instance.flowers = temp.flowers;
                PlayerData.Instance._canAttack = temp._canAttack;
            }

            PlayerData.Instance.lastScene = SceneManager.GetActiveScene().name;

            if (loadScene.ToString() == "Shop")
            {
                temp._canAttack = false;
            }

            SceneManager.LoadScene(loadScene.ToString());
        }
    }
}
