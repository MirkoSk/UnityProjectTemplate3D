using UnityEngine;

namespace NGlow
{
    [RequireComponent(typeof(ThirdPersonCharacterController))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacterController characterController;
        private Transform mainCamera;
        // the world-relative desired move direction, calculated from the camForward and user input.
        private Vector3 moveDirection;
        private bool jump;


        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                mainCamera = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            characterController = GetComponent<ThirdPersonCharacterController>();
        }


        private void Update()
        {
            if (!jump)
            {
                jump = Input.GetButtonDown(Constants.INPUT_JUMP);
            }
        }

        
        private void FixedUpdate()
        {
            // read inputs
            float horizontal = Input.GetAxis(Constants.INPUT_HORIZONTAL);
            float vertical = Input.GetAxis(Constants.INPUT_VERTICAL);

            // calculate move direction to pass to character
            if (mainCamera != null)
            {
                // calculate camera relative direction to move:
                Vector3 camForward = Vector3.Scale(mainCamera.forward, new Vector3(1, 0, 1)).normalized;
                moveDirection = vertical * camForward + horizontal * mainCamera.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                moveDirection = vertical * Vector3.forward + horizontal * Vector3.right;
            }

            // pass all parameters to the character control script
            characterController.Move(moveDirection, jump);
            jump = false;
        }
    }
}
