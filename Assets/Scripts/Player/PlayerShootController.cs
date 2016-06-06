using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(Player))]
    public class PlayerShootController : ShootControllerBase {
        public PlayerHeadController _headObject;
		public bool IsShooting;

        private KeyCode _shootKey;
        private Vector2 _shootDirection;
        private Player _player;

        public override void Start() {
            base.Start();
            _player = GetComponent<Player>();
        }

        public override void Update() {
            base.Update();

			if (IsShooting) {
				SetHeadDirection (_shootKey);
				return;
			}

            if (InputHelpers.IsAnyKey(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow)) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    _shootDirection = new Vector2(0, BulletSpeed);
                    _shootKey = KeyCode.UpArrow;
                } else if (Input.GetKey(KeyCode.DownArrow)) {
                    _shootDirection = new Vector2(0, -BulletSpeed);
                    _shootKey = KeyCode.DownArrow;
                } else if (Input.GetKey(KeyCode.LeftArrow)) {
                    _shootDirection = new Vector2(-BulletSpeed, 0);
                    _shootKey = KeyCode.LeftArrow;
                } else if (Input.GetKey(KeyCode.RightArrow)) {
                    _shootDirection = new Vector2(BulletSpeed, 0);
                    _shootKey = KeyCode.RightArrow;
                }
                StartCoroutine(Shoot());
            }
        }

        IEnumerator Shoot() {
            IsShooting = true;
            while (Input.GetKey(_shootKey)) {
				// Instantiate the bullet prefab and set the shooter as the player
				var bullet = (Rigidbody2D)Instantiate(BulletPrefab, new Vector3(transform.position.x, transform.position.y, 0f), new Quaternion());
				bullet.GetComponent<BulletScript>().Shooter = transform.gameObject;

				// Rotation of the bullet sprite
				if (_shootDirection.y > 0) {
                    bullet.transform.Rotate(0, 0, -90);
                } else if (_shootDirection.y < 0) {
                    bullet.transform.Rotate(0, 0, 90);
                } else if (_shootDirection.x > 0) {
                    TransformHelpers.FlipX(bullet.gameObject);
                }
                bullet.AddForce(_shootDirection);
                bullet.AddForce(_player.GetComponent<Rigidbody2D>().GetPointVelocity(_player.transform.position) * 0.02f);

                if (ShootClips.Any())
                {
                    var clipToPlay = ShootClips[Random.Range(0, ShootClips.Count)];
                    clipToPlay.pitch = Random.Range(MinShootPitch, MaxShootPitch);
                    clipToPlay.Play();
                }

                yield return new WaitForSeconds(ShootingSpeed);
            }
            IsShooting = false;
			_headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);

            //Reset head flipping
            if (_headObject.transform.localScale.x < 0) {
                TransformHelpers.FlipX(_headObject.gameObject);
            }
        }

		// Set the head to the direction of the shooting
        private void SetHeadDirection(KeyCode shootKey) {
            switch (shootKey) {
                case KeyCode.UpArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Up);
                    break;
                case KeyCode.DownArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);
                    break;
                case KeyCode.LeftArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Left);
                    break;
                case KeyCode.RightArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Right);
                    break;
            }
        }
    }
}
