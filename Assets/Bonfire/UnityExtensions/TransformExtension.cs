using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension {

	public static void AddAndFitTo (this Transform toFit, Transform holder) {
		toFit.SetParent(holder);
		toFit.rotation = holder.rotation;
		toFit.position = holder.position;
		toFit.localScale = Vector3.one;
	}
}
