using UnityEngine;
using System.Collections;

public class ScrollTexture : UpdateBehaviour {

	
	// Update is called once per frame     
	public Vector2 uvOffset = Vector2.zero;
	
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2( 0.0f, -0.2f );
    public string textureName = "_MainTex";
	
	protected override void OnLateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}
	}



}
