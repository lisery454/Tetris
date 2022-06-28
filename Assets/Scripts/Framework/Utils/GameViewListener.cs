using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public class GameViewListener {
        public int lastWidth { get; private set; }
        public int lastHeight { get; private set; }

        public void ViewListenerUpdate() {
            if (lastWidth != Screen.width || lastHeight != Screen.height) {
                lastWidth = Screen.width;
                lastHeight = Screen.height;

                var sceneName = SceneManager.GetActiveScene().name;
                if (OnViewChange.ContainsKey(sceneName))
                    OnViewChange[sceneName].Invoke(lastWidth, lastHeight);
            }
        }

        //当某个场景中，分辨率改变时时启用的操作
        //先宽后高
        public readonly Dictionary<string, Action<int, int>> OnViewChange =
            new Dictionary<string, Action<int, int>>();
    }
}