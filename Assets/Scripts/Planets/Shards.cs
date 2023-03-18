using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Planets
{
    public class Shards : MonoBehaviour
    {
        private Enemy[] shards;
        // Start is called before the first frame update
        void Start()
        {
            shards = GetComponentsInChildren<Enemy>();
        }

        public void ActivateShards()
        {
            foreach (Enemy s in shards)
            {
                s.enabled = true;
            }
        }
    }
}

