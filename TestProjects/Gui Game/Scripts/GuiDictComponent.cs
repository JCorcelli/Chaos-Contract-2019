
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace GuiGame
{

	public class GuiDictComponent : MonoBehaviour{ 
		public GuiDict _dict  ;
		public GuiDict dict {
			get {
				if (_dict == null)
				{
					_dict = new GuiDict();
					//Debug.Log("autogen dictionary");
				}
				return _dict;
			}
			
			set{
				_dict = value;
				UpdateKeys();
			}
		}
		
		public void UpdateKeys(){
			
			de = "";
			
			if (_dict == null) return;
			var k = _dict.Keys;
			foreach (string v in k)
				de += v + ". . .";
		}
		
		public void Instance(){
			dict.Instance();
		}
		public void Instance(string key){
			dict.Instance(key);
			
		}
		
		public string de = "";
		public void Add(string key){
			dict.Add(key);
		}
		public void Add(string key, string g){
			dict.Add(key, g);
			
		}
		public void Add(string key, GuiDict g){
			dict.Add(key, g);
			
		}
		
		public void Add(GuiDict g){
			dict.Add(g);
			
		}
		
		public bool Contains(string key)
		{
			return dict.Contains(key);
			
		}
		public void Nullify(string key, int re)
		{
			dict.Nullify(key, re);
		}
		public void Destroy(string key, int re)
		{
			dict.Destroy(key, re);
		}
		public bool Remove(string key)
		{
			return dict.Remove(key);
		}
		public GuiDict this[string key]
		{
			get =>  dict[key];
			set => dict[key] = value;
			
		}	
		public GuiDict this[int index]
		{
			get =>  dict[index];
			set {
				dict[index] = value;
			}
		}			
		
		public string[] Keys
		{
			get => dict.Keys;
			
		}
	
	}
}