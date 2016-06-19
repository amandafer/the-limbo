using System;

namespace Assets.Scripts.Items {
	public class DamageItem : ItemBase {
		public int damageAddition;

		public override bool UseItem(Player player) {
			/*
			var tempDamage = player._moveSpeed + _movementSpeedAddition;

			if (tempDamage > 10) {
				damageAddition = 10 - player._moveSpeed;
			} else if (tempDamage < ) {
				damageAddition = player._moveSpeed - ;
			}

			player.GetComponent<Player> ()._damage += damageAddition;
			*/
			ShowItemText ();
			return true;
		}
	}
}

