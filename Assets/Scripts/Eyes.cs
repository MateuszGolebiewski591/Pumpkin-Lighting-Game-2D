using UnityEngine;
using System.Collections;
public class Eyes : MonoBehaviour
{
    private Animator animator;
    private bool down = true;

    void Awake()
    {
        this.animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(RandomBlink());
        StartCoroutine(Move());
    }

    private IEnumerator RandomBlink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 15f));
            animator.SetTrigger("blink");
            yield return null;
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (down)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime/5, transform.position.z);
                if (transform.position.y <= 2.5) down = false;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime/5, transform.position.z);
                if (transform.position.y >= 2.6) down = true;
            }
            yield return null;
        }
    }
}
