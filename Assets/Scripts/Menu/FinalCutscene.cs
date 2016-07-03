using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof(Animator))]

public class FinalCutscene : MonoBehaviour {
	void Update() {
		var cutscene = GameObject.FindGameObjectsWithTag ("cutscene");

		StartCoroutine (WaitForAnimation(cutscene));

		foreach (GameObject objects in cutscene) {
			if (objects.name.Contains ("CutsceneFinal")) {
				if (!objects.GetComponent<AudioSource> ().isPlaying ||
				   Input.GetKeyUp ("return") || Input.GetKey (KeyCode.KeypadEnter)) {
					SceneManager.LoadScene ("MainMenu");
				}
			}
		}
	}

	private IEnumerator WaitForAnimation (GameObject[] cutscene) {
		yield return new WaitForSeconds (4f);

		foreach (GameObject objects in cutscene) {
			if (objects.name.Contains("CutsceneFinal")) {
				objects.GetComponent<Animator> ().enabled = true;
				objects.GetComponent<Animator> ().Play ("FinalCutscene");
			} 
		}

		yield return new WaitForSeconds (6f);
		foreach (GameObject objects in cutscene) {
			if (objects.name.Contains ("CutsceneFinal")) {
				objects.GetComponent<Animator> ().enabled = false;
				objects.GetComponent<SpriteRenderer> ().enabled = false;
				//objects.GetComponent<Animator> ().Play ("FinalCutscene");
			} else if (objects.name == "HeartBeat") {
				objects.GetComponent<AudioSource> ().enabled = false;
			} else if (objects.name == "Created") {
				objects.GetComponent<SpriteRenderer> ().enabled = true;
			}
		}

	}
}
