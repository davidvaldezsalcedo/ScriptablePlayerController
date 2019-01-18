using UnityEngine;

public class PlayerController : StateManager
{
    // Use a Scriptable Object Library to use a TransfromVariable
    // [SerializeField]
    // private TransformVariable _MyTransform;
    public bool ReadyToMove;

    protected override void Awake()
    {
        base.Awake();

        // if(_MyTransform != null)
        // {
        //     _MyTransform.Value = this.transform;
        // }
    }

}
