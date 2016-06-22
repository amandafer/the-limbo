using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.Items {
	public class DamageItem : ItemBase {
		public int damageAddition;

		public override bool IsInstantlyDestroyedAfterUse {
			get { return false; }
		}

		public override bool UseItem(Player player) {
			var tempDamage = player._damage + damageAddition;

			if (tempDamage > 10) {
				damageAddition = 10 - player._damage;
			} else if (tempDamage < 1) {
				damageAddition = player._damage - 1;
			}

			player._damage += damageAddition;

			if (this._isInstantEffect == false) {
				StartCoroutine (temporaryAddDamage (player));
			} else {
				ShowItemText ();
			}

			return true;
		}

		private IEnumerator temporaryAddDamage(Player player) {
			var currentRoom = player.CurrentRoom;

			while (player.CurrentRoom == currentRoom) {
				yield return null;
			}
			Destroy(gameObject);
			player._damage -= damageAddition;
		}

	}
}

