using LCPS.SlipForge.Engine;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponLog : PersistantSingleton<WeaponLog>
{
    public GameObject[] ItemList;
    public GameObject[] OneHandedWeaponList;
    public GameObject[] TwoHandedWeaponList;
    protected override void OnAwake()
    {
        Assert.IsTrue(ItemList != null && OneHandedWeaponList != null && TwoHandedWeaponList != null);
    }

    public GameObject[] GetWeapons()
    {
        GameObject[] returnObject;
        System.Random rand = new System.Random();
        int dropType = rand.Next(0, 3);
        int item;

        switch (dropType)
        {
            case 0:
                item = rand.Next(0, OneHandedWeaponList.Length);
                returnObject = new GameObject[2];
                returnObject[0] = OneHandedWeaponList[item];
                item = rand.Next(0, OneHandedWeaponList.Length);
                returnObject[1] = OneHandedWeaponList[item];
                break;
            case 1:
                item = rand.Next(0, TwoHandedWeaponList.Length);
                returnObject = new GameObject[1];
                returnObject[0] = TwoHandedWeaponList[item];
                break;
            case 2:
                item = rand.Next(0, ItemList.Length);
                returnObject = new GameObject[1];
                returnObject[0] = ItemList[item];
                break;
            default:
                returnObject = new GameObject[0];
                break;

        }

        return returnObject;
    }
}
