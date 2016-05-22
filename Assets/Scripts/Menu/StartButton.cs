using UnityEngine;
using UnityEditor.SceneManagement;

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(GUITexture))]
    public class StartButton : ButtonBase
    {
        protected override void OnButtonClicked()
        {
			EditorSceneManager.LoadScene("ProceduralGenerationTest");
        }
    }
}
