using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowLevel : MonoBehaviour{
	private float time = 3;
	private FloorGenerator floorGenerator;

	public GUIText levelText;

	IEnumerator Start () {
		floorGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<FloorGenerator>();
		levelText.GetComponent<GUIText> ().enabled = true;
		levelText.text = "Level " + floorGenerator.level;

		yield return new WaitForSeconds(time);
		levelText.GetComponent<GUIText> ().enabled = false;
	}
}