using System.Collections;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerShootController))]
public class Player : CharacterBase
{
    public Vector2 Momentum { get; private set; }
    public SpriteRenderer _gunObject;
    public PlayerHeadController _headObject;
    public ItemBase CurrentItem { get; private set; }
    public Room CurrentRoom { get; set; }

    private PlayerShootController _shootController;
	private bool _invulnerable = false;

    public void Start() {
        _shootController = GetComponent<PlayerShootController>();
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        HandleItemUse();
    }

    private void HandleItemUse() {
        if (Input.GetKeyDown(KeyCode.Space) && CurrentItem != null) {
            CurrentItem.UseItem(this);
            
			if(CurrentItem.IsInstantlyDestroyedAfterUse)
                Destroy(CurrentItem.gameObject);

            CurrentItem = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
		// The player loses HP with it collides with the enemy or a game object that contains the name Bullet
		if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.name.Contains("Bullet") ) {
            Health--;
        }
    }

    public void OnPickUp(ItemBase item) {      
        if (!item._isInstantEffect) {
            if (CurrentItem != null) {
                CurrentItem.transform.parent = transform.parent;
                CurrentItem.transform.position = transform.position + new Vector3(1.0f, 0, 0);
                CurrentItem.Enable();
            }
            StartCoroutine(PlayPickUpAnimation(item));
            CurrentItem = item;
        } else {
			var usedItem = item.UseItem(this);

			if (usedItem) {
				item.Disable ();
				StartCoroutine (DestroyItem (item));
			}
        }
    }

    IEnumerator DestroyItem(ItemBase item) {
        yield return new WaitForSeconds(0.5f);
        Destroy(item.gameObject);
    }

    private IEnumerator PlayPickUpAnimation(ItemBase item) {
        item.transform.parent = transform;
        item.transform.localPosition = Vector3.zero + new Vector3(0, 3.0f, 0);
        item.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        item.GetComponent<SpriteRenderer>().sortingOrder = 10;
        Animator.Play("PickUpItem");
        _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);
        DisableCharacter();
        yield return new WaitForSeconds(1f);
        EnableCharacter();
        _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);
        item.Disable();
        yield return null;
    }

    protected override Vector3 DetermineMovement() {
        var movement = new Vector3();

		if (!IsDead && InputHelpers.IsAnyKey ("w", "s", "a", "d")) {
			ShouldMove = true;
			_gunObject.enabled = false;

			var headDirection = PlayerHeadController.HeadDirection.Down;

			if (Input.GetKey ("w") && Input.GetKey ("a")) {
				movement += new Vector3 (-1, 1, 0);
				headDirection = PlayerHeadController.HeadDirection.Left;
			} else if (Input.GetKey ("w") && Input.GetKey ("d")) {
				movement += new Vector3 (1, 1, 0);
				headDirection = PlayerHeadController.HeadDirection.Right;
			} else if (Input.GetKey ("s") && Input.GetKey ("a")) {
				movement += new Vector3 (-1, -1, 0);
				headDirection = PlayerHeadController.HeadDirection.Left;
			} else if (Input.GetKey ("s") && Input.GetKey ("d")) {
				movement += new Vector3 (1, -1, 0);
				headDirection = PlayerHeadController.HeadDirection.Right;
			} else if (Input.GetKey ("w")) {
				movement += new Vector3 (0, 1, 0);
				headDirection = PlayerHeadController.HeadDirection.Up;
			} else if (Input.GetKey ("s")) {
				movement += new Vector3 (0, -1, 0);
				_gunObject.enabled = true;
				headDirection = PlayerHeadController.HeadDirection.Down;
			} else if (Input.GetKey ("a")) {
				movement += new Vector3 (-1, 0, 0);
				headDirection = PlayerHeadController.HeadDirection.Left;
			} else if (Input.GetKey ("d")) {
				movement += new Vector3 (1, 0, 0);
				headDirection = PlayerHeadController.HeadDirection.Right;
			}

			movement.Normalize ();

			if (!_shootController.IsShooting) {
				_headObject.SetHeadDirection (headDirection);
			}       
		} else if (IsDead) {
			ShouldMove = false;
		} else {
			_headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);	
		}
        return movement;
    }

    protected override void HandleMovement(Vector3 movement) {
        Momentum = movement * _moveSpeed;
        gameObject.GetComponent<Rigidbody2D>().AddForce(Momentum);
    }

    protected override void Die() {
        base.Die();
        Animator.Play("Die");
        DisableCharacter();
		StartCoroutine (Restart());
    }

	protected override void TakeDamage() {
		if (!_invulnerable) {
				base.TakeDamage ();
				StartCoroutine (BeInvulnerable ());
		}
	}

	IEnumerator BeInvulnerable() {
		_invulnerable = true;
		yield return new WaitForSeconds (0.5f);
		_invulnerable = false;
	}

	IEnumerator Restart() {
		yield return new WaitForSeconds (1.8f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex) ;
	}

    private void DisableCharacter() {
        _gunObject.enabled = false;
        _headObject.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void EnableCharacter() {
        _gunObject.enabled = true;
        _headObject.GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
