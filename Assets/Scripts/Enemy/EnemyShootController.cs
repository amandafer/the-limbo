﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class EnemyShootController : ShootControllerBase {
		private Vector2 _shootDirection;
		private bool _shooting;
		private GameObject _player;

        public bool _boss = false;
		public bool _canShoot = true;
		public bool _explodes = false;
		public bool _shootSpeedVaries = false;

		public override void Start() {
			base.Start ();
			_player = GameObject.FindGameObjectWithTag ("Player");
			_shooting = false;
            //BulletSpeed = 0.5f;
            //ShootingSpeed = 0.1f;
		}

		public override void Update() {
			base.Update ();

			if (CanShootPlayer ()) {
				StartCoroutine(Shoot());
			}
		}

		IEnumerator Shoot() {
			if (!_shooting) {
				_shooting = true;
				var bullet = (Rigidbody2D)Instantiate (BulletPrefab);
				bullet.GetComponent<BulletScript>().Shooter = transform.gameObject;
				bullet.transform.position = transform.position;

				if (_shootDirection.y > 0) {
					bullet.transform.Rotate (0, 0, -90);
				} else if (_shootDirection.y < 0) {
					bullet.transform.Rotate (0, 0, 90);
				} else if (_shootDirection.x > 0) {
					TransformHelpers.FlipX (bullet.gameObject);
				}
				bullet.AddForce (_shootDirection);

			    if (ShootClips.Any()) {
					int bossHax = 0;

					if (_boss)
						bossHax = 4;
			        
					var clipToPlay = ShootClips[Random.Range(bossHax, ShootClips.Count)];
			        clipToPlay.pitch = Random.Range(MinShootPitch, MaxShootPitch);
                    clipToPlay.Play();
			    }

				//enemies with shoot speed that varies
				if (_shootSpeedVaries) {
					if (_shootSpeed == 0) {
						yield return new WaitForSeconds (0);
						_shootSpeed = 0.3f;
					} else {
						yield return new WaitForSeconds (_shootSpeed * 3);
						_shootSpeed = 0;
					}
				} else {
					yield return new WaitForSeconds (_shootSpeed * 3);
				}
				_shooting = false;
			}
		}

        public void BossShoot() {
           // GetComponent<Animator>().SetBool("Shooting", true);
            var targets = new Vector3[9];
            var positions = new Vector3[9];
            positions[0] = new Vector3(-0.35f, 0.84f, 0);
            positions[1] = new Vector3(0.40f, 0.84f, 0);

            positions[2] = new Vector3(-0.73f, 0.28f, 0);
            positions[3] = new Vector3(0, 0.28f, 0);
            positions[4] = new Vector3(0.74f, 0.28f, 0);

            positions[5] = new Vector3(-1.03f, -0.29f, 0);
            positions[6] = new Vector3(-0.32f, -0.29f, 0);
            positions[7] = new Vector3(0.41f, -0.29f, 0);
            positions[8] = new Vector3(1, -0.29f, 0);

            for (int i = 0; i < 9; i++) {
                targets[i] = _player.transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f), 0);

                Vector2 shootDirection = targets[i] - (transform.position + positions[i]);
                var bullet = (Rigidbody2D)Instantiate(BulletPrefab);
                bullet.GetComponent<BulletScript>().Shooter = transform.gameObject;
                bullet.transform.position = transform.position + positions[i];

                shootDirection.Normalize();
                shootDirection.Scale(new Vector2(_bulletSpeed, _bulletSpeed));
                bullet.AddForce(shootDirection);
            }

            if (ShootClips.Any()) {
                var clipToPlay = ShootClips[Random.Range(4, ShootClips.Count)];
                clipToPlay.pitch = Random.Range(MinShootPitch, MaxShootPitch);
                clipToPlay.Play();
            }
        }

        public void BossExplode() {
			if (_explodes) {
				// GetComponent<Animator>().SetBool("Shooting", true);
				var targets = new Vector3[9];
				var positions = new Vector3[9];
				positions [0] = new Vector3 (-0.35f, 0.84f, 0);
				positions [1] = new Vector3 (0.40f, 0.84f, 0);

				positions [2] = new Vector3 (-0.73f, 0.28f, 0);
				positions [3] = new Vector3 (0, 0.28f, 0);
				positions [4] = new Vector3 (0.74f, 0.28f, 0);

				positions [5] = new Vector3 (-1.03f, -0.29f, 0);
				positions [6] = new Vector3 (-0.32f, -0.29f, 0);
				positions [7] = new Vector3 (0.41f, -0.29f, 0);
				positions [8] = new Vector3 (1, -0.29f, 0);

				for (int i = 0; i < 9; i++) {
					targets [i] = _player.transform.position + new Vector3 (UnityEngine.Random.Range (-1.5f, 1.5f), UnityEngine.Random.Range (-1.5f, 1.5f), 0);

					var shootDirection = UnityEngine.Random.onUnitSphere - positions [i];
					shootDirection.z = 0;
					var bullet = (Rigidbody2D)Instantiate (BulletPrefab);
					bullet.GetComponent<BulletScript> ().Shooter = transform.gameObject;
					bullet.transform.position = transform.position + positions [i];

					shootDirection.Normalize ();
					shootDirection.Scale (new Vector2 (_bulletSpeed, _bulletSpeed));
					bullet.AddForce (shootDirection);
				}
  
				if (ShootClips.Any ()) {
					var clipToPlay = ShootClips [Random.Range (4, ShootClips.Count)];
					clipToPlay.pitch = Random.Range (MinShootPitch, MaxShootPitch);
					clipToPlay.Play ();
				}
			}
        }
		
		private bool CanShootPlayer() {
			if (_canShoot) {
				_shootDirection = - (transform.position - _player.transform.position);
				//_shootDirection = Vector3.zero - _player.transform.position;
				_shootDirection.Normalize ();
				_shootDirection.Scale (new Vector2 (_bulletSpeed, _bulletSpeed));
				return true;
			}
			return false;
		}
	}
}
