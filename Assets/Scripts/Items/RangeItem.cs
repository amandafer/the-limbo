using System;

namespace Assets.Scripts.Items {
	public class RangeItem : ItemBase {
		public int rangeAddition;

		public override bool UseItem(Player player) {
			var tempRange = player._range + rangeAddition;

			if (tempRange > 10) {
				rangeAddition = 10 - player._range;
			} else if (tempRange < 2) {
				rangeAddition = player._range - 2;
			}

			player.GetComponent<Player> ()._range += rangeAddition;
			return true;
		}
	}
}

