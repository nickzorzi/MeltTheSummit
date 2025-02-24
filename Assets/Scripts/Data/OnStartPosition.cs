using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnStartPosition : MonoBehaviour
{

    [SerializeField] private Transform player;
    [Space(10)]
    [SerializeField] private telePos[] positions;
    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Missing Player Transform in " + this.name);
        }
        foreach (telePos posDat in positions)
        {
            if (PlayerData.Instance.lastScene == posDat.comingFrom)
            {
                player.position = posDat.position.position;
                break;
            }
        }
    }


    [System.Serializable]

    public struct telePos
    {
        public Transform position;
        public string comingFrom;
    }

}
