using Neat.InputHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Neat.Tools
{
    public class _RifleAnimatorComponent : MonoBehaviour
    {
        public bool crouching;
        public bool aiming;


        //public float acceleration;
        //public float maxSpeed;

        public bool rootMotion;

        private Animator animator;
        private Rigidbody rb;
        private Keybinds keybinds;

        public float runSpeed;

        private Vector3 localVelocity => transform.InverseTransformDirection(rb.velocity); // // https://answers.unity.com/questions/193398/velocity-relative-to-local.html
        //public Vector3 normalizedVelocity => localVelocity / runSpeed;

        public float maxVelocityChange;
        public Vector2 moveInput2;
        public Vector3 dampenedInput3;

        private void Awake()
        {
            this.animator = GetComponent<Animator>();
            this.keybinds = GetComponent<KeybindsComponent>().keybinds;
            this.rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            GetInput();
            UpdateAnimator();
        }
        private void OnValidate()
        {
            this.rb = GetComponent<Rigidbody>();
            this.animator = GetComponent<Animator>();
            rb.isKinematic = rootMotion;
            animator.applyRootMotion = rootMotion;

        }

        void GetInput()
        {
            crouching = Input.GetKey(KeyCode.LeftControl);
            aiming = Input.GetMouseButton(1);


            moveInput2 = keybinds.FPS.Move.ReadValue<Vector2>();
            var moveInput3 = new Vector3(moveInput2.x, 0, moveInput2.y);

            // is only applied if rigidbody is not kinematic
            dampenedInput3 = Vector3.MoveTowards(dampenedInput3, moveInput3, maxVelocityChange * Time.deltaTime);
            //rb.AddRelativeForce(dampenedInput3, ForceMode.VelocityChange);
            rb.velocity = transform.InverseTransformDirection(dampenedInput3);
            //rb.MovePosition(transform.InverseTransformDirection(dampenedInput3));
            //rb.MovePosition(transform.InverseTransformPoint(dampenedInput3));

        }
        private void UpdateAnimator()
        {
            animator.SetBool("Aiming", aiming);
            animator.SetBool("Crouching", crouching);

            //animator.SetFloat("Move X", input.x);
            //animator.SetFloat("Move Y", input.y);

            if (rootMotion)
            {
                animator.SetFloat("Move X", dampenedInput3.x);
                animator.SetFloat("Move Y", dampenedInput3.z);
            }
            else
            {
                animator.SetFloat("Move X", localVelocity.x);
                animator.SetFloat("Move Y", localVelocity.z);
            }
        }
    }
}
