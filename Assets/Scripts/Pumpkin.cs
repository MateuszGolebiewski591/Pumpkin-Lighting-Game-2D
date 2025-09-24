using UnityEngine;
using UnityEngine.InputSystem;
public class Pumpkin : MonoBehaviour
{
    private Animator anim;

    private bool lit = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void NextState()
    {
        if (!lit)
        {
            anim.SetTrigger("Lightup");
            lit = true;
        }
        else
        {
            anim.SetTrigger("Snuffout");
            lit = false;
        }
    }

    
}
