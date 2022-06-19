using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork;
using UnityEngine;

namespace Tetris {
    public class TetrisLogicOperation : AbstractOperation {
        private TetrisGameModel _model;
        private List<BoxLoc> currentBoxGroup;
        private GameConfig _gameConfig;
        private float time;
        private Queue<Action> MoveQueue;

        public override void Init() {
            _model = GetModel<TetrisGameModel>();
            currentBoxGroup = new List<BoxLoc>();
            _gameConfig = TetrisGame.Instance.GetConfig<GameConfig>();
            MoveQueue = new Queue<Action>();

            //$$
            currentBoxGroup.Add(new BoxLoc {x = _gameConfig.NewBoxLoc.x, y = _gameConfig.NewBoxLoc.y});
            _model.BoxInfos[_gameConfig.NewBoxLoc.x, _gameConfig.NewBoxLoc.y].Color = Color.magenta;
            _model.BoxInfos[_gameConfig.NewBoxLoc.x, _gameConfig.NewBoxLoc.y].IsBox = true;


            TetrisGame.Instance.OnUpdate += OnUpdate;
        }

        protected override void OnUpdate() {
            time += Time.deltaTime;
            if (time > _gameConfig.FallInterval) {
                time -= _gameConfig.FallInterval;

                MoveQueue.Enqueue(MoveDownOneGrid);
                Debug.Log("MoveDownOneGrid");
            }

            while (MoveQueue.Count > 0) {
                MoveQueue.Dequeue().Invoke();
            }
        }

        public void MoveBox(MoveDir moveDir) {
            switch (moveDir) {
                case MoveDir.Down:
                    MoveQueue.Enqueue(MoveDown);
                    Debug.Log("Down");
                    break;
                case MoveDir.Left:
                    MoveQueue.Enqueue(MoveLeft);
                    Debug.Log("Left");
                    break;
                case MoveDir.Right:
                    MoveQueue.Enqueue(MoveRight);
                    Debug.Log("Right");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveDir), moveDir, null);
            }

