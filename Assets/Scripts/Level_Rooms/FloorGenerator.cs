using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloorGenerator : MonoBehaviour {

	public int numberOfRooms = 4;
	public float _branchingProbability = 0.6f;
	public Room _firstRoom, _bossRoom;
	public Player _playerPrefab;
	public Enemy _enemyPrefab;
	public List<Room> _roomPrefabs = new List<Room>();
	public List<GameObject> _obstacleLayouts = new List<GameObject>();

	private FloorGrid _floorGrid = new FloorGrid(6, 6);
	public FloorGrid Grid {get {return _floorGrid;}}

	public const float HorizontalDelta = 14.5f;//13.4f;
	public const float VerticalDelta = 10;//9f;

	// Generates the floor and the player
	public void Awake() {
		Random.seed = DateTime.Now.Second;
		TryGenerateFloor();
		AddPlayer();
	}

	public void TryGenerateFloor() {
		try {
			GenerateFloorLayout();
		} catch (Exception ex) {
			Debug.LogError("Failed to create level");
			Debug.LogException(ex);
		}
	}

	public void ClearFloor() {
		_floorGrid.Rooms.ToList().ForEach(r => Destroy(r.gameObject));
		_floorGrid = new FloorGrid(6,6);
	}

	private void AddPlayer() {
		Instantiate(_playerPrefab);
		_floorGrid.FirstRoom.OnPlayerEntersRoom(_playerPrefab);
	}

	private void GenerateFloorLayout() {
		var coordinates = new RoomCoordinates(Random.Range(1, 4), Random.Range(1, 4));
		var firstRoom = CreateFirstRoom(coordinates);

		//No rooms were created yet
		int numberOfRoomsCreated = CreateBranch(firstRoom, coordinates, 0);

		if (Random.Range(0.0f, 1.0f) <= _branchingProbability) {
			numberOfRoomsCreated = CreateBranch(firstRoom, coordinates, numberOfRoomsCreated);
		}

		var previousRoom = firstRoom;
		while (numberOfRoomsCreated < numberOfRooms-1) {
			RoomDirection direction;
			try {
				direction = DetermineNextRoomLocation(coordinates);
			} catch (Exception ex) {
				previousRoom = _floorGrid.Rooms.FirstOrDefault(r => _floorGrid.GetValidDirectionsFromRoom(r).Any());

				if (previousRoom == null) {
					throw new Exception("Everything is ruined", ex);
				}
				direction = DetermineNextRoomLocation(_floorGrid.GetCoordinatesForRoom(previousRoom));
				coordinates = _floorGrid.GetCoordinatesForRoom(previousRoom);

			}

			coordinates = DetermineNewCoordinates(direction, coordinates);

			if (!_floorGrid.IsDeadEnd(coordinates.X, coordinates.Y)) {
				previousRoom = AddNewRoom(previousRoom, direction, coordinates);
				numberOfRoomsCreated++;
			}

			if (numberOfRoomsCreated < numberOfRooms-1 &&  Random.Range(0.0f, 1.0f) <= _branchingProbability) {
				numberOfRoomsCreated = CreateBranch(previousRoom, coordinates, numberOfRoomsCreated);
			}
		}

		if (!_floorGrid.GetValidDirectionsFromRoom(previousRoom).Any()) {
			var validRooms = _floorGrid.Rooms.Where(
				r => r != firstRoom && _floorGrid.GetValidDirectionsFromRoom(r).Any()).ToList();
			previousRoom = validRooms.ElementAt(Random.Range(0, validRooms.Count));

			coordinates = _floorGrid.GetCoordinatesForRoom(previousRoom);
		}

		CreateBossRoom(previousRoom, coordinates);

		if (!_floorGrid.GetValidDirectionsFromRoom(previousRoom).Any()) {
			var validRooms = _floorGrid.Rooms.Where(
				r => r._roomType == RoomType.NormalRoom && _floorGrid.GetValidDirectionsFromRoom(r).Any()).ToList();
			previousRoom = validRooms.ElementAt(Random.Range(0, validRooms.Count));

			coordinates = _floorGrid.GetCoordinatesForRoom(previousRoom);
		}
		//var dir = _floorGrid.GetValidDirectionsFromRoom(coordinates).First();
		//AddNewRoom(previousRoom, dir, DetermineNewCoordinates(dir, coordinates), RoomType.TreasureRoom);
	}

	// Creates the "graph" of the level and the room coordinates
	private int CreateBranch(Room previousRoom, RoomCoordinates coordinates, int numberOfRoomsCreated) {
		var branchLength = Random.Range(1, 4);
		var previousBranchRoom = previousRoom;
		var branchCoordinates = coordinates;

		while (branchLength > 0) {
			try {
				RoomDirection direction = DetermineNextRoomLocation(branchCoordinates);
				branchCoordinates = DetermineNewCoordinates(direction, branchCoordinates);

				if (_floorGrid.CanRoomBeAdded(branchCoordinates.X, branchCoordinates.Y)) {
					previousBranchRoom = AddNewRoom(previousBranchRoom, direction, branchCoordinates);
					numberOfRoomsCreated++;
				}
			} catch (Exception) {
				Debug.Log("Branching failed :(");
				break;
			}
			branchLength--;
		}
		return numberOfRoomsCreated;
	}


	private Room CreateFirstRoom(RoomCoordinates coordinates) {
		Room previousRoom;

		if (_firstRoom != null) {
			previousRoom = (Room) Instantiate(_firstRoom);
			_floorGrid.AddRoom(coordinates.X, coordinates.Y, previousRoom);
		} else {
			previousRoom = (Room) Instantiate(_roomPrefabs.First());
			_floorGrid.AddRoom(coordinates.X, coordinates.Y, previousRoom);
		}

		return previousRoom;
	}


	private RoomDirection DetermineNextRoomLocation(RoomCoordinates coordinates) {
		var validDirections = _floorGrid.GetValidDirectionsFromRoom(coordinates.X, coordinates.Y).ToList();

		if (!validDirections.Any()) 
			throw new Exception("Created dead-end :(");

		return validDirections.ElementAt(Random.Range(0, validDirections.Count()));
	}


	private RoomCoordinates DetermineNewCoordinates(RoomDirection direction, RoomCoordinates previousCoordinates) {
		switch (direction) {
			case RoomDirection.North:
				return new RoomCoordinates(previousCoordinates.X, previousCoordinates.Y + 1);
			case RoomDirection.East:
				return new RoomCoordinates(previousCoordinates.X + 1, previousCoordinates.Y);
			case RoomDirection.South:
				return new RoomCoordinates(previousCoordinates.X, previousCoordinates.Y - 1);
			case RoomDirection.West:
				return new RoomCoordinates(previousCoordinates.X - 1, previousCoordinates.Y);
			default:
				return previousCoordinates;
		}
	}


	private Room AddNewRoom(Room previousRoom, RoomDirection direction, RoomCoordinates coordinates, RoomType roomType = RoomType.NormalRoom) {
		var newRoom = CreateRoom(previousRoom, direction, roomType);

		if (previousRoom != null) {
			previousRoom.SetAdjacentRoom(newRoom, direction);
			newRoom.SetAdjacentRoom(previousRoom, GetOppositeRoomDirection(direction));
		}

		_floorGrid.AddRoom(coordinates.X, coordinates.Y, newRoom);
		previousRoom = newRoom;

		return previousRoom;
	}

	private void CreateBossRoom(Room previousRoom, RoomCoordinates coordinates)
	{
		Debug.Log("Room before boss: "+ previousRoom.name);
		var validDirections = _floorGrid.GetValidDirectionsFromRoom(coordinates.X, coordinates.Y).ToList();
		if (!validDirections.Any())
		{
			throw new Exception("Failed to create boss room.");
		}

		var direction = validDirections.ElementAt(Random.Range(0, validDirections.Count()));
		AddNewRoom(previousRoom, direction, DetermineNewCoordinates(direction, coordinates), RoomType.BossRoom);
	}

	private static RoomDirection GetOppositeRoomDirection(RoomDirection direction)
	{
		return (RoomDirection)(((int)direction + 2) % 4);
	}

	// Creates the room with coordinates and instantiate the enemies
	private Room CreateRoom(Room previousRoom, RoomDirection direction, RoomType roomType = RoomType.NormalRoom)
	{
		Room prefab;
		// Type of the room
		switch (roomType) {
			case RoomType.StartRoom:
				prefab = _firstRoom;
				break;
			case RoomType.NormalRoom:
				prefab = _roomPrefabs.First();
				break;
			case RoomType.BossRoom:
				prefab = _bossRoom;
				break;
			default:
				throw new ArgumentOutOfRangeException("roomType");
		}

		var room = (Room)Instantiate(prefab);
		// Direction of the room
		if (previousRoom != null) {
			var position = previousRoom.transform.position;

			switch (direction) {
				case RoomDirection.North:
					position.y += VerticalDelta;
					break;
				case RoomDirection.East:
					position.x += HorizontalDelta;
					break;
				case RoomDirection.South:
					position.y -= VerticalDelta;
					break;
				case RoomDirection.West:
					position.x -= HorizontalDelta;
					break;
			}
			room.transform.position = position;
		}

		// Instantiate boss or normal enemies
		if (roomType == RoomType.BossRoom) {
			var enemyLayouts = room.GetComponent<EnemyLayout>().EnemyLayouts;
			var enemyLayout = (GameObject)Instantiate(enemyLayouts.ElementAt(Random.Range(0, enemyLayouts.Count)));
			//enemyLayout.transform.localPosition = Vector3.zero;
			enemyLayout.transform.parent = room.transform;

			var enemies = enemyLayout.GetComponentsInChildren<Enemy>().ToList();
			enemies.ForEach(e => room.AddEnemy(e, e.transform.position));
		} else if (roomType == RoomType.NormalRoom ) {
			var obstacleLayout = (GameObject)Instantiate(_obstacleLayouts.ElementAt(Random.Range(0, _obstacleLayouts.Count)));
			obstacleLayout.transform.parent = room.transform;
			obstacleLayout.transform.localPosition = Vector3.zero;

			// Add enemies for rooms type normal
			var enemyLayouts = obstacleLayout.GetComponent<EnemyLayout>().EnemyLayouts;

			if (enemyLayouts.Count == 0)
				Debug.Log ("Empty list of enemies");
			else {
				var enemyLayout = (GameObject)Instantiate (enemyLayouts.ElementAt (Random.Range(0, enemyLayouts.Count)));
				//enemyLayout.transform.localPosition = Vector3.zero;
				enemyLayout.transform.parent = room.transform;
				var enemies = enemyLayout.GetComponentsInChildren<Enemy> ().ToList ();
				enemies.ForEach (e => room.AddEnemy (e, e.transform.position));
			}
		}
		return room;
	}
}

