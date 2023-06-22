using CoFramework.Events.Internal;
using System;
using System.Diagnostics;

namespace CoFramework.Events
{
    public static class CoEventManagerEx1
    {
        [DebuggerHidden]
        public static ICoVarOperator<EventType> Operator<EventType>(this object cov) where EventType : ISendEventBase
        {
            CoEvent module = Framework.GetModule<CoEvent>();
            Type type = typeof(EventType);
            if (!module.container.ContainsKey(type)) module.container.Add(type, new CoOperator<ICoEventBase>());
            CoOperator<ICoEventBase> cop =module.container[type];
            return CoUnsafeAs.As<CoOperator<ICoEventBase>, CoOperator<EventType>>(ref cop);
        }
    }
    public static class CoEventManagerEx2
    {
        [DebuggerHidden]
        public static ICoVarOperator<EventType> Operator<EventType>(this object cov) where EventType : ICallEventBase
        {
            CoEvent module = Framework.GetModule<CoEvent>();
            Type type = typeof(EventType);
            if (!module.container.ContainsKey(type)) module.container.Add(type, new CoOperator<ICoEventBase>());
            CoOperator<ICoEventBase> cop = module.container[type];
            return CoUnsafeAs.As<CoOperator<ICoEventBase>, CoOperator<EventType>>(ref cop);
        }
    }
}
