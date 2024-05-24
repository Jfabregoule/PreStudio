using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    private Transform playerTransform;
    private Rigidbody rb;
    private bool shouldFloat = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    public float floatHeight = 1.0f;
    public float floatSpeed = 1.0f;
    public LayerMask groundLayer;

    private void Awake()
    {
        Character character = FindObjectOfType<Character>();
        playerTransform = character.transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (shouldFloat == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f, groundLayer))
            {
                shouldFloat = true;
                startPosition = transform.position;
                targetPosition = startPosition + Vector3.up * floatHeight;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                StartCoroutine(RotateToIdentity());
            }
        }

        if (shouldFloat == false) return;

        transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.PingPong(Time.time * floatSpeed, 1f));
        transform.Rotate(0.0f, 0.2f, 0.0f);
    }

    public void Interact()
    {
        Destroy(gameObject);
    }

    private IEnumerator RotateToIdentity()
    {
        Quaternion initialRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.identity;
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRotation;
    }
}
