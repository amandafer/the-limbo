using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts
{
    public class Room : MonoBehaviour
    {
        private readonly List<Door> _doors = new List<Door>();

		public Door NorthDoor, SouthDoor, EastDoor, WestDoor, _doorPrefab, _bossDoorPrefab;
		public int X, Y;
        public RoomType _roomType = RoomType.NormalRoom;
        public LevelSwitchDoor _levelSwitchDoorPrefab;

        public bool IsBossRoom {
            get { return _roomType == RoomType.BossRoom; }
        }

        public BoxCollider2D _northDoorWall, _southDoorWall, _eastDoorWall, _westDoorWall;
        public List<Enemy> _enemies = new List<Enemy>(); 

        public bool IsVisibleOnMap { get; set; }
        public bool PlayerHasVisited { get; private set; }
        public GameObject _bossBar, bossBarPrefab;

        [SerializeField]
        private bool _playerIsInRoom;
        public bool PlayerIsInRoom {
            get { return _playerIsInRoom; }
            set {
                _playerIsInRoom = value;

                if (value) {
                    PlayerHasVisited = true;
                    IsVisibleOnMap = true;

                    if (NorthDoor != null) {
                        NorthDoor.ConnectingRoom.IsVisibleOnMap = true;
                    } else if (SouthDoor != null) {
                        SouthDoor.ConnectingRoom.IsVisibleOnMap = true;
                    } else if (WestDoor != null) {
                        WestDoor.ConnectingRoom.IsVisibleOnMap = true;
                    } else if (EastDoor != null) {
                        EastDoor.ConnectingRoom.IsVisibleOnMap = true;
                    }

                    StartCoroutine(WakeUpEnemies());
                }
            }
        }

        IEnumerator WakeUpEnemies() {
            yield return new WaitForSeconds(1f);
            _enemies.ForEach(e => e.Enable());

            if (_roomType == RoomType.BossRoom) {
                _bossBar = (GameObject)Instantiate(bossBarPrefab);
                _bossBar.transform.position = _bossBar.transform.position + transform.position;
            }
            yield return null;
        }

        public bool ContainsEnemies { get { return _enemies.Count > 0; } }

        public void SetAdjacentRoom(Room room, RoomDirection direction) {
            var position = new Vector3();
            var rotation = new Quaternion();
            Door doorPrefab = _doorPrefab;

			if (_roomType == RoomType.NormalRoom || _roomType == RoomType.StartRoom) {
                switch (room._roomType) {
                    case RoomType.StartRoom:
                        doorPrefab = _doorPrefab;
                        break;
                    case RoomType.NormalRoom:
                        doorPrefab = _doorPrefab;
                        break;
                    case RoomType.BossRoom:
                        doorPrefab = _bossDoorPrefab;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
			} else if (_roomType == RoomType.BossRoom) {
                doorPrefab = _bossDoorPrefab;
            }

			// Sets the doors prefabs of the room and its position
            var door = (Door)Instantiate(doorPrefab);
            switch (direction) {
                case RoomDirection.North:
                    position += new Vector3(0, 3.5f);
                    NorthDoor = door;
                    door.Wall = _northDoorWall;
                    break;
                case RoomDirection.East:
                    position += new Vector3(5.75f, 0);
                    rotation = Quaternion.Euler(0, 0, 270);
                    EastDoor = door;
                    door.Wall = _eastDoorWall;
                    break;
                case RoomDirection.South:
                    position += new Vector3(0, -3.55f);
                    rotation = Quaternion.Euler(0, 0, 180);
                    SouthDoor = door;
                    door.Wall = _southDoorWall;
                    break;
                case RoomDirection.West:
                    position += new Vector3(-5.79f, 0);
                    rotation = Quaternion.Euler(0, 0, 90);
                    WestDoor = door;
                    door.Wall = _westDoorWall;
                    break;
            }

            door.ConnectingRoom = room;
            door.Direction = direction;
            door.transform.parent = transform;
            door.transform.localPosition = position;
            door.transform.rotation = rotation;
            door.OwnerRoom = this;

            _doors.Add(door);
        }


        public void InstantiateEnemy(Enemy enemyPrefab, Vector2 positionInRoom) {   
            var enemy = (Enemy) Instantiate(enemyPrefab);
            enemy.transform.parent = transform;
            enemy.transform.localPosition = Vector3.zero + new Vector3(positionInRoom.x, positionInRoom.y, 0);
            enemy.OwnerRoom = this;
            _enemies.Add(enemy);
            enemy.Disable();
        }


		public void AddEnemy(Enemy enemy, Vector2 positionInRoom) {   
			enemy.transform.parent = transform;
			enemy.transform.localPosition = Vector3.zero + new Vector3(positionInRoom.x, positionInRoom.y, 0);
			enemy.OwnerRoom = this;
			_enemies.Add(enemy);
			enemy.Disable();
		}

		public void OnEnemyDied(Enemy enemy) {
			_enemies.Remove(enemy);

            if (!ContainsEnemies) {
                _doors.ForEach(d => d.IsOpen = true);
				var doorOpenClip = _doors.First()._doorOpenClip;

				if (doorOpenClip != null) {
					doorOpenClip.Play();
				}

                Destroy(_bossBar);
                if (_roomType == RoomType.BossRoom) {
                    var audioSources = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
                    audioSources.ElementAt(1).Stop();
                    audioSources.ElementAt(2).Play();
                    audioSources.ElementAt(0).PlayDelayed(9.629f);
                    
					var d = (LevelSwitchDoor) Instantiate(_levelSwitchDoorPrefab);
                    d.transform.parent = transform;
                    d.transform.localPosition = Vector3.zero;
                }
                
                SpawnItem();
            }
        }

		// Spawn items after enemies die
        private void SpawnItem() {
            if (_roomType != RoomType.NormalRoom) {
                return;
            }
            
			var spawner = GetComponentInChildren<ItemSpawner>();
            
			if (spawner != null) {
                spawner.Spawn();
            } else {
                Debug.LogError("Item Spawner missing");
            }

        }

		// Handles the audio for doors
        public void OnPlayerEntersRoom(Player player) {
            var audioSources = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();

            if (_roomType == RoomType.BossRoom) {
                audioSources.ElementAt(0).Stop();
                audioSources.ElementAt(1).Play();
            } else if (!audioSources.ElementAt(0).isPlaying)
                audioSources.ElementAt(0).Play();
            
			PlayerIsInRoom = true;

            if (ContainsEnemies) {
                _doors.ForEach(d => d.IsOpen = false);
				var doorCloseClip = _doors.First()._doorCloseClip;
                
				if (doorCloseClip != null)
					doorCloseClip.Play();
            } else {
				_doors.ForEach(d => d.IsOpen = true);
                /*var doorCloseClip = _doors.First()._doorCloseClip;

                if (doorCloseClip != null) {
                    doorCloseClip.Play();
                }*/
            }

            player.CurrentRoom = this;
            //Debug.Log("Player entered room: " + name);
        }
    }
}

public enum RoomType {
    StartRoom,
    NormalRoom,
    BossRoom
}