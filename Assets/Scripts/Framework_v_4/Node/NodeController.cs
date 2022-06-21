namespace FrameWork {
    public class NodeController {
        private readonly IOCContainer<INode> IocContainer;

        private readonly Leader BelongedLeader;

        public NodeController(Leader belongedLeader) {
            BelongedLeader = belongedLeader;
            IocContainer = new IOCContainer<INode>();
        }

        public T GetOperation<T>() where T : class, IOperation {
            return IocContainer.Get<T>();
        }

        public T GetModel<T>() where T : class, IModel {
            return IocContainer.Get<T>();
        }

        public void Register<T>(T node) where T : class, INode {
            IocContainer.Add(node);
            node.BelongedLeader = BelongedLeader;
            node.Init();
        }

        public void RegisterWithoutInit<T>(T node) where T : class, INode {
            IocContainer.Add(node);
            node.BelongedLeader = BelongedLeader;
        }

        public void UnRegister<T>(T node) where T : class, INode {
            IocContainer.Remove<T>();
            node.BelongedLeader = null;
        }
    }
}