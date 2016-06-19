using UnityEngine;

namespace Assets.Scripts.Items {
    public class ShootSpeedItem : ItemBase {
        public float shootSpeedReduction;

		// Shoot speed is inversily proportional to the speed of the shot. 
        public override bool UseItem(Player player) {
			var tempShootSpeed = player.GetComponent<PlayerShootController>()._shootSpeed - shootSpeedReduction;

			if (tempShootSpeed > 1) {
				shootSpeedReduction = -(1 - player.GetComponent<PlayerShootController>()._shootSpeed);
			} else if (tempShootSpeed < 0.1f) {
				shootSpeedReduction = player.GetComponent<PlayerShootController>()._shootSpeed - 0.1f;
			}

			player.GetComponent<PlayerShootController>()._shootSpeed -= shootSpeedReduction;
			return true;
        }
    }
}
