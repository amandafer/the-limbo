using System;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Items {
	public class ImmobilizeEnemies : ItemBase {
		public override bool IsInstantlyDestroyedAfterUse
		{
			get { return false; }
		}

		public override bool UseItem(Player player) {
			if (player.CurrentRoom.ContainsEnemies) {
				foreach (var enemy in player.CurrentRoom._enemies) {
					enemy.GetComponent<Enemy> ().enabled = false;
					enemy.GetComponent<EnemyShootController> ().enabled = false;
				}

				StartCoroutine (Wait (player));
			}
			return true;
		}

		private IEnumerator Wait(Player player) {
			yield return new WaitForSeconds (2);

			foreach (var enemy in player.CurrentRoom._enemies) {
				enemy.GetComponent<Enemy> ().enabled = true;
				enemy.GetComponent<EnemyShootController> ().enabled = true;
			}

			Destroy (gameObject);
		}
	}
}

