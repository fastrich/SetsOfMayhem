using System;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
using PlayerCtrl;
using static UIconfig;
using static CommunicationEvents;
using UnityEngine.InputSystem;

//namespace UnityStandardAssets.Characters.FirstPerson
namespace Characters.FirstPerson

{
    [Serializable]
    public class MouseLook1
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smoothCamMotion;
        public float smoothTime = 5f;
        public bool lockCursor = false;

        private ControlMapping input_ControlMapping;
        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = false;


        //private PlayerInput playerInput;
        //Store the controls
       // private InputAction action_LookCamera;


        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
            input_ControlMapping = new ControlMapping();
            input_ControlMapping.Actionmap1.LookCamera.Enable();
            input_ControlMapping.Actnmp_HC.LookCamera.Enable();


        }
        private void OnEnable()
        {
            input_ControlMapping.Actionmap1.LookCamera.Enable();
            input_ControlMapping.Actnmp_HC.LookCamera.Enable();
            //NPC1.setPlayer(gameObject);
        }

        private void OnDisable()
        {
            input_ControlMapping.Actionmap1.LookCamera.Disable();
            input_ControlMapping.Actnmp_HC.LookCamera.Disable();

        }





        public void LookRotation(Transform character, Transform camera)
        {
            XSensitivity = UIconfig.camRotatingSensitivity;
            YSensitivity = XSensitivity;
            Vector2 a=Vector2.zero;

            float yRot=0;
            float xRot=0;

            if (UIconfig.InputManagerVersion == 1)
            {
                // Read input
                a.x = CrossPlatformInputManager.GetAxis("Mouse X") ;
                a.y = CrossPlatformInputManager.GetAxis("Mouse Y") ;


            }
            if (UIconfig.InputManagerVersion == 2)
            {

                
                if (CommunicationEvents.Opsys == OperationSystem.Android)
                {
                    a = input_ControlMapping.Actnmp_HC.LookCamera.ReadValue<Vector2>();

                }
                else
                {
                    a = input_ControlMapping.Actionmap1.LookCamera.ReadValue<Vector2>();

                }

                
                //xRot =  XSensitivity * input_ControlMapping.Actionmap1.MouseX.ReadValue<float>();
                //yRot = 100* YSensitivity * input_ControlMapping.Actionmap1.MouseY.ReadValue<float>();
            }
            if (UIconfig.InputManagerVersion == 3)
            {
               
                
                // Read input

                a.y = ButtonsToAxe(DPAD[1, 1], DPAD[1, 0]);
                a.x = ButtonsToAxe(DPAD[1, 3], DPAD[1, 2]);

           
                
                

            }
            if (a.sqrMagnitude > 1)
            {
                a.Normalize();
            }
            //xRot = 1;
            yRot = a.x * XSensitivity;
            xRot = a.y * YSensitivity;


            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smoothCamMotion)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;//Wieso magst du nicht nach Build!
                
            }
            //if (yRot != 0) { Debug.Log("my" + yRot); Debug.Log("mc" + camera.localRotation); }
            //if (xRot != 0) { Debug.Log("mx" + xRot); Debug.Log("mc" + camera.localRotation); } //Hoch runter
            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }


        public float ButtonsToAxe(float up, float down)
        {
                float axe = 0;

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
