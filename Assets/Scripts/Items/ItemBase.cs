using UnityEngine;
using System.Collections;

namespace Assets.Scripts {
    public abstract class ItemBase : MonoBehaviour {
        [SerializeField]
        public AudioSource _spawnClip;
        public AudioSource _pickUpClip;
        public float MinSpawnPitch = -3.0f, MaxSpawnPitch = 3.0f;
        public bool _isInstantEffect;

        public virtual bool IsInstantlyDestroyedAfterUse { get { return true; } }

        public void Start() {
            if (_spawnClip != null) {
                _spawnClip.pitch = Random.Range(MinSpawnPitch, MaxSpawnPitch);
                _spawnClip.Play();
            }
        }


        public void OnCollisionEnter2D(Collision2D collision) {
            GameObject o = collision.gameObject;

            if (o.CompareTag("Player")) {
                OnPickUp(o.GetComponent<Player>());
                //Destroy(gameObject);
            }
        }

        public void Enable() {
            enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        public void Disable() {
            enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }


        private void OnPickUp(Player player) {
            if (_pickUpClip != null)
                _pickUpClip.Play();
            player.OnPickUp(this);
        }

		public void ShowItemText() {
			StartCoroutine (itemText());
		}
		
		public IEnumerator itemText() {
			var nameItemText = GameObject.FindGameObjectWithTag ("NameItemText").GetComponent<GUIText> ();
			var effectItemText = GameObject.FindGameObjectWithTag ("EffectItemText").GetComponent<GUIText> ();

			nameItemText.enabled = true;
			nameItemText.text = this.name;
			effectItemText.enabled = true;
			effectItemText.text = this.GetComponent<GUIText> ().text;

			yield return new WaitForSeconds(1.0f) ;
			nameItemText.GetComponent<GUIText> ().enabled = false;
			effectItemText.GetComponent<GUIText> ().enabled = false;
		}


        public abstract bool UseItem(Player player);
    }
}
