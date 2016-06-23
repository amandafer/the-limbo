using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu {
	[RequireComponent(typeof(GUITexture))]
	public class BackButton : ButtonBase {
		protected override void OnButtonClicked() {
			SceneManager.LoadScene("MainMenu");
		}

		public void Update() {
			if (Input.GetKey (KeyCode.Escape))
				SceneManager.LoadScene ("MainMenu");
		}
	}
}

