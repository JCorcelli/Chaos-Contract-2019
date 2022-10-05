using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// by Mr Float


// url https://answers.unity.com/questions/756038/using-textgenerator.html

/*
var m_TextSettings = new TextGenerationSettings();

m_TextSettings.textAnchor = TextAnchor.MiddleCenter;
m_TextSettings.color = Color.red;
m_TextSettings.generationExtents = new Vector2(100, 100);
m_TextSettings.pivot = new Vector2(0.5f, 0.5f); ;
m_TextSettings.richText = true;
m_TextSettings.font = m_Font;
m_TextSettings.fontSize = 14;
m_TextSettings.fontStyle = FontStyle.Normal;
m_TextSettings.verticalOverflow = VerticalWrapMode.Overflow;
m_TextSettings.horizontalOverflow = HorizontalWrapMode.Wrap;
m_TextSettings.lineSpacing = 1;
m_TextSettings.generateOutOfBounds = true;
m_TextSettings.resizeTextForBestFit = false;
m_TextSettings.scaleFactor = 1f;

#########

canvas renderer
 m_CanvasRenderer.SetMaterial(m_Font.material, null);
 m_CanvasRenderer.SetMesh(m_Mesh);
 
 
 ########
 
mesh renderer
 GetComponent<MeshFilter>().mesh = m_Mesh;
 m_MeshRenderer.sharedMaterial = m_Font.material;
 
 ######
*/
 public static class TextExtensions
     {
		 // quickly exposed the method to gather mesh vertices
		 // o_Mesh = out Mesh
         static public void GetMesh(this TextGenerator i_Generator, ref Mesh o_Mesh)
         {
             if (o_Mesh == null)
                 return;
 
             int vertSize = i_Generator.vertexCount;
             Vector3[] tempVerts = new Vector3[vertSize];
             Color32[] tempColours = new Color32[vertSize];
             Vector2[] tempUvs = new Vector2[vertSize]; // probably depicts the font char uv
			 
             IList<UIVertex> generatorVerts = i_Generator.verts;
             for (int i = 0; i < vertSize; ++i)
             {
                 tempVerts[i] = generatorVerts[i].position;
                 tempColours[i] = generatorVerts[i].color;
                 tempUvs[i] = generatorVerts[i].uv0;
             }
             o_Mesh.vertices = tempVerts;
             o_Mesh.colors32 = tempColours;
             o_Mesh.uv = tempUvs;
 
             int characterCount = vertSize / 4;
             int[] tempIndices = new int[characterCount * 6];
             for(int i = 0; i < characterCount; ++i)
             {
                 int vertIndexStart = i * 4;
                 int trianglesIndexStart = i * 6;
                 tempIndices[trianglesIndexStart++] = vertIndexStart;
                 tempIndices[trianglesIndexStart++] = vertIndexStart + 1;
                 tempIndices[trianglesIndexStart++] = vertIndexStart + 2;
                 tempIndices[trianglesIndexStart++] = vertIndexStart;
                 tempIndices[trianglesIndexStart++] = vertIndexStart + 2;
                 tempIndices[trianglesIndexStart] = vertIndexStart + 3;
             }
             o_Mesh.triangles = tempIndices;
             //TODO: setBounds manually
             o_Mesh.RecalculateBounds();
         }
     }