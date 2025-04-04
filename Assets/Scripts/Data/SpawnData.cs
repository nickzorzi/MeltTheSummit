using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour
{
    public static SpawnData Instance { get; private set; }

    [Header("Spawn List")]
    public List<EnemyData> enemies;

    [Header("Spawn List")]
    public List<ItemData> items;

    [Header("NPC List")]
    public List<NPCData> npcs;

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

    public void AddItem(GameObject item, int id, bool dead)
    {
        ItemData newItemData = new ItemData(item, id, dead);
        items.Add(newItemData);
    }

    public void RemoveItem(int id)
    {
        ItemData itemToRemove = items.Find(e => e.id == id);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
        }
    }

    public List<ItemData> GetItems()
    {
        return items;
    }

    [System.Serializable]
    public class ItemData
    {
        public GameObject item;
        public int id;
        public bool dead;

        public ItemData(GameObject item, int id, bool dead)
        {
            this.item = item;
            this.id = id;
            this.dead = dead;
        }
    }

    public void AddNPC(GameObject npc, int id, bool spokenTo)
    {
        NPCData newNPCData = new NPCData(npc, id, spokenTo);
        npcs.Add(newNPCData);
    }

    public void RemoveNPC(int id)
    {
        NPCData npcToRemove = npcs.Find(e => e.id == id);
        if (npcToRemove != null)
        {
            npcs.Remove(npcToRemove);
        }
    }

    public List<NPCData> GetNPCs()
    {
        return npcs;
    }

    [System.Serializable]
    public class NPCData
    {
        public GameObject npc;
        public int id;
        public bool spokenTo;

        public NPCData(GameObject npc, int id, bool spokenTo)
        {
            this.npc = npc;
            this.id = id;
            this.spokenTo = spokenTo;
        }
    }
}
