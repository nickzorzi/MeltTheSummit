using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
