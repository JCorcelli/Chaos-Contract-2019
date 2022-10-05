using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Utility.Tree
{
	public class SendReadyUpOnLoad : MonoBehaviour {

		// prototype
		
		protected AsyncOperation synced;
		protected SelectHUB select;
		protected bool running = false;
		protected bool additiveLoad = false;
		public string levelName = "RoomWithABox";
		public void OnEnable() {
			synced = LoadingTree.synced;
			select = GetComponentInParent<SelectHUB>();
			Load();
		}
		public void OnDisable() {
			running = false;
		}
		
		
		protected void Load() {
			levelName = LoadingTree.level;
			additiveLoad = LoadingTree.additive;
			
			StartCoroutine("_DelayLoad");
		}
		
		protected IEnumerator _DelayLoad() {
			
			yield return null;
			
			Application.backgroundLoadingPriority = ThreadPriority.Low;
			
			
			if (additiveLoad)
				synced = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
			
			else
				synced = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
				
			
			synced.allowSceneActivation = false;
			
			running = false;
			
			yield return new WaitForSeconds(.1f);
			
			
			running = true;
			
		}
		protected void FinishLoad() {
			synced.allowSceneActivation = true;
			synced = null;
			running = false;
			select.Ready();
			
		}
		public float progressBar = 0f;
		protected void Update(){
			if (synced == null) return;
			progressBar = synced.progress;
			if (!running ) return;
			if (synced.progress < .8f) return;
			FinishLoad();
				
		}
	}
}