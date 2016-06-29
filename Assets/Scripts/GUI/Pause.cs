using System.Collections;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
	private GameObject[] pauseObjects;
	private Player player;
	private PlayerShootController playerController;

	public void Start () {
		Time.timeScale = 1;
		pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShootController> ();
		hidePaused();
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if(Time.timeScale == 1) {
				Time.timeScale = 0;
				showPaused();
			} else if (Time.timeScale == 0) {
				Time.timeScale = 1;
				hidePaused();
			}
		}
	}

	//shows objects with ShowOnPause tag
	public void showPaused(){
		foreach(GameObject g in pauseObjects){
			if (g.GetComponent<GUIText> ()) {
				//levelText.enabled = true;
				g.GetComponent<GUIText> ().text = player.GetComponent<Player> ()._damage + "\n" +
												  (player._moveSpeed - 7) + "\n" +
												  (10 - 10*playerController._shootSpeed) + "\n" +
												  player._range;
			}
			g.SetActive(true);
		}
	}

	//hides objects with ShowOnPause tag
	public void hidePaused(){
		foreach(GameObject g in pauseObjects){
			g.SetActive(false);
		}
	}
}

