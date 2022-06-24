using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class SliderText : MonoBehaviour {
        private Text text { get; set; }
        private Slider slider { get; set; }

        private void Start() {
            text = GetComponentInChildren<Text>();
            slider = GetComponent<Slider>();

            slider.onValueChanged.AddListener(value => { text.text = ((int) value).ToString(); });
        }

        public float SliderValue {
            get => slider.value;
            set => slider.value = value;
        }

        public void SetMaxAndMin(float max, float min) {
            slider.maxValue = max;
            slider.minValue = min;
        }
    }
}