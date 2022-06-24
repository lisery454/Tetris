using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetris {
    public class TetrisLogicOperation : Operation {
        private TetrisGameModel gameModel;
        private ScoreModel scoreModel;
        private GameConfig gameConfig;

        private float time;
        private Queue<Action> OperationQueue;

        private float RHue {
            get {
                rHueNum++;
                if (rHueNum >= colorWheelNum) {
                    rHueNum -= colorWheelNum;
                }

                return rHueNum * 1f / colorWheelNum;
            }
        }

        private int rHueNum;
        private static int colorWheelNum = 12;

        public override void Init() {
            gameModel = GetModel<TetrisGameModel>();
            scoreModel = GetModel<ScoreModel>();
            gameConfig = GetConfig<GameConfig>();
            OperationQueue = new Queue<Action>();
        }

        public void StartGame() {
            CreateNextDynamicBoxGroup();

            NextBoxGroup();

            TriggerEvent(new UpdateBoxViewEvt(gameModel.StaticBoxInfos, gameModel.DynamicBoxInfos,
                GetDynamicBoxShadowInfos()));

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
        /// 旋转方块
        /// </summary>
        public void Rotate() {
            OperationQueue.Enqueue(RotateDynamicBox);
        }

        /// <summary>
        /// 向右移
        /// </summary>
        private void MoveRight() {
            //判断是不是到边了或者碰到方块了
            if (gameModel.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.X + 1 >= gameConfig.Width ||
                    gameModel.StaticBoxInfos[dynamicBoxInfo.X + 1, dynamicBoxInfo.Y].IsBox)) {
                return;
            }

            //不然右移一格
            gameModel.DynamicBoxInfos.ForEach(info => { info.X++; });
            gameModel.DynamicBoxGroupRotCenter.X++;

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(gameModel.StaticBoxInfos, gameModel.DynamicBoxInfos,
                GetDynamicBoxShadowInfos()));
        }

        /// <summary>
        /// 向左移
        /// </summary>
        private void MoveLeft() {
            //判断是不是到边了或者碰到方块了
            if (gameModel.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.X - 1 < 0 ||
                    gameModel.StaticBoxInfos[dynamicBoxInfo.X - 1, dynamicBoxInfo.Y].IsBox)) {
                return;
            }

            //不然左移一格
            gameModel.DynamicBoxInfos.ForEach(info => { info.X--; });
            gameModel.DynamicBoxGroupRotCenter.X--;

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(gameModel.StaticBoxInfos, gameModel.DynamicBoxInfos,
                GetDynamicBoxShadowInfos()));
        }

        /// <summary>
        /// 直接落地
        /// </summary>
        private void MoveDown() {
            //如果没有落到底部或其他方块上
            while (!IsFallOnBottomOrBox()) {
                //下降一格
                gameModel.DynamicBoxInfos.ForEach(info => { info.Y--; });
                gameModel.DynamicBoxGroupRotCenter.Y--;
            }

            SetDynamicToStatic();
            NextBoxGroup();

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(gameModel.StaticBoxInfos, gameModel.DynamicBoxInfos,
                GetDynamicBoxShadowInfos()));
        }

        /// <summary>
        /// 向下移动一格
        /// </summary>
        private void MoveDownOneGrid() {
            //判断是不是到边了或者碰到方块了
            if (!IsFallOnBottomOrBox()) {
                //不然下移一格
                gameModel.DynamicBoxInfos.ForEach(info => { info.Y--; });
                gameModel.DynamicBoxGroupRotCenter.Y--;
            }
            else {
                SetDynamicToStatic();
                NextBoxGroup();
            }


            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(gameModel.StaticBoxInfos, gameModel.DynamicBoxInfos,
                GetDynamicBoxShadowInfos()));
        }

        private void OnUpdate() {
            time += Time.deltaTime;
            if (time > gameConfig.FallInterval / gameModel.SpeedFactor) {
                time -= gameConfig.FallInterval / gameModel.SpeedFactor;

                OperationQueue.Enqueue(MoveDownOneGrid);
            }

            while (OperationQueue.Count > 0) {
                OperationQueue.Dequeue().Invoke();
            }
        }

        /// <summary>
        /// 判断是不是到底了或者碰到方块了
        /// </summary>
        /// <returns></returns>
        private bool IsFallOnBottomOrBox() {
            return gameModel.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.Y - 1 < 0 ||
                    gameModel.StaticBoxInfos[dynamicBoxInfo.X, dynamicBoxInfo.Y - 1].IsBox);
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
            if (gameModel.StaticBoxInfos[boxLoc.X, boxLoc.Y].IsBox)
                return true;
            return false;
        }

        /// <summary>
        /// 把动的方块变成静止
        /// </summary>
        private void SetDynamicToStatic() {
            foreach (var info in gameModel.DynamicBoxInfos) {
                gameModel.StaticBoxInfos[info.X, info.Y].IsBox = true;
                gameModel.StaticBoxInfos[info.X, info.Y].Color = info.Color;
            }

            gameModel.DynamicBoxInfos = null;
            gameModel.DynamicBoxGroupRotCenter = null;

            CheckEliminate();
            CheckFail();
        }

        /// <summary>
        /// 判断是否要消除
        /// </summary>
        private void CheckEliminate() {
            var height = gameConfig.Height;
            var width = gameConfig.Width;
            var staticBoxInfos = gameModel.StaticBoxInfos;
            bool isAllLineBox, isAllLineNotBox;

            var eliminateCount = 0;

            for (var h = 0; h < height;) {
                isAllLineBox = true;
                isAllLineNotBox = true;
                //判断一行是不是全是方块
                for (var w = 0; w < width; w++) {
                    if (staticBoxInfos[w, h].IsBox) continue;
                    isAllLineBox = false;
                    break;
                }

                //判断一行是不是全不是方块
                for (var w = 0; w < width; w++) {
                    if (!staticBoxInfos[w, h].IsBox) continue;
                    isAllLineNotBox = false;
                    break;
                }

                if (isAllLineBox) {
                    eliminateCount++;
                    h++;
                }
                else {
                    if (eliminateCount != 0) {
                        //移动
                        for (var nh = h; nh < height; nh++) {
                            for (var nw = 0; nw < width; nw++) {
                                staticBoxInfos[nw, nh - eliminateCount] = staticBoxInfos[nw, nh];
                            }
                        }

                        //最后几行
                        for (var nh = height - eliminateCount; nh < height; nh++) {
                            for (var nw = 0; nw < width; nw++) {
                                staticBoxInfos[nw, nh] = new StaticBoxInfo();
                            }
                        }
                    }


                    scoreModel.Score += eliminateCount * eliminateCount;
                    TriggerEvent(new ScoreUpdateEvt(scoreModel.Score));


                    if (isAllLineNotBox) {
                        break;
                    }
                    else {
                        h++;
                        eliminateCount = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 检测是否失败
        /// </summary>
        private void CheckFail() {
            for (var w = 0; w < gameConfig.Width; w++) {
                if (gameModel.StaticBoxInfos[w, gameConfig.LimitHeight].IsBox) {
                    //时间停止流动
                    Update -= OnUpdate;
                    //记录最高分
                    if (GetConfig<RecordConfig>().MaxScore < scoreModel.Score)
                        GetConfig<RecordConfig>().MaxScore = scoreModel.Score;
                    
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
            gameModel.DynamicBoxInfos = gameModel.NextDynamicBoxInfos;
            gameModel.DynamicBoxGroupRotCenter = gameModel.NextDynamicBoxGroupRotCenter;

            CreateNextDynamicBoxGroup();

            TriggerEvent(new NextBoxGroupEvt(gameModel.NextDynamicBoxInfos, gameModel.NextDynamicBoxGroupRotCenter));
        }

        private void CreateNextDynamicBoxGroup() {
            var rCount = Random.Range(0, gameModel.BoxGroupPrefabs.Count);
            gameModel.NextDynamicBoxInfos = gameModel.BoxGroupPrefabs[rCount].Clone();
            gameModel.NextDynamicBoxGroupRotCenter = (FloatTuple) gameModel.BoxGroupRotCenter[rCount].Clone();
            var rColor = Random.ColorHSV(RHue, RHue, 0.5f, 0.5f, 0.9f, 0.9f);
            gameModel.NextDynamicBoxInfos.ForEach(info => { info.Color = rColor; });
        }

        /// <summary>
        /// 旋转当前动态方块
        /// </summary>
        private void RotateDynamicBox() {
            //旋转坐标
            var beforeRotLoc = gameModel.DynamicBoxInfos.Select(info => new FloatTuple(info.X, info.Y)).ToList();
            var afterRotLoc = new IntTuple[beforeRotLoc.Count];
            for (var i = 0; i < beforeRotLoc.Count; i++) {
                beforeRotLoc[i] -= gameModel.DynamicBoxGroupRotCenter;
                beforeRotLoc[i] = gameConfig.RotMatrix.Multiply(beforeRotLoc[i]);
                beforeRotLoc[i] += gameModel.DynamicBoxGroupRotCenter;
                afterRotLoc[i] = (IntTuple) beforeRotLoc[i];
            }

            //检测是否会碰到其他物体
            if (afterRotLoc.Any(IsOnBorderOrBox)) return;

            //更新model
            for (var i = 0; i < gameModel.DynamicBoxInfos.Count; i++) {
                gameModel.DynamicBoxInfos[i].X = afterRotLoc[i].X;
                gameModel.DynamicBoxInfos[i].Y = afterRotLoc[i].Y;
            }

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(gameModel.StaticBoxInfos, gameModel.DynamicBoxInfos,
                GetDynamicBoxShadowInfos()));
        }

        private List<DynamicBoxInfo> GetDynamicBoxShadowInfos() {
            var dynamicBoxShadowInfos = gameModel.DynamicBoxInfos.Clone();
            var staticBoxInfos = gameModel.StaticBoxInfos;

            foreach (var shadowInfo in dynamicBoxShadowInfos) {
                shadowInfo.Color = Color.white;
            }


            var isTouched = false;


            for (var i = 0; i < gameConfig.Height; i++) {
                foreach (var shadowInfo in dynamicBoxShadowInfos) {
                    if (shadowInfo.Y == 0 || staticBoxInfos[shadowInfo.X, shadowInfo.Y - 1].IsBox) {
                        isTouched = true;
                    }
                }

                if (isTouched) break;
                else {
                    foreach (var t in dynamicBoxShadowInfos)
                        t.Y--;
                }
            }

            return dynamicBoxShadowInfos;
        }

        public void SpeedChange(bool isSpeedUp) {
            gameModel.SpeedFactor = isSpeedUp ? gameConfig.SpeedUpFactor : 1f;
        }
    }
}