using FrameWork;

namespace Tetris {
    public abstract class StartUISceneLeaf : AbstractLeaf{
        protected virtual void Awake() {
            BelongedStem = TetrisPlant.Instance.StartUIStem;
        }
    }
    
    public abstract class MainPlaySceneLeaf : AbstractLeaf{
        protected virtual void Awake() {
            BelongedStem = TetrisPlant.Instance.TetrisGameStem;
        }
    }
}