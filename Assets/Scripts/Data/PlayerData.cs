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
    public float temp;
    //public int heatCost;
    public int coolCost;
    //public int burn;
    public bool _canAttack;
    //public bool _isAttacking;
    public bool _isTransformed;
    public bool _isBurning;
    public int currency;
    public int flowers;
    public bool _canAbility;
    public float abilityCooldown;

    private void Awake()
    {
        #region SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
