using CoFramework.Tasks;
using System;
using System.Collections.Generic;

namespace CoFramework.FSM
{
    [ModuleDepends(null, typeof(TaskModule))]
    public class FSMModule : IModule
    {

        //DynamicDictionary<string, IMachine> machines = new DynamicDictionary<string,IMachine>();   
        //public void StartMachine(string name,IMachine machine)
        //{
        //    machine.OnStartMachine();
        //    if (machines.ContainsKey(name)) throw new InvalidOperationException("machine existed");
        //    machines.Enqueue(name, machine);

        //}
        //public void DestroyMachine(string name)
        //{
        //    if(!machines.ContainsKey(name)) throw new InvalidOperationException("machine not existed");
        //    machines[name].OnDestroyMachine();
        //    machines.Remove(name);
        //}

        void IModule.OnCreate(CreateParameters parameters)
        {
      
        }

        void IModule.OnUpdate()
        {
            //machines.RefreshTraversalCount();
            //int traversalCount = machines.TraversalCount;
            //for (int i = 0; i < traversalCount; ++i)
            //{
            //    if (machines.TryDequeue(out var machine, out var name))
            //    {
            //        machine.OnUpdateMahcine();
            //        machines.TryEnqueue(name, machine);
            //    }
            //}
        }

        void IModule.OnDestroy()
        {
            
        }
    }
}
