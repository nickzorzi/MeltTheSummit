using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SFXSeq : MonoBehaviour
{
    [SerializeField] private AudioClip menuSFX;
    void Start()
    {
        StartCoroutine(MenuSFX());
    }

    public IEnumerator MenuSFX()
    {
        yield return new WaitForSeconds(1.9f);
        SoundFXManager.instance.PlaySoundClip(menuSFX, transform, 1f);
        yield return null;
    }
}
