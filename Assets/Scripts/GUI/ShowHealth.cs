using UnityEngine;

public class ShowHealth : MonoBehaviour {
	public Texture2D _healthTexture, _halfHealthTexture, _emptyHealthTexture, _blueHealthTexture, _blueHalfTexture;

    private Player _playerCharacter;

    public void Start() {
        _playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void OnGUI() {
        if (_playerCharacter == null)
            return;

		Texture2D fullHeart, halfHeart;
        var heartWidth = Screen.width / 20;
        var heartHeight = Screen.height / 20;
		var origLeft = Screen.width - Screen.width / 1.1f;
		var left = Screen.width - Screen.width/1.1f;
        var top = Screen.height/30.0f;

		int i;

		// The player can only have 10 hearts
		if (_playerCharacter.Health > _playerCharacter._maxHealth)
			_playerCharacter.Health = _playerCharacter._maxHealth;

		// Check who is the player and sets the heart GUI accordingly
		if (_playerCharacter.name == "Samael") {
			fullHeart = _blueHealthTexture; 
			halfHeart = _blueHalfTexture;
		} else {
			fullHeart = _healthTexture; 
			halfHeart = _halfHealthTexture;
		}

        for (i = 1; i <= _playerCharacter.Health / 2; i++) {
            GUI.DrawTexture(new Rect(left, top, heartWidth, heartHeight), fullHeart, ScaleMode.ScaleToFit);
            left += heartWidth;

            if (i % 5 == 0) {
                left = origLeft;
                top += heartHeight;
            }

        }

        if (_playerCharacter.Health % 2 == 1) {
            GUI.DrawTexture(new Rect(left, top, heartWidth, heartHeight), halfHeart, ScaleMode.ScaleToFit);
            left += heartWidth;

			if (i % 5 == 0) {
				left = origLeft;
				top += heartHeight;
			}
            i++;
        }
			
        for (; i <= _playerCharacter._maxHealth / 2; i++) {
            GUI.DrawTexture(new Rect(left, top, heartWidth, heartHeight), _emptyHealthTexture, ScaleMode.ScaleToFit);
            left += heartWidth;

            if (i > 0 && i % 5 == 0) {
                left = origLeft;
                top += heartHeight;
            }
        }
        
    }
}
