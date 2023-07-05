
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

namespace CoFramework.FSM 
{
    public class StringMachine//:IMachine
    {
        private IState current =null;
        public IState Current=>current;

        private string currentName = null;
        public string CurrentName => currentName;

        private Dictionary<string, IState> states= new Dictionary<string, IState>();

        private Dictionary<string, Dictionary<string, ICondition>> map = new Dictionary<string, Dictionary<string, ICondition>>();

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddState(string name,IState state)
        {
            if (states.ContainsKey(name)) throw new InvalidOperationException("State existed!");
            states.Add(name, state);
        }
        /// <summary>
        /// 添加状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddState<T>(string name) where T:IState,new()
        {
            if (states.ContainsKey(name)) throw new InvalidOperationException("State existed!");
            states.Add(name, new T());
        }
        /// <summary>
        /// 添加转换
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        /// <param name="condition"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetTransition(string fromState,string toState,ICondition condition) 
        {
            if(!states.ContainsKey(fromState)) throw new InvalidOperationException("State not existed!");
            if (!states.ContainsKey(toState)) throw new InvalidOperationException("State not existed!");
            if (map.ContainsKey(fromState) && map[fromState].ContainsKey(toState)) throw new InvalidOperationException("transition existed!");

            if(map.TryGetValue(fromState, out var trans))
            {
                trans.Add(toState, condition);
            }
            else
            {
                map.Add(fromState, new Dictionary<string, ICondition>());
                map[fromState].Add(toState, condition);
            }
        }

        /// <summary>
        /// 移除转换
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveTransition(string fromState, string toState)
        {
            if (!states.ContainsKey(fromState)) throw new InvalidOperationException("State not existed!");
            if (!states.ContainsKey(toState)) throw new InvalidOperationException("State not existed!");
            if (!map.ContainsKey(fromState) || !map[fromState].ContainsKey(toState)) throw new InvalidOperationException("transition not existed!");

            if(map.ContainsKey(fromState))
            {
                if (map[fromState].ContainsKey(toState))
                {
                    map[fromState].Remove(toState);
                }
                if (map[fromState].Count==0)
                {
                    map.Remove(fromState);
                }
            }
        }

        /// <summary>
        /// 添加转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        public void SetTransition<T>(string fromState, string toState) where T:class, ICondition, new()
        {
            SetTransition(fromState, toState, new T());
        }

       


        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveState(string name)
        {
            if (!states.ContainsKey(name)) throw new InvalidOperationException("State not existed!");
            states.Remove(name);
            
        }

        /// <summary>
        /// 开启状态机
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void StartFrom(string name)
        {
            if(!states.ContainsKey(name)) throw new InvalidOperationException("State not existed!");
            currentName= name;
            current = states[name];
        }

        //public void OnStartMachine()
        //{

        //}

        void OnUpdateMahcine()
        {
            if (current == null) return;
            if (!map.ContainsKey(currentName)) return;
            var trans = map[currentName];
            foreach(var t in trans)
            {
                if(t.Value.Judge())
                {
                    if (!states.ContainsKey(t.Key)) throw new InvalidOperationException("State not existed!");
                    currentName = t.Key;
                    current = states[t.Key];
                    break;
                }
            }
        }


        public StringMachine()
        {
            Framework.Update += OnUpdateMahcine;
        }
        ~StringMachine()
        {
            Framework.Update -= OnUpdateMahcine;
        }
        //public void OnDestroyMachine()
        //{
            
        //}
    }
}