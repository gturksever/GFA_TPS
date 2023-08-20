using GFA.TPS.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFA.TPS
{
    public class ProjectileDamage : MonoBehaviour
    {
        private ProjectileMovement _projectileMovement;
        [SerializeField] private float _damage = 1;
        private void Awake()
        {
            _projectileMovement = GetComponent<ProjectileMovement>();
        }

        private void OnEnable()
        {
            _projectileMovement.Impacted += OnImpacted;
        }

        private void OnDisable()
        {
            _projectileMovement.Impacted -= OnImpacted;
        }

        private void OnImpacted(RaycastHit hit)
        {
            if(hit.transform.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.ApplyDamage(_damage, gameObject);

            }
        }
    }
}

