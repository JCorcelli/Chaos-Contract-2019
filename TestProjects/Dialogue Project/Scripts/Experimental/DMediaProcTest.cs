
using UnityEngine;

using UnityEngine.UI;

using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{


	
	public class DWordScrambler  {

		public class Scrambler
		{
			public string Main(string words)
			{
				string pattern = @"\w+  # Matches all the characters in a word.";     

				string newWords = "";                      
				MatchEvaluator evaluator = new MatchEvaluator(WordScrambler);

				try
				{
					newWords = Regex.Replace(words, pattern, evaluator, 	 RegexOptions.IgnorePatternWhitespace);   
				}
				catch
				{}			  
			  return newWords;
			}

			public string WordScrambler(Match match)
			{
				int arraySize = match.Value.Length;
				// Define two arrays equal to the number of letters in the match.
				double[] keys = new double[arraySize];
				char[] letters = new char[arraySize];

				// Instantiate random number generator'
				System.Random rnd = new System.Random();

				for (int ctr = 0; ctr < match.Value.Length; ctr++)
				{
					// Populate the array of keys with random numbers.
					keys[ctr] = rnd.NextDouble();
					// Assign letter to array of letters.
					letters[ctr] = match.Value[ctr];
				}         
				Array.Sort(keys, letters, 0, arraySize, Comparer.Default);      
				return new String(letters);
			}
		}		
		
		
		
		
		
	}

}	