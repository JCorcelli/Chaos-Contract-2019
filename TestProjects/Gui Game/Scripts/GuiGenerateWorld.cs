
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace GuiGame
{
	/// I think this was used with guiblock.txt to experiment

	public class A{
		public List<A> As = new List<A>();
		public static B operator +(A a, A b){
			if (a is B) {
				a = (a as B) + b;
				return a as B;
			}
			else
				return new B(a, b);
		}
		public void logcount(){
			
			Debug.Log(As.Count);
		}
	}
	public class B : A{
		public static B operator +(B a, A b){
			a.As.Add(b);
			return a;
		}
		public B(A a, A b)
		{
			As.Add(a);
			As.Add(b);
		}
	}
	
	public class GuiGenerateWorld : MonoBehaviour{ 
	
		public TextAsset text;
		
		
		public GuiAtlas w;
		public int x;
		public int y;
		
		protected void Awake(){
			
			w = new GuiAtlas();
			w.Generate(text.ToString());
		}
		
		 [ContextMenu("Test")]
		protected void Test(){
			A a = new A();
			a.logcount();  //0 expected
			a += new A();  //
			a.logcount();  //2
			a += new A();  //
			a.logcount();  //3
			a += new A();  //
			a.logcount();  //4
			a += new A();  //
			a.logcount();  //5
		}
		 [ContextMenu("Build")]
		protected void Build(){
			if (w == null)
			w = new GuiAtlas();
			w.Generate(text.ToString());
		}
		 [ContextMenu("ToString")]
		new protected void ToString(){
			
			string g = w.ToString(x);
			Debug.Log(g);
		}
		 [ContextMenu("XCast")]
		protected void XCast(){
			
			string g = w.XCast(x);
			Debug.Log(g);
		}
		 [ContextMenu("YCast")]
		protected void YCast(){
			
			string g = w.YCast(y);
			Debug.Log(g);
			
		}
		
	}
}