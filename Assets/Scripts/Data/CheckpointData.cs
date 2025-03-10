using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointData : MonoBehaviour
{
    public static CheckpointData Instance { get; private set; }

    [Header("Enemy List")]
    public List<CheckpointEnemyData> checkpointEnemies;

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

    public void AddEnemy(GameObject enemy, int id, bool dead)
    {
        CheckpointEnemyData newCheckpointEnemyData = new CheckpointEnemyData(enemy, id, dead);
        checkpointEnemies.Add(newCheckpointEnemyData);
    }

    public void RemoveEnemy(int id)
    {
        CheckpointEnemyData checkpointEnemyToRemove = checkpointEnemies.Find(e => e.id == id);
        if (checkpointEnemyToRemove != null)
        {
            checkpointEnemies.Remove(checkpointEnemyToRemove);
        }
    }

    public List<CheckpointEnemyData> GetEnemies()
    {
        return checkpointEnemies;
    }

    [System.Serializable]
    public class CheckpointEnemyData
    {
        public GameObject enemy;
        public int id;
        public bool dead;

        public CheckpointEnemyData(GameObject enemy, int id, bool dead)
        {
            this.enemy = enemy;
            this.id = id;
            this.dead = dead;
        }
    }
}
