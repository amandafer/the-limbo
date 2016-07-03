using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu {
	[RequireComponent(typeof(GUITexture))]
	public class QuitGameButton : ButtonBase {
		protected override void OnButtonClicked() {
			Time.timeScale = 1;
			SceneManager.LoadScene("ChooseCharacter");
		}
	}
}

