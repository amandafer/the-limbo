using UnityEngine;

namespace Assets.Scripts.Items {
	public class HitAllEnemiesItem : ItemBase {
		private int damage;

		public override bool UseItem(Player player) {
			damage = player.GetComponent<Player>()._damage;

			if (player.CurrentRoom.ContainsEnemies) {
				foreach (var enemy in player.CurrentRoom._enemies) {
					enemy.Health -= damage;
				}
			} 

			return true;
        }
    }
}
