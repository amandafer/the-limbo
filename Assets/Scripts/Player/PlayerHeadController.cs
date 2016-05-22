using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerHeadController : MonoBehaviour {
        public Sprite _headFront, _headBack, _headLeft, _headRight;
        public SpriteRenderer _spriteRenderer;

        public void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetHeadDirection(HeadDirection direction) {
            switch (direction) {
                case HeadDirection.Up:
                    _spriteRenderer.sprite = _headBack;
                    break;
                case HeadDirection.Down:
                    _spriteRenderer.sprite = _headFront;
                    break;
                case HeadDirection.Left:
                    _spriteRenderer.sprite = _headLeft;
                    break;
                case HeadDirection.Right:
                    _spriteRenderer.sprite = _headRight;
                    break;
            }
        }

        public enum HeadDirection {
            Up,
            Down,
            Left,
            Right
        }
    }
}
