using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Utility.GUI
{
	public class GuiShrink : AbstractButtonHandler {

	
		
		protected IndicatorCompass ink;
		protected Color color;
		protected int fullScale;
		
		protected bool isRotating = false;
		protected void Awake () {
			ink = GetComponent<IndicatorCompass>();
			fullScale = ink.fullScale;
			color = ink.colorInView;
			buttonName = "mouse 2";
			
			TurnOff();
		}
		
		
		
		
		
		protected override void OnEnable() {
			base.OnEnable();
			TurnOn();
		}
		protected override void OnPress() {
			StopCoroutine("_DelayOff");
			TurnOn();
		}
		
		protected override void OnRelease() {
			DelayOff();
		}
		protected override void OnDisable() {
			base.OnDisable();
			TurnOff();
			StopCoroutine("_DelayOff");
		}
		protected void DelayOff() {
			StopCoroutine("_DelayOff");
			StartCoroutine("_DelayOff");
		}
		protected IEnumerator _DelayOff(){
			if (SelectGlobal.rotateOffTimer < 0) yield break;
			yield return new WaitForSeconds(SelectGlobal.rotateOffTimer);
			TurnOff();
		}
		protected void TurnOn(){
			
			isRotating = true;
			ink.colorInView = color;
			ink.fullScale = fullScale;
		}
		protected void TurnOff(){
			
			isRotating = false;
			ink.colorInView = ink.colorAtEdge;
			ink.fullScale = ink.edgeScale;
		}
	}
}