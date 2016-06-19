using UnityEngine;

namespace Assets.Scripts.Items {
	public class MovementSpeedItem : ItemBase {
		[SerializeField]
		public float _movementSpeedAddition;


		public override bool UseItem(Player player) {
			var tempSpeed = player._moveSpeed + _movementSpeedAddition;

			if (tempSpeed > 15) {
				_movementSpeedAddition = 15 - player._moveSpeed;
			}

			player.GetComponent<Player> ()._moveSpeed += _movementSpeedAddition;
			return true;
		}
	}
}

