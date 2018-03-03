using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeInFadeOutSceneTransition : MonoBehaviour
{
	public bool fadeInOnStart = true;
	public Animator anim;

	void Start ()
	{
		if (fadeInOnStart) {
			anim.SetBool ("FadeIn", true);
            StartCoroutine(ChangeScene());

		}
	}
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

}
