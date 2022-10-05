using UnityEngine;
using System.Collections;

public class OnEnableDisableOther : MonoBehaviour {
	[SerializeField] private GameObject other;

	private void OnEnable(){

		other.SetActive (false);
	}
}
