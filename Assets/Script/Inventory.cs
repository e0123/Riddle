using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoSingleton<Inventory> {

	#region 資料儲存
	public class Itembox
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

	List<Itembox> list_itemboxs;

	bool hasitem
	{
		get{
			return list_itemboxs != null && list_itemboxs.Count != 0;
		}
	}

	int itemcount
	{
		get{
			if (!hasitem)
				return 0;
			else
				return list_itemboxs.Count;
		}
	}

	#endregion




	#region 遊戲互動端


	public void AddItem(string _name,int _quantity)
	{
		Itembox newitem = new Itembox (_name, _quantity,boxorder_gave);
		list_itemboxs.Add (newitem);
		boxorder_gave++;
		Debug.Log ("Add");
		Sort_Itembox ();
		if (OnItemChange != null)
			OnItemChange (list_itemboxs);
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

		if (OnItemChange != null)
			OnItemChange (list_itemboxs);

		return rtn;
	}

	public Itembox GetItem(int _order)
	{
		if (!hasitem)
			return null;

		if (_order >= itemcount)
			return null;

		return list_itemboxs [_order];
	}


	void Sort_Itembox(){
	
		if (!hasitem)
			return;

		//根據box分配的順序, 物件欄顯示
		list_itemboxs.Sort ((x, y) => {
			return x.order.CompareTo (y.order);
		});
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
		list_itemboxs = new List<Itembox>();
	}

	void OnEnable()
	{
		UIManager.Instance.UnSelectItem ();
		OnItemChange = null;
		OnItemChange += UIManager.Instance.Refresh_Itembar_data;
		OnItemChange (list_itemboxs);
	}

	public delegate void MyDelegate(List<Itembox> _itemlist);
	public MyDelegate OnItemChange;//每當物品改變時，把完整物品列表，傳送給該傳的人。例如:UI物品欄
}
