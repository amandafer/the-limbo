using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Menu {
    [RequireComponent(typeof(GUITexture))]
    public abstract class ButtonBase : MonoBehaviour {
        public Texture Texture, HoverTexture;
        public AudioSource ClickClip;

        public void OnMouseEnter() {
            GetComponent<GUITexture>().texture = HoverTexture;
        }

        public void OnMouseExit() {
            GetComponent<GUITexture>().texture = Texture;
        }

        public void OnMouseUp() {
            if (ClickClip != null) {
                ClickClip.Play();
            }
            OnButtonClicked();
        }

		//TODO: make it better
        protected abstract void OnButtonClicked();
    }
}
