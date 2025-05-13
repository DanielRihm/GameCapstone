using LCPS.SlipForge.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LootTable")]
public class LootTable : ScriptableObject
{
    public List<WeaponData> weapons;
	public int[] weights = {35, 30, 15, 10, 5}; //defaults
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
