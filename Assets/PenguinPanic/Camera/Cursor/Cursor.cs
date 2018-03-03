using UnityEngine;
using System;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField] Graphic visual;
    [SerializeField] Image icon;
    [SerializeField] float visualFadeDuration;

    Func<Vector3> targetPositionGetter;
    Vector2 origin;

    /// <summary>
    /// Tells wether the cursor is hidden or not.
    /// </summary>
    /// <value><c>true</c> if the cursor is hidden; otherwise, <c>false</c>.</value>
    public bool IsHidden { get; private set; }

    /// <summary>
    /// The position returned by the position getter is the one the cursor will point at.
    /// </summary>
    /// <param name="getter">Position getter.</param>
    public void SetPositionGetter(Func<Vector3> getter)
    {
        targetPositionGetter = getter;
    }

    /// <summary>
    /// Boundaries within which to keep the cursor.
    /// </summary>
    /// <param name="boundaries">Boundaries.</param>
    public void SetBoundaries(Rect boundaries)
    {
        origin = boundaries.center;
    }

    /// <summary>
    /// Sets the icon to display within the cursor.
    /// </summary>
    /// <param name="iconSprite">Icon sprite.</param>
    public void SetIcon(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }

    /// <summary>
    /// Hides the cursor.
    /// </summary>
    public void Hide()
    {
        if (!IsHidden)
        {
            IsHidden = true;
            visual.CrossFadeAlpha(0, visualFadeDuration, true);
            icon.CrossFadeAlpha(0, visualFadeDuration, true);
        }
    }

    /// <summary>
    /// Shows the cursor.
    /// </summary>
    public void Show()
    {
        if (IsHidden)
        {
            IsHidden = false;
            visual.CrossFadeAlpha(1, visualFadeDuration, true);
            icon.CrossFadeAlpha(1, visualFadeDuration, true);
        }
    }

    void Update()
    {
        var targetPos = targetPositionGetter();
        transform.localPosition = BindPosition(targetPos);
        transform.rotation = Quaternion.FromToRotation(Vector3.down, targetPos);
        icon.transform.rotation = Quaternion.identity;
    }


    /// <summary>
    /// Binds position within screen boundaries.
    /// </summary>
    /// <returns>The bound position.</returns>
    /// <param name="position">Position to bind.</param>
    Vector3 BindPosition(Vector3 position)
    {
        return new Vector3(
            Mathf.Sign(position.x) * Mathf.Min(Mathf.Abs(position.x), origin.x),
            Mathf.Sign(position.y) * Mathf.Min(Mathf.Abs(position.y), origin.y),
            0);
    }
}
