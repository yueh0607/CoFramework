﻿/********************************************************************************************
 * Date : 2023.1.30
 * Description : 
 * 可顺序遍历且可以随机移除的队列+字典
 ********************************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
namespace CoFramework
{
    public class DynamicDictionary<T, K> : IEnumerable<KeyValuePair<T, K>> where T : notnull
    {

        //队列顺序
        private readonly Queue<T> queue;
        //实际存储，即时
        private readonly Dictionary<T, K> dictionary;
        //临时移除状态
        private readonly Dictionary<T, int> state;

        /// <summary>
        /// 真正剩余元素数量
        /// </summary>
        public int Count => dictionary.Count;

        /// <summary>
        /// 遍历出队的次数
        /// </summary>
        public int TraversalCount { get; private set; } = 0;

        /// <summary>
        /// 刷新遍历数量，在每次遍历之前都要进行
        /// </summary>
        public int RefreshTraversalCount() => TraversalCount = queue.Count;

        public DynamicDictionary()
        {
            queue = new Queue<T>();
            dictionary = new Dictionary<T, K>();
            state = new Dictionary<T, int>();
        }
        public DynamicDictionary(IEqualityComparer<T> equalityComparer)
        {
            queue = new Queue<T>();
            dictionary = new Dictionary<T, K>(equalityComparer);
            state = new Dictionary<T, int>(equalityComparer);
        }


        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Enqueue(T key, K value)
        {
            if (TryEnqueue(key, value)) return;
            throw new ArgumentException("Same key exists");
        }
        /// <summary>
        /// 尝试入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryEnqueue(T key, K value)
        {
            if (!dictionary.TryAdd(key, value))
            {
                return false;
            }
            queue.Enqueue(key);
            return true;
        }
        /// <summary>
        /// 从队中移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(T key)
        {
            if (!TryRemove(key))
            {
                throw new ArgumentException("error remove key ");
            }
        }
        /// <summary>
        /// 尝试从队中移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool TryRemove(T key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                //Count--;
                if (state.ContainsKey(key)) state[key]++;
                else state.Add(key, 1);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 尝试获取队首
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPeek(out K result)
        {
            do
            {
                if (queue.TryPeek(out var key))
                {
                    if (state.TryGetValue(key, out int count))
                    {
                        state[key] = --count;
                        if (TraversalCount > 0) TraversalCount--;
                        if (count == 0) state.Remove(key);
                        queue.Dequeue();
                    }
                    else return dictionary.TryGetValue(key, out result);
                }
                else
                {
                    result = default;
                    return false;
                }
            }
            while (true);
        }
        /// <summary>
        /// 获取队首
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public K Peek()
        {
            if (TryPeek(out var result)) return result;
            throw new InvalidOperationException("Empty Pool");
        }
        /// <summary>
        /// 尝试出队
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryDequeue(out K result)
        {
            return TryDequeue(out result, out _);
        }

        public bool TryDequeue(out K result, out T k)
        {
            if (queue.TryDequeue(out var key))
            {
                while (state.TryGetValue(key, out var count))
                {
                    state[key] = --count;
                    if (TraversalCount > 0) TraversalCount--;
                    if (count == 0) state.Remove(key);
                    if (!queue.TryDequeue(out key))
                    {
                        result = default(K);
                        k = default;
                        return false;
                    }
                }
                if (dictionary.TryGetValue(key, out result))
                {
                    k = key;
                    dictionary.Remove(key);
                    return true;
                }
            }

            result = default;
            k = default;
            return false;
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public K Dequeue()
        {
            if (TryDequeue(out var result))
            {
                return result;
            }
            throw new InvalidOperationException("Empty Queue");
        }

        /// <summary>
        /// 访问以K为键的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public K this[T key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }
        /// <summary>
        /// 是否存在以K为键的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(T key) => dictionary.ContainsKey(key);

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
            queue.Clear();
            state.Clear();
        }


        /// <summary>
        /// -----无序遍历迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<T, K>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }
}


