using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public void OnVisible()
    {
        SceneInstance instance = GameManager.Instance.SceneInstance();
        instance.SafeAreaController.RegisterSafeArea(this);
    }

    public void OnInvisible()
    {
        SceneInstance instance = GameManager.Instance.SceneInstance();
        instance.SafeAreaController.UnRegisterSafeArea(this);
    }

    public void Refresh(Vector2 anchorMin, Vector2 anchorMax)
    {
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
