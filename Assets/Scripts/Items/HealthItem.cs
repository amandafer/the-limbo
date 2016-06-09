using Assets.Scripts;
using UnityEngine;

public class HealthItem : ItemBase {

    [SerializeField]
    public int _healthAddition = 2;

    public override bool UseItem(Player player) {
		if (player.Health != player._maxHealth) {
			player.Health += _healthAddition;

			if (player.Health > player._maxHealth)
				player.Health = player._maxHealth;

			return true;
		} else {
			return false;
		}
    }
}
