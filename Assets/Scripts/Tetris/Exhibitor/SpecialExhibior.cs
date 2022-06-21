using FrameWork;
using UnityEngine;

namespace Tetris {
    //预先定义了每个场景中使用的exhibitor

    public abstract class StartUISceneExhibitor : Exhibitor {
        protected virtual void Awake() {
            BelongedLeader = FindObjectOfType<TetrisGame>().startUILeader;
        }
    }

    public abstract class MainPlaySceneExhibitor : Exhibitor {
        protected virtual void Awake() {
            BelongedLeader = FindObjectOfType<TetrisGame>().tetrisGameLeader;
        }
    }

    public abstract class EndUISceneExhibitor : Exhibitor {
        protected virtual void Awake() {
            BelongedLeader = FindObjectOfType<TetrisGame>().endUILeader;
        }
    }
}