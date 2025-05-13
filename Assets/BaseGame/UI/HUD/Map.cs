using LCPS.SlipForge.Engine;
using LCPS.SlipForge.Enum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    public class Map : MonoBehaviour
    {
        // Room sprites
        [SerializeField] private Sprite TreasureSprite;
        [SerializeField] private Sprite ShopSprite;
        [SerializeField] private Sprite StartSprite;
        [SerializeField] private Sprite BossSprite;
        [SerializeField] private Sprite CombatSprite;
        [SerializeField] private Sprite MiniBossSprite;
        [SerializeField] private Sprite ExitSprite;

        // Path sprites
        [SerializeField] private Sprite PathSprite;
        [SerializeField] private Sprite UnknownSprite;
        [SerializeField] private Sprite CurrentRoomSprite;

        // Sprite scale ratio
        [Range(0.05f, 1f)]
        [Tooltip("The ratio of the room sprite size to the map width")]
        [SerializeField] private float ScaleRatio = 0.1f;

        // Buffer ratio on map edges
        [Range(0.05f, 1f)]
        [Tooltip("The ratio for the buffer on each of the sides of the map")]
        [SerializeField] private float BufferRatio = 0.1f;

        // Dictionaries for room sprites and map objects
        private Dictionary<RoomType, Sprite> _roomSprites;
        private readonly Dictionary<Vector2, GameObject> _mapRooms = new();
        private readonly Dictionary<(Vector2 to, Vector2 from), GameObject> _mapPaths = new();

        // private variables
        private GameObject _currentRoom;
        private Vector2 _minRoom;
        private Vector2 _maxRoom;

        // dungeon observable
        private Observable<bool> _isDungeon;

        private void Awake()
        {
            _isDungeon = DataTracker.GetObservable(data => data.IsDungeon);
            _isDungeon.Subscribe(OnDungeonChange);

            DungeonManager.CurrentRoomChanged += UpdateMap;
            DungeonManager.OnLevelChange += ChangeLevel;

            _roomSprites = new Dictionary<RoomType, Sprite>
            {
                {RoomType.Treasure, TreasureSprite},
                {RoomType.Shop, ShopSprite},
                {RoomType.Start, StartSprite},
                {RoomType.Boss, BossSprite},
                {RoomType.Combat, CombatSprite},
                {RoomType.MiniBoss, MiniBossSprite},
                {RoomType.Exit, ExitSprite}
            };

            // Create the current room object
            _currentRoom = new("CurrentRoom", typeof(Image));
            _currentRoom.transform.SetParent(transform);
            _currentRoom.GetComponent<Image>().sprite = CurrentRoomSprite;
            RectTransform currentRoomRect = _currentRoom.GetComponent<RectTransform>();
            SetRectAnchorsAndScale(currentRoomRect);
            currentRoomRect.anchoredPosition = GetRoomPosition(new Vector2(2, 2));
            _currentRoom.SetActive(false);
        }

        private void OnDestroy()
        {
            DungeonManager.CurrentRoomChanged -= UpdateMap;
            DungeonManager.OnLevelChange -= ChangeLevel;
        }

        private void OnDungeonChange(bool isDungeon)
        {
            if (!isDungeon)
                ClearMap();
        }

        private void ChangeLevel(int level)
        {
            ClearMap();
            SetMinAndMaxRooms();
        }

        private void SetMinAndMaxRooms()
        {
            (Dictionary<Vector2, RoomType> rooms, _) = DungeonManager.Instance.GetRooms();
            _minRoom = new Vector2(4, 4);
            _maxRoom = new Vector2(0, 0);
            foreach (var room in rooms.Keys)
            {
                if (room.x < _minRoom.x)
                    _minRoom.x = room.x;
                if (room.y < _minRoom.y)
                    _minRoom.y = room.y;
                if (room.x > _maxRoom.x)
                    _maxRoom.x = room.x;
                if (room.y > _maxRoom.y)
                    _maxRoom.y = room.y;
            }
        }

        private void ClearMap()
        {
            foreach (var room in _mapRooms.Values)
                Destroy(room);
            _mapRooms.Clear();
            foreach (var path in _mapPaths.Values)
                Destroy(path);
            _mapPaths.Clear();

            // this method gets called on event subscription
            // on the subscription, _currentRoom is null and
            // calling SetActive will throw an error if it is null
            if (_currentRoom != null)
                _currentRoom.SetActive(false);
        }

        private void UpdateMap(Vector2 room)
        {
            // Update the map
            (Dictionary<Vector2, RoomType> rooms, List<(Vector2 to, Vector2 from)> paths) = DungeonManager.Instance.GetRooms();
            UpdateRoom(room, _roomSprites[rooms[room]]);

            foreach (var (to, from) in paths)
            {
                if (room == from && !_mapRooms.ContainsKey(to))
                {
                    UpdateRoom(to, UnknownSprite);
                    UpdatePath(to, from);
                }
            }

            // this should be last so that the current room sprite is on top
            SetCurrentRoom(room);
        }

        private void SetCurrentRoom(Vector2 room)
        {
            _currentRoom.SetActive(true);
            _currentRoom.GetComponent<RectTransform>().anchoredPosition = GetRoomPosition(room);
            // set current room to be the last child so it is on top
            _currentRoom.transform.SetAsLastSibling();
        }

        private void UpdatePath(Vector2 to, Vector2 from)
        {
            if (!_mapPaths.ContainsKey((to, from)))
                CreatePath(to, from);
        }

        private void CreatePath(Vector2 to, Vector2 from)
        {
            GameObject pathObj = new("MapPath", typeof(Image));
            pathObj.transform.SetParent(transform);
            pathObj.GetComponent<Image>().sprite = PathSprite;
            RectTransform pathRect = pathObj.GetComponent<RectTransform>();

            // Set the path position
            SetRectAnchorsAndScale(pathRect);
            pathRect.anchoredPosition = GetRoomPosition((to + from) / 2);

            // add to the map paths dictionary
            _mapPaths[(to, from)] = pathObj;
        }

        private void UpdateRoom(Vector2 room, Sprite sprite)
        {
            if (_mapRooms.ContainsKey(room))
                _mapRooms[room].GetComponent<Image>().sprite = sprite;
            else
                CreateRoom(room, sprite);
        }

        private void CreateRoom(Vector2 room, Sprite sprite)
        {
            GameObject roomObj = new("MapRoom", typeof(Image));
            roomObj.transform.SetParent(transform);
            roomObj.GetComponent<Image>().sprite = sprite;
            RectTransform roomRect = roomObj.GetComponent<RectTransform>();

            // Set the room position
            SetRectAnchorsAndScale(roomRect);
            roomRect.anchoredPosition = GetRoomPosition(room);

            // add to the map rooms dictionary
            _mapRooms[room] = roomObj;
        }

        private void SetRectAnchorsAndScale(RectTransform roomRect)
        {
            (float mapWidth, float mapHeight) = GetMapLocalSize();
            Vector2 bufferVector = BufferRatio * Vector2.one;

            float width = mapWidth * ScaleRatio - mapWidth;
            float height = mapWidth * ScaleRatio - mapHeight;

            roomRect.anchorMin = new Vector2(0f, 0f) + bufferVector;
            roomRect.anchorMax = new Vector2(1f, 1f) - bufferVector;
            roomRect.sizeDelta = new Vector2(width, height);
            roomRect.localScale = Vector3.one;
        }

        private Vector3 GetRoomPosition(Vector2 room)
        {
            // scales the room position to the map size
            (float width, float height) = GetMapLocalSize();
            return NormalizeRoomVector(room) * new Vector2(width, height) - new Vector2(width / 2, height / 2);
        }

        private Vector2 NormalizeRoomVector(Vector2 room)
        {
            return (room - _minRoom) / (_maxRoom - _minRoom);
        }

        private (float width, float height) GetMapLocalSize()
        {
            (float width, float height) = GetComponent<RectTransform>().GetLocalSize();
            width *= 1 - 2 * BufferRatio;
            height *= 1 - 2 * BufferRatio;
            return (width, height);
        }
    }
}
