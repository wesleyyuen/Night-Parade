using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject exitBlock;
    [SerializeField] float bossExitShuttingTime;
    [SerializeField] Vector2 bossExitShuttingDirection;
    [SerializeField] Animator animator;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            // Start Boss Engagement
            animator.SetBool("FightStarted", true);
            // Closing Boss Exit
            Timing.RunCoroutine(_BossExitControl(bossExitShuttingDirection));
        }
    }

    // Allow it to be called when boss dies
    public void OpenExit()
    {
        Timing.RunCoroutine(_BossExitControl(new Vector2(0f, 0f)));
    }

    // Coroutine to open or close exit based on direction given
    IEnumerator<float> _BossExitControl(Vector2 direction)
    {
        float x = exitBlock.transform.position.x;
        float y = exitBlock.transform.position.y;
        for (float t = 0f; t < 1f; t += Timing.DeltaTime / bossExitShuttingTime) {
            exitBlock.transform.position = new Vector3(Mathf.SmoothStep(x, direction.x, t), Mathf.SmoothStep(y, direction.y, t), exitBlock.transform.position.z);
            yield return Timing.WaitForOneFrame;
        }
    }
}