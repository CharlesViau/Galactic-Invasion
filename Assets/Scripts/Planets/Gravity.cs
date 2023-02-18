using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public abstract class Gravity: MonoBehaviour
    {
        public abstract void Affect(Transform enemyTransform);
    }
}

