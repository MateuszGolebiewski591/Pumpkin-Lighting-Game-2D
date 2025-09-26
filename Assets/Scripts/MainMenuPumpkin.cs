using UnityEngine;
using System.Collections;

public class MenuBackgroundCycle : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float minIdleTime = 0f;
    [SerializeField] private float maxIdleTime = 30f;
    [SerializeField] private float minLitTime = 0f;
    [SerializeField] private float maxLitTime = 30f;
    [SerializeField] private float jumpAmount = 0.5f; // how much Y shift
    [SerializeField] private float minY = -2f;        // lower bound
    [SerializeField] private float maxY = 2f;         // upper bound

    private Vector3 basePosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Start()
    {
        basePosition = transform.position;
        StartCoroutine(CycleRoutine());
    }

    private IEnumerator CycleRoutine()
    {
        while (true)
        {
            // --- Idle Phase ---
            float idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(idleTime);

            // trigger lighting up
            animator.SetTrigger("alight");

            // wait until lit animation state is entered
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("lit"));

            // --- Lit Phase ---
            float litTime = Random.Range(minLitTime, maxLitTime);
            yield return new WaitForSeconds(litTime);

            // trigger return to idle
            animator.SetTrigger("dim");

            yield return new WaitForSeconds(2f);

            // adjust Y position randomly
            float newY = Mathf.Clamp(
                transform.position.y + Random.Range(-jumpAmount, jumpAmount),
                minY, maxY
            );
            transform.position = new Vector3(basePosition.x, newY, basePosition.z);
        }
    }
}
