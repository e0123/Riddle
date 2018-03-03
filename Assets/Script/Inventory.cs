using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoSingleton<Inventory> {

	#region 資料儲存
	class Itembox
	{
		public string name;
		public int quantity;
		public int order;//順序

		public Itembox(string n,int q,int o)
		{
			this.name = n;
			this.quantity = q;
			this.order = o;
		}
	}

	int boxorder_gave = 0;//給出去的box 號碼到幾號了

	List<Itembox> list_itemboxs = new List<Itembox>();


	#endregion


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region 遊戲互動端

	int select_uibox;
	public int Select_uibox
	{
		get{ return select_uibox; }
		set{ 
			select_uibox = Mathf.Clamp (value, -1, list_uiitembar.Count);
		}
	}

	public void SelectItem(int id)
	{
		if (id >= list_itemboxs.Count)
			return;

		if (id == Select_uibox)//一樣的就取消選取
			Select_uibox = -1;		
		else
			Select_uibox = id;
		
		Refresh_Itembar ();
	}

	public string GetSelectItemName()
	{
		if (Select_uibox == -1)
			return "NULL";
		else
			return list_itemboxs[Select_uibox].name;
	}

	public void AddItem(string _name,int _quantity)
	{
		Itembox newitem = new Itembox (_name, _quantity,boxorder_gave);
		list_itemboxs.Add (newitem);
		boxorder_gave++;
		Debug.Log ("Add");
		Refresh_Itembar ();
	}

	/// <summary>
	/// 不夠/沒有該物件 回傳false
	/// </summary>
	/// <returns><c>true</c>, if item was removed, <c>false</c> otherwise.</returns>
	/// <param name="_n">N.</param>
	/// <param name="_q">Q.</param>
	public bool RemoveItem(string _n,int _q)
	{
		bool rtn;
		Itembox removeitem = list_itemboxs.Find (x => x.name == _n);
		if (removeitem == null) {
			Debug.LogWarning ("remove item not exist");
			rtn= false;
		}

		removeitem.quantity -= _q;

		if (removeitem.quantity > 0)
			rtn = true;//成功刪除，有剩
		else if (removeitem.quantity == 0) {
			list_itemboxs.Remove (removeitem);
			rtn = true;
		} else {
			Debug.LogWarning ("remove item not enough");
			list_itemboxs.Remove (removeitem);
			rtn = false;
		}
		Debug.Log ("Remove");
		Refresh_Itembar ();
		return rtn;
	}



	#endregion

	#region UI介面

	[System.Serializable]
	public struct UIBar
	{
		public Text text;
	}

	public List<UIBar> list_uiitembar;

	public Image select_img;

	void Refresh_Itembar()
	{
		if (list_uiitembar == null)
			return;

		//根據box分配的順序, 物件欄顯示
		list_itemboxs.Sort ((x, y) => {
			return x.order.CompareTo (y.order);
		});


		for (int i = 0; i < list_uiitembar.Count; i++) {//每個UI物件欄
			if (i < list_itemboxs.Count)//有物件塞入物件文字
				list_uiitembar [i].text.text = list_itemboxs [i].name;
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

	#endregion
}
