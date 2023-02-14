using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private readonly HashSet<TopDownManager> _topDownManagers = new HashSet<TopDownManager>();
        private readonly HashSet<MonoBehaviourManager> _monoBehaviourManagers = new HashSet<MonoBehaviourManager>();

        #region MainEntry

        private void Awake()
        {
            AddManagersToList();
            InitManagers();
        }

        private void Start()
        {
            PostInitManagers();
        }

        private void Update()
        {
            RefreshManagers();
        }

        private void FixedUpdate()
        {
            FixedRefreshManagers();
        }

        private void LateUpdate()
        {
            LateRefreshManagers();
        }

        private void OnDestroy()
        {
            CleanManagers();
        }

        #endregion

        #region Class Methods

        private void LateRefreshManagers()
        {
            foreach (var manager in _topDownManagers)
            {
                manager.LateRefresh();
            }
        }

        private void InitManagers()
        {
            foreach (var manager in _topDownManagers)
            {
                manager.Init();
            }
        }

        private void PostInitManagers()
        {
            foreach (var manager in _topDownManagers)
            {
                manager.PostInit();
            }
        }

        private void RefreshManagers()
        {
            foreach (var manager in _topDownManagers)
            {
                manager.Refresh();
            }
        }

        private void FixedRefreshManagers()
        {
            foreach (var manager in _topDownManagers)
            {
                manager.FixedRefresh();
            }
        }

        private void CleanManagers()
        {
            foreach (var manager in _topDownManagers)
            {
                manager.Clean();
            }

            foreach (var manager in _monoBehaviourManagers)
            {
                manager.Clean();
            }
        }

        private void AddManagersToList()
        {
            foreach (var type in
                     Assembly.GetAssembly(typeof(TopDownManager)).GetTypes().Where(myType =>
                         myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(TopDownManager))))
            {
                _topDownManagers.Add((TopDownManager)type.GetProperty("Instance",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)?.GetValue(null));
            }
            
            foreach (var type in
                     Assembly.GetAssembly(typeof(MonoBehaviourManager)).GetTypes().Where(myType =>
                         myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(MonoBehaviourManager))))
            {
                _monoBehaviourManagers.Add((MonoBehaviourManager)type.GetProperty("Instance",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)?.GetValue(null));
            }
            
        }

        #endregion
    }
}