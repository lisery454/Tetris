using FrameWork;

namespace Tetris {
    public class UpdateBoxViewEvt : AbstractEvent {
        public BoxInfo[,] BoxInfos;

        public UpdateBoxViewEvt(BoxInfo[,] boxInfos) {
            BoxInfos = boxInfos;
        }
    }
}