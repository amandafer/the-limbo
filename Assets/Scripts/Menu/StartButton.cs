using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(GUITexture))]
    public class StartButton : ButtonBase
    {
        protected override void OnButtonClicked()
        {
			//EditorSceneManager.LoadScene("ProceduralGenerationTest");
        }
    }
}
