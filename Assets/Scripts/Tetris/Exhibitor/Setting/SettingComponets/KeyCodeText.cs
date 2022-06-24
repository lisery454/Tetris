using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyCodeText : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    private Text keyValueText;
    [HideInInspector] public Text keyNameText;
    private KeyCode keyCode;
    private Image image;


    private Color originalColor;
    private readonly Color selectedColor = new Color(1f, 0.8f, 0.8f);

    public KeyCode KeyCode {
        get => keyCode;

        set {
            keyCode = value;
            keyValueText.text = value.ToString();
        }
    }

    private void Awake() {
        image = GetComponent<Image>();
        originalColor = image.color;

        keyValueText = transform.Find("KeyValueText").GetComponent<Text>();
        keyNameText = transform.Find("KeyNameText").GetComponent<Text>();
    }

    private bool isListenCode;


    public void OnPointerClick(PointerEventData eventData) {
        if (!isListenCode) {
            isListenCode = true;
            image.color = selectedColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }


    private void OnGUI() {
        if (isListenCode) {
            if (Input.anyKeyDown) {
                isListenCode = false;
                KeyCode = Event.current.keyCode;
                image.color = originalColor;
            }
        }
    }
}