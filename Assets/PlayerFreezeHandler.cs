using System.Collections;
using UnityEngine;

public class PlayerFreezeHandler : MonoBehaviour
{
    public float freezeDuration = 4f;
    public GameObject freezeOverlay;
    private bool isFrozen = false;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (freezeOverlay != null)
            freezeOverlay.SetActive(false);
    }

    public void Freeze()
    {
        if (!isFrozen)
            StartCoroutine(FreezeCoroutine());
    }

    private IEnumerator FreezeCoroutine()
    {
        isFrozen = true;

        Vector2 originalVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        animator.speed = 0f;

        if (freezeOverlay != null)
            freezeOverlay.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        if (freezeOverlay != null)
            freezeOverlay.SetActive(false);

        yield return new WaitForSeconds(freezeDuration - 0.5f);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = originalVelocity;
        animator.speed = 1f;

        isFrozen = false;
    }

}
