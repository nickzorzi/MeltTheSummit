using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{

    public static PlayerData Instance { get; private set; }

    [Header("Scene Info")]
    public string lastScene;

    [Header("Player Info")]
    //public float _moveSpeed;
    public float health;
    //public float maxHealth;
    //public float temp;
    //public float maxTemp;
    //public int heatCost;
    public int coolCost;
    //public int burn;
    public bool _canAttack;
    //public bool _isAttacking;
    //public bool _isTransformed;
    //public bool _isBurning;
    public int currency;
    public int flowers;

    private void Awake()
    {
        // Singleton Paradox Killer
        #region SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Duplicate PlayerData Detected -- Deleting Duplicate...");
            Destroy(gameObject);
        }
        #endregion
    }
}
