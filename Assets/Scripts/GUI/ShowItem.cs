using UnityEngine;

public class ShowItem : MonoBehaviour {
    private Player _playerCharacter;

    public void Start() {
        _playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	public void OnGUI() {
        if (_playerCharacter == null || _playerCharacter.CurrentItem == null) {
            return;
        }

	    var iconTexture = _playerCharacter.CurrentItem.GetComponent<SpriteRenderer>().sprite.texture;
        var iconWidth = Screen.width / 13;
        var iconHeight = Screen.height / 13;

        GUI.DrawTexture(new Rect(Screen.width - Screen.width / 1.01f, Screen.height / 25.0f, iconWidth, iconHeight), iconTexture, ScaleMode.ScaleToFit);
	}
}
