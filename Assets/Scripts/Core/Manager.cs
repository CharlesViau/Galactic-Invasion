using System;
using System.Collections.Generic;
using System.Linq;
using Generic;
using UnityEngine;
// ReSharper disable VirtualMemberCallInConstructor

// ReSharper disable InconsistentNaming

namespace Core
{

    #region Interfaces and Abstract Classes for Managers

    public interface ICleanable
    {
        public void Clean();
    }

    public interface ICollectionManager<in T>
    {
        public void Add(T obj);
        public void Remove(T obj);
    }

    /// <summary>
    /// Classes that inherit that class are Singleton Manager.
    /// that will be get by the game Manager to get their function called.
    /// </summary>
    public abstract class TopDownManager : ICleanable, IBootable, IUpdatable
    {
        public abstract void Init();
        public abstract void PostInit();
        public abstract void Refresh();
        public abstract void FixedRefresh();
        public abstract void LateRefresh();
        public abstract void Clean();
    }

    public abstract class MonoBehaviourManager : ICleanable
    {
        public abstract void Clean();
    }

    #endregion
    
    //Managers that call the update of the object themselves
    #region Top-Down Architecture Manager

    #region Manager<T> (Generic Manager that can be instantiated, not a Singleton)

    /// <summary>
    /// Manager that can manage any collection of any type of object. can be instantiated with the new operator.
    /// </summary>
    /// <typeparam name="T">Type of  the objects to Manage</typeparam>
    public class Manager<T> : IBootable, IUpdatable, ICleanable, ICollectionManager<T> where T : IManageable
    {
        #region Variables & Properties

        private readonly HashSet<T> collection;
        private readonly Stack<T> m_toAdd;
        private readonly Stack<T> m_toRemove;

        #endregion

        public Manager()
        {
            collection = new HashSet<T>();
            m_toAdd = new Stack<T>();
            m_toRemove = new Stack<T>();
        }

        #region Public Methods

        public void Init()
        {
            AddStackItemsToCollection();
            InitCollection();
        }

        public void PostInit()
        {
            PostInitCollection();
        }

        public void Refresh()
        {
            RemoveStackItemsFromCollection();
            UpdateCollection();
            AddStackItemsToCollection();
        }

        public void FixedRefresh()
        {
            FixedUpdateCollection();
        }

        public void LateRefresh()
        {
            LateRefreshCollection();
        }

        public void Add(T item)
        {
            m_toAdd.Push(item);
        }

        public void Remove(T item)
        {
            m_toRemove.Push(item);
        }

        public void Clean()
        {
            CleanManager();
        }

        #endregion

        #region Private Methods

        private void InitCollection()
        {
            foreach (var item in collection)
            {
                item.Init();
            }
        }

        private void PostInitCollection()
        {
            foreach (var item in collection)
            {
                item.PostInit();
            }
        }

        private void AddStackItemsToCollection()
        {
            while (m_toAdd.Count > 0)
            {
                collection.Add(m_toAdd.Pop());
            }
        }

        private void RemoveStackItemsFromCollection()
        {
            while (m_toRemove.Count > 0)
            {
                collection.Remove(m_toRemove.Pop());
            }
        }

        private void UpdateCollection()
        {
            foreach (var item in collection)
            {
                item.Refresh();
            }
        }

        private void FixedUpdateCollection()
        {
            foreach (var item in collection)
            {
                item.FixedRefresh();
            }
        }

        private void LateRefreshCollection()
        {
            foreach (var item in collection)
            {
                item.LateRefresh();
            }
        }

        private void CleanManager()
        {
            collection.Clear();
            m_toAdd.Clear();
            m_toRemove.Clear();
        }

        #endregion
    }

    #endregion

    #region Manager <T,M> (Singleton Wrapper for Manager)

