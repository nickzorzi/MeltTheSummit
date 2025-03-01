using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour
{
    public static SpawnData Instance { get; private set; }

    [Header("Spawn List")]
    public List<EnemyData> enemies;

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
        EnemyData newEnemyData = new EnemyData(enemy, id, dead);
        enemies.Add(newEnemyData);
    }

    public void RemoveEnemy(int id)
    {
        EnemyData enemyToRemove = enemies.Find(e => e.id == id);
        if (enemyToRemove != null)
        {
            enemies.Remove(enemyToRemove);
        }
    }

    public List<EnemyData> GetEnemies()
    {
        return enemies;
    }

    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemy;
        public int id;
        public bool dead;

        public EnemyData(GameObject enemy, int id, bool dead)
        {
            this.enemy = enemy;
            this.id = id;
            this.dead = dead;
        }
    }
}
