using System;
using UnityEngine;

public class Pause : MonoBehaviour {
	private GameObject[] pauseObjects;

	public void Start () {
		Time.timeScale = 1;
		pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
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

