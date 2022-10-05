using UnityEngine;
using System.Collections;

namespace Datesim.Setup
{
	public class ErasedByEraser : MonoBehaviour {

		protected void OnTriggerEnter2D(Collider2D col)
		{
			if (col.name == "Eraser") GameObject.Destroy(gameObject);
		}
	}

}