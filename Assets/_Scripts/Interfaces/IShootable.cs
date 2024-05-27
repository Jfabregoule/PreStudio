using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    float Damage { get; set; }
    float FireRate { get; set; }
    float ReloadingTime { get; set; }
    int AmmoNum { get; set; }
    int CurrentAmmos { get; set; }
    GameObject ProjectilePrefab { get; set; }

    void Shoot(float amount);
    
    void Reload();
}
