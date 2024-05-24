using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ChestXP : MonoBehaviour, IInteractable
{
    public GameObject xpBallPrefab;
    public float minValue;
    public float maxValue;
    public void Interact()
    {
        SpawnXP();
        gameObject.SetActive(false);
    }

    private void SpawnXP()
    {
        float randomValue = UnityEngine.Random.Range(minValue, maxValue);
        for (int i = 0; i < randomValue; i++)
        {
            float randomSize = UnityEngine.Random.Range(0.5f, 2f);

            GameObject xpBall = Instantiate(xpBallPrefab, transform.position + Vector3.up, Quaternion.identity);
            xpBall.transform.localScale = xpBall.transform.localScale * randomSize;
            Rigidbody xpBallRB = xpBall.GetComponent<Rigidbody>();

            float randomX = UnityEngine.Random.Range(-5f, 5f);
            float randomZ = UnityEngine.Random.Range(-5f, 5f);

            Vector3 randomForce = new Vector3(randomX, 3f, randomZ);
            xpBallRB.AddForce(randomForce, ForceMode.Impulse);
        }
    }
}
