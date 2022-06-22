using System.Collections.Generic;
using FrameWork;

namespace Tetris {
    public class NextBoxGroupEvt : Event {
        public List<DynamicBoxInfo> NextDynamicBoxInfos { get; }
        public FloatTuple NextDynamicBoxGroupRotCenter { get; }

        public NextBoxGroupEvt(List<DynamicBoxInfo> nextDynamicBoxInfos, FloatTuple nextDynamicBoxGroupRotCenter) {
            NextDynamicBoxInfos = nextDynamicBoxInfos;
            NextDynamicBoxGroupRotCenter = nextDynamicBoxGroupRotCenter;
        }
    }
}