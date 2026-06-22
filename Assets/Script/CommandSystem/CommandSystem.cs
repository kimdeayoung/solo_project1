using System.Collections.Generic;
using UnityEngine;
#if ENABLE_UNITASK
using System.Threading;
using Cysharp.Threading.Tasks;
#endif

public static class CommandSystem
{

#if ENABLE_UNITASK
    private static readonly HashSet<int> EXECUTED_PAUSE_IDENTIFIERS = new HashSet<int>(4);
#else
    private static readonly Queue<WaitForSecondsRealtime> WAIT_FOR_SECONDS_REALTIME = new Queue<WaitForSecondsRealtime>(8);
    private static readonly Dictionary<int, Coroutine> EXECUTED_PAUSE_COROUTINES = new Dictionary<int, Coroutine>(4);
#endif

    public static void OnEnterScene()
    {

    }

    public static bool CreateNewCommandBehaviour(int identifier, int contextIndex = 0)
    {


        return true;
    }
}
