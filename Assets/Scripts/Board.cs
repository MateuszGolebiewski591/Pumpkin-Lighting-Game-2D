using UnityEngine;

public class Board : MonoBehaviour
{

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    public void updateText(string newText)
    {
        anim.SetTrigger(newText);
    }
}