public class FloorGrid {
	private readonly Room[,] _rooms;

	private readonly List<Room> _roomList = new List<Room>();
	public IEnumerable<Room> Rooms { get { return _roomList; } } 

	public int Width { get; private set; }
	public int Height { get; private set; }

	public Room FirstRoom { get; private set; }

	public FloorGrid(int height, int width)
	{
		Height = height;
		Width = width;
		_rooms = new Room[height, width];
	}

	public bool ContainsRoom(int x, int y)
	{
		return _rooms[x, y] != null;
	}

	public bool ContainsRoom(RoomCoordinates coordinates)
	{
		return ContainsRoom(coordinates.X, coordinates.Y);
	}

	public bool CanRoomBeAdded(int x, int y)
	{
		return x >= 0 && x < Width && y >= 0 && y < Height && !ContainsRoom(x, y);
	}

	public bool CanRoomBeAdded(RoomCoordinates coordinates)
	{
		return CanRoomBeAdded(coordinates.X, coordinates.Y);
	}

	public bool IsDeadEnd(int x, int y)
	{
		return !CanRoomBeAdded(x + 1, y) && !CanRoomBeAdded(x - 1, y) && !CanRoomBeAdded(x, y + 1) &&
			!CanRoomBeAdded(x, y - 1);
	}

