using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFA.TPS.Movement
{
    public class ProjectileMovement : MonoBehaviour

    {
        [SerializeField] private float _speed;
        public float Speed 
        {
            get =>  _speed; 
            set =>  _speed = value; 
        }

        [SerializeField] private Vector3  _movementPlane = Vector3.one;


        [SerializeField] private bool _shouldDestroyCollision;
        public bool ShouldDestroyCollision
        {
            get { return _shouldDestroyCollision; }
            set { _shouldDestroyCollision = value; }
        }


        [SerializeField] private bool _shouldDisableCollision;
        public bool ShouldDisableCollision
        {
            get { return _shouldDisableCollision; }
            set { _shouldDisableCollision = value; }
        }

        [SerializeField] private bool _shouldBounce;
        public bool ShouldBounce
        {
            get => _shouldBounce;
            set => _shouldBounce = value;
        }

        [SerializeField] private float _pushPower;

        public event Action<RaycastHit> Impacted;

        private void Update()
        {
            var direction = transform.forward;
            direction.x *= _movementPlane.x;
            direction.y *= _movementPlane.y;
            direction.z *= _movementPlane.z;
            direction.Normalize();
            var distance = _speed * Time.deltaTime;
            var targetPosition = transform.position + direction * distance;

            if(Physics.Raycast(transform.position, direction, out var hit, distance)) 
            {
                if (hit.rigidbody)
                {
                   hit.rigidbody.AddForceAtPosition(-hit.normal * _speed * _pushPower, hit.point, ForceMode.Impulse);
                }
                
                if(ShouldDestroyCollision)
                {
                    Destroy(gameObject);
                }

                if(ShouldDisableCollision)
                {
                    enabled = false;
                }
                targetPosition = hit.point;

                

                if (ShouldBounce)
                {
                    var reflectedDirection = Vector3.Reflect(direction, hit.normal);
                    transform.forward = reflectedDirection;
                }

                Impacted?.Invoke(hit);



            }

            Debug.DrawLine(transform.position, targetPosition,Color.red);
            transform.position = targetPosition;
            Debug.DrawRay(transform.position, direction * distance, Color.green);
        }

    }

}


