using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField] MeshRenderer body;
    [SerializeField] MeshRenderer hips;
    [SerializeField] MeshRenderer[] limbs;

    public void ApplySwatch(ColorSwatch swatch)
    {
        swatch.Initialize();
        swatch.PaintBody(body);
        swatch.PaintHips(hips);
        swatch.PaintLimbs(limbs);
    }
}
