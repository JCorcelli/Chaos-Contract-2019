using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace Dungeon
{
	public class DungeonPreviewRead : MonoBehaviour {

	
		public enum ReadEnum {
			ByChar = 0,
			ByLine = 1
		}
		public ReadEnum delayMethod = 0;
		
		public string file = "";
		public string remainingFile = "";
		public string onScreen = "";
		
		public float cps = 10f;
		public float lineDelay = .1f;
		public float runDelay = .1f;
		public Text text;
		
		public bool running = false;
		
		
		public static DungeonPreviewRead instance;
		
		public void Stop (){
			StopAllCoroutines();
			running = false;
			
		}
		public void Play (){
			
			RunByMethod();
		}
		
		public void Clear () {
			running = false;
			text.text = "";
			remainingFile = "";
			onScreen= "";
		}
		
		protected void Awake(){
			text = GetComponent<Text>();
			
			if (instance != null) return;
			instance = this;
			
			
			
		}
		
		protected void OnDisable(){
			StopAllCoroutines();
		}
		
		
		public void Restart() {
			Stop();
			
			text.text = "";
			if (file == "") return;
			
			remainingFile = file;
			
		}
		public void Run(string s) {
			Stop();
			Clear();
			
			file = s;
			remainingFile = file;
			
			Play();
		}
		public void HotSwap(string s) {
			
			
			file = s;
			if (remainingFile == "")
				onScreen = "";
			else
				onScreen += "\n";
			
			if (file == "") return;
			
			
			remainingFile = file;
			
			
		
		}
		
		protected void RunByMethod(){
			if (running) 
				StopAllCoroutines();
			
			running = true;
			
			if (delayMethod == ReadEnum.ByLine)
				StartCoroutine("RunByLine");
			else if (delayMethod == ReadEnum.ByChar)
				StartCoroutine("RunByChar");
		}
		protected IEnumerator RunByChar () {
			if (runDelay > 0.01f)
				yield return new WaitForSeconds(runDelay);
			
			float timer = 0;
			int trim = 0;
			int newLine = file.IndexOf("\n");
			
			bool bLineDelay = lineDelay > .01f;
			while (trim < file.Length - 1)
			{
				timer += Time.deltaTime;
				
				trim = (int)Mathf.Floor(cps * timer);
				
				trim = Mathf.Min(trim, newLine, file.Length - 1);
				
				onScreen = file.Substring(0 , trim );
				
				text.text = onScreen;
				
				
				remainingFile = file.Substring(trim );
				
				
				if (bLineDelay && trim >= newLine)
				{
					newLine = file.IndexOf("\n", newLine + 1);
					if (newLine < 0) newLine = 100000;
					if (lineDelay > 0.01f) 
						yield return new WaitForSeconds(lineDelay);
				
					
					remainingFile = file.Substring(trim + 1);
				}
				
				yield return null;
				
			}
			onScreen += remainingFile;
			remainingFile = "";
			text.text = onScreen;
				
			yield return null;
			running = false;
		}
		
		protected IEnumerator RunByLine () {
			if (runDelay > 0.01f)
				yield return new WaitForSeconds(runDelay);
			
			int newLine = remainingFile.IndexOf("\n");
			while (newLine >= 0)
			{
				//reading a line at a time, for effect
				// could easily be used to make a notepad
				
				onScreen += remainingFile.Substring( 0, newLine + 1);
				
				remainingFile = remainingFile.Substring(newLine + 1);
				
				text.text = onScreen;
				
				if (lineDelay > 0.01f) yield return new WaitForSeconds(lineDelay);
				
				newLine = remainingFile.IndexOf("\n");
			}
		
			onScreen += remainingFile;
			remainingFile = "";
			text.text = onScreen;
			
			yield return null;
			running = false;
			
		}
	}
}