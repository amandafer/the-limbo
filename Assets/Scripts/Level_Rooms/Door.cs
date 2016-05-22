using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour {
    public AudioSource _doorOpenClip, _doorCloseClip;
	public BoxCollider2D Wall;
	public Room OwnerRoom, ConnectingRoom;
	public RoomDirection Direction;

	private Animator _animator;
	private Camera _mainCamera;

    [SerializeField]
    private bool? _isOpen;
    public bool IsOpen {
        get { return _isOpen.HasValue && _isOpen.Value; }
        set {
            if (!_isOpen.HasValue || value != _isOpen) {
                if (_animator == null) {
                    _animator = GetComponent<Animator>();
                }

                if (value) {
                    _animator.SetInteger("Is Open", 1);
                    //if (DoorOpenClip != null)
                    //{
                    //    DoorOpenClip.Play();
                    //}
                    Wall.enabled = false;
                } else {
                    _animator.SetInteger("Is Open", 0);
                    //if (DoorCloseClip != null)
                    //{
                    //    DoorCloseClip.Play();
                    //}
                    Wall.enabled = true;
                }
                _isOpen = value;
            }
        }
    }

	public void Start () {
	    _animator = GetComponent<Animator>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

    private const float PlayerMovement = 3.0f;

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<Player>();
            //IsOpen = true;

            var cameraMovement = new Vector2();
            var playerMovement = new Vector3();

			// Camera direction
            switch (Direction) {
                case RoomDirection.North:
                    cameraMovement = new Vector2(0, 10);
                    playerMovement = new Vector3(0, PlayerMovement, 0);
                    break;
                case RoomDirection.East:
                    cameraMovement = new Vector2(15f, 0);
                    playerMovement = new Vector3(PlayerMovement, 0, 0);
                    break;
                case RoomDirection.South:
                    cameraMovement = new Vector2(0, -10);
                    playerMovement = new Vector3(0, -PlayerMovement, 0);
                    break;
                case RoomDirection.West:
                    cameraMovement = new Vector2(-15f, 0);
                    playerMovement = new Vector3(-PlayerMovement, 0, 0);
                    break;
            }

            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StartCoroutine(MoveCamera(cameraMovement));
            other.transform.Translate(playerMovement);
            OwnerRoom.PlayerIsInRoom = false;
            ConnectingRoom.OnPlayerEntersRoom(player);
        } 
    }

    private IEnumerator MoveCamera(Vector2 direction) {
        for (int i = 0; i < 10; i++) {
            _mainCamera.transform.Translate(direction.x / 10, direction.y / 10, 0);
            yield return null;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //IsOpen = false;
        }  
    }
}
