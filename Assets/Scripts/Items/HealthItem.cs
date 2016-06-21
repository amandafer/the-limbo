using Assets.Scripts;
using UnityEngine;

namespace Assets.Scripts.Items {
	public class HealthItem : ItemBase {
	    public int _healthAddition;

	    public override bool UseItem(Player player) {
			if (player.Health != player._maxHealth &&
				!player.name.Contains("Samael") || _healthAddition < 0 || !this._isInstantEffect) {
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
