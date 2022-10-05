using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class HayRegeneratorTarget : MonoBehaviour {

		// Use this for initialization
		
		public float maxSize = 1f;
		public float minSize = 0f;
		public float size = 1f;
		public float consumeRate = 0.1f;
		private Transform target;
		private Vector3 baseSize; // z scale, for now
		// Update is called once per frame
		void Start () {
			target = transform.GetChild(0);
			baseSize = target.localScale;
			
			SetSize(size);
		}
		
		private void SetSize(float s){
			
			target.localScale = new Vector3 (baseSize.x, baseSize.y, s) ;
			size = s;
		}
		
		public float Regenerate(float amt){
			size += amt;
			if ( size >= maxSize) 
			{
				SetSize(maxSize) ;
				return  amt + this.size;
			}
			if ( size <= minSize) 
			{
				SetSize(minSize) ;
				return  amt + this.size;
			}
			else
			{
				SetSize(size) ;
				return amt;
			}
		}
		public void Consume(){
			size -= consumeRate;
			if ( size <= minSize) 
			{
				SetSize(minSize) ;
			}
			else
				SetSize(size);
			
		}
	}
}