using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLootTable", menuName = "Loot Table")]
public class LootTable : ScriptableObject
{
    public List<LootItem> lootItems;

    public List<GameObject> GetRandomLoot()
    {
        List<GameObject> list = new List<GameObject>();

        foreach (LootItem item in lootItems)
        {
            float randomValue = Random.Range(0, 100);
            if (randomValue <= item.dropChance)
            {
                list.Add(item.itemPrefab);
            }
        }
        return list;
    }
}
