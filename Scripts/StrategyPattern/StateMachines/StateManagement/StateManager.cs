using UnityEngine;
using StateMachine;

[RequireComponent(typeof(Rigidbody))]
public class StateManager : MonoBehaviour
{
    public State CurrentState;
    
    public GameObject OwnerGO { get; private set;}
    public Animator Anim { get; private set;}
    public Rigidbody Rb { get; private set;}
    public float Delta { get; private set;}   

    protected virtual void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody>();
        Rb.constraints = RigidbodyConstraints.FreezeRotation;
        OwnerGO = gameObject;
    }

    private void Update()
    {
        Delta = Time.deltaTime;

        if(CurrentState != null)
        {
            CurrentState.Tick(this);
        }
    }
}

