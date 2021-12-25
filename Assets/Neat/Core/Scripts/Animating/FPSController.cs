using Neat.InputHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neat.DDD
{
    public enum FPSPose
    {
        Standing,
        Aiming,

    }

    //[ExecuteInEditMode]
    public class FPSController : MonoBehaviour
    {
        public Animator animator;
        public Rigidbody rb;

        [Range(0f, 100f)]
        public float moveScale;

        [Range(0f, 50f)]
        public float runSpeed;

        [Range(0f, 10f)]
        public float animSpeed;

        private MouseRotator look;
        
        private Vector3 localVelocity => transform.InverseTransformDirection(rb.velocity); // // https://answers.unity.com/questions/193398/velocity-relative-to-local.html
        public Vector3 normalizedVelocity => localVelocity / runSpeed;

        public const string IDLE = "Idle";
        public const string STANDING = "Standing";

        // layers
        public const int L_LEGS = 1;
        public const int L_BASE = 0;

        // parameters
        public const string SPEED_X = "Speed X";
        public const string SPEED_Z = "Speed Z";



        public static readonly string[] poses =
        {
            "T",
            "Standing",
        };


        private Keybinds.FPSActions input;

        private Vector3 moveInput;

        private void OnDrawGizmos()
        {
            // Gizmos.DrawLine(transform.position, transform.position +);
        }

        private void OnEnable()
        {
            input = GetComponent<KeybindsComponent>().keybinds.FPS;
            look = GetComponent<MouseRotator>();
        }

        private void OnValidate()
        {
            //text = "";
            //text += $"normalizedVelocity= {normalizedVelocity}";
            //text += $"\nclipInfoLength= {animator.GetCurrentAnimatorClipInfo(0).Length}";

            //animator.Play(STANDING, 1);

            animator.speed = animSpeed;
        }

        void Start()
        {
            animator.Play(STANDING, 1);
        }

        void Update()
        {
            moveInput = input.Move3D.ReadValue<Vector3>();

            animator.SetFloat(SPEED_X, normalizedVelocity.x);
            animator.SetFloat(SPEED_Z, normalizedVelocity.z);
        }
        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            var forceVector = moveInput * moveScale;
            rb.AddRelativeForce(forceVector, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, runSpeed);
        }
    }


#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FPSController), true, isFallback = true)]
    public class FPSControllerEditor : Editor
    {
        int selected;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = target as FPSController;
            var animator = _target.animator;


            //EditorGUI.BeginChangeCheck();
            //selected = EditorGUILayout.Popup(selected, FPSController.poses);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    animator.Play(FPSController.poses[selected])
            //}


            //EditorGUILayout.EnumPopup(typeof(FPSPose) as System.Enum);

            if (GUILayout.Button("Idle"))
            {
                Undo.RecordObject(target, "Setting default animation state");
                animator.StopPlayback();
                animator.Play("Standing", 1);
                animator.Update(0);
            }

            else if (GUILayout.Button("T Pose"))
            {
                Undo.RecordObject(target, "To T pose");
                animator.StopPlayback();
                animator.Play("T", 0);
                animator.Update(0);
            }
        }
    }
#endif
}
