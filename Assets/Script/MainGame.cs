﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour {

	#region Class Reference

	UserInput _input;
	Inventory _inv;
	UIManager _UiM;

	#endregion

	void Awake()
	{
		_input = UserInput.Instance;
		_inv = Inventory.Instance;
		_UiM = UIManager.Instance;
	}

	// Use this for initialization
	void Start () {

		_UiM.UnSelectItem ();
	}
	
	// Update is called once per frame
	void Update () {


		ProcUiTouch ();
		ProcGameobjectTouch ();


	}

	#region 點擊動作

	void ProcUiTouch()
	{
		//偵測UI點擊
		UserInput.UITouchInfo uitouch = _input.GetUITouchInfo();

		if (uitouch != null) {

			// 單點
			if (uitouch.phase == TouchPhase.Began) {
				switch (uitouch.ui_obj) {

				case UserInput.UI_OBJ.LEFT_BTN:
					int prevcs = ((int)nowCamScene + (int)CAMERA_SCENE.MAX - 1) % (int)CAMERA_SCENE.MAX;
					Change_CameraScene (prevcs);
					break;

				case UserInput.UI_OBJ.RIGHT_BTN:
					int nextcs = ((int)nowCamScene + 1) % (int)CAMERA_SCENE.MAX;
					Change_CameraScene (nextcs);
					break;

				case UserInput.UI_OBJ.ITEM_0:
				case UserInput.UI_OBJ.ITEM_1:
				case UserInput.UI_OBJ.ITEM_2:
				case UserInput.UI_OBJ.ITEM_3:					
					_UiM.SelectItem (uitouch.ui_obj);
					break;


				case UserInput.UI_OBJ.RESET:
					Application.Quit ();
					return;
				}		
			}
			// 長按
			else if (uitouch.phase == TouchPhase.Stationary) {
				switch (uitouch.ui_obj) {

				case UserInput.UI_OBJ.LEFT_BTN:
					break;

				case UserInput.UI_OBJ.RIGHT_BTN:
					break;				

				}		
			}
		}
	}

	void ProcGameobjectTouch(){
	
		// 點3D物件
		if (_input.GetObjectTouch() != null) {

			SceneObject so = _input.GetObjectTouch ().GetComponent<SceneObject> ();
			if (so != null) {

				// 單純收集物件
				if (so.collectable) {

					_inv.AddItem (so.so_name, 1);
					Destroy (so.gameObject);
				} 

				// 跟物件互動
				else {
					string select_item = _UiM.GetSelectItemName ();
					if ( select_item != "NULL") {

						switch (so.so_name) {

						default:
							break;

						case "red_b":
						case "yellow":
							SceneObject_Child1 so_c1 = so as SceneObject_Child1;
							if (so_c1 != null) {
								if (select_item == "green")
									so_c1.ChangePhase (SceneObject_Child1.PHASE.PHASE_GREEN);
								else if (select_item == "red_s")
									so_c1.ChangePhase (SceneObject_Child1.PHASE.PHASE_RED);

								_inv.RemoveItem (_UiM.GetSelectItemName (), 1);
								_UiM.UnSelectItem ();
							}
							break;

						}					
					}									
				}

			}
		}
	}


	#endregion









	#region 攝影機/切換場景

	public enum CAMERA_SCENE
	{		
		//正常左右按鈕移動的場景
		ROOM1=0,
		ROOM2,



		MAX,
		//點擊物件才切換的場景，需要手動設定進入和退出的場景
		ROOM1_BIGBOX
	}

	CAMERA_SCENE nowCamScene;

	void Change_CameraScene(int nextscene)
	{
		if (nextscene == (int)nowCamScene)
			return;
		Transform parent = GameObject.Find ("CameraTrans").transform.Find(((CAMERA_SCENE)nextscene).ToString());
		if (parent == null)
			return;
		Camera.main.transform.parent = parent;
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localScale = Vector3.one;
		Camera.main.transform.localEulerAngles = Vector3.zero;

		nowCamScene = (CAMERA_SCENE)nextscene;
	}

	#endregion
}
