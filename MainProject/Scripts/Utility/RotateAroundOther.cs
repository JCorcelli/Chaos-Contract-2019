

using UnityEngine;
using System.Collections;
using CameraSystem.Swing;

public class RotateAroundOther : UpdateBehaviour 
{
    //	 DOESN'T DO ANYTHING YET
	


	public Transform targetPivot;
	public Transform hand;
	protected Quaternion old;
	
    protected void Awake() {
		
		old = hand.rotation;
	}
	// Update is called once per frame
	protected override void OnLateUpdate () 
    {
		
		Quaternion rotationDelta;
		rotationDelta = Quaternion.Inverse(old) * hand.rotation;
		old = hand.rotation;
		
		transform.position = RotatePointAroundPivot(transform.position, targetPivot.position, rotationDelta);
	}

    
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        Vector3 dir = point - pivot;
        dir = angles * dir;
        point = dir + pivot;
        return point;
    }
}