	public bool IsDeadEnd(RoomCoordinates coordinates)
	{
		return IsDeadEnd(coordinates.X, coordinates.Y);
	}

	public void AddRoom(int x, int y, Room room)
	{
		if (ContainsRoom(x, y))
		{
			throw new Exception("The cell already contains a room");
		}
		if (FirstRoom == null)
		{
			FirstRoom = room;
		}
		_rooms[x, y] = room;
		room.name = string.Format(room._roomType +" ({0},{1})", x, y);
		_roomList.Add(room);
		room.X = x;
		room.Y = y;
	}

	public void AddRoom(RoomCoordinates coordinates, Room room)
	{
		AddRoom(coordinates.X, coordinates.Y, room);
	}

	public IEnumerable<RoomDirection> GetValidDirectionsFromRoom(int x, int y)
	{
		var validDirections = new List<RoomDirection>();
		if (CanRoomBeAdded(x + 1, y))
		{
			validDirections.Add(RoomDirection.East);
		}
		if (CanRoomBeAdded(x - 1, y))
		{
			validDirections.Add(RoomDirection.West);
		}
		if (CanRoomBeAdded(x, y + 1))
		{
			validDirections.Add(RoomDirection.North);
		}
		if (CanRoomBeAdded(x, y - 1))
		{
			validDirections.Add(RoomDirection.South);
		}
		return validDirections;
	}

	public IEnumerable<RoomDirection> GetValidDirectionsFromRoom(RoomCoordinates coordinates)
	{
		return GetValidDirectionsFromRoom(coordinates.X, coordinates.Y);
	}

	public IEnumerable<RoomDirection> GetValidDirectionsFromRoom(Room room)
	{
		return GetValidDirectionsFromRoom(GetCoordinatesForRoom(room));
	}

	public RoomCoordinates GetCoordinatesForRoom(Room room)
	{
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				if (_rooms[i, j] == room)
				{
					return new RoomCoordinates(i, j);
				}
			}
		}
		throw new ArgumentException("Room cannot be found from the grid.");
	}

	//TODO: Do not instantiate rooms until the room is ready
	public void InstantiateRooms()
	{
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				if (_rooms[i, j] != null)
				{

				}
			}
		}
	}
}

public struct RoomCoordinates
{
	public int X { get; private set; }
	public int Y { get; private set; }

	public RoomCoordinates(int x, int y) : this()
	{
		X = x;
		Y = y;
	}
}

public enum RoomDirection
{
	North = 0,
	East = 1,
	South = 2,
	West = 3
}