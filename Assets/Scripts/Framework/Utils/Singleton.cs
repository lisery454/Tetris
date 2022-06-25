using UnityEngine;

namespace FrameWork {
    public class Singleton<T> : MonoBehaviour where T : Component {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType(typeof(T)) as T;
                    if (_instance == null) {
                        var obj = new GameObject();
                        DontDestroyOnLoad(obj);
                        obj.gameObject.name = typeof(T).Name;
                        _instance = obj.AddComponent(typeof(T)) as T;
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake() {
            DontDestroyOnLoad(gameObject);
            if (_instance == null) {
                _instance = this as T;
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}