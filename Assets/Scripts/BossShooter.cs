using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : EnemyShooter
{
    public float patternSwitchInterval = 5f;
    private Coroutine patternRoutine;

    public List<FirePattern> availablePatterns = new List<FirePattern>
    {
        FirePattern.CircleShot,
        FirePattern.AimedBurstWithWarning,
        FirePattern.FastBurst,
        FirePattern.SpreadShot
    };

    private void Start()
    {
        patternRoutine = StartCoroutine(PatternCycle());
    }

    IEnumerator PatternCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(patternSwitchInterval);
            SwitchToRandomPattern();
        }
    }

    void SwitchToRandomPattern()
    {
        if (availablePatterns.Count == 0) return;

        int index = Random.Range(0, availablePatterns.Count);
        firePattern = availablePatterns[index];
    }
}
