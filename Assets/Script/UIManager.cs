using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager> {

	#region 物品欄UI

	//物品欄
	[System.Serializable]
	public struct UIBar
	{
		public Text text;
	}

	Image select_img;
	List<UIBar> list_uiitembar;//UI物件

	//排序後的Inventory列表
	List<Inventory.Itembox> sortedlist_UIItemboxs;

	public void Refresh_Itembar_data(List<Inventory.Itembox> _list)
	{
		sortedlist_UIItemboxs = _list;
	}

	void Refresh_Itembar()
	{
		for (int i = 0; i < list_uiitembar.Count; i++) {//每個UI物件欄
			if (i < sortedlist_UIItemboxs.Count)//有物件塞入物件文字
				list_uiitembar [i].text.text = sortedlist_UIItemboxs [i].name;
			else //沒物件文字清空
				list_uiitembar [i].text.text = "";

		}

		//鎖定物件
		if (Select_uibox == -1)
			select_img.gameObject.SetActive (false);
		else {
			select_img.gameObject.SetActive (true);
			select_img.transform.position = list_uiitembar [Select_uibox].text.transform.position;
		}
	}



	int Select_uibox;


	public void SelectItem(UserInput.UI_OBJ selectobj)
	{
		int id;
		switch (selectobj) {

		default:
			id = -1;
			Debug.LogWarning ("select item ui object error");//
			return;

		case UserInput.UI_OBJ.ITEM_0:
		case UserInput.UI_OBJ.ITEM_1:
		case UserInput.UI_OBJ.ITEM_2:
		case UserInput.UI_OBJ.ITEM_3:
			id = selectobj - UserInput.UI_OBJ.ITEM_0;//0~3
			break;
		}

		//不能選得超出物品欄位數 and 不能超過物品位數
		if (id >= list_uiitembar.Count)//無視
			return;

		if (id >= sortedlist_UIItemboxs.Count)//無視
			return;

		if (id == Select_uibox)//一樣的就取消選取
			UnSelectItem();
		else
			Select_uibox = id;
	}


	public void UnSelectItem()
	{
		Select_uibox = -1;
	}

	public string GetSelectItemName()
	{
		if (Select_uibox == -1)
			return "NULL";
		else
			return sortedlist_UIItemboxs[Select_uibox].name;
	}
	#endregion




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake()
	{
		//場景物件設定
		select_img = transform.Find("Itembar/select").GetComponent<Image>();

		list_uiitembar = new List<UIBar> ();
		Transform itemRoot = transform.Find ("Itembar/Bar");
		if (itemRoot == null) {
		
			Debug.LogWarning ("No Itembar Found!");
			return;
		}

		for (int i = 0; i < itemRoot.childCount; i++) {
		
			Text _text = itemRoot.GetChild (i).GetComponentInChildren<Text> ();
			UIBar _bar = new UIBar ();
			_bar.text = _text;
			list_uiitembar.Add (_bar);
		}

		sortedlist_UIItemboxs = new List<Inventory.Itembox> ();
	}


	void LateUpdate()
	{
		Refresh_Itembar ();
	}
}
