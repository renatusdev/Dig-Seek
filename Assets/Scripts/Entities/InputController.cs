using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController i;

    public Vector2 Move     { get; private set; }
    public bool JumpDown    { get; private set; }    
    public bool JumpHold    { get; private set; }
    public bool JumpUp      { get; private set; }
    public bool DrillHold   { get; private set; }
    public bool DrillDown   { get; private set; }
    public bool DrillUp     { get; private set; }
    public bool Escape      { get; private set; }

    private void Awake()
    {
        if(i == null)
        {
            i = this;
            DontDestroyOnLoad(this.gameObject);    
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Update()
    {
        Move        = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        JumpHold    = Input.GetButton       ("Jump");
        JumpDown    = Input.GetButtonDown   ("Jump");
        JumpUp      = Input.GetButtonUp     ("Jump");
        DrillHold   = Input.GetButton       ("DrillOrAttack");
        DrillDown   = Input.GetButtonDown   ("DrillOrAttack");
        DrillUp     = Input.GetButtonUp     ("DrillOrAttack");

        Escape      = Input.GetKeyDown(KeyCode.Escape);
    }
}