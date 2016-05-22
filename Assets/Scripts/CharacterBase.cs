using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public abstract class CharacterBase : MonoBehaviour {
		public float _moveSpeed;
		public List<GameObject> _bloodPrefab;
		public int _maxHealth;
		public bool IsDead { get { return _health <= 0; } }
		public int _range = 8;
		public List<AudioSource> _takeDamageClips = new List<AudioSource>(1);
		public float MinDamagedPitch = -3.0f, MaxDamagedPitch = 3.0f;
		public AudioSource _dieClip;
		public float MinDiePitch = -3.0f, MaxDiePitch = 3.0f;
		public bool _mirrorAnimation;

        protected bool ShouldMove;
        protected Animator Animator;
        protected SpriteRenderer SpriteRenderer;

        [SerializeField]
        private int _health = 8;
        public int Health {
            get { return _health; }
            set {
                if (value < _health) {
                    TakeDamage();
                }
                _health = value;

                if (_health <= 0) {
                    Die();
                }
            }
        }

        public virtual void Awake() {
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator.enabled = true;
        }

        public virtual void FixedUpdate()
        {
            var movement = DetermineMovement();
            HandleAnimation(movement);
            HandleMovement(movement);
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);
        }

        protected abstract void HandleMovement(Vector3 movement);

        protected abstract Vector3 DetermineMovement();

        private const float ANIMATION_STOP_VELOCITY = 0.35f;

        protected virtual void HandleAnimation(Vector3 movement) {
            Vector3 currentPosition = transform.position;
			/*
            if (Animator.enabled 
			    && gameObject.GetComponent<Rigidbody2D>().velocity.x < ANIMATION_STOP_VELOCITY 
			    && gameObject.GetComponent<Rigidbody2D>().velocity.x > -ANIMATION_STOP_VELOCITY
                && gameObject.GetComponent<Rigidbody2D>().velocity.y < ANIMATION_STOP_VELOCITY 
			    && gameObject.GetComponent<Rigidbody2D>().velocity.y > -ANIMATION_STOP_VELOCITY)
            {
                //Animator.enabled = false;
                return;
            }*/

            if (!ShouldMove)
                return;

            var target = currentPosition + movement;
            Animator.enabled = true;

            SetAnimationDirection(movement, target, currentPosition);
        }



        protected virtual void TakeDamage() {
            StartCoroutine(DamageBlink());
            if (_takeDamageClips.Any())
            {
                var clipToPlay = _takeDamageClips[Random.Range(0, _takeDamageClips.Count)];
                clipToPlay.pitch = Random.Range(MinDamagedPitch, MaxDamagedPitch);
                clipToPlay.Play();
            }
				
        }

        private IEnumerator DamageBlink() {
            Color c = gameObject.GetComponent<SpriteRenderer>().color;
            ChangeSpriteColorRecursively(Color.red);
            yield return new WaitForSeconds(0.1f);
            ChangeSpriteColorRecursively(c);
        }

        private void ChangeSpriteColorRecursively(Color color) {
            SpriteRenderer.color = color;

            foreach (var childrenSprite in GetComponentsInChildren<SpriteRenderer>()) {
                childrenSprite.color = color;
            }
        }

        protected virtual void Die() {
            if (_dieClip != null) {
                _dieClip.pitch = Random.Range(MinDiePitch, MaxDiePitch);
                _dieClip.Play();
            }
				
        }

        private void SetAnimationDirection(Vector3 movement, Vector3 target, Vector3 currentPosition)
        {
            int animationDirection = Animator.GetInteger("Direction");
            int newDirection = -1;
            if (movement.x != 0.0f && movement.y != 0.0f)
            {
                newDirection = target.y > currentPosition.y ? 0 : 2;
            }
            else if (movement.x != 0.0f && movement.y == 0.0f)
            {
                newDirection = target.x > currentPosition.x ? 1 : 3;
            }
            else if (movement.x == 0.0f && movement.y != 0.0f)
            {
                newDirection = target.y > currentPosition.y ? 0 : 2;
            }

            if (animationDirection != newDirection)
            {
                Animator.SetInteger("Direction", newDirection);
                if (_mirrorAnimation)
                {
                    if ((transform.localScale.x < 0 && newDirection == 3) ||
                        (transform.localScale.x > 0 && newDirection == 1) ||
                        (transform.localScale.y < 0 && newDirection == 0) ||
                        (transform.localScale.y > 0 && newDirection == 2))
                    {
                        TransformHelpers.FlipX(gameObject);
                    }
                }
            }
        }
    }
}
