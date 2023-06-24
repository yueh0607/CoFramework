using CoFramework.Events.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;
using CoFramework;
namespace CoFramework.Events
{
    public class EventModule : IModule
    {
        //事件容器
        internal Dictionary<Type, CoOperator<ICoEventBase>> container = new Dictionary<Type, CoOperator<ICoEventBase>>();
        //移除标记
        internal HashSet<Type> removeMarks = new HashSet<Type>();

        //如果认为容器中空余的事件Operator占内存，则随便写个脚本计时调用这个方法即可。
        private void CoEventGC()
        {
            foreach (var con in container)
            {
                if (con.Value.Count == 0) removeMarks.Add(con.Key);
            }
            foreach (var tp in removeMarks)
            {
                container.Remove(tp);
            }
            removeMarks.Clear();
        }

        private CoEventUpdateDriver monoPublisher = null;

        private float recycleInterval = -1, currentTempDeltaTime = 0;
        public MonoBehaviour Mono => monoPublisher;


        public ICoVarOperator<IUpdate> Update => Mono.Operator<IUpdate>();
        public ICoVarOperator<ILateUpdate> LateUpdate => Mono.Operator<ILateUpdate>();
        public ICoVarOperator<IFixedUpdate> FixedUpdate => Mono.Operator<IFixedUpdate>();


        void IModule.OnCreate(CreateParameters parameters)
        {
            //参数检查
            var _params = parameters as CoEventCreateParameters;
            if (_params == null) throw new ArgumentException("Mismatched module parameter types");

            GameObject publisher = new GameObject("[CoEventDriver]");
            GameObject.DontDestroyOnLoad(publisher);
            monoPublisher = publisher.AddComponent<CoEventUpdateDriver>();
            if(!_params.CloseFixedUpdatePublish)publisher.AddComponent<CoEventFixedUpdateDriver>();
            if (!_params.CloseLateUpdatePublish) publisher.AddComponent<CoEventLateUpdateDriver>();
            recycleInterval = _params.RecycleInterval;
        }

        void IModule.OnUpdate()
        {
            if(recycleInterval!=-1f)
            {
                currentTempDeltaTime += Framework.deltaTime;
                if(currentTempDeltaTime>=recycleInterval)
                {
                    currentTempDeltaTime = 0;
                    CoEventGC();
                }
            }
        }

        void IModule.OnDestroy()
        {
            GameObject.Destroy(monoPublisher.gameObject);
        }

    }


}
