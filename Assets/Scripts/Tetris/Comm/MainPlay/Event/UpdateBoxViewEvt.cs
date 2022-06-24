using System.Collections.Generic;
using FrameWork;

namespace Tetris {
    public class UpdateBoxViewEvt : Event {
        public StaticBoxInfo[,] StaticBoxInfo { get; }
        public List<DynamicBoxInfo> DynamicBoxInfo { get; }
        public List<DynamicBoxInfo> DynamicBoxShadowInfos { get; }


        public UpdateBoxViewEvt(StaticBoxInfo[,] staticBoxInfo, List<DynamicBoxInfo> dynamicBoxInfo, List<DynamicBoxInfo> dynamicBoxShadowInfos) {
            StaticBoxInfo = staticBoxInfo;
            DynamicBoxInfo = dynamicBoxInfo;
            DynamicBoxShadowInfos = dynamicBoxShadowInfos;
        }
    }
}