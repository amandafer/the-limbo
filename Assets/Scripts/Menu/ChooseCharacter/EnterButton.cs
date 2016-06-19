using System;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Assets.Scripts.Menu {
	[RequireComponent(typeof(GUITexture))]
	public class EnterButton : ButtonBase {
		

		protected override void OnButtonClicked() {
			EditorSceneManager.LoadScene("ProceduralGeneration");
		}

		public void FixedUpdate() {
			if (Input.GetKey ("enter") || Input.GetKey ("return"))
				


				EditorSceneManager.LoadScene ("ProceduralGeneration");
		}
	}
}

