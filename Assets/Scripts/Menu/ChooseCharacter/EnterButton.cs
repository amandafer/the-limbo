using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu {
	[RequireComponent(typeof(GUITexture))]
	public class EnterButton : ButtonBase {
		public MovieTexture cutscene;

		protected override void OnButtonClicked() {
			SceneManager.LoadScene("ProceduralGeneration");
		}

		public void Update() {
			if (Input.GetKey ("enter") || Input.GetKey ("return")) {
				SceneManager.LoadScene ("CutScene1");
			}
		}
	}
}

