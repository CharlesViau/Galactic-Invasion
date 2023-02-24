using System;
using System.Reflection;
using UnityEngine;

namespace Helper
{
    public abstract class Helper
    {
        /// <summary>
        /// Get the Closest Object by asking a singleton manager to return you the closest.
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="currentTransform"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Transform GetClosetInRange(Type managerType, Transform currentTransform, float range) // 
        {
            var manager = managerType
                .GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                ?.GetValue(null);
            var info = managerType.GetMethod("GetClosest");
            if (info == null)
            {
                Debug.Log("Couldn't find method in manager ");
                return null;
            }

            var newTransform = (Transform)info.Invoke(manager, new object[] { currentTransform, range });
            return newTransform == null ? null : newTransform;
        }
    }
}