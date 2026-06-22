using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Identifiers;
#if ENABLE_UNITASK
using System.Threading;
using Cysharp.Threading.Tasks;
#endif

public static partial class CommandSystem
{
    private static MonoBehaviour monoBehaviour;
#if ENABLE_UNITASK
    private static readonly HashSet<int> EXECUTED_PAUSE_IDENTIFIERS = new HashSet<int>(4);
#else
    private static readonly Queue<WaitForSecondsRealtime> WAIT_FOR_SECONDS_REALTIME = new Queue<WaitForSecondsRealtime>(8);
    private static readonly Dictionary<int, Coroutine> EXECUTED_PAUSE_COROUTINES = new Dictionary<int, Coroutine>(4);
#endif

    public static void OnEnterScene(MonoBehaviour behaviour)
    {
        monoBehaviour = behaviour;
    }

    public static bool CreateNewCommandBehaviour(int identifier, int contextIndex = 0)
    {


        return true;
    }

    #region WaitForSeconds

#if ENABLE_UNITASK
    public static async UniTask WaitForSeconds(float seconds, Action action, CancellationTokenSource token)
    {
        var isCanceled = await UniTask.WaitForSeconds(seconds, false, cancellationToken: token.Token).SuppressCancellationThrow();

        if (!isCanceled)
        {
            action?.Invoke();
        }
    }
#else
    public static void WaitForSeconds(float seconds, Action action)
    {
        monoBehaviour.StartCoroutine(DelayedInvoke(seconds, action));
    }

    private static IEnumerator DelayedInvoke(float seconds, Action action)
    {
        WaitForSecondsRealtime waitForSecondsRealtime;
        if (WAIT_FOR_SECONDS_REALTIME.Count > 0)
        {
            waitForSecondsRealtime = WAIT_FOR_SECONDS_REALTIME.Dequeue();
            waitForSecondsRealtime.waitTime = seconds;
            waitForSecondsRealtime.Reset();
        }
        else
        {
            waitForSecondsRealtime = new WaitForSecondsRealtime(seconds);
        }

        yield return waitForSecondsRealtime;
        action?.Invoke();
    }
#endif

#endregion

    #region DelayedPause

#if ENABLE_UNITASK
        public static async UniTask RegisterDelayedPause(int identifier, float delay, CancellationTokenSource token)
        {
            var isCanceled = await UniTask.WaitForSeconds(delay, false, cancellationToken: token.Token).SuppressCancellationThrow();

            if (!isCanceled)
            {
                EXECUTED_PAUSE_IDENTIFIERS.Add(identifier);
                SetPauseInternal();
            }
        }
        
        public static void ReleasePause(int identifier)
        {
            if (EXECUTED_PAUSE_IDENTIFIERS.Remove(identifier))
            {
                SetResumeInternal();
            }
        }
#else
    public static void RegisterDelayedPause(int identifier, float delay)
    {
        Coroutine coroutine = monoBehaviour.StartCoroutine(DelayedPause(identifier, delay));

        EXECUTED_PAUSE_COROUTINES.Add(identifier, coroutine);
    }

    public static void ReleasePause(int identifier)
    {
        if (EXECUTED_PAUSE_COROUTINES.TryGetValue(identifier, out Coroutine coroutine))
        {
            if (coroutine == null)
            {
                SetResumeInternal();
            }
            else
            {
                monoBehaviour.StopCoroutine(coroutine);
            }

            EXECUTED_PAUSE_COROUTINES.Remove(identifier);
        }
    }

    private static IEnumerator DelayedPause(int identifier, float delay)
    {
        WaitForSecondsRealtime waitForSecondsRealtime;
        if (WAIT_FOR_SECONDS_REALTIME.Count > 0)
        {
            waitForSecondsRealtime = WAIT_FOR_SECONDS_REALTIME.Dequeue();
            waitForSecondsRealtime.waitTime = delay;
            waitForSecondsRealtime.Reset();
        }
        else
        {
            waitForSecondsRealtime = new WaitForSecondsRealtime(delay);
        }

        yield return waitForSecondsRealtime;
        WAIT_FOR_SECONDS_REALTIME.Enqueue(waitForSecondsRealtime);

        EXECUTED_PAUSE_COROUTINES[identifier] = null;
        SetPauseInternal();
    }
#endif

    #endregion
}
