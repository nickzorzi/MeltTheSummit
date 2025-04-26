using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDelay : MonoBehaviour
{
    [SerializeField] private GameObject button;
    void Start()
    {
        StartCoroutine(DelayedActivate());
    }

    public IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(1);

        button.SetActive(true);

        EventSystem.current.SetSelectedGameObject(button.transform.GetChild(0).gameObject);
    }
}
