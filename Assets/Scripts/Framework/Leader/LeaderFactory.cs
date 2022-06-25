using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public class LeaderFactory {
        private readonly IGame BelongedGame;

        private Dictionary<string, Leader> Leaders;

        public LeaderFactory(IGame belongedGame) {
            BelongedGame = belongedGame;
            Leaders = new Dictionary<string, Leader>();
        }

        public Leader CreateLeader(string sceneName) {
            var leader = new Leader(BelongedGame);
            Leaders[sceneName] = leader;
            return leader;
        }

        public Leader GetLeader(string sceneName) {
            if (Leaders.ContainsKey(sceneName)) {
                return Leaders[sceneName];
            }
            else return null;
        }

        public Leader GetCurrentLeader() {
            var currentLeader = GetLeader(SceneManager.GetActiveScene().name);
            return currentLeader ?? null;
        }

        public void RemoveLeader(string sceneName) {
            if (Leaders.ContainsKey(sceneName)) {
                Leaders.Remove(sceneName);
            }
        }
    }
}