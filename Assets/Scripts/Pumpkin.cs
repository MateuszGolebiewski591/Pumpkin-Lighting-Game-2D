using UnityEngine;
using UnityEngine.InputSystem;
public class Pumpkin : MonoBehaviour
{
    private Animator anim;
    private PumpkinLogic logic;

    private bool lit = false;
    private bool allowClick = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        logic = GetComponentInParent<PumpkinLogic>();
    }

    public void AllowClicking()
    {
        this.allowClick = true;
    }

    public void DisableClicking()
    {
        this.allowClick = false;
    }

    public void ResetPumpkin()
    {
        if (this.lit == true) anim.SetTrigger("Snuffout");
        this.lit = false;
        this.allowClick = false;
    }

    public void ClickResponse()
    {
        if (this.allowClick)
        {
            anim.SetTrigger("Lightup");
            this.lit = true;
            this.logic.HandleClick();
        }
    }  
    public void TriggerAnim()
    {
        if (this.allowClick)
        {
            anim.SetTrigger("Lightup");
            this.lit = true;
        }
    }    
}
