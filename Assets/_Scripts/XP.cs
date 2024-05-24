using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    public float followSpeed = 2f;
    public float range = 1f;
    private Transform playerTransform;
    private bool isFollowing = false;

    private void Awake()
    {
        Character character = FindObjectOfType<Character>();
        playerTransform = character.transform;
    }
    void Start()
    {
        StartCoroutine(StartFollowing());
    }

    void Update()
    {
        if (isFollowing)
        {
            float step = followSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < range && isFollowing)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator StartFollowing()
    {
        yield return new WaitForSeconds(1f);
        isFollowing = true;
    }
}
