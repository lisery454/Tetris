using UnityEngine;

namespace Tetris {
    public class Box : MonoBehaviour {
        private SpriteRenderer _spriteRenderer;

        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetBoxColorAndInfo(Color color, bool isActive) {
            _spriteRenderer.enabled = isActive;
            _spriteRenderer.color = color;
        }
    }
}