    /// <summary>
    /// Manager that is a Singleton.
    /// </summary>
    /// <typeparam name="T">Type to Manage</typeparam>
    /// <typeparam name="M">Manager type</typeparam>
    // ReSharper disable once InconsistentNaming
    public abstract class Manager<T, M> : TopDownManager, ICollectionManager<T>
        where T : IManageable where M : TopDownManager, new()
    {
        #region Singleton

        private static M _instance;
        public static M Instance => _instance ??= new M();

        protected Manager()
        {
            collection = new HashSet<T>();
            m_toAdd = new Stack<T>();
            m_toRemove = new Stack<T>();
        }

        #endregion

        #region Variables & Properties

        // ReSharper disable once MemberCanBePrivate.Global
        protected readonly HashSet<T> collection;
        private readonly Stack<T> m_toAdd;
        private readonly Stack<T> m_toRemove;

        #endregion

        #region Public Methods

        public override void Init()
        {
            AddStackItemsToCollection();
            InitCollection();
        }

        public override void PostInit()
        {
            PostInitCollection();
        }

        public override void Refresh()
        {
            RemoveStackItemsFromCollection();
            UpdateCollection();
            AddStackItemsToCollection();
        }

        public override void FixedRefresh()
        {
            FixedUpdateCollection();
        }

        public override void LateRefresh()
        {
            LateRefreshCollection();
        }

        public void Add(T item)
        {
            m_toAdd.Push(item);
        }

        public void Remove(T item)
        {
            m_toRemove.Push(item);
        }

        public override void Clean()
        {
            CleanManager();
        }

        #endregion

        #region Private Methods

        private void InitCollection()
        {
            foreach (var item in collection)
            {
                item.Init();
            }
        }

        private void PostInitCollection()
        {
            foreach (var item in collection)
            {
                item.PostInit();
            }
        }

        private void AddStackItemsToCollection()
        {
            while (m_toAdd.Count > 0)
            {
                collection.Add(m_toAdd.Pop());
            }
        }

        private void RemoveStackItemsFromCollection()
        {
            while (m_toRemove.Count > 0)
            {
                collection.Remove(m_toRemove.Pop());
            }
        }

        private void UpdateCollection()
        {
            foreach (var item in collection)
            {
                item.Refresh();
            }
        }

        private void FixedUpdateCollection()
        {
            foreach (var item in collection)
            {
                item.FixedRefresh();
            }
        }

        private void LateRefreshCollection()
        {
            foreach (var item in collection)
            {
                item.LateRefresh();
            }
        }

        private void CleanManager()
        {
            collection.Clear();
            m_toAdd.Clear();
            m_toRemove.Clear();
        }

        #endregion
    }

    #endregion

    #region Manager<T,E,A,M> (Singleton Wrapper for Manager including Factory and Object Pool)

    /// <summary>
    /// Singleton Manager that also contains a Factory and Object Pool.
    /// Refer Yourself to the class factory&lt;T,E,A&gt; for more information
    /// concerning the factory.
    /// </summary>
    /// <typeparam name="T">Type to Manage</typeparam>
    /// <typeparam name="E">Enum listing different type of 'T' for the factory</typeparam>
    /// <typeparam name="A">Arguments to provide to the factory</typeparam>
    /// <typeparam name="M">Manager Type</typeparam>
    public abstract class Manager<T, E, A, M> : Manager<T, M>, IFactory<T, E, A>
        where T : IManageable, ICreatable<A>, IPoolable
        where E : Enum
        where A : ConstructionArgs
        where M : TopDownManager, new()
    {
        protected Manager()
        {
            //Factory
            if (string.IsNullOrEmpty(PrefabLocation))
                Debug.LogError("The PrefabLocation Property of " + typeof(M) + " has not been set");
            else m_prefabFactory = new PrefabFactory<T, E, A>(PrefabLocation);

            //ObjectPool
            m_pool = new ObjectPool();
        }

        #region Variables & Properties

        /// <summary>
        /// Needed for the Factory, It will start Loading automatically from "Resources/" root.
        ///Write the rest of the path and don't forget to put the " / " at the end.
        /// </summary>
        protected abstract string PrefabLocation { get; }

        private readonly PrefabFactory<T, E, A> m_prefabFactory;
        private readonly ObjectPool m_pool;

        #endregion

        #region Public Methods

        public override void Init()
        {
            m_prefabFactory.Init();
            base.Init();
        }

        public T Create(ValueType type, A constructionArgs)
        {
            var toReturn = (T)m_pool.Depool(type);

            if (toReturn is null)
            {
                toReturn = m_prefabFactory.Create(type, constructionArgs);
                toReturn.Init();
                toReturn.PostInit();
            }

            toReturn.Construct(constructionArgs);
            Add(toReturn);

            return toReturn;
        }

        public void Pool(T toPool)
        {
            m_pool.Pool(toPool);
            Remove(toPool);
        }

        #endregion
    }

    #endregion

    #endregion

