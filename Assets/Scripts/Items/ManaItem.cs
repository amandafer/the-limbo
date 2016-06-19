using Assets.Scripts;
using UnityEngine;

namespace Assets.Scripts.Items {
	public class ManaItem : ItemBase {
		private int _healthAddition;

		public override bool UseItem(Player player) {
			if (player.name == "Samael") {
				_healthAddition = 2;
			} else {
				_healthAddition = 1;
			}

			if (player.Health != player._maxHealth) {
				player.Health += _healthAddition;

				if (player.Health > player._maxHealth)
					player.Health = player._maxHealth;

				ShowItemText ();
				return true;
			} else {
				return false;
			}
		}
	}
}

