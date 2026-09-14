
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public struct CubeCoord
{
    public int x;
    public int y;
    public int z;
    public CubeCoord(int x,int y, int z){
        this.x = x;
        this.y = y;
        this.z = z;
    }

}

public interface IHoverable
{
    void OnClicked();
    void OnHoverEnter();
    void OnHoverExit();
}



public class Utils 
{
    private static Image fadeImage;
    private static Sequence fadeSequence;

    /// <summary>화면을 어둡게 만든 후 콜백을 실행합니다.</summary>
    public static void FadeOut(Action onComplete = null, float duration = 0.5f)
    {
        StartFade(1f, onComplete, duration, false);
    }

    /// <summary>화면을 밝게 만든 후 콜백을 실행합니다.</summary>
    public static void FadeIn(Action onComplete = null, float duration = 0.5f)
    {
        StartFade(0f, onComplete, duration, false);
    }

    /// <summary>암전 후 action을 실행하고 다시 밝아집니다. duration은 각 페이드의 시간입니다.</summary>
    public static void FadeOutIn(Action action, float duration = 0.5f)
    {
        StartFade(1f, action, duration, true);
    }

    private static void StartFade(float targetAlpha, Action action, float duration, bool fadeBackIn)
    {
        if (float.IsNaN(duration) || float.IsInfinity(duration) || duration < 0f)
            throw new ArgumentOutOfRangeException(nameof(duration));

        // 새 요청은 이전 페이드와 아직 실행되지 않은 콜백을 취소합니다.
        fadeSequence?.Kill();
        EnsureFadeImage(targetAlpha == 0f ? 1f : 0f);
        fadeImage.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        fadeSequence = sequence;
        sequence.Append(fadeImage.DOFade(targetAlpha, duration).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            if (!fadeBackIn && targetAlpha == 0f)
                fadeImage.gameObject.SetActive(false);

            // 콜백에 오류가 있어도 다시 밝아지는 애니메이션은 계속 진행합니다.
            try { action?.Invoke(); }
            catch (Exception exception) { Debug.LogException(exception); }
        });

        if (fadeBackIn)
        {
            sequence.Append(fadeImage.DOFade(0f, duration).SetEase(Ease.Linear));
            sequence.AppendCallback(() => fadeImage.gameObject.SetActive(false));
        }

        sequence.OnKill(() =>
        {
            if (fadeSequence == sequence)
                fadeSequence = null;
        });
    }

    private static void EnsureFadeImage(float initialAlpha)
    {
        if (fadeImage != null)
            return;

        GameObject canvasObject = new GameObject("ScreenFade", typeof(Canvas), typeof(GraphicRaycaster));
        UnityEngine.Object.DontDestroyOnLoad(canvasObject);
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;

        GameObject imageObject = new GameObject("BlackOverlay", typeof(RectTransform), typeof(Image));
        imageObject.transform.SetParent(canvasObject.transform, false);
        fadeImage = imageObject.GetComponent<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, initialAlpha);
        fadeImage.raycastTarget = true;
        RectTransform rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    public static CubeCoord OffsetToCube(Vector2Int offset)
    {
        int offsetX = offset.x - (offset.y + (offset.y & 1)) / 2;
        int offsetZ = offset.y;
        int offsetY = -offsetX - offsetZ;

        return new CubeCoord(offsetX, offsetY , offsetZ);
    }

    public static int GetHexDistance(Vector2Int a , Vector2Int b)
    {
        CubeCoord cubeA = OffsetToCube(a);
        CubeCoord cubeB = OffsetToCube(b);

        return Mathf.Max(Mathf.Abs(cubeA.x - cubeB.x), Mathf.Abs(cubeA.y - cubeB.y), Mathf.Abs(cubeA.z - cubeB.z));

    }
    
}

