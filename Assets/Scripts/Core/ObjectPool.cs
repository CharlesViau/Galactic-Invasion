using System;
using System.Collections.Generic;

namespace Core
{
    public class ObjectPool
    {
        private readonly Dictionary<ValueType, Stack<IPoolable>> _poolDict;

        public ObjectPool()
        {
            _poolDict = new Dictionary<ValueType, Stack<IPoolable>>();
        }

        public void Pool(IPoolable toPool)
        {
            if (!_poolDict.ContainsKey(toPool.ValueType))
                _poolDict.Add(toPool.ValueType, new Stack<IPoolable>());

            _poolDict[toPool.ValueType].Push(toPool);
            toPool.Pool();
        }

        public IPoolable Depool(ValueType componentType)
        {
            if (!_poolDict.ContainsKey(componentType))
                return null;

            if (_poolDict[componentType].Count <= 0) return null;
            _poolDict[componentType].Peek().Depool();
            return _poolDict[componentType].Pop();
        }

        public void Clear()
        {
            _poolDict.Clear();
        }
    }
}