using LCPS.SlipForge;
using LCPS.SlipForge.CustomRand;
using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : PersistantSingleton<DungeonManager>
{
    /**
     * <summary>
     * <see cref="OnLevelChange"/> is invoked when the dungeon level changes and is called before <see cref="CurrentRoomChanged"/><para/>
     * </summary>
     */
    public static event Action<int> OnLevelChange;
    public static event Action<Vector2> CurrentRoomChanged;

    public int TreasureRooms = 2;
    public int ShopRooms = 0;

    public GameObject[] CombatRoomPrefabs;
    public GameObject[] TreasureRoomPrefabs;
    public GameObject[] ShopRoomPrefabs;
    public GameObject[] StartRoomPrefabs;
    public GameObject[] FirstRoomPrefabs;
    public GameObject[] BossRoomPrefabs;
    public GameObject[] ExitRoomPrefabs;

    public GameObject[] Doors;
    public GameObject[] BossDoors;
    public GameObject LeaveDoor;
    public GameObject NextDoor;

    public GameObject[] EasySpawners;
    public GameObject[] BossSpawners;

    public GameObject DungeonParent;

    private Dictionary<Vector2, int> _enemiesInRoom;
    private Dictionary<Vector2, List<GameObject>> _enemiesInRoomObj;
    private List<Vector2> _spawnerRooms;

    private LevelLoader _levelLoader;
    private DataTracker _dataTracker;
    private readonly Observable<Vector2> _currentRoom = new();

    protected override void OnAwake()
    {
        _levelLoader = new LevelLoader();
        _levelLoader.Instantiate(this);

        _dataTracker = DataTracker.Instance;
        _enemiesInRoom = new Dictionary<Vector2, int>();
        _enemiesInRoomObj = new Dictionary<Vector2, List<GameObject>>();

        _spawnerRooms = new List<Vector2>();

        if (_dataTracker.DungeonLevel == -1)
        {
            StartDungeon();
        }
        else
        {
            CustomRandom _rand = new(_dataTracker.DungeonSeed);

            for (int i = 0; i < _dataTracker.DungeonSeedTaps; i++)
            {
                _rand.Next();
            }

            NextLevel(_rand);
        }

        // Wrapping the observable in an event to protect it from being set from outside
        // Shouldn't need to unsubscribe from this event OnDestroy because the observable is a member of this class:
        // both will be destroyed at the same time
        _currentRoom.Subscribe((room) =>
        {
            CurrentRoomChanged?.Invoke(room);
        });
    }

    public void RegisterSpawner(Vector2 room)
    {
        _spawnerRooms.Add(room);
        Debug.Log("Registered Room: " + room);
    }

    public void RegisterEnemy(Vector2 room, GameObject enemy)
    {
        if (!_enemiesInRoom.ContainsKey(room))
        {
            _enemiesInRoom.Add(room, 0);
        }
        _enemiesInRoom[room]++;

        if (!_enemiesInRoomObj.ContainsKey(room))
        {
            _enemiesInRoomObj.Add(room, new List<GameObject>());
        }
        _enemiesInRoomObj[room].Add(enemy);

        enemy.SetActive(false);
        //UnloadPrevious();
    }

    public void EnemyKilled(Vector2 room)
    {
        _enemiesInRoom[room]--;
        if (_enemiesInRoom[room] == 0)
        {
            _levelLoader.EnableDoors(room);
            if (_levelLoader.EndRoom == room && DataTracker.Instance.DungeonLevel == 3)
            {
                SoundManager.Instance.PlaySFX("positive_sound");
            }
        }
    }

    public void SwitchRoom(Vector2 room)
    {
        _levelLoader._floorRooms[room].SetActive(true);
        _currentRoom.Value = room;
        _levelLoader.DisableDoors(room);

        if (_enemiesInRoomObj.ContainsKey(room)){
            if (_enemiesInRoom[room] != 0)
            {
                foreach (var enemyObj in _enemiesInRoomObj[room])
                {
                    enemyObj.SetActive(true);
                }
            }
        }

        if (!_spawnerRooms.Contains(room))
        {
            _levelLoader.EnableDoors(room);
        }
        else
        {
            if (_enemiesInRoom.ContainsKey(room))
            {
                if (_enemiesInRoom[room] == 0)
                {
                    _levelLoader.EnableDoors(room);
                }
            }
        }
        /*if (!_enemiesInRoom.ContainsKey(room))
        {
            _levelLoader.EnableDoors(room);
        }
        else if (_enemiesInRoom[room] == 0)
        {
            _levelLoader.EnableDoors(room);
        }
        else
        {
            _levelLoader.DisableDoors(room);
        }*/
    }

    //public void UnloadPrevious()
    //{
    //    _levelLoader._floorRooms[_prevRoom].SetActive(false);
    //}

    private void StartDungeon()
    {
        System.Random _totallyRand = new();
        int _tempSeed = _totallyRand.Next();
        CustomRandom _rand = new(_tempSeed);

        _dataTracker.DungeonSeed = _tempSeed;
        _dataTracker.DungeonLevel = 0;

        NextLevel(_rand);
    }

    private void NextLevel(CustomRandom _rand)
    {
        _dataTracker.DungeonLevel++;
        _levelLoader.GenerateLevel(_rand, _dataTracker.DungeonLevel);
        OnLevelChange?.Invoke(_dataTracker.DungeonLevel); // has to be before current room updates and after the level is generated
        _currentRoom.Value = new Vector2(2, 2);
        _levelLoader._floorRooms[_currentRoom.Value].SetActive(true);
        _dataTracker.DungeonSeedTaps = _rand.GetTaps();
    }

    /**
     * <summary>
     * Returns a tuple containing a dictionary of room types and a list of paths between rooms<para/>
     * Rooms exist between(0, 0) and(4, 4) inclusive<para/>
     * Paths are represented as a tuple of two vectors, the first being the destination room and the second being the source room
     * </summary>
     */
    public (Dictionary<Vector2, RoomType>, List<(Vector2 to, Vector2 from)>) GetRooms()
    {
        return _levelLoader.GetRooms();
    }
}
