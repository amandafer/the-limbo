using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.Items {
	public class DamageItem : ItemBase {
		public int damageAddition;

		private Room itemUsedRoom;

		public override bool UseItem(Player player) {
			var tempDamage = player._damage + damageAddition;

			if (tempDamage > 10) {
				damageAddition = 10 - player._damage;
			} else if (tempDamage < 1) {
				damageAddition = player._damage - 1;
			}

			player._damage += damageAddition;
			itemUsedRoom = player.CurrentRoom;


			if (this._isInstantEffect == false) {
				StartCoroutine (temporaryAddDamage (player.CurrentRoom));
			} else {
				ShowItemText ();
			}

			return true;
		}

		private IEnumerator temporaryAddDamage(Room currentRoom) {
			var _currentRoom = currentRoom;

			while (_currentRoom == currentRoom) {
				Debug.Log ("player current room = " + currentRoom.name);
				yield return null;
			}
			//Destroy(gameObject);
		}

	}
}

