using UnityEngine;
using System.Collections;

public class OnEnableToggleThese : MonoBehaviour {
	[SerializeField] protected GameObject[] enableThese;
	[SerializeField] protected GameObject[] disableThese;

	protected void OnEnable(){

		foreach( GameObject g in enableThese )
			g.SetActive(true);
		foreach( GameObject g in disableThese )
			g.SetActive(false);
			
	}
	protected void OnDisable(){

		foreach( GameObject g in enableThese )
			g.SetActive(false);
			
		foreach( GameObject g in disableThese )
			g.SetActive(true);
			
	}
}
