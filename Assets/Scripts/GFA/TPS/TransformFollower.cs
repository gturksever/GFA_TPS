using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFA.TPS
{
    public class TransformFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void Update()
        {
            transform.position = _target.position;
        }

    }
}


