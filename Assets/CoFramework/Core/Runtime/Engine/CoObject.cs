using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework
{
    public interface ICoObject
    {
        /// <summary>
        /// 所有CoObject不会重复的ID值，但是不保证析构后不会再次使用ID
        /// </summary>
        public ulong ID { get; }
    }


    public abstract class CoObject : ICoObject,IEquatable<CoObject>
    {

        private static readonly HashSet<ulong> _pool = new HashSet<ulong>();
        private static ulong _pointer = 0;
    
        public CoObject()
        {
            //使用while在ulong溢出时不会导致深循环，溢出时全部ID接近于MAX，突然重置为0后一般在极少的循环
            //次数内即可找到未占用的ID值，即时有少量的长期占用区域，也可以被快速跳过
            while (_pool.Contains(++_pointer))
            {
                if (_pointer == ulong.MaxValue)
                {
                    _pointer = 0;
                    continue;
                }
                _pool.Add(_pointer);
                _id = _pointer++;
            }
        }

        ~CoObject()
        {
            _pool.Remove(_id);
        }

        private readonly ulong _id;
        public ulong ID => _id;



        public bool Equals(CoObject other) => this.ID == other.ID;

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode()=>base.GetHashCode();

        public override string ToString()=>$"[{typeof(CoObject).FullName}]:ID={ID}";
        public static bool operator==(CoObject a  ,CoObject b )=>a.ID == b.ID;
        public static bool operator !=(CoObject a, CoObject b)=> a.ID != b.ID;
        

    }

    public class IDComparer : IEqualityComparer<ICoObject>
    {
        public bool Equals(ICoObject x, ICoObject y) => x.ID==y.ID;

        public int GetHashCode(ICoObject obj) => obj.ID.GetHashCode();
    }
}
