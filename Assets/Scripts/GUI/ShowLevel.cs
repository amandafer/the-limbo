using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowLevel : MonoBehaviour {
	private float time = 1.0f;
	private FloorGenerator floorGenerator;
	private GUIText levelText;

	public IEnumerator Start () {
		floorGenerator = GameObject.FindGameObjectWithTag ("GameController").GetComponent<FloorGenerator> ();
		levelText = GameObject.FindGameObjectWithTag ("LevelText").GetComponent<GUIText> ();
		levelText.enabled = true;
		levelText.text = "Level " + floorGenerator.level;

		yield return new WaitForSeconds (time);
		levelText.GetComponent<GUIText> ().enabled = false;
		yield return null;
	}
}