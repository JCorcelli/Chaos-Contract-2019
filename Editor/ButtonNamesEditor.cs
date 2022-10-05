using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

public class ButtonNamesEditor :  EditorWindow {

	public ButtonNames bn;
    // [MenuItem("EditorScript/SetAllButtons")]
	
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ButtonNamesEditor));
    }
 
    public void OnGUI()
    {
        if (GUILayout.Button("Set all the buttons"))
        {
			if (bn == null) 
			{
				Debug.Log("that's gone");
				return;
			}
            GetAllButtons();
        }
    }
	public void GetAllButtons()
	{
		ReadInputManager r = new ReadInputManager();
		r.ReadAxes();
		
		bn.buttonNames = r.buttonNames.ToArray();
		
		
	}
}
 
public class ReadInputManager
{
	 public List<string> buttonNames = new List<string>();
	 public void ReadAxes()
	 {
		 var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

		 SerializedObject obj = new SerializedObject(inputManager);

		 SerializedProperty axisArray = obj.FindProperty("m_Axes");

		 if (axisArray.arraySize == 0)
			 Debug.Log("No Axes");

		
		 for( int i = 0; i < axisArray.arraySize; ++i )
		 {
			 var axis = axisArray.GetArrayElementAtIndex(i);

			 var name = axis.FindPropertyRelative("m_Name").stringValue;
			// var axisVal = axis.FindPropertyRelative("axis").intValue;
			// var inputType = (InputType)axis.FindPropertyRelative("type").intValue;
			buttonNames.Add(name);
		 }
	}


 public enum InputType
 {
	 KeyOrMouseButton,
	 MouseMovement,
	 JoystickAxis,
 };
}