            TriggerEvent(new UpdateBoxViewEvt(_model.BoxInfos));
        }

        private void MoveRight() {
            //暂时先存起来
            var tempBoxLocGroup = new List<BoxLoc>(currentBoxGroup);
            var tempBoxColorGroup = new List<Color>();
            foreach (var boxLoc in tempBoxLocGroup) {
                tempBoxColorGroup.Add(_model.BoxInfos[boxLoc.x, boxLoc.y].Color);
                _model.BoxInfos[boxLoc.x, boxLoc.y].IsBox = false;
            }


            //判断是不是到边了或者碰到方块了
            if (tempBoxLocGroup.Any(boxLoc =>
                boxLoc.x + 1 >= _gameConfig.Width || _model.BoxInfos[boxLoc.x + 1, boxLoc.y].IsBox)) {
                foreach (var loc in tempBoxLocGroup) { //重新放回去
                    _model.BoxInfos[loc.x, loc.y].IsBox = true;
                }
            }
            else {
                //不然右移一格
                foreach (var t in tempBoxLocGroup)
                    t.x += 1;
                currentBoxGroup = tempBoxLocGroup;
                for (var i = 0; i < tempBoxLocGroup.Count; i++) {
                    _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y].Color = tempBoxColorGroup[i];
                    _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y].IsBox = true;
                }

                //更新视图
                TriggerEvent(new UpdateBoxViewEvt(_model.BoxInfos));
            }
        }

        private void MoveLeft() {
            //暂时先存起来
            var tempBoxLocGroup = new List<BoxLoc>(currentBoxGroup);
            var tempBoxColorGroup = new List<Color>();
            foreach (var boxLoc in tempBoxLocGroup) {
                tempBoxColorGroup.Add(_model.BoxInfos[boxLoc.x, boxLoc.y].Color);
                _model.BoxInfos[boxLoc.x, boxLoc.y].IsBox = false;
            }


            //判断是不是到边了或者碰到方块了
            if (tempBoxLocGroup.Any(boxLoc =>
                boxLoc.x - 1 < 0 || _model.BoxInfos[boxLoc.x - 1, boxLoc.y].IsBox)) {
                foreach (var loc in tempBoxLocGroup) { //重新放回去
                    _model.BoxInfos[loc.x, loc.y].IsBox = true;
                }
            }
            else {
                //不然左移一格
                foreach (var t in tempBoxLocGroup)
                    t.x -= 1;
                currentBoxGroup = tempBoxLocGroup;
                for (var i = 0; i < tempBoxLocGroup.Count; i++) {
                    _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y].Color = tempBoxColorGroup[i];
                    _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y].IsBox = true;
                }

                //更新视图
                TriggerEvent(new UpdateBoxViewEvt(_model.BoxInfos));
            }
        }

        private void MoveDown() {
            var tempBoxLocGroup = new List<BoxLoc>(currentBoxGroup);
            var tempBoxColorGroup = new List<Color>();

            foreach (var boxLoc in tempBoxLocGroup) {
                tempBoxColorGroup.Add(_model.BoxInfos[boxLoc.x, boxLoc.y].Color);
                _model.BoxInfos[boxLoc.x, boxLoc.y].IsBox = false;
            }


            //如果没有落到底部或其他方块上
            while (!IsFallOnBottom(tempBoxLocGroup)) {
                //下降一格
                foreach (var t in tempBoxLocGroup) {
                    t.y -= 1;
                }
            }


            //修改model数据
            for (var i = 0; i < tempBoxLocGroup.Count; i++) {
                _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y].Color = tempBoxColorGroup[i];
                _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y].IsBox = true;
            }

            currentBoxGroup = tempBoxLocGroup;

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(_model.BoxInfos));
        }

        private void MoveDownOneGrid() {
            var tempBoxLocGroup = new List<BoxLoc>(currentBoxGroup);
            currentBoxGroup.Clear();
            var tempBoxColorGroup = new List<Color>();

            foreach (var boxLoc in tempBoxLocGroup) {
                tempBoxColorGroup.Add(_model.BoxInfos[boxLoc.x, boxLoc.y].Color);
                _model.BoxInfos[boxLoc.x, boxLoc.y].IsBox = false;
            }

            //判断是不是到底了或者是不是落到其他方块上了
            foreach (var boxLoc in tempBoxLocGroup) {
                if (boxLoc.y <= 0 || _model.BoxInfos[boxLoc.x, boxLoc.y - 1].IsBox) {
                    //TODO 触发下一个方块组事件
                    //$$
                    foreach (var loc in currentBoxGroup) {
                        Debug.Log($"before current loc: x:{loc.x}  y:{loc.y}");
                    }

                    currentBoxGroup.Add(new BoxLoc {x = _gameConfig.NewBoxLoc.x, y = _gameConfig.NewBoxLoc.y});
                    Debug.Log($"config loc: x:{_gameConfig.NewBoxLoc.x}  y:{_gameConfig.NewBoxLoc.y}");
                    foreach (var loc in currentBoxGroup) {
                        Debug.Log($"after current loc: x:{loc.x}  y:{loc.y}");
                    }

                    _model.BoxInfos[_gameConfig.NewBoxLoc.x, _gameConfig.NewBoxLoc.y].Color = Color.magenta;
                    _model.BoxInfos[_gameConfig.NewBoxLoc.x, _gameConfig.NewBoxLoc.y].IsBox = true;


                    foreach (var loc in tempBoxLocGroup) { //重新放回去
                        _model.BoxInfos[loc.x, loc.y].IsBox = true;
                        Debug.Log("tempBoxLocGroup" + loc.x + "   " + loc.y);
                    }


                    Debug.Log("hello");

                    return;
                }
            }

            //下降一格
            for (var i = 0; i < tempBoxLocGroup.Count; i++) {
                _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y - 1].Color = tempBoxColorGroup[i];
                _model.BoxInfos[tempBoxLocGroup[i].x, tempBoxLocGroup[i].y - 1].IsBox = true;
                currentBoxGroup.Add(new BoxLoc {x = tempBoxLocGroup[i].x, y = tempBoxLocGroup[i].y - 1});
            }

            //更新视图
            TriggerEvent(new UpdateBoxViewEvt(_model.BoxInfos));
        }

        private bool IsFallOnBottom(List<BoxLoc> boxLocGroup) {
            foreach (var boxLoc in boxLocGroup) {
                if (boxLoc.y - 1 < 0 || _model.BoxInfos[boxLoc.x, boxLoc.y - 1].IsBox) {
                    return true;
                }
            }

            return false;
        }
    }
}