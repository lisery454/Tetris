using System.Collections;
using UnityEngine;

namespace Tetris {
    public class Box : MainPlaySceneExhibitor {
        private static float fallInterval = 0.5f;
        private static float CheckHitDistance = 0.6f;
        private static WaitForSeconds fallIntervalSeconds = new WaitForSeconds(fallInterval);

        private void Start() {
            GetComponent<SpriteRenderer>().color = Random.ColorHSV(0, 1, 0, 0.2f, 0.9f, 1f);
            StartCoroutine(Fall());
        }


        private IEnumerator Fall() {
            while (!IsFallOnEntity()) {
                transform.Translate(0, -1, 0);
                yield return fallIntervalSeconds;
            }

            SendCommand<NextBoxCmd>();
        }

        private bool IsFallOnEntity() {
            var hit2D = Physics2D.Raycast(
                transform.position + Vector3.down * CheckHitDistance,
                Vector2.down,
                0.1f,
                LayerMask.GetMask("Entity"));

            if (hit2D) return true;
            else return false;
        }
    }
}