using UnityEngine;

/// <summary>
/// Heads Up Display.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class HUD : MonoBehaviour
{
    #if UNITY_EDITOR
    /// <summary>
    /// There should be only one instance of the HUD at any specific time.
    /// </summary>
    void Awake()
    {
        Debug.Assert(FindObjectsOfType<HUD>().Length == 1);
    }
    #endif
}
