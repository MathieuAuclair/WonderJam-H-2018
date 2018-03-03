using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TwoByTwoDynamicRectGrid
{

    public enum CellQuadrant
    {
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT,
        NONE
    }

    public const float TOP_OR_LEFT = 1f;
    public const float BOTTOM_OR_RIGHT = 0f;
    public const float HALF = 0.5f;
	
    [SerializeField] float progressPerSecond = 1f;

    DoubleRectArea topArea = new DoubleRectArea(new Rect(0, 0, 1, 1));
    DoubleRectArea bottomArea = new DoubleRectArea(new Rect(0, 0, 1, 0));
    float verticalSplitTarget = 1f;
    IDictionary<CellQuadrant, bool> isCellActive = new Dictionary<CellQuadrant, bool>()
    {
        { CellQuadrant.TOP_LEFT, true },
        { CellQuadrant.TOP_RIGHT, false },
        { CellQuadrant.BOTTOM_LEFT, false },
        { CellQuadrant.BOTTOM_RIGHT, false },
    };

    public void Update()
    {
        AdjustRectsTowardSplitTargets();
    }

    public void SetRectActive(CellQuadrant quadrant, bool active)
    {
        isCellActive[quadrant] = active;
        AdjustSplitTargets();
    }

    public Rect GetRectAt(CellQuadrant position)
    {
        Rect rect = new Rect(0, 0, 0, 0);

        switch (position)
        {
            case CellQuadrant.TOP_LEFT:
                rect = topArea.left;
                break;
            case CellQuadrant.TOP_RIGHT:
                rect = topArea.right;
                break;
            case CellQuadrant.BOTTOM_LEFT:
                rect = bottomArea.left;
                break;
            case CellQuadrant.BOTTOM_RIGHT:
                rect = bottomArea.right;
                break;
        }

        return rect;
    }

    void AdjustSplitTargets()
    {
        verticalSplitTarget = GetSplitTarget(verticalSplitTarget, IsATopCellActive(), IsABottomCellActive());
        topArea.splitTarget = GetSplitTarget(topArea.splitTarget, isCellActive[CellQuadrant.TOP_LEFT], isCellActive[CellQuadrant.TOP_RIGHT]);
        bottomArea.splitTarget = GetSplitTarget(bottomArea.splitTarget, isCellActive[CellQuadrant.BOTTOM_LEFT], isCellActive[CellQuadrant.BOTTOM_RIGHT]);
    }

    bool IsATopCellActive()
    {
        return isCellActive[CellQuadrant.TOP_LEFT] || isCellActive[CellQuadrant.TOP_RIGHT];
    }

    bool IsABottomCellActive()
    {
        return isCellActive[CellQuadrant.BOTTOM_LEFT] || isCellActive[CellQuadrant.BOTTOM_RIGHT];
    }

    float GetSplitTarget(float currentTarget, bool topOrLeftIsActive, bool bottomOrRightIsActive)
    {
        float splitTarget = currentTarget;
        if (topOrLeftIsActive)
            splitTarget = bottomOrRightIsActive ? HALF : TOP_OR_LEFT;
        else if (bottomOrRightIsActive)
            splitTarget = BOTTOM_OR_RIGHT;
        return splitTarget;
    }

    public void SetVerticalSplitTarget(float target)
    {
        verticalSplitTarget = target;
    }

    public void SetTopSplitTarget(float target)
    {
        topArea.splitTarget = target;
    }

    public void SetBottomSplitTarget(float target)
    {
        bottomArea.splitTarget = target;
    }

    void AdjustRectsTowardSplitTargets()
    {
        AdjustDoubleRectAreaCells(topArea);
        AdjustDoubleRectAreaCells(bottomArea);

        float verticalStep = GetStep(topArea.height, verticalSplitTarget);
        topArea.height = verticalStep;
        bottomArea.height = 1f - verticalStep;
        topArea.y = 1f - topArea.height;
        bottomArea.y = 0;
    }

    void AdjustDoubleRectAreaCells(DoubleRectArea area)
    {
        float horizontalStep = area.splitTarget;
        if (area.height != 0)
            horizontalStep = GetStep(area.left.width, area.splitTarget);
        area.SetLeftRatio(horizontalStep);
    }

    float GetStep(float currentValue, float targetValue)
    {
        float progress = Time.deltaTime * progressPerSecond;
        float step = 0f;
        if (currentValue > targetValue)
        {
            step = Mathf.Max(currentValue - progress, targetValue);
        }
        else
        {
            step = Mathf.Min(currentValue + progress, targetValue);
        }
        return step;
    }

}

public class DoubleRectArea
{

    public Rect left;
    public Rect right;
    public float splitTarget;
    Rect self;
    float leftRatio = 1f;

    public float height
    {
        get
        {
            return self.height;
        }

        set
        {
            self.height = value;
            AdjustContentSize();
        }
    }

    public float y
    {
        get
        {
            return self.y;
        }

        set
        {
            self.y = value;
            AdjustContentSize();
        }
    }

    public DoubleRectArea(Rect rect)
    {
        CascadeRect(rect);
    }

    public void CascadeRect(Rect rect)
    {
        self = rect;
        AdjustContentSize();
    }

    public void SetLeftRatio(float ratio)
    {
        leftRatio = Mathf.Clamp(ratio, 0f, 1f);
        AdjustContentSize();
    }

    void AdjustContentSize()
    {
        left = new Rect(self);
        left.width = left.width * leftRatio;
        right = new Rect(self);
        right.width = right.width * (1f - leftRatio);
        right.x = right.x + left.width;
    }
}
