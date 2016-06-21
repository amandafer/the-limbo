using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu {
	[RequireComponent(typeof(GUITexture))]
	public class EnterButton : ButtonBase {
		protected override void OnButtonClicked() {
			SceneManager.LoadScene("ProceduralGeneration");
		}

		public void FixedUpdate() {
			if (Input.GetKey ("enter") || Input.GetKey ("return"))
				SceneManager.LoadScene ("ProceduralGeneration");
		}
	}
}

