namespace CoFramework.Events
{
    public class EventModuleCreateParameters : CreateParameters
    {
        /// <summary>
        /// 回收间隔，CoEvent运行时会存在一些极少的垃圾，如果令该数值为-1则永不回收(影响极小)
        /// 如果令该值为非正数(不包含-1)则每帧回收一次，如果令该值大于0，则根据该间隔回收
        /// </summary>
        public float RecycleInterval { get; set; } = -1;

        /// <summary>
        /// CoEvent自动发布FixedUpdate生命周期，如果令该值为true，CoEvent不会派发FixedUpdate事件
        /// </summary>
        public bool CloseFixedUpdatePublish { get; set; } = false;

        /// <summary>
        /// CoEvent自动发布LateUpdate生命周期，如果令该值为true，CoEvent不会派发ILateUpdate事件
        /// </summary>

        public bool CloseLateUpdatePublish { get; set; } = false;
    }
}
