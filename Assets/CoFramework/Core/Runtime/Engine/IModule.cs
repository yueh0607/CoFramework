using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CoFramework
{

    /// <summary>
    /// 模块接口
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 在创建模块时调用
        /// </summary>
        public void OnCreate(CreateParameters parameters);

        /// <summary>
        /// 模块更新时调用
        /// </summary>

        public void OnUpdate();

        /// <summary>
        /// 在模块销毁时调用
        /// </summary>
        public void OnDestroy();

    }


}