using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private string dialogueToSay;
    [SerializeField] private TMP_Text textElement;
    [SerializeField] private float speed;

    [SerializeField] private bool isDropping;
    [SerializeField] private GameObject dropVisual;

    public bool isSpeedUp = false;

    private void Start()
    {
        Run(dialogueToSay, textElement);
    }

    private void Update()
    {
        if (!isSpeedUp && InputManager.isInteractTriggered)
        {
            speed = 50;
        }
    }

    public void Run(string textToType, TMP_Text textLabel)
    {
        StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.unscaledDeltaTime * speed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        textLabel.text = textToType;

        isSpeedUp = true;

        if (isDropping)
        {
            dropVisual.SetActive(true);
        }
    }
}
