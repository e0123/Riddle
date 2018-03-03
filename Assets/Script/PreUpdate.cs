using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 專門處理PreUpdate
/// </summary>
public class PreUpdate : MonoBehaviour {

	public static event System.Action PreUpdateFunc;


	void Awake()
	{
		PreUpdateFunc = null;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PreUpdateFunc != null)
			PreUpdateFunc();
	}
}
