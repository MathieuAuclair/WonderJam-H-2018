using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Camera))]
public class OffscreenDetector : MonoBehaviour
{
    [Tooltip(
        "Prefab of the cursors that will be displayed when a target becomes offscreen.")]
    [SerializeField] Cursor cursorPrefab;

    [Tooltip(
        "Mapping of icons to display in the cursors.")]
    [SerializeField] SpriteMapping icons;

    [Tooltip(
        "Wether the Camera component is set to Perspective or Orthographic projection.")]
    [SerializeField] bool isPerspective;

    [Tooltip(
        "When using perspective, this value determines at which point the target's " +
        "horizontal position should be adjusted to avoid the cursor from going " +
        "haywire.")]
    [SerializeField] float zCriticalDistance;
    [SerializeField] HUD hud;
    [SerializeField] Camera screen;

    [Header("Debug variables")]
    [SerializeField] Transform debugTarget;


    IDictionary<Transform, Cursor> targets = new Dictionary<Transform, Cursor>();

    void Start()
    {
        #if UNITY_EDITOR
        if (debugTarget != null)
            AddTarget(debugTarget, "Player2");
        #endif
    }

    /// <summary>
    /// Adds an object for which we want to show a cursor when offscreen.
    /// </summary>
    /// <param name="targetTransform">Target's transform.</param>
    /// <param name="iconName">Target's cursor icon's name as appears in the sprite mapping.</param>
    public void AddTarget(Transform targetTransform, string iconName)
    {
        Cursor offscreenPointer = Instantiate(cursorPrefab);
        offscreenPointer.SetBoundaries(screen.pixelRect);
        offscreenPointer.SetPositionGetter(() => GetOnScreenPosition(targetTransform));
        offscreenPointer.transform.SetParent(hud.transform, false);
        offscreenPointer.SetIcon(icons[iconName]);
        targets.Add(targetTransform, offscreenPointer);
    }

    public void RemoveTarget(Transform targetTransform)
    {
        Destroy(targets[targetTransform].gameObject);
        targets.Remove(targetTransform);
    }

    Vector3 GetOnScreenPosition(Transform target)
    {
        var screenPos = screen.WorldToScreenPoint(target.position);
        float zDistance = Mathf.Abs(screenPos.z);
        screenPos.x -= screen.pixelWidth * 0.5f;
        screenPos.y -= screen.pixelHeight * 0.5f;
        screenPos.z = 0;

        if (isPerspective)
        {
            if (zDistance < zCriticalDistance)
            {
                screenPos.x *= zDistance / zCriticalDistance;
            }
            var dotProduct = Vector3.Dot(
                                 screen.transform.forward, 
                                 target.position - screen.transform.position);
            var sign = Mathf.Sign(dotProduct);
            screenPos *= sign;
        }

        return screenPos;
    }

    void Update()
    {
        foreach (var targetCursor in targets)
        {
            bool isOffscreen = CheckIfOffscreen(targetCursor.Key.position);
            if (isOffscreen)
            {
                targetCursor.Value.Show();
            }
            else
            {
                targetCursor.Value.Hide();
            }
        }
    }

    bool CheckIfOffscreen(Vector3 position)
    {
        Vector3 onScreenPos = screen.WorldToScreenPoint(position);
        return (!screen.pixelRect.Contains(onScreenPos) || onScreenPos.z < 0);
    }
}