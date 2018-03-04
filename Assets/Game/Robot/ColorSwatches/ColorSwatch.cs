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
            { Part.LIMB, limb },
            { Part.JOINT, joint },
            { Part.BODY, body },
            { Part.GLASS, glass },
        };
    }

    public void PaintPart(MeshRenderer part, Part color)
    {
        part.material = swatch[color];
    }

    public void PaintBody(MeshRenderer mainBody)
    {
        mainBody.materials = new Material[]
        {
            body,
            glass,
            limb,
            joint
        };
    }
}
