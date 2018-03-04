using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ColorSwatch : ScriptableObject
{
    public enum Part
    {
        LIMB,
        JOINT,
        BODY,
        GLASS,
    }

    [SerializeField] Material body;
    [SerializeField] Material glass;
    [SerializeField] Material limb;
    [SerializeField] Material joint;

    IDictionary<Part, Material> swatch;

    public void Initialize()
    {
        swatch = new Dictionary<Part, Material>(4)
        {
            { Part.BODY, body },
            { Part.GLASS, glass },
            { Part.LIMB, limb },
            { Part.JOINT, joint },
        };
    }

    public void PaintLimbs(MeshRenderer[] limbs)
    {
        foreach (var l in limbs)
        {
            l.material = swatch[Part.LIMB];
        }
    }

    public void PaintHips(MeshRenderer hips)
    {
        hips.material = swatch[Part.JOINT];
    }

    public void PaintBody(MeshRenderer mainBody)
    {
        mainBody.materials = new Material[]
        {
            limb,
            body,
            joint,
            glass,
        };
    }
}
