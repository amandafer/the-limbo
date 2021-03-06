﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Menu {
	public class ArrowsButton : ButtonBase {
		public List<Player> _charactersPrefab = new List<Player>();
		public List<GameObject> _charactersCarousel = new List<GameObject> ();
		protected int _numberCharacterSelected = 1;
		public Player choosenCharacter;

		protected override void OnButtonClicked() {
			//EditorSceneManager.LoadScene("ProceduralGeneration");
		}

		public void Start() {
			choosenCharacter = _charactersPrefab [_numberCharacterSelected];
			GlobalData.choosenCharacter = choosenCharacter;
		}

		public void Update() {
			arrowController ();

			Player _object = _charactersPrefab [_numberCharacterSelected];
			foreach (var character in _charactersCarousel) {
				if (character.name == _object.name) {
					character.GetComponent<GUITexture> ().enabled = true;
				} else {
					character.GetComponent<GUITexture> ().enabled = false;
				}
			}
		}

		public void arrowController () {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				int temp = _numberCharacterSelected - 1;

				if (temp < 0) {
					choosenCharacter = _charactersPrefab [_charactersPrefab.Count - 1];
					_numberCharacterSelected = _charactersPrefab.Count - 1;
				} else {
					choosenCharacter = _charactersPrefab [temp];
					_numberCharacterSelected -= 1;
				}

				GlobalData.choosenCharacter = choosenCharacter;
				//Debug.Log ("Number: " + _numberCharacterSelected + " and choosen character is: " + choosenCharacter.name);
			} /*else if (Input.GetKey (KeyCode.RightArrow)) {
				int temp = _numberCharacterSelected + 1;

				if (temp > 2) {
					choosenCharacter = _charactersPrefab [0];
					_numberCharacterSelected = 0;
				} else {
					choosenCharacter = _charactersPrefab [temp];
					_numberCharacterSelected += 1;
				}
				Debug.Log ("Number: " + _numberCharacterSelected + " and choosen character is: " + choosenCharacter.name);
			}
		*/}
	}
}

