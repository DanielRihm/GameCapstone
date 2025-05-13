using LCPS.SlipForge;
using LCPS.SlipForge.CustomRand;
using LCPS.SlipForge.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelLoader
{
    public Vector2 EndRoom;

    private DungeonManager _dungeonManager;

    private int _totalRooms;
    private int _roomDistance = 75;
    private DirectionEnum _directionEnum;

    private Dictionary<Vector2, List<GameObject>> _doors;
    private Dictionary<Vector2, RoomType> _savedRooms;
    private List<(Vector2 to, Vector2 from)> _paths;

    public Dictionary<Vector2, GameObject> _floorRooms;


    public void Instantiate(DungeonManager dungeonManager)
    {
        _dungeonManager = dungeonManager;
        _directionEnum = new DirectionEnum();
        _doors = new Dictionary<Vector2, List<GameObject>>();
        _paths = new List<(Vector2 to, Vector2 from)>();
        _savedRooms = new Dictionary<Vector2, RoomType>();
    }

    public void GenerateLevel(CustomRandom rand, int floor)
    {
        //Generate the tree of the rooms
        var _chosenPaths = GenerateTree(rand);

        //Create a singular Dictionary to pass in to make rooms easier
        Dictionary<Vector2, (List<DirectionEnum.Direction> doors, bool leaf)> _individualRoomData =
            new();

        //Catches all "from" rooms
        foreach (var (to, from, toward) in _chosenPaths)
        {
            var _roomKey = from;

            // If the room already has data, append to its doors
            if (_individualRoomData.ContainsKey(_roomKey))
            {
                var (doors, leaf) = _individualRoomData[_roomKey];
                doors.Add(toward);
                _individualRoomData[_roomKey] = (doors, leaf);
            }
            else
            {
                // If the room is not in _individualRoomData, add it with the first door
                List<DirectionEnum.Direction> _tempDoors = new() { toward };
                _individualRoomData.Add(_roomKey, (_tempDoors, false));
            }
            this._paths.Add((to, from));
        }

        //Catches all leaf nodes
        foreach (var (to, _, toward) in _chosenPaths)
        {
            var _roomKey = to;

            // If the room already has data, append to its doors
            if (!_individualRoomData.ContainsKey(_roomKey))
            {
                // If the room is not in _individualRoomData, add it with the first door
                List<DirectionEnum.Direction> _tempDoors = new() { toward };
                _individualRoomData.Add(_roomKey, (_tempDoors, true)); ;
            }
        }

        //Randomly assign room type to room node
        var _assignedRooms = AssignRooms(_individualRoomData, rand, floor);

        this._savedRooms = _assignedRooms;

        //Spawn rooms
        var _roomObjects = SpawnRooms(_assignedRooms, rand, floor);

        _floorRooms = _roomObjects;

        Vector2 _bossRoom = new(-1, -1);

        foreach (var _room in _assignedRooms)
        {
            if (_room.Value == RoomType.Boss || _room.Value == RoomType.MiniBoss || _room.Value == RoomType.Exit)
            {
                _bossRoom = _room.Key;
            }
        }

        Assert.IsTrue(_bossRoom != new Vector2(-1, -1), "No boss room found.");
        EndRoom = _bossRoom;


        //Spawn & Connect doors
        var _moreDoors = SpawnDoors(_roomObjects, _chosenPaths, _bossRoom, floor);

        foreach (var _another in _moreDoors)
        {
            if (!_doors.ContainsKey(_another.Key))
            {
                this._doors.Add(_another.Key, _another.Value);
            }
            else
            {
                foreach (var _anotherDoor in _another.Value)
                {
                    this._doors[_another.Key].Add(_anotherDoor);
                }
            }
        }

        //Spawn Spawners
        SpawnSpawners(_assignedRooms, _roomObjects, rand);
    }

    private List<(Vector2 to, Vector2 from, DirectionEnum.Direction toward)> GenerateTree(CustomRandom rand)
    {
        List<Vector2> _includedRooms = new();
        List<(Vector2 to, Vector2 from, DirectionEnum.Direction toward)> _potentialPaths = new();
        List<(Vector2 to, Vector2 from, DirectionEnum.Direction toward)> _chosenPaths = new();
        Vector2 _currentRoom = new(2, 2);
        int _totalRoomsLevel = rand.Next(7, 10);

        _includedRooms.Add(_currentRoom);

        //Loop for number of desired rooms in tree
        for (int i = 0; i < _totalRoomsLevel; i++)
        {
            Dictionary<Vector2, DirectionEnum.Direction> _neighbors = new()
            {
                { new Vector2(_currentRoom.x, _currentRoom.y + 1), DirectionEnum.Direction.North },
                { new Vector2(_currentRoom.x, _currentRoom.y - 1), DirectionEnum.Direction.South },
                { new Vector2(_currentRoom.x + 1, _currentRoom.y), DirectionEnum.Direction.East },
                { new Vector2(_currentRoom.x - 1, _currentRoom.y), DirectionEnum.Direction.West }
            };

            //Check if neighbors are valid for potential paths or not
            foreach (var _neighbor in _neighbors)
            {
                //Don't want loops in our dungeon path
                if (!_includedRooms.Contains(_neighbor.Key))
                {
                    //Want to be within bounds of 5 x 5 map
                    if (!(_neighbor.Key.x < 0) && !(_neighbor.Key.x > 4) && !(_neighbor.Key.y < 0) && !(_neighbor.Key.y > 4))
                    {
                        _potentialPaths.Add((_neighbor.Key, _currentRoom, _neighbor.Value));
                    }
                }
            }

            //Pick random path in _potentialPaths to continue down randomly, and add to _chosenPaths
            int _nextRoomIndex = rand.Next(0, _potentialPaths.Count);
            _chosenPaths.Add(_potentialPaths[_nextRoomIndex]);

            //Set current room
            _currentRoom = _potentialPaths[_nextRoomIndex].to;

            //Set "from" room to no longer be a leaf and add to included rooms
            _includedRooms.Add(_currentRoom);

            //Clean up
            for (int index = _potentialPaths.Count - 1; index >= 0; index--)
            {
                var (to, from, _) = _potentialPaths[index];
                if (to == _currentRoom || from == new Vector2(2, 2))
                {
                    _potentialPaths.RemoveAt(index);
                }
            }
        }

        this._totalRooms = _totalRoomsLevel;
        return (_chosenPaths);
    }

    private Dictionary<Vector2, RoomType> AssignRooms(Dictionary<Vector2, (List<DirectionEnum.Direction> doors, bool leaf)> nodes, CustomRandom rand, int level)
    {
        Dictionary<Vector2, RoomType> _assignedRooms = new();
        List<RoomType> _rooms = new();
        Vector2 _bossRoom = new();

        bool _bossRoomFound = false;
        var _nodeKeys = nodes.Keys.ToList();

        //Find a leaf nod for the boss room
        while (!_bossRoomFound)
        {
            int _randIndex = rand.Next(0, _nodeKeys.Count);
            var _nodeKey = _nodeKeys[_randIndex];

            if (nodes[_nodeKey].leaf)
            {
                _bossRoomFound = true;
                _bossRoom = _nodeKey;
            }
            else
            {
                _nodeKeys.RemoveAt(_randIndex);
            }
        }

        //Fill out all rooms we want outside of boss and start rooms
        if (_dungeonManager.ShopRooms != 0)
        {
            for (int i = 0; i < _dungeonManager.ShopRooms; i++)
            {
                _rooms.Add(RoomType.Shop);
            }
        }
        if (_dungeonManager.TreasureRooms != 0)
        {
            for (int i = 0; i < _dungeonManager.TreasureRooms; i++)
            {
                _rooms.Add(RoomType.Treasure);
            }
        }
        int _combatRooms = _totalRooms - _rooms.Count - 2;
        for (int i = 0; i <= _combatRooms; i++)
        {
            _rooms.Add(RoomType.Combat);
        }

        //Randomly assign rooms with room types
        foreach (var _node in nodes)
        {
            if (_node.Key == new Vector2(2, 2))
            {
                _assignedRooms.Add(_node.Key, RoomType.Start);
            }
            else if (_node.Key == _bossRoom && /*(level % 10 == 0)*/ level == 2)
            {
                _assignedRooms.Add(_node.Key, RoomType.Boss);
            }
            else if (_node.Key == _bossRoom && /*(level % 5 == 0)*/ level == 3)
            {
                _assignedRooms.Add(_node.Key, RoomType.MiniBoss);
            }
            else if (_node.Key == _bossRoom)
            {
                _assignedRooms.Add(_node.Key, RoomType.Exit);
            }
            else
            {
                int _randIndex = rand.Next(0, _rooms.Count);
                _assignedRooms.Add(_node.Key, _rooms[_randIndex]);
                _rooms.RemoveAt(_randIndex);
            }
        }

        return _assignedRooms;
    }

    private Dictionary<Vector2, GameObject> SpawnRooms(Dictionary<Vector2, RoomType> rooms, CustomRandom rand, int _level)
    {
        var _startRooms = _dungeonManager.StartRoomPrefabs;
        var _firstRooms = _dungeonManager.FirstRoomPrefabs;
        var _shopRooms = _dungeonManager.ShopRoomPrefabs;
        var _treasureRooms = _dungeonManager.TreasureRoomPrefabs;
        var _combatRooms = _dungeonManager.CombatRoomPrefabs;
        var _bossRooms = _dungeonManager.BossRoomPrefabs;
        var _exitRooms = _dungeonManager.ExitRoomPrefabs;

        Dictionary<Vector2, GameObject> _roomObjects = new();
        GameObject _gameObject;
        GameObject _door1 = null;
        GameObject _door2 = null;
        Vector2 _endRoom = new(-1, -1);

        //Instantiate rooms for their types
        foreach (var _room in rooms)
        {
            //bool startRoom = false;
            if (_room.Value == RoomType.Start && _level == 1)
            {
                //startRoom = true;
                int _randomIndex = rand.Next(0, _startRooms.Count());
                _gameObject = Object.Instantiate(_startRooms[_randomIndex], new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _dungeonManager.DungeonParent.transform);
            }
            else if (_room.Value == RoomType.Start)
            {
                int _randomIndex = rand.Next(0, _firstRooms.Count());
                _gameObject = Object.Instantiate(_firstRooms[_randomIndex], new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _dungeonManager.DungeonParent.transform);
            }
            else if (_room.Value == RoomType.Shop)
            {
                int _randomIndex = rand.Next(0, _shopRooms.Count());
                _gameObject = Object.Instantiate(_shopRooms[_randomIndex], new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _dungeonManager.DungeonParent.transform);
            }
            else if (_room.Value == RoomType.Treasure)
            {
                int _randomIndex = rand.Next(0, _treasureRooms.Count());
                _gameObject = Object.Instantiate(_treasureRooms[_randomIndex], new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _dungeonManager.DungeonParent.transform);
            }
            else if (_room.Value == RoomType.Combat)
            {
                int _randomIndex = rand.Next(0, _combatRooms.Count());
                _gameObject = Object.Instantiate(_combatRooms[_randomIndex], new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _dungeonManager.DungeonParent.transform);
            }
            else if (_room.Value == RoomType.Boss || _room.Value == RoomType.MiniBoss)
            {
                _endRoom = _room.Key;
                Debug.Log("Boss floor");
                int _randomIndex = rand.Next(0, _bossRooms.Count());
                _gameObject = Object.Instantiate(_bossRooms[_randomIndex], new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _dungeonManager.DungeonParent.transform);
                if (_level != 3)
                {
                    _door1 = Object.Instantiate(_dungeonManager.NextDoor, new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance + 20), Quaternion.identity, _gameObject.transform);
                }
                _door2 = Object.Instantiate(_dungeonManager.LeaveDoor, new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _gameObject.transform);
            }
            else if (_room.Value == RoomType.Exit)
            {
                _endRoom = _room.Key;
                Debug.Log("Exit floor");
                int _randomIndex = rand.Next(0, _exitRooms.Count());
                _gameObject = Object.Instantiate(_exitRooms[_randomIndex], new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance), Quaternion.identity, _dungeonManager.DungeonParent.transform);
                if (_level != 3)
                {
                    _door1 = Object.Instantiate(_dungeonManager.NextDoor, new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance + 5), Quaternion.identity, _gameObject.transform);
                }
                _door2 = Object.Instantiate(_dungeonManager.LeaveDoor, new Vector3(_room.Key.x * _roomDistance, 0, _room.Key.y * _roomDistance - 5), Quaternion.identity, _gameObject.transform);
            }
            else
            {
                _gameObject = null;
                Debug.LogError("RoomTypeEnum null");
            }

            var _floorScript = _gameObject.GetComponent<DungeonFloor>();
            _floorScript.Location = _room.Key;

            _roomObjects.Add(_room.Key, _gameObject);
            //if (!startRoom)
            //{
            //    _gameObject.SetActive(false);
            //}
        }

        if (!this._doors.ContainsKey(_endRoom))
        {
            _doors.Add(_endRoom, new List<GameObject>());
        }
        if (_level != 3)
        {
            _doors[_endRoom].Add(_door1);
        }
        _doors[_endRoom].Add(_door2);

        return _roomObjects;

    }

    private Dictionary<Vector2, List<GameObject>> SpawnDoors(Dictionary<Vector2, GameObject> _roomObjects, List<(Vector2 to, Vector2 from, DirectionEnum.Direction toward)> _paths, Vector2 _bossRoom, int _floor)
    {
        GameObject _fromRoom;
        GameObject _toRoom;
        DirectionEnum.Direction _pathDirection;
        Dictionary<Vector2, List<GameObject>> doors = new();

        for (int i = 0; i < _paths.Count; i++)
        {
            _fromRoom = _roomObjects[_paths[i].from];
            _toRoom = _roomObjects[_paths[i].to];
            _pathDirection = _paths[i].toward;

            var _fromRoomFloor = _fromRoom.GetComponent<DungeonFloor>();
            var _toRoomFloor = _toRoom.GetComponent<DungeonFloor>();

            Assert.IsTrue(_fromRoomFloor != null && _toRoomFloor != null, "Room prefab does not have DungeonFloor script attached");

            GameObject _fromDoor;
            GameObject _toDoor;

            if (_paths[i].to == _bossRoom && /*_floor % 5 == 0*/ (_floor == 2 || _floor == 3))
            {
                _fromDoor = Object.Instantiate(_dungeonManager.BossDoors[0], _fromRoomFloor.GetLocationOf(_pathDirection) + _fromRoom.transform.position, _directionEnum.QuaternionFacing(_pathDirection), _fromRoom.transform);
            }
            else
            {
                _fromDoor = Object.Instantiate(_dungeonManager.Doors[0], _fromRoomFloor.GetLocationOf(_pathDirection) + _fromRoom.transform.position, _directionEnum.QuaternionFacing(_pathDirection), _fromRoom.transform);
            }

            _toDoor = Object.Instantiate(_dungeonManager.Doors[0], _toRoomFloor.GetLocationOf(_directionEnum.Opposite(_pathDirection)) + _toRoom.transform.position, _directionEnum.QuaternionFacing(_directionEnum.Opposite(_pathDirection)), _toRoom.transform);

            var _fromDoorTrigger = _fromDoor.GetComponent<DungeonDoor>();
            var _toDoorTrigger = _toDoor.GetComponent<DungeonDoor>();

            Assert.IsTrue(_fromDoorTrigger != null && _toDoorTrigger != null, "Door prefabs do not have DungeonDoor script attached");

            _fromDoorTrigger.ToPosition = _toRoomFloor.GetLocationOf(_directionEnum.Opposite(_pathDirection));
            _fromDoorTrigger.ToRoom = _toRoom;

            _toDoorTrigger.ToPosition = _fromRoomFloor.GetLocationOf(_pathDirection);
            _toDoorTrigger.ToRoom = _fromRoom;

            if (!doors.ContainsKey(_paths[i].to))
            {
                doors.Add(_paths[i].to, new List<GameObject>());
            }
            doors[_paths[i].to].Add(_toDoor);
            if (!doors.ContainsKey(_paths[i].from))
            {
                doors.Add(_paths[i].from, new List<GameObject>());
            }
            doors[_paths[i].from].Add(_fromDoor);
        }

        return doors;
    }

    private void SpawnSpawners(Dictionary<Vector2, RoomType> rooms, Dictionary<Vector2, GameObject> _roomObjects, CustomRandom rand)
    {
        foreach (var room in rooms)
        {
            if (room.Value == RoomType.Combat)
            {
                var _roomScript = _roomObjects[room.Key].GetComponent<DungeonFloor>();
                int _randomIndex = rand.Next(0, _dungeonManager.EasySpawners.Count());
                Object.Instantiate(_dungeonManager.EasySpawners[_randomIndex], new Vector3(room.Key.x * _roomDistance + _roomScript.SpawnPoint.x, 0, room.Key.y * _roomDistance + _roomScript.SpawnPoint.y), Quaternion.identity, _roomObjects[room.Key].transform);
            }
            if (room.Value == RoomType.MiniBoss || room.Value == RoomType.Boss)
            {
                var _roomScript = _roomObjects[room.Key].GetComponent<DungeonFloor>();
                int _randomIndex = rand.Next(0, _dungeonManager.BossSpawners.Count());
                Object.Instantiate(_dungeonManager.BossSpawners[_randomIndex], new Vector3(room.Key.x * _roomDistance + _roomScript.SpawnPoint.x, 0, room.Key.y * _roomDistance + _roomScript.SpawnPoint.y), Quaternion.identity, _roomObjects[room.Key].transform);
            }
        }
    }

    public void EnableDoors(Vector2 room)
    {
        var doors = this._doors[room];

        foreach (var door in doors)
        {
            door.SetActive(true);
        }

    }

    public void DisableDoors(Vector2 room)
    {
        var doors = this._doors[room];

        foreach (var door in doors)
        {
            door.SetActive(false);
        }

        Debug.Log("Doors Disabled in: " + room);
    }

    public (Dictionary<Vector2, RoomType>, List<(Vector2 to, Vector2 from)>) GetRooms()
    {
        return (_savedRooms, _paths);
    }
}
