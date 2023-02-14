using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable InconsistentNaming

namespace Core
{
    /// <summary>
    /// Factory Class made to Instantiate Unity Prefabs.
    /// <list type="bullet"><item>
    /// <description>Prefabs MUST be named the Same way as they are in the Enum.</description></item><item>
    /// <description>'T' Must have an internal public class that inherit from ConstructionArgs.
    /// it will contain the construction Args for the "create" function.</description></item></list>
    /// Example of inheritance:
    ///<list type="bullet"><item>
    /// <description>EnemyFactory : &lt;Enemy, EnemyTypes, Enemy.Args&gt;</description></item></list>
    /// </summary>
    /// <typeparam name="T">Type of the object it creates</typeparam>
    /// <typeparam name="E">Enum of the types of thing it can instantiate</typeparam>
    /// <typeparam name="A">Argument Class for the create method</typeparam>
    public class PrefabFactory<T, E, A> : IFactory<T, E, A>
        where T : ICreatable<A>
        where E : Enum
        where A : ConstructionArgs
    {
        public PrefabFactory(string prefabLocation)
        {
            _prefabDictionary = new Dictionary<ValueType, GameObject>();
            PrefabLocation = prefabLocation;
        }

        #region Properties and Variables

        /// <summary>
        /// It will start Loading automatically from "Resources/" root. Write the rest of the path and don't forget to put the " / " at the end.
        /// </summary>
        private readonly string PrefabLocation;

        private readonly Dictionary<ValueType, GameObject> _prefabDictionary;

        #endregion

        #region Public Methods

        public void Init()
        {
            //Get all Prefab types from the enum and put them in an Array.
            var allPrefabTypes = Enum.GetValues(typeof(E)).Cast<ValueType>().ToArray();


            //Load all Prefabs to the Dictionary
            foreach (var element in allPrefabTypes)
            {
                _prefabDictionary.Add(element, Resources.Load<GameObject>(PrefabLocation + element));
                if (_prefabDictionary[element] is null)
                {
                    Debug.Log("Can't add " + element + " to prefab dictionary at location : " + PrefabLocation);
                }
            }
        }

        /// <summary>
        /// Instantiate asked prefab.
        /// </summary>
        /// <param name="type">Enum Value</param>
        /// <param name="constructionArgs">Args needed for the initialization</param>
        /// <returns></returns>
        public T Create(ValueType type, A constructionArgs)
        {
            return Object.Instantiate(_prefabDictionary[type]).GetComponent<T>();
        }

        #endregion
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IFactory<out T, in E, in A>
        where T : ICreatable<A>
        where E : Enum
        where A : ConstructionArgs
    {
        public T Create(ValueType type, A constructionArgs);
    }

    public abstract class ConstructionArgs
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public Vector3 spawningPosition;

        protected ConstructionArgs(Vector3 spawningPosition)
        {
            this.spawningPosition = spawningPosition;
        }
    }

    public interface ICreatable<in A> where A : ConstructionArgs
    {
        public void Construct(A constructionArgs);
    }
}