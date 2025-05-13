using Assets.BaseGame.Guns.Scripts;
using Assets.BaseGame.Scripts.Engine.Data;
using LCPS.SlipForge.Engine;
using LCPS.SlipForge.UI;
using LCPS.SlipForge.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace LCPS.SlipForge
{
    public class DataTracker : PersistantSingleton<DataTracker>
    {
        [Header("Observable Data")]
        [Space(10)]
        public DataTrackerSaveData SaveData = new();        

        //Player inventory
        public Observable<WeaponData> LeftWeapon => SaveData.Weapons.LeftWeapon;
        public Observable<WeaponData> RightWeapon => SaveData.Weapons.RightWeapon;
        public Observable<ActiveData> ActiveItem => SaveData.Weapons.ActiveItem;
        public Observable<int> CurrentHealth => SaveData.HP;
        public Observable<int> MaxHealth => SaveData.Stats.MaxHP;

        [Header("Runtime Observable Data")]
        [Space(10)]
        public Observable<int> LeftWeaponAmmoCount = new();
        public Observable<int> RightWeaponAmmoCount = new();
        public Observable<bool> LeftWeaponReloading = new();
        public Observable<bool> RightWeaponReloading = new();
        public Observable<float> ActiveItemCooldown = new();


        [Header("Trigger Save")]
        public Observable<bool> Save = new();
        [Header("Trigger Load")]
        public Observable<bool> Load = new();

        [Header("Boring Not-Observable Data")]
        [Space(10)]
        public TriggerActionInvoker ActiveTrigger = null;
        public readonly Stack<IMenu> MenuStack = new(); // this stack should only be popped by IsMenuOpen()
        public int Invincibility = 0;


        //Player stats
        public int MagicCDReduction;
        public int PlayerAdditionalSpeed;
        public int DamageMultiplier;
        public int[] Stats = new int[3];


        public InventoryItem[] StorageItems;

        public InventoryItem[] InventoryItems;

        //Dungeon tracking
        public Observable<bool> IsDungeon = new();
        public int DungeonSeed;
        public int DungeonSeedTaps;
        public int DungeonLevel = -1;

        //Game state tracking
        public event Action<InputDevice> OnDeviceChange;
        public bool IsKeyboardAndMouse { get; private set; } = false;

        protected override void OnAwake()
        {
            DontDestroyOnLoad(this);

        }


        private void Start()
        {
            // Some Observable examples
            // These lambda examples are only a concie example, you cannot cleanup the subscription this way

            LeftWeapon.OnValueChanged += (value) => Debug.Log($"LeftWeapon changed to {value}");

            InputSystem.onEvent += OnAnyInput;
        }

        public void LoadTrackerData()
        {
 
            PlayerSaveData.LoadData(SaveData);
        }

        public void SaveTrackerData()
        {
            PlayerSaveData.SaveData(SaveData);
        }

        //Gets called when the player leaves the dungeon. Calls for a boolean for if the player died
        public void LeaveDungeonEvent(bool died)
        {

            CurrentHealth.Value = MaxHealth.Value;

            // TODO: It will be up to the player to clear the inventory if leaving the dungeon or chaning floors 
            //LeftWeapon = null;
            //RightWeapon = null;

            if (died)
            {
                Instance.SaveData.Currency.PotatoChips.Value = Instance.SaveData.Currency.PotatoChips.Value / 2;
            }


            SetObservableProperty(data => data.SaveData.Weapons.LeftWeapon, null);
            SetObservableProperty(data => data.SaveData.Weapons.RightWeapon, null);
            SetObservableProperty(data => data.SaveData.Weapons.ActiveItem, null);

            SaveTrackerData();
        }

        //Finds the number of items of a type in one of the inventories. Calls for a boolean on whether you're looking in the storage or active dungeon inventory
        //Returns -1 if not found
        public int FindItemAmount(bool storage, string itemName)
        {
            if (storage)
            {
                foreach (var item in StorageItems)
                {
                    if (item.item == itemName)
                    {
                        return item.amount;
                    }
                }
            }
            else
            {
                foreach (var item in InventoryItems)
                {
                    if (item.item == itemName)
                    {
                        return item.amount;
                    }
                }
            }
            return -1;
        }

        private void OnDestroy()
        {
            InputSystem.onEvent -= OnAnyInput;
        }

        public void InteractionWithActiveTrigger()
        {
            if (ActiveTrigger == null) return;
            if (this.IsMenuOpen()) return;
            ActiveTrigger.Interact();
        }

        private void OnAnyInput(InputEventPtr eventPtr, InputDevice device)
        {
            // return if the device has not changed
            bool currentDeviceIsKeyboard = device.name == "Keyboard" || device.name == "Mouse";
            if (IsKeyboardAndMouse == currentDeviceIsKeyboard) return;

            // return if there are no changed controls
            // this is done to prevent the double input TEXT bug with eventPtr.EnumerateChangedControls()
            if (!currentDeviceIsKeyboard)
            {
                // this has to be done because for some reason InputSystem.onEvent is called even when unregistered inputs are used (i.e. a controller gyro)
                if (CountChangedControls(eventPtr) == 0) return;
            }

            // device has changed
            Debug.Log("Device changed: " + device.name);
            IsKeyboardAndMouse = currentDeviceIsKeyboard;
            OnDeviceChange?.Invoke(device);
        }

        private int CountChangedControls(InputEventPtr eventPtr)
        {
            int eventCount = 0;
            // iterating over the eventPtr.EnumerateChangedControls() collection causes a crash if the input includes a keyboard text input
            foreach (var _ in eventPtr.EnumerateChangedControls())
            {
                eventCount++;
            }
            return eventCount;
        }

        public static void Subscribe<T>(Func<DataTracker, Observable<T>> observerSelector, Action<T> observer)
        {
            if (Instance == null) return;

            observerSelector
                .Invoke(Instance)
                .Subscribe(observer);
        }

        public static void Subscribe<T>(Func<DataTracker, Observable<T>> observerSelector, Action<Observable<T>> observerRef)
        {
            if (Instance == null) return;

            observerSelector
                .Invoke(Instance)
                .Subscribe(observerRef);
        }

        public static void Subscribe<T>(Func<DataTracker, ObservableList<T>> observerSelector, Action<int?, List<T>> observer)
        {
            if (Instance == null) return;

            observerSelector
                .Invoke(Instance)
                .Subscribe(observer);
        }

        public static void UnSubscribe<T>(Func<DataTracker, Observable<T>> observerSelector, Action<T> observer)
        {
            if (Instance == null) return;

            observerSelector
                .Invoke(Instance)
                .UnSubscribe(observer);
        }

        public static void UnSubscribe<T>(Func<DataTracker, Observable<T>> observerSelector, Action<Observable<T>> observerRef)
        {
            if (Instance == null) return;

            observerSelector
                .Invoke(Instance)
                .UnSubscribe(observerRef);
        }

        public static void UnSubscribe<T>(Func<DataTracker, ObservableList<T>> observerSelector, Action<int?, List<T>> observer)
        {
            if (Instance == null) return;

            observerSelector
                .Invoke(Instance)
                .UnSubscribe(observer);
        }

        public static Observable<T> GetObservable<T>(Func<DataTracker, Observable<T>> observerSelector)
        {
            if (Instance == null) return default;
            
            return observerSelector.Invoke(Instance);
        }

        public static T GetObservableProperty<T>(Func<DataTracker, Observable<T>> observerSelector)
        {
            if (Instance == null) return default;
            
            return observerSelector.Invoke(Instance).Value;
        }

        public static void SetObservableProperty<T>(Func<DataTracker, Observable<T>> observerSelector, T value)
        {
            if (Instance == null) return;

            observerSelector.Invoke(Instance).Value = value;
        }
    }
}
