using System.Collections.Generic;
using FrameWork;

namespace Tetris {
    public class UpdateBoxViewEvt : AbstractEvent {
        public StaticBoxInfo[,] StaticBoxInfo { get; }
        public List<DynamicBoxInfo> DynamicBoxInfo { get; }

        public UpdateBoxViewEvt(StaticBoxInfo[,] staticBoxInfo, List<DynamicBoxInfo> dynamicBoxInfo) {
            StaticBoxInfo = staticBoxInfo;
            DynamicBoxInfo = dynamicBoxInfo;
        }
    }
}