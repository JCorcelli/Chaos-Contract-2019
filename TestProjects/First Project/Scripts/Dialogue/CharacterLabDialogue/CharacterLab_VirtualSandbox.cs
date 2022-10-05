using UnityEngine;
using System.Collections;
using System;

			
namespace PlayerAssets.Story
{
	public class CharacterLab_VirtualSandbox : StoryDirector {
		[SerializeField] protected UnityEngine.EventSystems.EventSystem ES;
		[SerializeField] private GameObject effect;
		// [SerializeField] private Animation deathAnimation
		private int health = 2;
		private string[] s;
		
		private bool playing = true;
		private bool locked = false;
		
		
		// some conditions that are set externally
		public enum Incidents
		{
			Girl = 0,
			Fall = 1,
			Finish = 2,
		}
		// - 1 is restart
		private int incident = -1;
		public void Restart() { 
			incident = -1;
			playing = false;
			}
		// called externally
		public void SetIncident(Incidents ince) { incident = (int)ince; }
		
		// cleanup function, helps identify a moment of inaction
		private void Deselect()
		{
			ES.SetSelectedGameObject(null);
			clicked = false;
			
		}
		
		// decrease health external
		public void Damage()
		{
			health -= 1;
		}
	
		protected void Effect(string s, object k)
		{
			effect.SendMessage(s, k);
		}
		protected void Effect(string s)
		{
			effect.SendMessage(s);
		}
		
