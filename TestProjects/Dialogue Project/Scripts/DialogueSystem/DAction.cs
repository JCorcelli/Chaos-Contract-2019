
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	
	public delegate void DActionDelegate(string parsed);
	public class DLocalAction 
	{
		public Dictionary<string, DActionDelegate> a = new Dictionary<string, DActionDelegate>();
		
		
	}
	public class DAction   {
		public static Dictionary<string, DActionDelegate> actions;
		
		public static Dictionary<string, DActionDelegate> globalActions = new Dictionary<string, DActionDelegate>();
		
		public static List<string> registry = new List<string>();
		
		public string name;
		
		public DAction(string s){
			// make it, then start adding
			
			s = Trim(s);
			name = s;
			AddKey(s);
		}
		public static void Use(Dictionary<string, DActionDelegate> act){
			actions = act;
		}
		public static void Use(){
			actions = globalActions;
		}
		
		public void Add(DActionDelegate d){
			AddKey(name);
			actions[name] += d;
		}
		public void Remove(DActionDelegate d){
			//RemoveKey(name);
			actions[name] -= d;
		}
		public static void Add(string n, DActionDelegate d){
			AddKey(n);
			actions[n] += d;
		}
		public static void Remove(string n, DActionDelegate d){
			//RemoveKey(n);
			actions[n] -= d;
		}
		
		//## static things
		public static bool ValidAction(string markup){
			if (markup.Length < 1) return false;
			markup = Trim(markup);
			if (markup.Length < 1) return false;

			string[] prefxvar ;
			prefxvar = SplitFirst(markup, " ");
			
			bool b = registry.Contains(prefxvar[0]);
			
			return b;
		}
		
		protected static string[] SplitFirst(string text, string search)
		{
			// it's for splitting string prefab param[param param]
		  int pos = text.IndexOf(search);
		  if (pos < 0)
		  {
			return new string[]{text, ""};
		  }
		  return new string[]{text.Substring(0, pos), text.Substring(pos + search.Length).Trim()};
		}
		
		public static string Trim(string markup){
			
			// shouldn't have to trim the first edge... but I will be careful.
			
			
			markup = markup.Trim();
			if (markup.Length < 1) return "";
			if (markup[0] == '{') // if there's a r brace, it could be an inner mark
			markup = markup.Substring(1, markup.Length-2).Trim(); // avoid early split, could also do a left trim
			return markup;
		}
		public static void Do(string markup){
			string[] prefxvar ;
			markup = Trim(markup);
			
			prefxvar = SplitFirst(markup, " ");
			string s = prefxvar[0].Trim();
			
			FormattedDo(s, prefxvar[1]);
		}
		public static void FormattedDo(string n, string p){
			// should be formatted
			
			bool b = actions.ContainsKey(n);
			
			if (b && actions[n] != null)actions[n].Invoke(p);
		}
		
		public static void AbandonKey(string s){
			if (actions[s] == null)
			{
				actions.Remove(s);
				RemoveKey(s);
			}
		}
		public static void RemoveKey(string s){
			s = Trim(s);
			if (registry.Contains(s)) 	
				registry.Remove(s);
			
			
		}
		
		public static void AddKey(string s){
			s = Trim(s);
			if (!actions.ContainsKey(s)) 
				actions.Add(s, null);
			if (registry.Contains(s)) 	
				return;
			else
				registry.Add(s);
			
		}
		
	}
	
}