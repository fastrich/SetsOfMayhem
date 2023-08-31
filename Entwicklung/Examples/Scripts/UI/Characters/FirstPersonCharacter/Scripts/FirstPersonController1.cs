using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
using PlayerCtrl;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using static UIconfig;
using UnityEngine.InputSystem;
//using ControlMapping;

//namespace UnityStandardAssets.Characters.FirstPerson
namespace Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController1 : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook1 m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        public Camera m_Camera;
        //public Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private ControlMapping input_ControlMapping;
        public string Running_keyBind;

        public GameObject PositionOfActivePlayer_GObj;

        private PlayerInput playerInput;

        private Vector2 Movement_fromCallback;


        //public TaskCharakterAnimation NPC1;

        //Store the controls
        private InputAction action_jump;
        private InputAction action_movement;
        private InputAction action_run;



        private void Awake()
        {
            //New InputSystem
            input_ControlMapping = new ControlMapping();
            /*
            input_ControlMapping.Actionmap1.Movement.Enable();
                input_ControlMapping.Actnmp_HC.Movement.Enable();
                input_ControlMapping.Actnmp_HC.Move_Left.Enable();
                input_ControlMapping.Actnmp_HC.Move_Right.Enable();
                input_ControlMapping.Actnmp_HC.Move_Forward.Enable();
                input_ControlMapping.Actnmp_HC.Move_Backwards.Enable();
            */

            playerInput = GetComponent<PlayerInput>();
           
            
            //unity event;
            action_jump = playerInput.actions["Jump"];
            action_run = playerInput.actions["Run"];
            action_movement = playerInput.actions["Movement"];
            //action_movement.performed+=callbackMovement;

            //playerInput -> C# event
            //playerInput.onActionTriggered += PlayerInput_onActionTriggered;
            
        }

        //playerInput -> C# event
        private void PlayerInput_onActionTriggered(InputAction.CallbackContext context)
        {
            Debug.Log(context);
        }


        private void OnEnable()
        {
           input_ControlMapping.Actionmap1.Movement.Enable();
                input_ControlMapping.Actnmp_HC.Movement.Enable();
                input_ControlMapping.Actnmp_HC.Move_Left.Enable();
                input_ControlMapping.Actnmp_HC.Move_Right.Enable();
                input_ControlMapping.Actnmp_HC.Move_Forward.Enable();
                input_ControlMapping.Actnmp_HC.Move_Backwards.Enable();
           
        }

        private void OnDisable()
        {
            input_ControlMapping.Actionmap1.Movement.Disable();
                input_ControlMapping.Actnmp_HC.Movement.Disable();
                input_ControlMapping.Actnmp_HC.Move_Left.Disable();
                input_ControlMapping.Actnmp_HC.Move_Right.Disable();
                input_ControlMapping.Actnmp_HC.Move_Forward.Disable();
                input_ControlMapping.Actnmp_HC.Move_Backwards.Disable();
            
        }

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            //m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);

  
        }




        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                //TEST deactivated;
                //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            SetPositionOfActivePlayerGObj();
            
        }

        private void SetPositionOfActivePlayerGObj()
        {
            PositionOfActivePlayer_GObj.transform.position = gameObject.transform.position;
            PositionOfActivePlayer_GObj.transform.rotation = gameObject.transform.rotation;
        }

        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }

        public void callbackMovement(InputAction.CallbackContext ctx)
        {
            //print("ddgdg"); 
            if (ctx.performed)
            {
                Movement_fromCallback = ctx.ReadValue<Vector2>();
            }
            
        }

        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            float horizontal = 0; ;
            float vertical=0;

            if (UIconfig.InputManagerVersion == 1)
            {
                // Read input
                horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
                vertical = CrossPlatformInputManager.GetAxis("Vertical");
                //print("CrossInputH" + horizontal);
                //print("CrossInputH" + horizontal);
                //print("InputV" + Input.GetAxis("Vertical"));

#if !MOBILE_INPUT
                // On standalone builds, walk/run speed is modified by a key press.
                // keep track of whether or not the character is walking or running
                //m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
                //m_IsWalking = !Input.GetButtonDown(Running_keyBind);
                //m_IsWalking = !Input.GetKey(Running_keyBind);
#endif
                m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
                if (UIconfig.controlMode!= ControlMode.Mobile )
                {
                    //m_IsWalking = !Input.GetKey(Running_keyBind);
                }


                m_Input = new Vector2(horizontal, vertical);
            }
            if (UIconfig.InputManagerVersion == 2)
            {
                Vector2 a = Vector2.zero;
                float b = 0f;
                if (CommunicationEvents.Opsys == CommunicationEvents.OperationSystem.Android)
                {
                    switch (UIconfig.touchControlMode)
                    {
                        case 0:
                            
                            break;
                        case 1:
                            //vertical = ButtonsToAxe(input_ControlMapping.Actnmp_HC.Move_Left.ReadValue<float>(), input_ControlMapping.Actnmp_HC.Move_Right.ReadValue<float>());
                            //horizontal = ButtonsToAxe(input_ControlMapping.Actnmp_HC.Move_Forward.ReadValue<float>(), input_ControlMapping.Actnmp_HC.Move_Backwards.ReadValue<float>());
                            vertical = input_ControlMapping.Actnmp_HC.Move_Forward.ReadValue<float>();
                            horizontal = 0;


                            m_Input = new Vector2(horizontal, vertical);
                            break;
                        case 2:
                            a = input_ControlMapping.Actnmp_HC.Movement.ReadValue<Vector2>();

                            break;
                            
                        case 3:
                            a = input_ControlMapping.Actnmp_HC.Movement.ReadValue<Vector2>();
                           
                            break;

                        default:
                            a = input_ControlMapping.Actnmp_HC.Movement.ReadValue<Vector2>();
                           
                            break;
                    }
                    
                }
                else
                {
                    //a = input_ControlMapping.Actionmap1.Movement.ReadValue<Vector2>();
                    a = action_movement.ReadValue<Vector2>();
                    //print("ActionMovement: " + action_movement.ReadValue<Vector2>() +"+"+ Movement_fromCallback + " +" + action_jump.ReadValue<float>() + "bbisher " + input_ControlMapping.Actionmap1.Movement.ReadValue<Vector2>());
                    b = action_run.ReadValue<float>();
                }

                
                if (b != 0)
                {
                    m_IsWalking = false;
                }else{
                    m_IsWalking = true;
                }
                m_Input = a;


            }

            if (UIconfig.InputManagerVersion == 3)
            {

                vertical =  ButtonsToAxe(DPAD[0, 1], DPAD[0, 0]);
                horizontal = ButtonsToAxe(DPAD[0, 3], DPAD[0, 2]);


                m_Input = new Vector2(horizontal, vertical);

            }





                bool waswalking = m_IsWalking;


            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
           

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        public float ButtonsToAxe(float up, float down)
        {
            float axe=0;

            if (up != 0) { axe = up; }
            if (down != 0) { axe = down; }

            if (down != 0 && up != 0)
            {
                axe = 0;
            }

            return axe;

        }


    }




}
