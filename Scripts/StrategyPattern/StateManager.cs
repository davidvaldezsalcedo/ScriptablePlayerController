using SA.Variables;
using UnityEngine;

namespace SA
{
    public class StateManager : MonoBehaviour
    {
        [SerializeField]
        private RefTransform _PlayerTransform;
        [SerializeField]
        private RotateBasedOnPointer _PlayerFaceDirection;
        [SerializeField]
        private RefCam _Cam;

        public State CurrentState;

        public float Health;

        public Transform MyTransform;
        public Animator Anim;
        internal Rigidbody Rb;
        internal float Delta;
        

        void Awake()
        {
            MyTransform = this.transform;
            Anim = GetComponentInChildren<Animator>();
            Rb = GetComponent<Rigidbody>();
            Rb.constraints = RigidbodyConstraints.FreezeRotation;

            if(_PlayerTransform != null)
            {
                _PlayerTransform.Value = MyTransform;
            }
        }

        void Update()
        {
            Delta = Time.deltaTime;

            if(CurrentState != null)
            {
                /*      NOTE: uncomment to use the RotateBasedOnPointer example camera script

                Ray ray = _Cam.Cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 1000))
                {
                    Vector3 point = hit.point;
                    _PlayerFaceDirection.MousePosition = point;
                }
                else
                {
                    _PlayerFaceDirection.MousePosition = Vector3.zero;
                }

                */
                CurrentState.Tick(this);
            }
        }
    }
}
