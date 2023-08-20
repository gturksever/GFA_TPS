using GFA.TPS;
using GFA.TPS.Movement;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileMovement _projectileMovement;
    [SerializeField] private float _radius;
    [SerializeField] private int _ricochetCount = 5;
    [SerializeField] private bool removeOnComplete;

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

    private void OnImpacted(RaycastHit raycastHit)
    {
        if (_ricochetCount <= 0)
        {
            if (removeOnComplete)
            {
                Destroy(this);
            }
            return;
        }
        var hits = Physics.OverlapSphere(raycastHit.point, _radius);

        foreach (var hit in hits)
        {
            if (hit.transform != raycastHit.transform) continue;
            if(hit.transform.TryGetComponent<IDamagable>(out var _))
            {
                var dir =(hit.transform.position - transform.position).normalized;
                transform.forward = dir;
                _ricochetCount--;
                return;
            }
        }
    }
}
