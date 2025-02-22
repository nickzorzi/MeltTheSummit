using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (transform.childCount == 0)
        {
            Destroy(this); // clear empty storages
        }
    }
}
