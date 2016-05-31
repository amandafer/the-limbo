using Assets.Scripts;
using UnityEngine;

public class HealthItem : ItemBase {

    [SerializeField]
    private int _healthAddition = 2;
    public int HealthAddition {
        get { return _healthAddition; }
        set { _healthAddition = value; }
    }

    public override bool UseItem(Player player) {
		if (player.Health != player._maxHealth) {
			player.Health += HealthAddition;

			if (player.Health > player._maxHealth)
				player.Health = player._maxHealth;

			return true;
		} else {
			return false;
		}
    }
}
