
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{


	
	public class DRedBroken : DRedParser{
		
		public DRedBroken():base(){}
		public override void WalkWord()
		{
			// take one step
			// if conditions, then foo. May replace word.
			// BreakWord();
			// or just add the character
			if (currentWord.Length < 1) return;
			string ncWord = "";
			string filteredWord = currentWord.ToString();
			foreach(char cs in filteredWord.ToCharArray())
			{
				
				if (Random.value > .85f)
				{
					if (cs == ' ') ncWord += '.';
					else
					ncWord +=  ' ';
				}
				else
					ncWord += cs;
			}
			filteredWord = ncWord;
			
			c = filteredWord[inWordPos];
			inWordPos ++;
			// maybe a string replace....
			streamedText.DAppend( c);

			
		}
		
		public override void InvokePrefab(){
			
			DAction.Use();
			DAction.Do(currentPrefab.ToString());
			
			streamedText.DAppend( currentPrefab);
			
			// some kind of DEBUG VISION

		}
	}
}	