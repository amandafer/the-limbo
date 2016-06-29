using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu {
    [RequireComponent(typeof(GUITexture))]
    public class StartButton : ButtonBase {
        protected override void OnButtonClicked() {
			SceneManager.LoadScene("ChooseCharacter");
		}
    }
}
