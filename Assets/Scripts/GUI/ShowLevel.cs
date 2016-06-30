using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowLevel : MonoBehaviour {
	private float time = 1.0f;
	private FloorGenerator floorGenerator;
	private GUIText levelText;
	private GameObject[] cutscene;
	private GameObject player;
	private AudioSource[] audioSources;

	public IEnumerator Start () {
		floorGenerator = GameObject.FindGameObjectWithTag ("GameController").GetComponent<FloorGenerator> ();
		levelText = GameObject.FindGameObjectWithTag ("LevelText").GetComponent<GUIText> ();
		player = GameObject.FindGameObjectWithTag ("Player");

		// Initiate cutscene if player got to fourth level
		if (floorGenerator.level == 4) {
			cutscene = GameObject.FindGameObjectsWithTag ("cutscene");
			audioSources = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
			audioSources.ElementAt(0).Stop();

			yield return new WaitForSeconds (0.5f);
			player.SetActive (false);

			showHideCutscene (true);
			yield return new WaitForSeconds (5f);
			showHideCutscene (false);
			player.SetActive (true);

			audioSources.ElementAt (0).Play ();
		}

		// If cutscenes ends, the level is shown
		levelText.enabled = true;
		levelText.text = "Level " + floorGenerator.level;
		yield return new WaitForSeconds (time);
		levelText.GetComponent<GUIText> ().enabled = false;
		yield return null;
	}

	public void showHideCutscene (bool isShowing) {
		foreach (GameObject objects in cutscene) {
			objects.GetComponent<SpriteRenderer> ().enabled = isShowing;

			if (objects.name == "Cutscene2") {
				objects.GetComponents<AudioSource> ().ElementAt (0).enabled = isShowing;
				objects.GetComponents<AudioSource> ().ElementAt (1).enabled = isShowing;
				objects.GetComponents<AudioSource> ().ElementAt(0).Play();
				objects.GetComponents<AudioSource> ().ElementAt(1).Play();
			} else if (objects.name == "HeartBeat")
				objects.GetComponent<Animator> ().enabled = isShowing;
		}
	}
}