using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetris {
    public class TetrisLogicOperation : Operation {
        private TetrisGameModel model;
        private GameConfig gameConfig;

        private float time;
        private Queue<Action> OperationQueue;


        public override void Init() {
            model = GetModel<TetrisGameModel>();
            gameConfig = GetConfig<GameConfig>();
            OperationQueue = new Queue<Action>();
        }

        protected override void OnUpdate() {
            time += Time.deltaTime;
            if (time > gameConfig.FallInterval) {
                time -= gameConfig.FallInterval;

                OperationQueue.Enqueue(MoveDownOneGrid);
            }

            while (OperationQueue.Count > 0) {
                OperationQueue.Dequeue().Invoke();
            }
        }

        public void StartGame() {
            CreateNextDynamicBoxGroup();

            NextBoxGroup();

            TriggerEvent(new UpdateBoxViewEvt(model.StaticBoxInfos, model.DynamicBoxInfos));

            Update += OnUpdate;
        }

        /// <summary>
        /// 移动动态方块
        /// </summary>
        /// <param name="moveDir"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void MoveBox(MoveDir moveDir) {
            switch (moveDir) {
                case MoveDir.Down:
                    OperationQueue.Enqueue(MoveDown);
                    break;
                case MoveDir.Left:
                    OperationQueue.Enqueue(MoveLeft);
                    break;
                case MoveDir.Right:
                    OperationQueue.Enqueue(MoveRight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveDir), moveDir, null);
            }
        }

        /// <summary>
        /// 向右移
        /// </summary>
        private void MoveRight() {
            //判断是不是到边了或者碰到方块了
            if (model.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.X + 1 >= gameConfig.Width ||
                    model.StaticBoxInfos[dynamicBoxInfo.X + 1, dynamicBoxInfo.Y].IsBox)) {
                return;
            }

            //不然右移一格
            model.DynamicBoxInfos.ForEach(info => { info.X++; });
            model.DynamicBoxGroupRotCenter.X++;

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(model.StaticBoxInfos, model.DynamicBoxInfos));
        }

        /// <summary>
        /// 向左移
        /// </summary>
        private void MoveLeft() {
            //判断是不是到边了或者碰到方块了
            if (model.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.X - 1 < 0 ||
                    model.StaticBoxInfos[dynamicBoxInfo.X - 1, dynamicBoxInfo.Y].IsBox)) {
                return;
            }

            //不然左移一格
            model.DynamicBoxInfos.ForEach(info => { info.X--; });
            model.DynamicBoxGroupRotCenter.X--;

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(model.StaticBoxInfos, model.DynamicBoxInfos));
        }

        /// <summary>
        /// 直接落地
        /// </summary>
        private void MoveDown() {
            //如果没有落到底部或其他方块上
            while (!IsFallOnBottomOrBox()) {
                //下降一格
                model.DynamicBoxInfos.ForEach(info => { info.Y--; });
                model.DynamicBoxGroupRotCenter.Y--;
            }

            SetDynamicToStatic();
            NextBoxGroup();

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(model.StaticBoxInfos, model.DynamicBoxInfos));
        }

        /// <summary>
        /// 向下移动一格
        /// </summary>
        private void MoveDownOneGrid() {
            //判断是不是到边了或者碰到方块了
            if (!IsFallOnBottomOrBox()) {
                //不然下移一格
                model.DynamicBoxInfos.ForEach(info => { info.Y--; });
                model.DynamicBoxGroupRotCenter.Y--;
            }
            else {
                SetDynamicToStatic();
                NextBoxGroup();
            }


            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(model.StaticBoxInfos, model.DynamicBoxInfos));
        }

        /// <summary>
        /// 判断是不是到底了或者碰到方块了
        /// </summary>
        /// <returns></returns>
        private bool IsFallOnBottomOrBox() {
            return model.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.Y - 1 < 0 ||
                    model.StaticBoxInfos[dynamicBoxInfo.X, dynamicBoxInfo.Y - 1].IsBox);
        }

        /// <summary>
        /// 判断某个坐标是不是到边了或者碰到方块了
        /// </summary>
        /// <returns></returns>
        private bool IsOnBorderOrBox(IntTuple boxLoc) {
            if (boxLoc.X < 0 || boxLoc.X >= gameConfig.Width)
                return true;
            if (boxLoc.Y < 0 || boxLoc.Y >= gameConfig.Height)
                return true;
            if (model.StaticBoxInfos[boxLoc.X, boxLoc.Y].IsBox)
                return true;
            return false;
        }

        /// <summary>
        /// 把动的方块变成静止
        /// </summary>
        private void SetDynamicToStatic() {
            foreach (var info in model.DynamicBoxInfos) {
                model.StaticBoxInfos[info.X, info.Y].IsBox = true;
                model.StaticBoxInfos[info.X, info.Y].Color = info.Color;
            }

            model.DynamicBoxInfos = null;
            model.DynamicBoxGroupRotCenter = null;

            CheckEliminate();
            CheckFail();
        }

        /// <summary>
        /// 判断是否要消除
        /// </summary>
        private void CheckEliminate() {
            for (var h = 0; h < gameConfig.Height;) {
                var isAllLineBox = true;
                var isAllLineNotBox = true;
                //判断一行是不是全是方块
                for (var w = 0; w < gameConfig.Width; w++) {
                    if (model.StaticBoxInfos[w, h].IsBox) continue;
                    isAllLineBox = false;
                    break;
                }

                //判断一行是不是全不是方块
                for (var w = 0; w < gameConfig.Width; w++) {
                    if (!model.StaticBoxInfos[w, h].IsBox) continue;
                    isAllLineNotBox = false;
                    break;
                }

                if (isAllLineBox) {
                    for (var nh = h; nh < gameConfig.Height - 1; nh++) {
                        for (var nw = 0; nw < gameConfig.Width; nw++) {
                            model.StaticBoxInfos[nw, nh] = model.StaticBoxInfos[nw, nh + 1];
                        }
                    }

                    //最后一行
                    for (var nw = 0; nw < gameConfig.Width; nw++) {
                        model.StaticBoxInfos[nw, gameConfig.Height - 1] = new StaticBoxInfo();
                    }

                    GetModel<ScoreModel>().Score += 10;
                    TriggerEvent(new ScoreUpdateEvt(GetModel<ScoreModel>().Score));
                }
                else if (isAllLineNotBox) {
                    break;
                }
                else {
                    h++;
                }
            }
        }

        /// <summary>
        /// 检测是否失败
        /// </summary>
        private void CheckFail() {
            for (var w = 0; w < gameConfig.Width; w++) {
                if (model.StaticBoxInfos[w, gameConfig.LimitHeight].IsBox) {
                    //时间停止流动
                    Update -= OnUpdate;
                    //触发游戏失败事件
                    TriggerEvent<GameEndEvt>();

                    return;
                }
            }
        }

        /// <summary>
        /// 释放下一个方块组
        /// </summary>
        private void NextBoxGroup() {
            model.DynamicBoxInfos = model.NextDynamicBoxInfos;
            model.DynamicBoxGroupRotCenter = model.NextDynamicBoxGroupRotCenter;

            CreateNextDynamicBoxGroup();

            TriggerEvent(new NextBoxGroupEvt(model.NextDynamicBoxInfos));
        }


        private void CreateNextDynamicBoxGroup() {
            var rCount = Random.Range(0, model.BoxGroupPrefabs.Count);
            model.NextDynamicBoxInfos = model.BoxGroupPrefabs[rCount].Clone();
            model.NextDynamicBoxGroupRotCenter = (FloatTuple) model.BoxGroupRotCenter[rCount].Clone();
            var rHue = 1f / 10 * Random.Range(1, 10);
            var rColor = Random.ColorHSV(rHue, rHue, 0.4f, 0.4f, 0.9f, 0.9f);
            model.NextDynamicBoxInfos.ForEach(info => { info.Color = rColor; });
        }

        /// <summary>
        /// 旋转方块
        /// </summary>
        public void Rotate() {
            OperationQueue.Enqueue(RotateDynamicBox);
        }

        /// <summary>
        /// 旋转当前动态方块
        /// </summary>
        private void RotateDynamicBox() {
            //旋转坐标
            var beforeRotLoc = model.DynamicBoxInfos.Select(info => new FloatTuple(info.X, info.Y)).ToList();
            var afterRotLoc = new IntTuple[beforeRotLoc.Count];
            for (var i = 0; i < beforeRotLoc.Count; i++) {
                beforeRotLoc[i] -= model.DynamicBoxGroupRotCenter;
                beforeRotLoc[i] = gameConfig.RotMatrix.Multiply(beforeRotLoc[i]);
                beforeRotLoc[i] += model.DynamicBoxGroupRotCenter;
                afterRotLoc[i] = (IntTuple) beforeRotLoc[i];
            }

            //检测是否会碰到其他物体
            if (afterRotLoc.Any(IsOnBorderOrBox)) return;

            //更新model
            for (var i = 0; i < model.DynamicBoxInfos.Count; i++) {
                model.DynamicBoxInfos[i].X = afterRotLoc[i].X;
                model.DynamicBoxInfos[i].Y = afterRotLoc[i].Y;
            }

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(model.StaticBoxInfos, model.DynamicBoxInfos));
        }
    }
}