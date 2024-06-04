using DG.Tweening;
using UnityEngine;

public class StagePicker : MonoBehaviour
{
    [SerializeField]
    private RectTransform stageGrid;

    Tween currentAnimation;

    private void Start()
    {
        stageGrid.anchoredPosition = new Vector2(0, -stageGrid.rect.height);
    }
    public void Open()
    {
        currentAnimation?.Kill();
        currentAnimation = stageGrid.DOAnchorPosY(0, .5f);
    }
    public void Close()
    {
        currentAnimation?.Kill();
        currentAnimation = stageGrid.DOAnchorPosY(-stageGrid.rect.height, .5f);
    }
}
