using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssemblyCSharp {
	public class Audio : MonoBehaviour{
		//public AudioSource audio;
		static bool AudioBegin = false; 

		void Awake() {
			if (!AudioBegin) {
				GetComponent<AudioSource>().Play ();
				DontDestroyOnLoad (gameObject);
				AudioBegin = true;
			} 
		}

		void Update () {
			if(SceneManager.GetActiveScene().name == "CutScene1") {
				GetComponent<AudioSource>().Stop();
				AudioBegin = false;
			}
		}
	}
}

