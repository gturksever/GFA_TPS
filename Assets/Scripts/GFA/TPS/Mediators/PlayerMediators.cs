using GFA.TPS.Movement;
using GFA_TPS_Input;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GFA.TPS.Mediators
{
    public class PlayerMediators : MonoBehaviour
    {
        private CharacterMovement _characterMovement;
        private Shooter _shooter;

        private GameInput _gameInput; //input managerde olu�turulan input sistemini entegre eetik

        [SerializeField] private float _dodgePower;

        private Plane _plane = new Plane(Vector3.up, Vector3.zero); //sanal bir plane ol�turuyoruz sonsuz bir planede att���m�z raycast ile i�lem yapabiliyoruz.

        private Camera _camera;

        private void Awake()
        {
            _shooter = GetComponent<Shooter>();
            _characterMovement = GetComponent<CharacterMovement>();
            _gameInput = new GameInput();
            _camera = Camera.main;
        }


        private void OnEnable()
        {
            _gameInput.Enable(); //default olarak enable olmad��� i�in enable etmemiz laz�m
            _gameInput.Player.Dodge.performed += OnDodgeRequested;
            _gameInput.Player.Shoot.performed += OnShootRequested;
        }


        private void OnDisable()
        {
            _gameInput?.Disable();
            _gameInput.Player.Dodge.performed -= OnDodgeRequested;
            _gameInput.Player.Shoot.performed -= OnShootRequested;
        }
        private void OnShootRequested(InputAction.CallbackContext obj)
        {
            _shooter.Shoot();
        }

        private void OnDodgeRequested(InputAction.CallbackContext obj)
        {
            _characterMovement.ExternalForces += _characterMovement.Velocity.normalized * _dodgePower;
        }


        private void Update()
        {
            var movementInput = _gameInput.Player.Movement.ReadValue<Vector2>();
            _characterMovement.MovementInput = movementInput;

            var ray = _camera.ScreenPointToRay(_gameInput.Player.PointerPosition.ReadValue<Vector2>());

            var gamePadLookDir = _gameInput.Player.Look.ReadValue<Vector2>();

            if(gamePadLookDir.magnitude > 0.1f)
            {
                var angle = -Mathf.Atan2(gamePadLookDir.y,gamePadLookDir.x) * Mathf.Rad2Deg + 90;
                _characterMovement.Rotation = angle;
            }
            else
            {
                if (_plane.Raycast(ray, out float enter))
                {
                    var worldPosition = ray.GetPoint(enter);
                    var dir = (worldPosition - transform.position).normalized;
                    //Quaternion.LookRotation(dir).eulerAngles.y a�a��daki kod ile bu ayn� i�i yap�yor.
                    var angle = -Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + 90;
                    _characterMovement.Rotation = angle;
                }

            }
            
            
        }

    }    
}
