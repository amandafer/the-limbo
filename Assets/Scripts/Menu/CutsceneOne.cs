using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof(Animator))]

public class CutsceneOne : MonoBehaviour {
	void Update() {
		var cutscene = GameObject.FindGameObjectWithTag ("cutscene");

		StartCoroutine (WaitForAnimation(cutscene));

		if (!cutscene.GetComponent<AudioSource> ().isPlaying ||
			Input.GetKeyUp("return") || Input.GetKey(KeyCode.KeypadEnter)) {
			SceneManager.LoadScene ("ProceduralGeneration");
		}
	}

	private IEnumerator WaitForAnimation (GameObject animator) {
		yield return new WaitForSeconds (6.25f);

		animator.GetComponent<Animator> ().enabled = false;
		animator.GetComponent<SpriteRenderer> ().enabled = false;
	}
}