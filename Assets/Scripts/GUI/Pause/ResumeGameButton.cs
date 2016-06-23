using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu {
	[RequireComponent(typeof(GUITexture))]
	public class ResumeGameButton : ButtonBase {
		private GameObject[] pauseObjects;

		protected override void OnButtonClicked() {
			if (Time.timeScale == 0) {
				Time.timeScale = 1;

				pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");

				foreach(GameObject g in pauseObjects){
					g.SetActive(false);
				}
			}
		}
	}
}


