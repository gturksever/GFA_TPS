using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFA.TPS.Movement
{

    [RequireComponent(typeof(CharacterController))] //character controlleri bu scripte baðýmlý yaptýk silinemez yaptýk.

    public class CharacterMovement : MonoBehaviour
    {
        private CharacterController _characterController; //inspectordeki character controller için oluþturulan obje

        public Vector3 ExternalForces {  get; set; }

        public Vector2 MovementInput{ get; set; }

        [SerializeField] private float _movementSpeed = 4;

        public float Rotation { get; set; }

        public Vector3 Velocity => _characterController.velocity;


        private void Awake()
        {
            _characterController = GetComponent<CharacterController>(); //inspectordeki character controllerý tanýmladýk.
        }

        private void Update()
        {
            var movement = new Vector3(MovementInput.x, 0, MovementInput.y);

            transform.eulerAngles = new Vector3 (0, Rotation);

            _characterController.SimpleMove(movement* _movementSpeed + ExternalForces);

            ExternalForces = Vector3.Lerp(ExternalForces, Vector3.zero,t:8 * Time.deltaTime);
        }
    }

}


