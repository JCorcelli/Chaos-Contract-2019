using UnityEngine;
using System.Collections;

public interface ITrigger2D  {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col);
	void OnTriggerExit2D(Collider2D col);
}

public interface ITrigger {

	// Use this for initialization
	void OnTriggerEnter(Collider col);
	void OnTriggerExit(Collider col);
}
