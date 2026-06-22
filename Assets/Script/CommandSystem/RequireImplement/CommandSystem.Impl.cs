using UnityEngine;

public static partial class CommandSystem
{
    public static void SetPauseInternal()
    {
        Time.timeScale = 0.0f;
    }

    public static void SetResumeInternal()
    {
        Time.timeScale = 1.0f;
    }
}
