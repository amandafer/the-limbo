using UnityEngine;

namespace Assets.Scripts.Items {
    public class MaxHealthItem : ItemBase {
        [SerializeField]
        private int _maxHealthAddition = 2;

        public override bool UseItem(Player player) {
        	if (player._maxHealth <= 20) {
            	player._maxHealth += _maxHealthAddition;
            	return true;
            } else {
            	return false;
            }
        }
    }
}
