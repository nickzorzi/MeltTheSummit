using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAssign : MonoBehaviour
{
    [SerializeField] private GameObject button;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}
