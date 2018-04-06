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
        [SerializeField] float groundCheckDistance = 0.2f;
        [SerializeField] float animSpeedMultiplier = 1f;
        [SerializeField] float stationaryTurnSpeed = 5f;
        [SerializeField] float movingTurnSpeed = 10f;
        [SerializeField] float jumpPower = 6f;

        // Private Variables
        float rotateSpeed;
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
            CheckGroundStatus();
            direction = Vector3.ProjectOnPlane(direction, groundNormal);

            Turn(direction);

            // control and velocity handling is different when grounded and airborne:
            if (grounded)
            {
                HandleGroundedMovement(jump);
            }
            else
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


        void HandleGroundedMovement(bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpPower, rigidbody.velocity.z);
                grounded = false;
                animator.applyRootMotion = false;
            }
        }


        void HandleAirborneMovement()
        {
            if (rigidbody.velocity.y <= 0)
            {
                // apply extra gravity from multiplier
                Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
                rigidbody.AddForce(extraGravityForce);
            }
        }


        void Turn(Vector3 targetDirection)
        {
            // Interpolate between the turn speeds
            float forwardAmount = targetDirection.magnitude;
            rotateSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, animator.GetFloat(forwardSpeedHash));

            if (targetDirection.magnitude != 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
        }
        #endregion
    }
}