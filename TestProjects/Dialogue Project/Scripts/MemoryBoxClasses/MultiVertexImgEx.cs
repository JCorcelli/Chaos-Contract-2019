
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace Utility.UI
{
	public class MultiVertexImgEx : MultiVertexImg
	{
		
		public Color glowColor = Color.black;
		public bool doHighlight = true;
		public bool doGlow = true;
		
		
		protected void DoGlow(List<UIVertex> newVerts)
		{
			UIVertex vert;
			
			float prevBottomLine = -1f;
			float bottomLine ;
			
			UIVertex[] prevVert = new UIVertex[4];
		
			for ( int i = 0; i < newVerts.Count; i +=4)
			{
				vert = newVerts[i];
				bottomLine = vert.position.y;
				
				if (i > 1 && (prevBottomLine - bottomLine).IsZero())
				{
					// moves the previous vertex right
					prevVert[2].position.x = newVerts[i+2].position.x +3;
					prevVert[3].position.x = newVerts[i+3].position.x + 3;
					
					continue;
				}
				if (i>1)
					allVerts.AddRange(prevVert);
				
				prevBottomLine = bottomLine;
				
				//vert = newVerts[i];
				vert.position.x -=3;
				vert.position.y -=3;
				vert.color = glowColor;
				prevVert[0] = vert;
				
				vert = newVerts[i+3];
				vert.position.x +=3;
				vert.position.y -=3;
				vert.color = glowColor;
				prevVert[3] = vert;
				
				vert = newVerts[i+1];
				vert.position.x -=3;
				vert.position.y +=3;
				vert.color = glowColor;
				prevVert[1] = vert;
				
				vert = newVerts[i+2];
				vert.position.x +=3;
				vert.position.y +=3;
				vert.color = glowColor;
				prevVert[2] = vert;
			}
			 allVerts.AddRange(prevVert);
		}
		protected void DoHighlight(List<UIVertex> newVerts)
		{
			UIVertex vert;
			
			float prevBottomLine = -1f;
			float bottomLine ;
			
			UIVertex[] prevVert = new UIVertex[4];
		
			for ( int i = 0; i < newVerts.Count; i +=4)
			{
				vert = newVerts[i];
				bottomLine = vert.position.y;
				
				if (i > 1 && (prevBottomLine - bottomLine).IsZero())
				{
					// moves the previous vertex right
					prevVert[2].position.x = newVerts[i+2].position.x;
					prevVert[3].position.x = newVerts[i+3].position.x;
					
					continue;
				}
				if (i>1)
					allVerts.AddRange(prevVert);
				
				prevBottomLine = bottomLine;
				
				//vert = newVerts[i];
				
				vert.color = color;
				prevVert[0] = vert;
				
				vert = newVerts[i+3];
				
				vert.color = color;
				prevVert[3] = vert;
				
				vert = newVerts[i+1];
				
				vert.color = color;
				prevVert[1] = vert;
				
				vert = newVerts[i+2];
				
				vert.color = color;
				prevVert[2] = vert;
			}
			 allVerts.AddRange(prevVert);
		}
		public override void Add(List<UIVertex> newVerts)
		{
			
			// special highlight, makes outline
			if (doGlow) DoGlow(newVerts);
			
			// basic highlight
			
			if (doHighlight) DoHighlight(newVerts);
			
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
				newColors[i] = allVerts[i].color;
				
			}
			
			int v = 0;
			for ( int i = 0; i < triCount; i += 6)
			{
				newTriangles[i] =   v+0; 
				newTriangles[i+1] = v+3; 
				newTriangles[i+2] = v+1; 
				newTriangles[i+3] = v+3; 
				newTriangles[i+4] = v+1; 
				newTriangles[i+5] = v+2; 
				
				v += 4;
			}
			
			
			Mesh mesh = new Mesh();
			mesh.vertices = newVertices;
			mesh.uv = newUV;
			mesh.triangles = newTriangles;
			mesh.colors = newColors;
			mesh.RecalculateNormals();
			
			// assigning to canvas
			Material mat = materialForRendering;
			
			canvasRenderer.SetMaterial(mat, null); 
			
			canvasRenderer.SetMesh( mesh );
		}
	}
}