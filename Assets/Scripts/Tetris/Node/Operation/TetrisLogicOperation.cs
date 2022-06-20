using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetris {
    public class TetrisLogicOperation : AbstractOperation {
        private TetrisGameModel _model;
        private GameConfig _gameConfig;

        private float time;
        private Queue<Action> OperationQueue;
        private List<List<DynamicBoxInfo>> BoxGroupPrefabs;
        private List<FloatTuple> BoxGroupRotCenter;

        public override void Init() {
            _model = GetModel<TetrisGameModel>();
            _gameConfig = TetrisGame.Instance.GetConfig<GameConfig>();
            OperationQueue = new Queue<Action>();
            BoxGroupPrefabs = new List<List<DynamicBoxInfo>>();
            BoxGroupRotCenter = new List<FloatTuple>();

            foreach (var DBIs in
                _gameConfig.BoxGroupPrefabs.Select(boxLocs =>
                    boxLocs.Select(boxLoc => new DynamicBoxInfo(boxLoc.X, boxLoc.Y)).ToList())) {
                DBIs.ForEach(info => {
                    info.X += _gameConfig.NewBoxLoc.X;
                    info.Y += _gameConfig.NewBoxLoc.Y;
                });

                BoxGroupPrefabs.Add(DBIs);
            }

            BoxGroupRotCenter = _gameConfig.BoxGroupSRS.Clone();
            BoxGroupRotCenter.ForEach(tuple => {
                tuple.X += _gameConfig.NewBoxLoc.X;
                tuple.Y += _gameConfig.NewBoxLoc.Y;
            });

            NextBoxGroup();
            TriggerEvent(new UpdateBoxViewEvt(_model.StaticBoxInfos, _model.DynamicBoxInfos));

            TetrisGame.Instance.OnUpdate += OnUpdate;
        }

        protected override void OnUpdate() {
            time += Time.deltaTime;
            if (time > _gameConfig.FallInterval) {
                time -= _gameConfig.FallInterval;

                OperationQueue.Enqueue(MoveDownOneGrid);
            }

            while (OperationQueue.Count > 0) {
                OperationQueue.Dequeue().Invoke();
            }
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
            if (_model.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.X + 1 >= _gameConfig.Width ||
                    _model.StaticBoxInfos[dynamicBoxInfo.X + 1, dynamicBoxInfo.Y].IsBox)) {
                return;
            }

            //不然右移一格
            _model.DynamicBoxInfos.ForEach(info => { info.X++; });
            _model.DynamicBoxGroupRotCenter.X++;

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(_model.StaticBoxInfos, _model.DynamicBoxInfos));
        }

        /// <summary>
        /// 向左移
        /// </summary>
        private void MoveLeft() {
            //判断是不是到边了或者碰到方块了
            if (_model.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.X - 1 < 0 ||
                    _model.StaticBoxInfos[dynamicBoxInfo.X - 1, dynamicBoxInfo.Y].IsBox)) {
                return;
            }

            //不然左移一格
            _model.DynamicBoxInfos.ForEach(info => { info.X--; });
            _model.DynamicBoxGroupRotCenter.X--;

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(_model.StaticBoxInfos, _model.DynamicBoxInfos));
        }

        /// <summary>
        /// 直接落地
        /// </summary>
        private void MoveDown() {
            //如果没有落到底部或其他方块上
            while (!IsFallOnBottomOrBox()) {
                //下降一格
                _model.DynamicBoxInfos.ForEach(info => { info.Y--; });
                _model.DynamicBoxGroupRotCenter.Y--;
            }

            SetDynamicToStatic();
            NextBoxGroup();

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(_model.StaticBoxInfos, _model.DynamicBoxInfos));
        }

        /// <summary>
        /// 向下移动一格
        /// </summary>
        private void MoveDownOneGrid() {
            //判断是不是到边了或者碰到方块了
            if (!IsFallOnBottomOrBox()) {
                //不然下移一格
                _model.DynamicBoxInfos.ForEach(info => { info.Y--; });
                _model.DynamicBoxGroupRotCenter.Y--;
            }
            else {
                SetDynamicToStatic();
                NextBoxGroup();
            }


            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(_model.StaticBoxInfos, _model.DynamicBoxInfos));
        }

        /// <summary>
        /// 判断是不是到底了或者碰到方块了
        /// </summary>
        /// <returns></returns>
        private bool IsFallOnBottomOrBox() {
            return _model.DynamicBoxInfos.Any(
                dynamicBoxInfo =>
                    dynamicBoxInfo.Y - 1 < 0 ||
                    _model.StaticBoxInfos[dynamicBoxInfo.X, dynamicBoxInfo.Y - 1].IsBox);
        }

        /// <summary>
        /// 判断某个坐标是不是到边了或者碰到方块了
        /// </summary>
        /// <returns></returns>
        private bool IsOnBorderOrBox(IntTuple boxLoc) {
            if (boxLoc.X < 0 || boxLoc.X >= _gameConfig.Width)
                return true;
            if (boxLoc.Y < 0 || boxLoc.Y >= _gameConfig.Height)
                return true;
            if (_model.StaticBoxInfos[boxLoc.X, boxLoc.Y].IsBox)
                return true;
            return false;
        }

        /// <summary>
        /// 把动的方块变成静止
        /// </summary>
        private void SetDynamicToStatic() {
            foreach (var info in _model.DynamicBoxInfos) {
                _model.StaticBoxInfos[info.X, info.Y].IsBox = true;
                _model.StaticBoxInfos[info.X, info.Y].Color = info.Color;
            }

            _model.DynamicBoxInfos = null;
            _model.DynamicBoxGroupRotCenter = null;

            CheckEliminate();
            CheckFail();
        }

        /// <summary>
        /// 判断是否要消除
        /// </summary>
        private void CheckEliminate() {
            for (var h = 0; h < _gameConfig.Height;) {
                var isAllLineBox = true;
                var isAllLineNotBox = true;
                //判断一行是不是全是方块
                for (var w = 0; w < _gameConfig.Width; w++) {
                    if (_model.StaticBoxInfos[w, h].IsBox) continue;
                    isAllLineBox = false;
                    break;
                }

                //判断一行是不是全不是方块
                for (var w = 0; w < _gameConfig.Width; w++) {
                    if (!_model.StaticBoxInfos[w, h].IsBox) continue;
                    isAllLineNotBox = false;
                    break;
                }

                if (isAllLineBox) {
                    for (var nh = h; nh < _gameConfig.Height - 1; nh++) {
                        for (var nw = 0; nw < _gameConfig.Width; nw++) {
                            _model.StaticBoxInfos[nw, nh] = _model.StaticBoxInfos[nw, nh + 1];
                        }
                    }

                    //最后一行
                    for (var nw = 0; nw < _gameConfig.Width; nw++) {
                        _model.StaticBoxInfos[nw, _gameConfig.Height - 1] = new StaticBoxInfo();
                    }
                }
                else if (isAllLineNotBox) {
                    break;
                }
                else {
                    h++;
                }
            }
        }

        private void CheckFail() {
            for (var w = 0; w < _gameConfig.Width; w++) {
                if (_model.StaticBoxInfos[w, _gameConfig.LimitHeight].IsBox) {
                    //时间停止流动
                    TetrisGame.Instance.OnUpdate -= OnUpdate;
                    //触发游戏失败事件
                    TriggerEvent<FailEvt>();
                }
            }
        }

        /// <summary>
        /// 释放下一个方块组
        /// </summary>
        private void NextBoxGroup() {
            var r = Random.Range(0, BoxGroupPrefabs.Count);
            _model.DynamicBoxInfos = BoxGroupPrefabs[r].Clone();
            _model.DynamicBoxGroupRotCenter = (FloatTuple) BoxGroupRotCenter[r].Clone();
            var rColor = Random.ColorHSV(0f, 1f, 0.4f, 0.5f, 0.8f, 0.9f);

            _model.DynamicBoxInfos.ForEach(info => { info.Color = rColor; });
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
            var beforeRotLoc = _model.DynamicBoxInfos.Select(info => new FloatTuple(info.X, info.Y)).ToList();
            var afterRotLoc = new IntTuple[beforeRotLoc.Count];
            for (var i = 0; i < beforeRotLoc.Count; i++) {
                beforeRotLoc[i] -= _model.DynamicBoxGroupRotCenter;
                beforeRotLoc[i] = _gameConfig.RotMatrix.Multiply(beforeRotLoc[i]);
                beforeRotLoc[i] += _model.DynamicBoxGroupRotCenter;
                afterRotLoc[i] = (IntTuple) beforeRotLoc[i];
            }

            //检测是否会碰到其他物体
            if (afterRotLoc.Any(IsOnBorderOrBox)) return;

            //更新model
            for (var i = 0; i < _model.DynamicBoxInfos.Count; i++) {
                _model.DynamicBoxInfos[i].X = afterRotLoc[i].X;
                _model.DynamicBoxInfos[i].Y = afterRotLoc[i].Y;
            }

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(_model.StaticBoxInfos, _model.DynamicBoxInfos));
        }
    }
}