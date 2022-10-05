using UnityEngine;
using System.Collections;
using System;

/* 
EXAMPLES
/string sequence

string[] intro_1 = @"line1
line2
line3
line4
break
this won't display"
.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

			foreach (string line in intro_1)
			{
				if (line == "break") break;
				Debug.Log(line);
				
				if (!this.enabled)
					while (!this.enabled) yield return new WaitForSeconds(2);
				else
					yield return new WaitForSeconds(2);
			}
			
/question
			Debug.Log("yes, no");
			clicked = false;
			while (!clicked) yield return new WaitForSeconds(.1f);
			bool b = choice;
			yield return new WaitForSeconds(1);
			Debug.Log(b);
			
			
			
			*/

namespace PlayerAssets.Story
{
	public class StoryDirector : MonoBehaviour {

		// several lines of text with a delay
		
		protected IEnumerator Monologue(string m)
		{
			string[] g = m.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			
			foreach (string line in g)
			{
				if (line == "break") break;
				Send(line);
				
				if (!this.enabled)
					while (!this.enabled) yield return new WaitForSeconds(2);
				else
					yield return new WaitForSeconds(2);
			}
		}
		protected WaitForSeconds Sleep(float f)
		{
			
			return new WaitForSeconds(f);
		}
		
		// one line of text
		protected void Send(string s)
		{
			Debug.Log(s);
		}
	
		
		
		protected static bool clicked = false;
		protected static bool choice = false;
		public void OnClick(bool b)
		{
			clicked = true;
			choice = b;
		}
		


	}
}