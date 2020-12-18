using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtilities
{
    static int frameFreezeCount = 0;

    public static IEnumerator PauseForFrames(int frames, float defaultTimeScale)
    {
        if (frames < frameFreezeCount)
        {
            yield break;
        }

        frameFreezeCount = frames;
        Time.timeScale = 0f;
        while (true)
        {
            int framesEndPoint = Time.frameCount + frames;
            while (Time.frameCount < framesEndPoint)
            {
                yield return null;
            }
            Time.timeScale = defaultTimeScale;
            frameFreezeCount = 0;
            yield break;
        }
    }
}

// https://answers.unity.com/questions/1040319/whats-the-proper-way-to-queue-and-space-function-c.html
public class CoroutineQueue
{
    MonoBehaviour m_Owner = null;
    Coroutine m_InternalCoroutine = null;
    Queue<IEnumerator> actions = new Queue<IEnumerator>();

    public CoroutineQueue(MonoBehaviour aCoroutineOwner)
    {
        m_Owner = aCoroutineOwner;
    }

    public void StartLoop()
    {
        m_InternalCoroutine = m_Owner.StartCoroutine(Process());
    }

    public void StopLoop()
    {
        m_Owner.StopCoroutine(m_InternalCoroutine);
    }

    public void EnqueueAction(IEnumerator aAction)
    {
        actions.Enqueue(aAction);
        if (m_InternalCoroutine == null)
        {
            StartLoop();
        }
    }

    public void EnqueueWait(float aWaitTime)
    {
        actions.Enqueue(Wait(aWaitTime));
    }

    private IEnumerator Wait(float aWaitTime)
    {
        yield return new WaitForSeconds(aWaitTime);
    }

    private IEnumerator Process()
    {
        while (true)
        {
            if (actions.Count > 0)
                yield return m_Owner.StartCoroutine(actions.Dequeue());
            else
                yield return null;
        }
    }
}