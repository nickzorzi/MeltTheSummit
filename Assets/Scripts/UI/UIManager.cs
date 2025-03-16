using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject abilityUI;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (playerController._hasAbility && !abilityUI.activeSelf)
        {
            abilityUI.SetActive(true);
        }
    }
}
