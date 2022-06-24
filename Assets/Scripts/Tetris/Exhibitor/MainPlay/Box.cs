using UnityEngine;

namespace Tetris {
    public enum BoxState {
        Normal,
        Ghost
    }


    public class Box : MonoBehaviour {
        [SerializeField] private Sprite GhostSprite;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _stateSpriteRenderer;
        

        public void SetBoxColorAndInfo(Color color, bool isActive) {
            _spriteRenderer.enabled = isActive;
            _spriteRenderer.color = color;
        }

        public void SetBoxState(BoxState boxState) {
            _stateSpriteRenderer.sprite = boxState switch {
                BoxState.Normal => null,
                BoxState.Ghost => GhostSprite,
                _ => _stateSpriteRenderer.sprite
            };
        }
    }
}