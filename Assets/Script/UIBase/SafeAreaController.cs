using System;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaController
{
    private List<SafeArea> safeAreas = new List<SafeArea>(32);
    private Vector2Int screenSize;

    private Vector2 minAnchor;
    private Vector2 maxAnchor;

    public event Action OnChangedScreenSize;

    public SafeAreaController()
    {
        UpdateProperties();
    }

    public void OnUpdate()
    {
        if (screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            UpdateProperties();

            int loopCount = safeAreas.Count;
            for (int i = 0; i < loopCount; i++)
            {
                SafeArea safeArea = safeAreas[i];
                safeArea.Refresh(minAnchor, maxAnchor);
            }
            OnChangedScreenSize?.Invoke();
        }
    }

    public void RegisterSafeArea(SafeArea safeArea)
    {
        safeAreas.Add(safeArea);
        safeArea.Refresh(minAnchor, maxAnchor);
    }

    public void UnRegisterSafeArea(SafeArea safeArea)
    {
        safeAreas.Remove(safeArea);
    }

    private void UpdateProperties()
    {
        screenSize = new Vector2Int(Screen.width, Screen.height);

        Rect safeArea = Screen.safeArea;
        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = minAnchor + safeArea.size;

        this.minAnchor = new Vector2(minAnchor.x / Screen.width, minAnchor.y / Screen.height);
        this.maxAnchor = new Vector2(maxAnchor.x / Screen.width, maxAnchor.y / Screen.height);
    }
}
