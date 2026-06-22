using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class CommandContextContainerEditor
{
    private void TryCreateBeginCommandBehaviourCondition(SerializedProperty property, BeginCommandConditionType type)
    {
        switch (type)
        {
            case BeginCommandConditionType.None:
                property.managedReferenceValue = null;
                break;

            case BeginCommandConditionType.Once:
                //if (property.managedReferenceValue is not CommandBehaviour.Once)
                //{
                //    property.managedReferenceValue = new CommandBehaviour.Once();
                //}
                break;
        }
    }

    private CommandContext CreateCommandContext(CommandContextType type)
    {
        return type switch
        {
            _ => null
        };
    }
}
