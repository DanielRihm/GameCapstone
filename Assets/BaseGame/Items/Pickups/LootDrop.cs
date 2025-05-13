using LCPS.SlipForge.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootDrop
{
	// Start is called before the first frame update
	static ItemPickup Roll(LootTable LootTable)
	{
		int[] weights = LootTable.weights;
		int tier;
		int max = weights[0] + weights[1] + weights[2] + weights[3] + weights[4];
		int rng = Random.Range(1, max);
		if (rng <= weights[0])
		{
			tier = 1;
		}
		else if (rng <= weights[0] + weights[1])
		{
			tier = 2;
		}
		else if (rng <= weights[0] + weights[1] + weights[2])
		{
			tier = 3;
		}
		else if (rng <= weights[0] + weights[1] + weights[2] + weights[3])
		{
			tier = 4;
		}
		else if (rng <= weights[0] + weights[1] + weights[2] + weights[3] + weights[4])
		{
			tier = 5;
		}
		//possibly select random item from list, then only return it if it of the correct tier
		return new ItemPickup();
	}
}
