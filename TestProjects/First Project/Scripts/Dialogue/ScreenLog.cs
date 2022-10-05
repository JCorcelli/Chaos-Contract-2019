using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


	
public class ScreenLog : MonoBehaviour {
	
	// information class about a line in its completed form
	public class Context
	{
		public int who = 0;
		public int where = 0;
		public int what = 0;
		public int when = 0;
		public int why = 0;
		public int how = 0;
	}
	
	// information about a line in its raw form
	public class Dialogue
	{
		public string text;
		public float timer;
		public int actor;
		public static int prev_actor = 0;
		public static int prev_timer = 0;
		
		public Dialogue(string te){
			text = te;
			timer = prev_timer;
			actor = prev_actor;
			
		}
		public Dialogue(string te, float ti, int act){
			text = te;
			timer = ti;
			actor = act;
			prev_actor = act;
		}
		
	}
	
	// array of dialogue for this imaginary context
	
	private Dialogue[] a_dialogue = new Dialogue[] {
		new Dialogue("hello.",2f, 0),
		new Dialogue("how are you?",2f, 0),
		new Dialogue("I'm ok.",2f, 1),
		new Dialogue("Did you clean up your house?",2f, 0),
		new Dialogue("For some reason I forgot to.",1f,1),
		new Dialogue("It's been on my mind for a long time now.",.5f, 1),

		new Dialogue("I had a perfectly good chance to do it yesterday."),
		new Dialogue("But no.",8f, 1)
	};
	
	// the list of previously delivered lines
	public List<Context> houseLog;
	
	public GameObject bubble1; // technically the dialogue onScreenText
	public GameObject bubble2;
	private GameObject[] bubbles;
	
	public UnityEngine.UI.Text onScreenText;
	public GameObject onScreenTextCanvas;
	
	public bool running = true;
	
	public int logPos = 0;
	
	
	//
	public float textTimer = 0f;
	
	public float lettersPerSecond = 6.5f;
	private int textPosition = 0;
	private string textSlice = "";
	// initialized at Start
	public float timer;
	public int actor;
	private string text;
	// Use this for initialization
	
	void Start () {
		bubbles = new GameObject[] { bubble1, bubble2 };
		//<INCOMPLETE> <><><><><<><><><>
		// needs to save its position so I can make a long story
		//<INCOMPLETE> <><><><><<><><><>
		
		// load log
		// if log == null 
		houseLog = new List<Context>();
		text = a_dialogue[logPos].text;
		onScreenText.text = "";
		timer = a_dialogue[logPos].timer;
		actor = a_dialogue[logPos].actor;
		
		bubble1.SetActive( false ) ;
		bubble2.SetActive( false ) ;
		
		if (running)
			bubbles[actor].SetActive( true );
		else
			onScreenTextCanvas.SetActive( false ) ;
	}
	
	
	void BeginNewDialogue ( Dialogue[] nd){
		// reset
		a_dialogue = nd;
		
		logPos = 0;
		textPosition = 0;
		onScreenText.text = "";
		
		text = a_dialogue[logPos].text;
		timer = a_dialogue[logPos].timer;
		actor = a_dialogue[logPos].actor;
		
		bubble1.SetActive( false ) ;
		bubble2.SetActive( false ) ;
		
		if (running)
			bubbles[actor].SetActive( true );
		else
			onScreenTextCanvas.SetActive( false ) ;
			
		
		
	}
	// Update is called once per frame
	
	void Update () {
		if (!running) 
		{
			return;
		}
		if (logPos >= a_dialogue.Length) // end of List
		{
			text = "";
			onScreenText.text = text;
			onScreenTextCanvas.SetActive( false ) ;
			actor = -1;
			timer = -1;
			bubble1.SetActive( false ) ;
			bubble2.SetActive( false ) ;
			running = false;
			return;
		}
		onScreenTextCanvas.SetActive( true ) ;
		
		if ( textPosition < text.Length ){
			textTimer += Time.deltaTime;
			textPosition = (int)(lettersPerSecond * textTimer);
			if (textPosition >= text.Length)
			{
				textPosition = text.Length;
				onScreenText.text = text;
			}
			else
			{
				textSlice = text.Substring(0, textPosition);
				onScreenText.text = textSlice;
			}
			
			return;
				
		}
			
		if (timer >= 0)
		{
			timer -= Time.deltaTime;
		}
		else
		{
		
			// add to memory
			Context con = new Context();
			con.who = actor;
			con.what = logPos;
			houseLog.Add(con);
			
			logPos++;
			if (logPos >= a_dialogue.Length) // end of List, avoid index error
			{
				text = "";
				onScreenText.text = text;
				actor = -1;
				timer = -1;
				return;
			}
			
			text = a_dialogue[logPos].text;
			textSlice = "";
			onScreenText.text = "";
			textTimer = 0f;
			textPosition = 0;
			timer = a_dialogue[logPos].timer;
			bubbles[actor].SetActive( false );
			actor = a_dialogue[logPos].actor;
			bubbles[actor].SetActive( true );
			
				
		}
		
	}
}
