using Assets.Scripts;
using UnityEngine;

namespace Assets.Scripts.Items {
	public class HealthItem : ItemBase {
	    public int _healthAddition;

	    public override bool UseItem(Player player) {
			var tempHealth = player.Health + _healthAddition;

			if (tempHealth != player._maxHealth &&
				player.name != "Samael") {
				player.Health += _healthAddition;

				if (player.Health > player._maxHealth)
					player.Health = player._maxHealth;

				return true;
			} else {
				return false;
			}
	    }
	}
}
