using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFA.TPS.Movement.Tests
{
    public class ShooterTest : MonoBehaviour
    {
        [SerializeField]
        private GameObject _projectilePrefab;

        private void Start()
        {
            StartCoroutine(shooter());
        }

        private IEnumerator shooter()
        {
            while (1 == 1)
            {
                yield return new WaitForSeconds(0.2f);
                Instantiate(_projectilePrefab,transform.position,transform.rotation);
            }

            
        }
    }
}

