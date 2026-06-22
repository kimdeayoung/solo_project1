using System.Threading;
using UnityEngine;

public class WaitForSeconds : CommandContext
{
    [SerializeField] private float second;

#if ENABLE_UNITASK
    private CancellationTokenSource cancelToken;
#endif

    public override CommandContextType CommandContextType => CommandContextType.WaitForSeconds;

    protected override void OnEnter()
    {
#if ENABLE_UNITASK
        cancelToken = new CancellationTokenSource();
        CommandSystem.WaitForSeconds(second, MoveNext, cancelToken);
#else
        CommandSystem.WaitForSeconds(second, MoveNext);
#endif
    }

    public override void OnExit()
    {
#if ENABLE_UNITASK
        ReleaseCancelToken();
#endif
    }

    public override void OnStop()
    {
#if ENABLE_UNITASK
        ReleaseCancelToken();
#endif
    }

#if ENABLE_UNITASK
    private void ReleaseCancelToken()
    {
        if (cancelToken != null)
        {
            cancelToken.Cancel();
            cancelToken.Dispose();

            cancelToken = null;
        }
    }
#endif
}
