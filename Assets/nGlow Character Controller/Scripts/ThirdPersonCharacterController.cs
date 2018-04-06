using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGlow
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class ThirdPersonCharacterController : MonoBehaviour
    {

        #region Variable Declarations
        // Visible in Inspector
        [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
        [SerializeField] float groundCheckDistance = 0.1f;
        [SerializeField] float animSpeedMultiplier = 1f;

        // Private Variables
        bool grounded;
        Vector3 groundNormal;
        new Rigidbody rigidbody;
        Animator animator;
        // Animator Hashes
        int forwardSpeedHash;
        int groundedHash;
        int upVelocityHash;
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            forwardSpeedHash = Animator.StringToHash("ForwardSpeed");
            groundedHash = Animator.StringToHash("Grounded");
            upVelocityHash = Animator.StringToHash("UpVelocity");
        }
        #endregion



        #region Public Functions
        public void Move(Vector3 direction, bool jump)
        {
            if (direction.magnitude > 1f) direction.Normalize();
            direction = transform.InverseTransformDirection(direction);
            CheckGroundStatus();
            direction = Vector3.ProjectOnPlane(direction, groundNormal);

            // control and velocity handling is different when grounded and airborne:
            if (!grounded)
            {
                HandleAirborneMovement();
            }

            // send input and other state parameters to the animator
            UpdateAnimator(direction);
        }
        #endregion



        #region Private Functions
        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
            {
                groundNormal = hitInfo.normal;
                grounded = true;
                animator.applyRootMotion = true;
            }
            else
            {
                grounded = false;
                groundNormal = Vector3.up;
                animator.applyRootMotion = false;
            }
        }


        void UpdateAnimator(Vector3 moveDirection)
        {
            // update the animator parameters
            animator.SetFloat(forwardSpeedHash, moveDirection.magnitude, 0.1f, Time.deltaTime);
            animator.SetBool(groundedHash, grounded);
            if (!grounded)
            {
                animator.SetFloat(upVelocityHash, rigidbody.velocity.y);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (grounded && moveDirection.magnitude > 0)
            {
                animator.speed = animSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                animator.speed = 1;
            }
        }


        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rigidbody.AddForce(extraGravityForce);
        }
        #endregion
    }
}