
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace Utility.UI
{
	public class MultiVertexImg : Image
	{
		
		protected static List<UIVertex> allVerts = new  List<UIVertex> ();
		
		
		protected override void Start(){
			
			enabled=false;
		}
		public void Clear()
		{
			allVerts.Clear();
			canvasRenderer.Clear();
				
		}
		public void Set(List<UIVertex> newVerts)
		{
			
			allVerts.Clear();
			if (newVerts.Count < 1) 
				
				 // blank.
				canvasRenderer.Clear();
				
			else
				Add(newVerts);
			
		}
		public virtual void Add(List<UIVertex> newVerts)
		{
			
			UIVertex vert;
			for ( int i = 0; i < newVerts.Count; i ++)
			{
				vert = newVerts[i];
				vert.color = color;
				allVerts.Add(vert);
				
			}
			
			// mesh
			Vector3[] newVertices = new Vector3[allVerts.Count];
			Vector2[] newUV = new Vector2[allVerts.Count];
			Color[] newColors = new Color[allVerts.Count];
			int triCount = (int)(allVerts.Count * 1.5);
			int[] newTriangles = new int[triCount];
			
			
			
			for ( int i = 0; i < allVerts.Count; i ++)
			{
				newVertices[i] = allVerts[i].position;
				newUV[i] = allVerts[i].uv0;
				newColors[i] = color;
				
			}
			for ( int i = 0; i < triCount; i += 3)
			{
				newTriangles[0] = i+0; 
				newTriangles[1] = i+1; 
				newTriangles[2] = i+2; 
				
				
			}
			
			
			
			Mesh mesh = new Mesh();
			mesh.vertices = newVertices;
			mesh.uv = newUV;
			mesh.triangles = newTriangles;
			mesh.colors = newColors;
			mesh.RecalculateNormals();
			
			/// canvas 
			Material mat = materialForRendering;
			
			// param 2 : makes it textured
			
			canvasRenderer.SetMaterial(mat, null); 
				 // word... and other things
			


			canvasRenderer.SetMesh( mesh );
		}
	}
}