    //Managers Themselves are not MonoBehaviour, They are managing MonoBehaviour
    #region MonoBehaviourManagers (Component Based Architecture)

    #region MonoBehaviourManager<T, M> (Singleton)

    /// <summary>
    /// This Manager is preferred if you are managing MonoBehaviours Objects using the Unity Awake, Start, Update functions
    /// instead of the IManageable Interface.
    /// </summary>
    public abstract class MonoBehaviourManager<T, M> : MonoBehaviourManager, ICollectionManager<T>
        where T : MonoBehaviour
        where M : class, ICleanable, new()
    {
        #region Singleton

        private static M _instance;
        public static M Instance => _instance ??= new M();

        protected MonoBehaviourManager()
        {
            collection = new HashSet<T>();
        }

        #endregion

        #region Variables & Properties

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once CollectionNeverQueried.Global
        protected readonly HashSet<T> collection;

        #endregion

        #region Public Methods

        public void Add(T obj)
        {
            collection.Add(obj);
        }

        public void Remove(T obj)
        {
            collection.Remove(obj);
        }

        public override void Clean()
        {
            CleanManager();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// !!!DANGEROUS METHOD!!!
        /// <list type="bullet"><item>
        /// <description>Can be use in Awake to fill your collection
        /// with objects of type 'T' that are already present on the scene.</description></item>
        /// </list>
        /// If use elsewhere, you could have the same object multiple time
        /// in your collection or some other unintended bug, use with caution.
        /// </summary>
        protected void FindAllObjectsOfTypeToCollection()
        {
            var hashSet = new HashSet<T>(UnityEngine.Object.FindObjectsOfType<T>().ToList());
            foreach (var item in hashSet)
            {
                Add(item);
            }
#if UNITY_EDITOR
            Debug.LogWarning(
                "Be sure this 'FindAllObjectsOfTypeToCollection()' is called during an initialization phase or in other optimal condition");
#endif
        }

        #endregion

        #region Private Methods

        private void CleanManager()
        {
            collection.Clear();
        }

        #endregion
    }

    #endregion

    #region MonoBehaviourManager<T, E, A ,M> (Singleton, Object Pool, Factory)

    /// <summary>
    /// This Manager is preferred if you are managing MonoBehaviours Objects using the Unity Awake, Start, Update functions
    /// instead of the IManageable Interface. This Manager is a Singleton, also contains a Factory and Object Pool.
    /// Refer Yourself to the class factory&lt;T,E,A&gt; for more information
    /// concerning the factory.
    /// </summary>
    /// <typeparam name="T">Type to Manage</typeparam>
    /// <typeparam name="E">Enum listing different type of 'T' for the factory</typeparam>
    /// <typeparam name="A">Arguments to provide to the factory</typeparam>
    /// <typeparam name="M">Manager Type</typeparam>
    public abstract class MonoBehaviourManager<T, E, A, M> : MonoBehaviourManager<T, M>, IFactory<T, E, A>
        where T : MonoBehaviour, ICreatable<A>, IPoolable
        where E : Enum
        where A : ConstructionArgs
        where M : class, ICleanable, new()
    {
        protected MonoBehaviourManager()
        {
            //Factory
            if (string.IsNullOrEmpty(PrefabLocation))
                Debug.LogError("The PrefabLocation Property of " + typeof(M) + " has not been set");
            else m_prefabFactory = new PrefabFactory<T, E, A>(PrefabLocation);

            //ObjectPool
            m_pool = new ObjectPool();
        }

        #region Variables & Properties

        /// <summary>
        /// Needed for the Factory, It will start Loading automatically from "Resources/" root.
        ///Write the rest of the path and don't forget to put the " / " at the end.
        /// </summary>
        protected abstract string PrefabLocation { get; }

        private readonly PrefabFactory<T, E, A> m_prefabFactory;
        private readonly ObjectPool m_pool;

        #endregion


        public T Create(ValueType type, A constructionArgs)
        {
            var toReturn = (T)m_pool.Depool(type) ?? m_prefabFactory.Create(type, constructionArgs);
            //if the object is created in the factory, The Awake is call right after Instantiation.
            //Be careful on what is in the start function. It could mess with the values
            //that we are setting up in the construct function.
            toReturn.Construct(constructionArgs);
            Add(toReturn);

            return toReturn;
        }
        
        public void Pool(T toPool)
        {
            m_pool.Pool(toPool);
            Remove(toPool);
        }
    }

    #endregion

    #endregion
}