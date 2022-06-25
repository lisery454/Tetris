using UnityEngine;

namespace FrameWork {
    [CreateAssetMenu(menuName = "Sound/Create SoundClip ")]
    public class SoundClip : ScriptableObject {
        [SerializeField] public string clipName;
        [SerializeField] public AudioClip clip;
        [SerializeField] public float volume;
        [SerializeField] public bool isLoop;
    }
}