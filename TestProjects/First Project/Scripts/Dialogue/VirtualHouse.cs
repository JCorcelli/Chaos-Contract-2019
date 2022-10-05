using UnityEngine;
using System.Collections;
using System;

namespace PlayerAssets.Story
{
	public class VirtualHouse : StoryDirector {


	
	
		IEnumerator PlayStory( )
		{
			yield return new WaitForSeconds(1);
			
// Test to see how easily I can form an intro sequence
string[] intro_1 = @"You are about to see Bunneh.
Bunneh is not you.
Can you handle stress?
break
".Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

			// sequence 1
			foreach (string line in intro_1)
			{
				if (line == "break") break;
				Debug.Log(line);
				
				if (!this.enabled)
					while (!this.enabled) yield return new WaitForSeconds(2);
				else
					yield return new WaitForSeconds(2);
			}
			
			// question
			Debug.Log("yes, no");
			clicked = false;
			while (!clicked) yield return new WaitForSeconds(.1f);
			bool b = choice;
			yield return new WaitForSeconds(1);
			Debug.Log(b);
		}
		
	}
	
	
	
}