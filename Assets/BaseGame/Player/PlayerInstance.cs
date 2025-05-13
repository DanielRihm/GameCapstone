using Assets.BaseGame.Scripts.Engine.Data;
using LCPS.SlipForge;
using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Player;
using LCPS.SlipForge.Weapon;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerInstance : PersistantSingleton<PlayerInstance>
{
    public PlayerSettingsData PlayerData;

    public WeaponRig WeaponRig { get; set; }
    public DirectionEnum.Direction Facing;

    private int _invincibility = 0;

    private DataTracker _dataTracker;

    private Observable<ActiveData> _activeObservable;
    private Observable<WeaponData> _leftWeaponObservable;
    private Observable<WeaponData> _rightWeaponObservable;
    private Observable<int> _healthObservable;
    private Observable<int> _maxHealthObservable;

    protected override void OnAwake()
    {
        WeaponRig = GetComponentInChildren<WeaponRig>();

        _activeObservable = DataTracker.GetObservable(data => data.SaveData.Weapons.ActiveItem);
        _leftWeaponObservable = DataTracker.GetObservable(data => data.SaveData.Weapons.LeftWeapon);
        _rightWeaponObservable = DataTracker.GetObservable(data => data.SaveData.Weapons.RightWeapon);
        _healthObservable = DataTracker.GetObservable(data => data.CurrentHealth);
        _maxHealthObservable = DataTracker.GetObservable(data => data.MaxHealth);
        _dataTracker = DataTracker.Instance;

        Assert.IsNotNull(WeaponRig, $"{name} : WeaponRig not found");

        DataTracker.Instance.LoadTrackerData();
    }

    public void Update()
    {
        if (_invincibility > 0)
        {
            _invincibility--;
        }

    }

    public void GotHit()
    {
        if (_invincibility == 0)
        {
            Hurt();
            _invincibility = 300;
        }
    }

    public void LevelUp(int statType)
    {

        //TODO: Figure out what we want to do here

        /*Stats[statType] = Stats[statType]++;

        if (statType == 0)
        {
            //"Body"
            MaxHealth++;
            PlayerAdditionalSpeed += 25;
        }
        else if (statType == 1)
        {
            //"Soul"
            MagicCDReduction += 25;
        }
        else
        {
            //"Mind"
            DamageMultiplier += 25;
        }*/

    }

    public void Hurt()
    {
        Debug.Log("Player got hit");
        _healthObservable.Value--;

        if (_healthObservable.Value == 0)
        {
            SceneManager.ExitDungeon();
            _dataTracker.LeaveDungeonEvent(true);
        }
    }

    public void Heal(int amount)
    {
        _healthObservable.Value = Math.Min(_healthObservable.Value + amount, _maxHealthObservable.Value);
    }

    public void PickUpWeapon(WeaponData data, PlayerHand h)
    {
        if (h.Equals(PlayerHand.Left))
        {
            _leftWeaponObservable.Value = data;
        }
        else
        {
            _rightWeaponObservable.Value = data;
        }
    }

    public void Drop(Observable<WeaponData> data)
    {
        var left = DataTracker.GetObservable(data => data.SaveData.Weapons.LeftWeapon);
        var right = DataTracker.GetObservable(data => data.SaveData.Weapons.RightWeapon);
        bool dropBoth = data.Value.IsHeavy;
        if (left == data || dropBoth)
        {
            WeaponRig.CreateWeaponPickup(left.Value, new Vector3(-2, 1, 1.25f));
            _leftWeaponObservable.Value = null;
        }
        if (right == data || dropBoth)
        {
            WeaponRig.CreateWeaponPickup(right.Value, new Vector3(1, 1, 1.25f));
            _rightWeaponObservable.Value = null;
        }
    }

    public void Drop(ActiveData data, Vector3 offest)
    {
        WeaponRig.CreateActivePickup(data, offest);
    }

    public void PickUpActive(ActiveData data)
    {
        _activeObservable.Value = data;
    }

}
