using System;

namespace Assets.Scripts.Items {
	public class DamageItem : ItemBase {
		public int damageAddition;

		public override bool UseItem(Player player) {
			var tempDamage = player._damage + damageAddition;

			if (tempDamage > 10) {
				damageAddition = 10 - player._damage;
			} else if (tempDamage < 1) {
				damageAddition = player._damage - 1;
			}

			player.GetComponent<Player> ()._damage += damageAddition;
			ShowItemText ();
			return true;
		}
	}
}

