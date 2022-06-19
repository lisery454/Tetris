using FrameWork;

namespace Tetris {
    //预先定义了每个场景中使用的exhibitor
    
    public abstract class StartUISceneExhibitor : AbstractExhibitor{
        protected virtual void Awake() {
            belongedLeader = TetrisGame.Instance.startUILeader;
        }
    }
    
    public abstract class MainPlaySceneExhibitor : AbstractExhibitor{
        protected virtual void Awake() {
            belongedLeader = TetrisGame.Instance.tetrisGameLeader;
        }
    }
}