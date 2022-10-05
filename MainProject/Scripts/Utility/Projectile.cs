using UnityEngine;
using System.Collections;

public class Projectile  {

	// *********these must be set*********
	public float g = -Physics.gravity.y;
	public float increment = 0.5f;
	
	public float minForce = 0.01f;
	public float maxForce = 20f;
	
	public Vector3 endP = new Vector3(); 
	public Vector3 startP = new Vector3(); 
	
	// optional
	public float t = 0f;
	public float force = 0.01f;

	//--------this is calculated----------
		
	// angles in radians
	public float high  = 0;
	public float low   = 0;
	public float angle = 0;
	
	// basic math
	public float x = 0;
	public float y = 0;
	
	public float maxt = 0.5f;
	
	public float v = 0.1f;
	public float vmax = 20f;
	
		
	
	public Vector3 planarEnd = new Vector3(); 
	public Vector3 planarStart = new Vector3();
	
	//*****the results
	public bool success = false;
	public Vector3 vector = new Vector3();
	
	public void ProjectileMath()
	{
		
		float d = (
			Mathf.Pow(v, 4) - g * (
				g * Mathf.Pow(x, 2) + 2 * y * Mathf.Pow(v, 2)
			)
		);
		
		if (d < 0) 
			success = false;
		else if (d < .01f)
		{
			angle = 90f * Mathf.Deg2Rad; 
			force = v;
			success = true;
		}
		else
		{
			d = Mathf.Sqrt(d);
			// high jump
			high = Mathf.Atan((Mathf.Pow(v, 2) + d) / (g * x));
			
			// dive
			low = Mathf.Atan((Mathf.Pow(v, 2) - d) / (g * x));
			
			force = v;
			success = true;
		}
		
	
		
	}
	
	public void UpdateVars()
	{
		
		success = false;
	
		planarStart = new Vector3(startP.x,0,startP.z) ;
		planarEnd = new Vector3(endP.x,0,endP.z)   ;
		
		
		x = Vector3.Distance(planarEnd,planarStart);
		y = endP.y - startP.y;
		
		vmax = maxForce;
		g = -Physics.gravity.y;
		
		
	}
	public void SetBestAngle()
	{
		
		UpdateVars();
		
		v = minForce;
		while (v <= vmax && !success)
		{
			ProjectileMath();
			
			if (v < maxForce - 0.1f)
			{
				v += 0.5f;
				if (v > maxForce)
					v = maxForce;
			}
			else
				v ++;
			
		}
		if (success)
			angle = high;
		
		else
		{
			force = maxForce;
			angle = 45f * Mathf.Deg2Rad;
		}
		UpdateVector();
	}
	public void SetLowestAngle()
	{
		
		UpdateVars();
		
		v = maxForce;
		while (v > 0 && !success)
		{
			ProjectileMath();
		
			if (v < 0.1f)
			{
				v -= 0.5f;
				if (v < 0)
					v = 0;
			}
			else
				v --;
		}
		if (success)
		{
			angle = low;
		}
		else
		{
			angle = 30f * Mathf.Deg2Rad;
			force = maxForce;
		}
		UpdateVector();
	}
	
	public void UpdateVector()
	{	
		Vector3 vectorB = (planarEnd - planarStart);
		float sideB = vectorB.magnitude;
		float sideC = sideB / Mathf.Cos(angle);
		float sideA = Mathf.Sqrt(Mathf.Pow(sideC, 2) - Mathf.Pow(sideB , 2));
		Vector3 vectorA = Vector3.up * sideA;
		
		vector = (vectorB + vectorA).normalized * force;
		
		
		maxt = (2 * vector.magnitude * Mathf.Sin(angle)) / g;
		// Debug.Log("flight time = " + maxt);
		
		
	}
}