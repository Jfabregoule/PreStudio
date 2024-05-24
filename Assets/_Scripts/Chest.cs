using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public LootTable lootTable;
    public Transform dropPoint;

    public void Interact()
    {
        DropLoot();
        gameObject.SetActive(false);
    }

    private void DropLoot()
    {
        List<GameObject> loots = lootTable.GetRandomLoot();
        for (int i = 0; i < loots.Count; i++)
        {
            GameObject loot = Instantiate(loots[i], dropPoint.position, Quaternion.identity);
            Rigidbody lootRB = loot.GetComponent<Rigidbody>();

            float randomX = UnityEngine.Random.Range(-5f, 5f);
            float randomZ = UnityEngine.Random.Range(-5f, 5f);

            Vector3 randomForce = new Vector3(randomX, 5f, randomZ);
            lootRB.AddForce(randomForce, ForceMode.Impulse);

            float randomTorqueX = UnityEngine.Random.Range(-0.02f, 0.02f);
            float randomTorqueZ = UnityEngine.Random.Range(-0.02f, 0.02f);
            lootRB.AddTorque(new Vector3(randomTorqueX, 0, randomTorqueZ), ForceMode.Impulse);
        }
    }
}
