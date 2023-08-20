using Cinemachine;
using GFA.TPS.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFA.TPS
{
    public class ExplosiveBarrel : MonoBehaviour, IDamagable
    {
        [SerializeField] private float _health=5;

        [SerializeField] private float _explosionRadius = 5;

        [SerializeField] private float _explosionDamage = 5;

        [SerializeField] private float _explosionForce = 50;

        [SerializeField] private AnimationCurve _explosionFalloff;

        [SerializeField] private float _delayBeforeExplosion;
        private bool _isDead;

        private CinemachineImpulseSource _impulseSource;

        [SerializeField] private float _cameraShakePower=1;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void ApplyDamage(float damage, GameObject causer = null)
        {
            if (_isDead) return;
            _health -= damage;
            if (_health <= 0)
            {
                StartCoroutine(ExplodeDelayed());
                Explode();
                _isDead = true;
            }
        }

        IEnumerator ExplodeDelayed()
        {
            yield return new WaitForSeconds(_delayBeforeExplosion);
        }

        public void Explode()
        {
            var hits = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (var hit in hits)
            {
                if (hit.transform == transform) continue;
                var distance = Vector3.Distance(transform.position, hit.transform.position);
                var rate = distance / _explosionRadius;
                var falloff = _explosionFalloff.Evaluate(rate);

                if (hit.transform.TryGetComponent<IDamagable>(out var damagable))
                {
                    damagable.ApplyDamage(_explosionDamage * falloff);
                }

                if(hit.transform.TryGetComponent<CharacterMovement>(out var movement))
                {
                    movement.ExternalForces += (hit.transform.position - transform.position).normalized *_explosionForce* falloff * .2f;
                }

                if (hit.attachedRigidbody)
                {
                    hit.attachedRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1, ForceMode.Impulse);
                }
            }
            _impulseSource.GenerateImpulseAt(transform.position,new Vector3(1,1,0) * _cameraShakePower);
            Destroy(gameObject);
        }

        
    }
}

