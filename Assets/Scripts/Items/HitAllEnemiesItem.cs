using UnityEngine;

namespace Assets.Scripts.Items {
	public class HitAllEnemiesItem : ItemBase {
		//private int damage = player.GetComponent<Player>()._damage;

		public override bool UseItem(Player player) {
            foreach (var enemy in player.CurrentRoom._enemies) {
                //enemy.Health -= damage;
            }

			return true;
        }
    }
}
