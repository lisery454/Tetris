using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class SliderText : MonoBehaviour {
        private Text ValueText { get; set; }
        public Text TitleText { get; set; }
        private Slider slider { get; set; }


        private void Awake() {
            ValueText = transform.Find("Value").GetComponent<Text>();
            TitleText = transform.Find("Title").GetComponent<Text>();
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(value => { ValueText.text = ((int) value).ToString(); });
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