		// called when player gives up
		private IEnumerator CoRestart()
		{
			Effect("AllowMove", false);
			Deselect();
			Effect("HideGui","Reset");
			Effect("HideGui","Reset2");
			locked = false;
			Effect("AllowMove", false);
			Effect("Blanket"); // make screen black in one frame
			yield return Sleep(1);
			Effect("FadeIn");
			yield return Sleep(2);
			Effect("ShowAlien");
			Effect("AllowMove", true);
			
			yield return null;
		}
		


		
/// ###################################
/// ABDUCTED ##########################
/// ###################################
private IEnumerator Abducted()
{
	Send ("Girl: ^!$%");
	yield return Sleep(3);
	Send ("Girl: ^!$% !@@@# ^&*($@@#");
	
	// girl gets closer
	
	Send ("Girl: !#$%^*:P");
	
	// girl captures you
	// Alien
	Send ("O.O");
	yield return null;
}

	
/// vvvvvvvvvvvvvvvvvvvvvvvvv
/// BUMPED INTO WALL XXXXXXXX
/// ^^^^^^^^^^^^^^^^^^^^^^^^^
private bool debump = false;
private IEnumerator bumpWall;

private IEnumerator Debump()
{
	yield return StartCoroutine(bumpWall);
	debump = false;
}
public void WallBump()
{
	if (debump) return;
	bumpWall = Monologue(@"ey
ma
ey
ey
ey

ey
no


ey");
	debump = true;
	StartCoroutine("Debump");
	// continues
}
	
/// ###################################
/// SURVIVED AND EXITTED CAVE #######################
/// ###################################
private IEnumerator Finish()
{
	Send ("awesome, we done here");
	// exit cave 
	yield return null;
}

/// ###################################
/// DEAD #######################
/// ###################################
private IEnumerator Died()
{
	Send ("uuuuhggh cccchchchch mmn");
	// death 
	yield return null;
}
	
/// ###################################
/// STORY ARCS #########################
/// ###################################

private IEnumerator PlayStory()
{
	Effect("ShowGui","Screen"); // blocks raycast
	StartCoroutine("CoRestart"); // black screen, starts fade, shows alien

	Effect("HideGui","Questions");
	
	// [intro]
	
	// "help me" animation?
	
	yield return Sleep(2.5f);
	Send("tch.");
	yield return Sleep(2.5f);
	Send("...");
	Effect("ShowGui","Questions");
	
	Effect("PointAtScreenPoint", "buttonImage");
	yield return Sleep(1);
	
	Effect("PointAtWorldPoint","buttonModel");
	
	// need timelimit
	while (!clicked) yield return Sleep(2); // maintaining position uncomfortably?
	
	if (!clicked) 
	{
string mess = @"Message Log: It seems I broke my navigator and I'm here alone.
I'm not sure how long it will be before I can repair it.
This is certain to be a trecherous journey all alone, without technology to guide my hands and feet."; // make it long and funny
	
	IEnumerator co = Monologue(mess);
	StartCoroutine(co);
	// it ends if something tells the coroutine to end. Fair enough.
	while (!clicked) yield return Sleep(.5f); // maintaining position 
	Send("You're alive! I mean you're functioning! Why didn't you say anything? Oh nevermind, you aren't designed to carry a conversation. Just give me directions and keep me motivated.");
	
	
	
	StopCoroutine(co);
	Effect("PointAtScreenPoint", "buttonImage");
	yield return Sleep(1);
	
	Effect("PointAtWorldPoint","buttonModel");
	
	// need timelimit
	while (!clicked) yield return Sleep(2); // maintaining position 
	}
	
	
		
	Deselect();
	yield return Sleep(1);
	
	Effect("HideGui","Questions");
	Effect("HideGui","Screen");
	
	yield return Sleep(1);
	Deselect();
	Send("I think you have the hang of it");
	yield return Sleep(1);
	
	// save checkpoint
	Send("Look around, there are some strange things everywhere. I really like to get to know my surroundings");
	yield return Sleep(2);
	Send("Encourage me.  That's why you're my co-pilot.");
	yield return Sleep(1);
	Effect("ShowGui","Reset");
	yield return Sleep(1);
	
	Send("You can also give up.  I'll try to manage without you.");

	yield return Sleep(1);
	Send("I'll tell you how things when when you get back.");
	yield return Sleep(5);
	
	// checkpoint, save, enable skip
	yield return StartCoroutine("PlayStory2");
}

private IEnumerator PSContinue1()
{ 
	Effect("ShowGui","Screen"); // block screen
	Effect("HideGui","Questions");
	
	yield return Sleep(2);
	Effect("HideGui","Screen");
	Effect("ShowGui","Reset");
	yield return Sleep(1);
	
	Send("I think you have the hang of it");
	yield return Sleep(1);
	
	// save checkpoint
	Send("Look around, there are some strange things everywhere. I really like to get to know my surroundings");
	yield return Sleep(2);
	Send("Encourage me. Lavish me. That's why you're my co-pilot.");
	yield return Sleep(1);
	Send("Figuratively speaking.");
	Effect("ShowGui","Reset");
	yield return Sleep(1);
	
	Send("You can also give up. I'll try to manage without you.");

	yield return Sleep(1);
	Send("I'll tell you how things when when you get back.");
	yield return Sleep(5);
	
	
	// checkpoint, save, enable skip
	yield return StartCoroutine("PlayStory2");
}
private IEnumerator PSContinue2()
{
	Effect("ShowGui","Screen"); // block screen
	
	
	Effect("HideGui","Questions");
	yield return Sleep(2);
	Effect("HideGui","Screen");
	yield return StartCoroutine("PlayStory2");
	
}
private IEnumerator PlayStory2()
{
	// reveal "take a break"
	
	Send("You've been such a good sport.");
	yield return Sleep(1);
	Send("Have a more positive outlook.");
	yield return Sleep(1);
	Send("You can \"take a break\" anytime.");
	yield return Sleep(1);
	Effect("HideGui","Reset");
	Effect("ShowGui","Reset2");
	while (playing)
	{
		yield return Sleep(3);
		if (health <= 0) playing = false;
	}
}
/// #######################################
/// END STORY ARC #########################
/// #######################################
	

/// ###################################
/// MAINTENANCE #######################
/// ###################################
		
/// ###################################
/// START / RESTART #######################
/// ###################################
// Use this for initialization	
[SerializeField] private int checkpoint = 0;	
private void Start () {
	this.StopAllCoroutines();
	playing = true;
	if (checkpoint == 0)
		StartCoroutine("PlayStory");
	else if (checkpoint == 1)
		StartCoroutine("PSContinue1");
	else if (checkpoint == 2)
		StartCoroutine("PSContinue2");
		
	else return; // this shouldn't happen
	
	StartCoroutine("CoRestart");
return;
}


/// ###################################
/// PER-FRAME UPDATE #######################
/// ###################################
private bool m1Down = false;
private bool m2Down = false;
private bool kDown = false;
private int kDownCounter = 0;
private int m1DownCounter = 0;
private int m2DownCounter = 0;
void Update()
{
	m1Down = Input.GetButtonDown("mouse 1");
	m2Down = Input.GetButtonDown("mouse 2");
	kDown = !(m1Down || m2Down) && Input.anyKeyDown;
	if (kDown) kDownCounter ++;
	else if (m1Down) m1DownCounter ++;
	else if (m2Down) m2DownCounter ++;
	
	if (playing || locked) return;
	locked = true;
	if (incident == (int)Incidents.Girl) StartCoroutine("Abduction");
	else if (incident == (int)Incidents.Fall) StartCoroutine("Died");
	else if (incident == (int)Incidents.Finish) StartCoroutine("Finish");
	else 
	
		Start();
	
}





	}
	
}	