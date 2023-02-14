using System;

namespace Core
{
    public interface IPoolable
    {
        public ValueType ValueType { get; }
        void Pool();
        void Depool();
    }
}

