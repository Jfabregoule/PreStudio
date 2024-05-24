using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("PRESSED");
    }
}
