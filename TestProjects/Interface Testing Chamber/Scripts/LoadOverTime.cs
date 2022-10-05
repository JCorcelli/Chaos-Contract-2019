using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace LoadTest
{
	public class LoadOverTime : MonoBehaviour {

		// Use this for initialization
		
		public int prog = 0;
		protected IEnumerator Start () {
			yield return new WaitForSeconds(3f);
			//Application.LoadLevelAdditive("RoomWithABox");
			Application.backgroundLoadingPriority = ThreadPriority.Low;
			
			AsyncOperation synced = SceneManager.LoadSceneAsync("RoomWithABox", LoadSceneMode.Additive);
			
			synced.allowSceneActivation = false;
			
			while (synced.progress < .8f)
				yield return new WaitForSeconds(.5f);
			
			synced.allowSceneActivation = true;
			prog++;
			
			
			synced = SceneManager.LoadSceneAsync("RoomWith2DInterface", LoadSceneMode.Additive);
			// synced.allowSceneActivation = false;
			
			while (synced.progress < .8f)
				yield return new WaitForSeconds(.5f);
		   
			yield return new WaitForSeconds(2.5f);
			SceneManager.UnloadSceneAsync("RoomWith2DInterface");
			
			prog++;
			
			while (synced.progress < .8f)
				yield return new WaitForSeconds(.5f);
			
			prog++;
		}
		
	}
}