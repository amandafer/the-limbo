using System;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Items {
	public class Invulnearable : ItemBase {

		public override bool IsInstantlyDestroyedAfterUse {
			get { return false; }
		}

		public override bool UseItem(Player player) {
			StartCoroutine (Invulnerability (player));
			return true;
		}

		private IEnumerator Invulnerability(Player player) {
			player._invulnerable = true;

			player._shieldObject.GetComponent<SpriteRenderer>().enabled = true;
			player._shieldObject.GetComponent<CircleCollider2D>().enabled = true;
			yield return new WaitForSeconds (2);
			player._shieldObject.GetComponent<SpriteRenderer>().enabled = false;
			yield return new WaitForSeconds (0.2f);
			player._shieldObject.GetComponent<SpriteRenderer>().enabled = true;
			yield return new WaitForSeconds (0.5f);
			player._shieldObject.GetComponent<SpriteRenderer>().enabled = false;
			player._shieldObject.GetComponent<CircleCollider2D>().enabled = false;

			player._invulnerable = false;
		}
	}
}