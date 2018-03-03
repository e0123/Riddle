using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject_Child1: SceneObject
{
	public enum PHASE
	{
		DEFAULT,
		PHASE_RED,
		PHASE_GREEN
	}

	PHASE nowphase;

	public void ChangePhase(PHASE next)
	{
		if (next == nowphase)
			return;
		nowphase = next;

		switch (nowphase) {
		default:
		case PHASE.DEFAULT:

			break;
		case PHASE.PHASE_RED:
			gameObject.GetComponent<Renderer> ().material = Resources.Load ("Material/red") as Material;
			break;

		case PHASE.PHASE_GREEN:
			gameObject.GetComponent<Renderer> ().material = Resources.Load ("Material/green") as Material;
			break;
		}
	}
}


