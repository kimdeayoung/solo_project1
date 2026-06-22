using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum CommandContextFlag
{
    BlockBackspace = 1,
    MoveNextImmediate = 1 << 1,
}
