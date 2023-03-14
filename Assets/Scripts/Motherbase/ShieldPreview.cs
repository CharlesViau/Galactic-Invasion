using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Motherbase
{
    public class ShieldPreview : MonoBehaviour
    {
        private int index;
        
        [SerializeField] private CoreMotherBase mb;

        public void SetIndex(int p_index)
        {
            index = p_index;
        }
        private void OnMouseOver()
        {
            //Add feedback here
        }

        private void OnMouseDown()
        {
            mb.spawnShield(index);
            gameObject.SetActive(false);
        }

        private void OnMouseExit()
        {
            //Add feedback here
        }
    }
}

