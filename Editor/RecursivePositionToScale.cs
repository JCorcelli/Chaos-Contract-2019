using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RecursivePositionToScale : EditorWindow 
{
    [MenuItem("Window/RecursivePositionToScale")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RecursivePositionToScale));
    }
	static float searchScaleValue = .117f;
	static float setScaleValue = 1f; // trying to have it 1 not to break automatically
	
    public void OnGUI()
    {
		GUILayout.Label ("Select and Reposition", EditorStyles.boldLabel);
		searchScaleValue = EditorGUILayout.FloatField ("search value", searchScaleValue);
		
		
        if (GUILayout.Button("Select Objects With Scale Value"))
        {
            SearchSelect();
        }
		
		
		
		searchScaleValue = EditorGUILayout.FloatField ("reposition value", searchScaleValue);
        if (GUILayout.Button("pass 1 Reposition all children"))
        {
            PositionSelected();
        }
		
		setScaleValue = EditorGUILayout.FloatField ("set value", setScaleValue);
        if (GUILayout.Button("pass 2 a Scale all selected bounds"))
        {
            ScaleSelectedBounds();
        }
        if (GUILayout.Button("pass 3 Check all meshes"))
        {
            SelectChildMeshes();
        }
        if (GUILayout.Button("(optional) Check all renderers"))
        {
            SelectChildRenderers();
        }
		
        if (GUILayout.Button("#(optional) Select all child colliders"))
        {
			SelectChildColliders();
        }
        if (GUILayout.Button("#(optional) Scale colliders"))
        {
			ScaleSelectedColliders();
        }
		
		setScaleValue = EditorGUILayout.FloatField ("set value", setScaleValue);
        if (GUILayout.Button("##(optional) Scale all selected"))
        {
            RescaleSelected();
        }
    }
	
	
	static void RecursiveSelect(Transform tParent, List<GameObject> searchList){
		
		// doesn't affect the first one
		if (tParent.localScale.x ==  searchScaleValue || tParent.localScale.y ==  searchScaleValue || tParent.localScale.z ==  searchScaleValue)
			searchList.Add(tParent.gameObject);
		
		foreach (Transform t in tParent)
		{
			RecursiveSelect(t, searchList);
		}
	}
	static void RecursiveRendererSelect(Transform tParent, List<GameObject> searchList){
		
		// doesn't affect the first one
		if (tParent.GetComponent<Renderer>() != null)
			searchList.Add(tParent.gameObject);
		
		foreach (Transform t in tParent)
		{
			RecursiveRendererSelect(t, searchList);
		}
	}
	
	static void RecursiveMeshSelect(Transform tParent, List<GameObject> searchList){
		
		// doesn't affect the first one
		if (tParent.GetComponent<MeshFilter>() != null)
			searchList.Add(tParent.gameObject);
		else if (tParent.GetComponent<SkinnedMeshRenderer>() != null)
			searchList.Add(tParent.gameObject);
		
		foreach (Transform t in tParent)
		{
			RecursiveMeshSelect(t, searchList);
		}
	}
	
	static void RecursiveColliderSelect(Transform tParent, List<GameObject> searchList){
		
		// doesn't affect the first one
		if (tParent.GetComponent<Collider>() != null)
			searchList.Add(tParent.gameObject);
		
		foreach (Transform t in tParent)
		{
			RecursiveColliderSelect(t, searchList);
		}
	}
	
	static void Reposition(Transform t){
		position_count ++;
		t.localPosition = Vector3.Scale(t.localPosition, targetScale);
	}
	static void RecursiveReposition(Transform tParent){
		
		// doesn't affect the first one
		foreach (Transform t in tParent)
		{
			Reposition(t);
			RecursiveReposition(t);
			
		}
	}
	
	
	static Vector3 targetScale;
	static int position_count;
	
    private static void SearchSelect()
    {
        Transform[] go = Selection.transforms;
        int go_count = 0;
		int selection_count= 0;
		List<GameObject> searchList = new List<GameObject>();
        foreach (Transform g in go)
        {
            go_count++;
			RecursiveSelect(g, searchList);
        }
 
		Selection.objects = searchList.ToArray();
		selection_count = searchList.Count;	
			
        Debug.Log(string.Format("Searched {0}, Selected {1}", go_count, selection_count));
    }
	
	private static void ScaleSelectedColliders()
	{
        GameObject[] go = Selection.gameObjects;
        foreach (GameObject g in go)
        {
			//  BoxCollider, SphereCollider, CapsuleCollider
			
			if (g.GetComponent<SphereCollider>() != null)
			{
				SphereCollider s = g.GetComponent<SphereCollider>();
				s.center = s.center * setScaleValue;
				s.radius = s.radius * setScaleValue;
			}
			else if (g.GetComponent<BoxCollider>() != null)
			{
				BoxCollider s = g.GetComponent<BoxCollider>();
				s.center = s.center * setScaleValue;
				s.size = s.size * setScaleValue;
			}
			else if (g.GetComponent<CapsuleCollider>() != null)
			{
				CapsuleCollider s = g.GetComponent<CapsuleCollider>();
				s.center = s.center * setScaleValue;
				s.radius = s.radius * setScaleValue;
				s.height = s.height * setScaleValue;
			}
			
			
			
		}
		
	}
	
	private static void ScaleSelectedBounds()
	{
        GameObject[] go = Selection.gameObjects;
        foreach (GameObject g in go)
        {
			Bounds bounds;
			if (g.GetComponent<SkinnedMeshRenderer>() != null)
			{
				bounds = g.GetComponent<SkinnedMeshRenderer>().localBounds;
				bounds.center = bounds.center * setScaleValue;
				bounds.extents = bounds.extents * setScaleValue;
				g.GetComponent<SkinnedMeshRenderer>().localBounds = bounds;
			}
			else if (g.GetComponent<MeshFilter>() != null)
			{
				Mesh mesh = g.GetComponent<MeshFilter>().mesh;
				mesh.RecalculateBounds();
			}
			
			
			
		}
		
	}
    private static void SelectChildRenderers()
	{
        Transform[] go = Selection.transforms;
        int go_count = 0;
		int selection_count= 0;
		List<GameObject> searchList = new List<GameObject>();
        foreach (Transform g in go)
        {
            go_count++;
			RecursiveRendererSelect(g, searchList);
        }
 
		Selection.objects = searchList.ToArray();
		selection_count = searchList.Count;	
			
        Debug.Log(string.Format("Searched {0}, Selected {1}", go_count, selection_count));
		
	}
	
    private static void SelectChildColliders()
	{
        Transform[] go = Selection.transforms;
        int go_count = 0;
		int selection_count= 0;
		List<GameObject> searchList = new List<GameObject>();
        foreach (Transform g in go)
        {
            go_count++;
			RecursiveColliderSelect(g, searchList);
        }
 
		Selection.objects = searchList.ToArray();
		selection_count = searchList.Count;	
			
        Debug.Log(string.Format("Searched {0}, Selected {1}", go_count, selection_count));
		
	}
	
	
    private static void SelectChildMeshes()
	{
        Transform[] go = Selection.transforms;
        int go_count = 0;
		int selection_count= 0;
		List<GameObject> searchList = new List<GameObject>();
        foreach (Transform g in go)
        {
            go_count++;
			RecursiveMeshSelect(g, searchList);
        }
 
		Selection.objects = searchList.ToArray();
		selection_count = searchList.Count;	
			
        Debug.Log(string.Format("Searched {0}, Selected {1}", go_count, selection_count));
		
	}
	
    private static void PositionSelected()
    {
        GameObject[] go = Selection.gameObjects;
        int go_count = 0;
        foreach (GameObject g in go)
        {
            go_count++;
			targetScale = g.transform.localScale;
			RecursiveReposition(g.transform);
			g.transform.localScale = Vector3.one;
        }
 
        Debug.Log(string.Format("Rescaled {0}, Repositioned {1}", go_count, position_count));
    }
	
    private static void RescaleSelected()
    {
        Transform[] go = Selection.transforms;
        int go_count = 0;
        foreach (Transform g in go)
        {
            go_count++;
			g.localScale = g.localScale * setScaleValue;
        }
 
        Debug.Log(string.Format("Rescaled {0}", go_count));
    }
	
}
