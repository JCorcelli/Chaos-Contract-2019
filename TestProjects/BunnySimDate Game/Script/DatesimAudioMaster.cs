using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAudioMaster : MonoBehaviour{
		
		new public AudioSource audio;
		
		protected void Awake(){
			audio = GetComponent<AudioSource>();
		}
		public void PlayAudio(AudioClip newClip){
			audio.clip = newClip;
			audio.Play();
		}
	}
}