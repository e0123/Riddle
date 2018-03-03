using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoSingleton<UserInput> {

	public enum UI_OBJ
	{
		NONE,
		LEFT_BTN,
		RIGHT_BTN,
		ITEM_0,
		ITEM_1,
		ITEM_2,
		ITEM_3,
		RESET
	}

	public class UITouchInfo
	{
		public UI_OBJ ui_obj;
		public TouchPhase phase;
	}

	// touch info, 每個frame更新一次
	UITouchInfo uitouchinfo;
	GameObject now_3Dobj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		now_3Dobj = null;
		uitouchinfo = null;
	}

	void Update(){
	
	}
	/// <summary>
	/// 在PreUpdate裡，偵測有沒有Touch 3D 物件
	/// </summary>
	void GetInput_Touch3DObject()
	{
		if (Input.touchCount > 0) {

			if (Input.GetTouch (0).phase == TouchPhase.Began) {

				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit,100f)) {
					now_3Dobj = hit.collider.gameObject;
				}
			}
		}
	}

	/// <summary>
	/// 所有UI Input Event
	/// </summary>
	/// <param name="value">Value.</param>
	public void AddInputEvent_UIobject(UI_OBJ _obj,TouchPhase _phase)
	{
		if (uitouchinfo == null)
			uitouchinfo = new UITouchInfo ();
		uitouchinfo.ui_obj = _obj;
		uitouchinfo.phase = _phase;
	}

	void Awake()
	{
		//PreUpdate
		PreUpdate.PreUpdateFunc += GetInput_Touch3DObject;

		DontDestroyOnLoad (gameObject);
	}


	#region 其他class取用

	public UITouchInfo GetUITouchInfo()
	{
		return uitouchinfo;
	}

	public GameObject GetObjectTouch()
	{
		return now_3Dobj;
	}

	#endregion
}
