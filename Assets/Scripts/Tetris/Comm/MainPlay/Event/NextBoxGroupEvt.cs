using System.Collections.Generic;
using FrameWork;

namespace Tetris {
    public class NextBoxGroupEvt : Event {
        public List<DynamicBoxInfo> NextBoxGroup { get; }

        public NextBoxGroupEvt(List<DynamicBoxInfo> nextBoxGroup) {
            NextBoxGroup = nextBoxGroup;
        }
    }
}