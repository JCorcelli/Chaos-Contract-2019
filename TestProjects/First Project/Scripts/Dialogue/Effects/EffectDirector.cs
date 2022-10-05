using UnityEngine;
using System.Collections;


namespace Effects
{
	public class EffectDirector : MonoBehaviour {
		[SerializeField] private GameObject effects;
		public void e(string s)
		{
			effects.SendMessage("s");
		}
		
	}
}