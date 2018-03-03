using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler {


	public UserInput.UI_OBJ buttontype;
	bool isHold;
	float holdtime;
	const float holdtime_threshold = 0.5f;
	// Use this for initialization
	void Awake () {
		holdtime = 0f;  

		PreUpdate.PreUpdateFunc += SetUIButtonInput;
	}
	
	// Update is called once per frame
	void Update () {	


	}

	public void OnPointerEnter(PointerEventData eventData)
	{		
		//TouchIn ();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		TouchIn ();
	}

	public void OnPointerUp(PointerEventData eventData)
	{		
		Touchout ();
	}

	public void OnPointerExit(PointerEventData eventData)
	{		
		//Touchout ();
	}

	void TouchIn()
	{
		isHold = true;
		UserInput.Instance.AddInputEvent_UIobject (buttontype, TouchPhase.Began);
	}

	void Touchout()
	{
		isHold = false;
	}

	// PreUpdate
	void SetUIButtonInput()
	{
		if (isHold) {
			holdtime += Time.deltaTime;
			if(holdtime > holdtime_threshold)
				UserInput.Instance.AddInputEvent_UIobject (buttontype, TouchPhase.Stationary);//不會被touch覆蓋到，因為先event才preupdate
		}
		else
			holdtime = 0f;
	}
}
