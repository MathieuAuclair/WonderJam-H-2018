using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] Image logo;
    [SerializeField] Graphic[] instructions;
    [SerializeField] float fadeOutTime = 0.5f;

    public void HideLogo()
    {
        logo.CrossFadeAlpha(0, fadeOutTime, true);
    }

    public void HideInstructions()
    {
        foreach (var graphic in instructions)
        {
            graphic.CrossFadeAlpha(0, fadeOutTime, true);
        }
    }
}
