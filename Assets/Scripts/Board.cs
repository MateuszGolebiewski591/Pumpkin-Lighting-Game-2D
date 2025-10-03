using UnityEngine;

public class Board : MonoBehaviour
{

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    public void UpdateText(string newText)
    {
        anim.SetTrigger(newText);
    }
}
