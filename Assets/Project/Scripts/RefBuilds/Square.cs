
/*******************************************************
 * Code Generated By CoFramework?LISENCE Apache 2.0? 
 * DateTime : 2023/6/30 19:02:28
 * UVersion : 2021.3.24f1c1
 *******************************************************/
using UnityEngine;
using CoFramework.RefBuild;

namespace CoFramework.RefBuild.Cache
{
    public partial class Square : UnityEngine.MonoBehaviour
    {
        public UnityEngine.Transform Square_Transform { get; set; } = default;
        public BindableProperty<UnityEngine.Vector3> Square_Transform_position {get;set;}=default;


        public void InitRefs()
        {
            Square_Transform = GetComponent<UnityEngine.Transform>();
            Square_Transform_position = new BindableProperty<UnityEngine.Vector3>(()=>Square_Transform.position,(x)=>Square_Transform.position=x,false);

        }

        
        private void Awake()
        {
            InitRefs();
        }

    }
    
}
