using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace TestProject.Managers
{
	public class ProgressManager : MonoBehaviour {
		
		// things important while playing
		
		
		// predicting long-term             
		public int daysOld                  = 0;
		public int countPlayerVisited       = 0;
		public bool playerVisitedYesterday  = false;
		
		public bool hasAteFruit             = false;
		
		// predicting day behavior 
		public bool hasBrokeToday           = false;
		public bool hasPetToday             = false;
                                           
		// predicting spontaneous behavior 
		public bool isBunnyInCage           = false;
		public bool isPlayerIdle            = false;
		public bool isOwnerBusy             = false;
                                           
		// story foundation                
		public bool hasFoundSecret          = false;
		public bool hasVisitedX             = false;
		public bool hasSneaked              = false;

		// if I don't mind everything knowing this exists
		
		public static ProgressManager instance;
	
	
		void Awake () {
			if (instance == null)
			{
				instance = this;
				GameObject.DontDestroyOnLoad(instance);
			}
			else if ( instance != this)
			{
				Destroy(gameObject);
			}
		}
		
		public void Save() 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + "/bunnyInfo.dat");
			
			ProgressData data = new ProgressData();
			
			// eg the data = this.daysOld 
			data.daysOld                 = daysOld               ;
			data.countPlayerVisited      = countPlayerVisited    ;
			data.playerVisitedYesterday  = playerVisitedYesterday;
			data.hasAteFruit             = hasAteFruit           ;
			data.hasFoundSecret          = hasFoundSecret        ;
			data.hasVisitedX             = hasVisitedX           ;
			data.hasSneaked              = hasSneaked            ;
			
			
			bf.Serialize(file, data);
			file.Close();
		}
		public void Load() 
		{
			if(File.Exists(Application.persistentDataPath + "/bunnyInfo.dat")) 
			{
				
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/bunnyInfo.dat", FileMode.Open);
				ProgressData data = (ProgressData)bf.Deserialize(file) as ProgressData;
				file.Close();
				
				// eg this = the loaded data
				daysOld                	=	data.daysOld                 ;
				countPlayerVisited     	=	data.countPlayerVisited      ;
				playerVisitedYesterday	=	data.playerVisitedYesterday  ;
				hasAteFruit           	=	data.hasAteFruit             ;
				hasFoundSecret        	=	data.hasFoundSecret          ;
				hasVisitedX           	=	data.hasVisitedX             ;
				hasSneaked            	=	data.hasSneaked              ;
			}
		}
    }
	
	
	[Serializable]
	class ProgressData
	{
		
		// things important to save
		
		
		// predicting long-term             
		public int daysOld                  ;
		public int countPlayerVisited       ;
		public bool playerVisitedYesterday  ;
		                                    
		public bool hasAteFruit             ;
		                                    
		// predicting day behavior          
		// not saved
                                            
		// predicting spontaneous behavior 
		// not saved
                                            
		// story foundation                 
		public bool hasFoundSecret          ;
		public bool hasVisitedX             ;
		public bool hasSneaked              ;
		
		
		
		
	}
}