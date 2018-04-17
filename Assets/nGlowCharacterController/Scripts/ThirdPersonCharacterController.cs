using System.Collections;
using UnityEngine;


namespace NGlow.CharacterController
{
    /// <summary>
    /// Manages the movement and animations of the character.
    /// </summary>
    /// 
    [SelectionBase]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class ThirdPersonCharacterController : MonoBehaviour
    {

        #region Variable Declarations
        // Visible in Inspector
        [Header("Movement")]
        [SerializeField] float animSpeedMultiplier = 1f;
        [SerializeField] float stationaryTurnSpeed = 5f;
        [SerializeField] float movingTurnSpeed = 10f;

        [Header("Airborne")]
        [SerializeField] float jumpPower = 6f;
        [Range(0f, 1f)] [SerializeField] float airborneMoveDamp = 0.5f;
        [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
        [SerializeField] float maxFallingVelocity = 20f;
        [SerializeField] float groundCheckDistance = 0.2f;

        [Header("Constraints")]
        [Tooltip("Limits the character to only climb slopes that are less steep (in degrees) than the indicated value.")]
        [Range(0f, 90f)] [SerializeField] float maxSlope = 45f;
        //[Tooltip("Automatically step over obstacles that are smaller than this value.")]
        //[SerializeField] float stepOffset = 0.1f;

        [Header("Animator IK")]
        [SerializeField] bool ikActive;
        [SerializeField] Transform lookObj;
        [SerializeField] Transform rightHandObj;
        [Range(0f, 3f)] [SerializeField] float ikFadeInTime = 1f;
        [Range(0f, 3f)] [SerializeField] float ikFadeOutTime = 0.5f;

        [Header("Other")]
        [SerializeField] bool debug;
        [SerializeField] AnimationClip runClip;

        // Private State Variables
        Vector3 moveDirection;
        float runSpeed;
        float currentGroundCheckDistance;
        float turnSpeed;
        bool grounded;
        float groundAngle;
        Vector3 forward;
        Vector3 smoothMoveVelocity = new Vector3();
        PhysicMaterial frictionPhysics;
        PhysicMaterial maxFrictionPhysics;
        PhysicMaterial zeroFrictionPhysics;
        bool sliding;
        float ikWeight;
        // Animator Hashes
        int forwardSpeedHash;
        int groundedHash;
        int upVelocityHash;
        // References
        new Rigidbody rigidbody;
        Animator animator;
        new CapsuleCollider collider;

        bool active = true;
        int numberOfLocks;
        public bool Active
        {
            get { return active; }
            set
            {
                if (value)
                {
                    if (numberOfLocks > 0) numberOfLocks--;
                    if (numberOfLocks == 0) active = value;
                }
                else if (!value)
                {
                    if (numberOfLocks == 0) active = value;
                    numberOfLocks++;
                }

                if (active == false)
                {
                    animator.SetFloat(forwardSpeedHash, 0f, 0f, Time.deltaTime);
                    animator.SetBool(groundedHash, true);
                    animator.SetFloat(upVelocityHash, 0f);
                }
            }
        }
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            collider = GetComponent<CapsuleCollider>();

            forwardSpeedHash = Animator.StringToHash("ForwardSpeed");
            groundedHash = Animator.StringToHash("Grounded");
            upVelocityHash = Animator.StringToHash("UpVelocity");

            CreatePhysicsMaterials();

            currentGroundCheckDistance = groundCheckDistance;
            runSpeed = runClip.averageSpeed.magnitude;
        }

        // First Implementation of a step Offset. Not quite working yet...
        //private void OnCollisionEnter(Collision collision)
        //{
        //    print("collision");
        //    foreach (ContactPoint contactPoint in collision.contacts)
        //    {
        //        float distance = contactPoint.point.y - collider.bounds.min.y;
        //        print(distance);
        //        if (contactPoint.point.y < collider.bounds.min.y + stepOffset && contactPoint.point.y > collider.bounds.min.y + 0.03f)
        //        {
        //            print("boost");
        //            //StartCoroutine(SmoothMove(transform.position, contactPoint.point, 0.1f));
        //            //rigidbody.velocity += transform.up * distance * 10;
        //            transform.position = Vector3.MoveTowards(transform.position, contactPoint.point, 10);
        //            rigidbody.velocity = transform.up;
        //            break;
        //        }
        //    }
        //}
        #endregion



        #region Public Functions
        /// <summary>
        /// Moves the character in the specified direction.
        /// </summary>
        /// <remarks>This is the main function of the class. It needs to get called in FixedUpdate and internally calls all other needed functions from the class.</remarks>
        /// <param name="direction">The direction.</param>
        /// <param name="jump">if set to <c>true</c> [jump].</param>
        public void Move(Vector3 direction, bool jump)
        {
            if (!Active) return;

            if (direction.magnitude > 1f) direction.Normalize();
            moveDirection = direction;
            CheckGroundStatus();
            Turn(moveDirection);

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
            UpdateAnimator();
        }
        #endregion



        #region Private Functions
        void CreatePhysicsMaterials()
        {
            // slides the character through walls and edges
            frictionPhysics = new PhysicMaterial
            {
                name = "frictionPhysics",
                staticFriction = .25f,
                dynamicFriction = .25f,
                frictionCombine = PhysicMaterialCombine.Average
            };

            // prevents the collider from slipping on ramps
            maxFrictionPhysics = new PhysicMaterial
            {
                name = "maxFrictionPhysics",
                staticFriction = 1f,
                dynamicFriction = 1f,
                frictionCombine = PhysicMaterialCombine.Maximum
            };

            // air physics 
            zeroFrictionPhysics = new PhysicMaterial
            {
                name = "slippyPhysics",
                staticFriction = 0f,
                dynamicFriction = 0f,
                frictionCombine = PhysicMaterialCombine.Minimum
            };
        }


        void CheckGroundStatus()
        {
            RaycastHit hitInfo;

            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, currentGroundCheckDistance))
            {
                forward = Vector3.Cross(transform.right, hitInfo.normal);
                // Currently not used, but could be useful for later
                //forwardAngle = Vector3.SignedAngle(transform.forward, forward, -transform.right);
                groundAngle = Vector3.Angle(transform.up, hitInfo.normal);
                grounded = true;
                animator.applyRootMotion = true;
                
                if (debug)
                {
                    // helper to visualise the ground check ray in the scene view
                    Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * currentGroundCheckDistance), Color.green, 5f);
                }
            }
            else
            {
                grounded = false;
                groundAngle = 0;
                forward = transform.forward;
                animator.applyRootMotion = false;

                collider.material = zeroFrictionPhysics;
                
                if (debug)
                {
                    // helper to visualise the ground check ray in the scene view
                    Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * currentGroundCheckDistance), Color.red, 5f);
                }
            }

            if (debug)
            {
                // draws the forward vector in scene view
                Debug.DrawLine(transform.position + transform.up, transform.position + transform.up + forward, Color.blue);
            }
        }

        /// <summary>
        /// Gradually turns the character in the specified target direction.
        /// </summary>
        /// <param name="targetDirection">The direction to turn to (normally the input vector).</param>
        void Turn(Vector3 targetDirection)
        {
            // Interpolate between the turn speeds
            float forwardAmount = targetDirection.magnitude;
            turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);

            if (targetDirection != Vector3.zero && targetDirection != transform.forward)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }


        void HandleGroundedMovement(bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                Jump();
            }
            else
            {
                CheckMaxSlope();
            }
        }

        /// <summary>
        /// Initializes the jump of the character.
        /// </summary>
        void Jump()
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpPower, rigidbody.velocity.z);
            grounded = false;
            animator.applyRootMotion = false;
            currentGroundCheckDistance = 0.01f;
        }

        /// <summary>
        /// Checks the maximum slope. Makes the character slide down too steep slopes.
        /// </summary>
        void CheckMaxSlope()
        {
            if (groundAngle > maxSlope)
            {
                sliding = true;
                collider.material = zeroFrictionPhysics;
            }
            else
            {
                sliding = false;
                // Change the Physics Material depending on input (standing still or moving)
                if (moveDirection == Vector3.zero) collider.material = maxFrictionPhysics;
                else collider.material = frictionPhysics;
            }
        }


        void HandleAirborneMovement()
        {
            // Apply extra gravity when falling (feels more natural)
            if (rigidbody.velocity.y <= 0)
            {
                Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
                rigidbody.AddForce(extraGravityForce);
                currentGroundCheckDistance = groundCheckDistance;
            }
            else
            {
                currentGroundCheckDistance = 0.01f;
            }
        }


        void UpdateAnimator()
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


        /// <summary>
        /// Controlling the RootMotion of the character. Called after FixedUpdate (when Animator set to "Animate Physics").
        /// </summary>
        /// <remarks>
        /// We implement this function to override the default root motion.
        /// This allows us to modify the positional speed before it's applied.
        /// </remarks>
        private void OnAnimatorMove()
        {
            if (grounded && !sliding && Time.deltaTime > 0)
            {
                Vector3 newVelocity;

                // When falling: Use the normal Root Motion and keep the y component of the velocity
                if (rigidbody.velocity.y < 0)
                {
                    newVelocity = animator.deltaPosition / Time.deltaTime;
                    newVelocity.y = rigidbody.velocity.y;
                }
                else
                {
                    // Project the movement on the forward vector to "stick to the ground"
                    newVelocity = Vector3.Project(animator.deltaPosition / Time.deltaTime, forward);
                }

                rigidbody.velocity = newVelocity;
            }

            // if airborne, add specific movement control
            else if (!grounded && Time.deltaTime > 0)
            {
                // Use only x and z axis
                Vector3 newVelocity = Vector3.Scale(rigidbody.velocity, new Vector3(1, 0, 1));
                
                // Allow for gaining speed when jumping from (nearly) standing still
                if (newVelocity.magnitude < runSpeed / 2f) newVelocity = newVelocity + moveDirection * airborneMoveDamp;
                // Clamp the length of the vector when above the threshold.
                // Now the player can still control the direction, but can't gain any more speed.
                else newVelocity = Vector3.ClampMagnitude(newVelocity + moveDirection * airborneMoveDamp, newVelocity.magnitude);

                // we preserve the existing y part of the current velocity and take care it never gets below our maxFallingVelocity.
                newVelocity.y = Mathf.Max(rigidbody.velocity.y, -maxFallingVelocity);

                rigidbody.velocity = newVelocity;
            }
        }

        void OnAnimatorIK(int layerIndex)
        {
            if (animator)
            {

                //if the IK is active, set the position and rotation directly to the goal. 
                if (ikActive)
                {

                    // Set the look target position, if one has been assigned
                    if (lookObj != null)
                    {
                        animator.SetLookAtWeight(1);
                        animator.SetLookAtPosition(lookObj.position);
                    }

                    // Set the right hand target position and rotation, if one has been assigned
                    if (rightHandObj != null)
                    {
                        float angleToRightHandObj = Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(rightHandObj.position - transform.position, transform.up));
                        
                        // Grab Object
                        if (Vector3.Distance(transform.position + transform.up, rightHandObj.position) < 0.7f && angleToRightHandObj < 50f)
                        {
                            ikWeight += Time.deltaTime * (1 / ikFadeInTime);
                            if (ikWeight > 1) ikWeight = 1;
                        }
                        // Let go of object
                        else if (ikWeight > 0)
                        {
                            ikWeight -= Time.deltaTime * (1 / ikFadeOutTime);
                            if (ikWeight < 0) ikWeight = 0;
                        }

                        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
                    }
                }

                //if the IK is not active, set the position and rotation of the hand and head back to the original position
                else
                {
                    animator.SetLookAtWeight(0);
                }
            }
        }
        #endregion



        #region Coroutines
        IEnumerator SmoothMove(Vector3 current, Vector3 target, float smoothTime)
        {
            for (float i = 0; i < smoothTime; i += Time.deltaTime)
            {
                transform.position = Vector3.SmoothDamp(current, target, ref smoothMoveVelocity, smoothTime);
                yield return null;
            }
        }
        #endregion
    }
}