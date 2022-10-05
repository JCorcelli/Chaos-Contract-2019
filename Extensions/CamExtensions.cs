

//ViewportFraming.cs
using UnityEngine;
using System.Collections;


public static class ViewportFraming {
   //First 4 overloads isntantly change camera
 
   /// <summary>
   /// Adjust camera to place an object at a specified viewport point
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, Vector2 newViewportPoint ) {
     FrameObject( camera, focusObject, newViewportPoint, camera.transform.position, camera.orthographicSize );
   }
   /// <summary>
   /// Adjust camera to place an object at a specified viewport point, changing camera orthoSize
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   /// <param name="newOrthoSize">Change camera orthographic size (ignored by perspective camera) (default: no change)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, Vector2 newViewportPoint, float newOrthoSize ) {
     FrameObject( camera, focusObject, newViewportPoint, camera.transform.position, newOrthoSize );
   }
   /// <summary>
   /// Adjust camera to place an object at a specified viewport point, also moves camera
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   /// <param name="newCameraPosition">Where to move the camera (default: no move)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, Vector2 newViewportPoint, Vector3 newCameraPosition ) {
     FrameObject( camera, focusObject, newViewportPoint, newCameraPosition, camera.orthographicSize );
   }
   /// <summary>
   /// Adjust camera to place an object at a specified viewport point, changing camera orthoSize and moving camera
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   /// <param name="newCameraPosition">Where to move the camera (default: no move)</param>
   /// <param name="newOrthoSize">Change camera orthographic size (ignored by perspective camera) (default: no change)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, Vector2 newViewportPoint, Vector3 newCameraPosition, float newOrthoSize ) {
     camera.transform.position = newCameraPosition;
     camera.orthographicSize = newOrthoSize;
     camera.transform.LookAt( focusObject );
     Vector3 viewportPoint = new Vector3( newViewportPoint.x, newViewportPoint.y, 0 );
     Vector3 viewportWorldPosition = camera.ViewportToWorldPoint( viewportPoint );
     float hypotenuse = ( focusObject.position - camera.transform.position ).magnitude;
     float sideA = ( viewportWorldPosition - camera.transform.position ).magnitude;
     float newDistance = Mathf.Sqrt( ( hypotenuse * hypotenuse ) - ( sideA * sideA ) );
     viewportPoint.z = Mathf.Max( newDistance, 0.2f );
     Vector3 worldPoint = camera.ViewportToWorldPoint( viewportPoint );
     Vector3 dirToWorldPoint = ( worldPoint - camera.transform.position ).normalized;
     camera.transform.rotation = Quaternion.FromToRotation( dirToWorldPoint, camera.transform.forward ) * camera.transform.rotation;
   }
 
   //Next 4 overloads tween adjustment over time
 
   /// <summary>
   /// Tween camera over time to place an object at a specified viewport point
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="tweenDuration">Length of time to adjust camera (defulat: 0)</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, float tweenDuration, Vector2 newViewportPoint ) {
     FrameObject( camera, focusObject, tweenDuration, newViewportPoint, camera.transform.position, camera.orthographicSize );
   }
   /// <summary>
   /// Tween camera over time to place an object at a specified viewport point, changing camera orthoSize
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="tweenDuration">Length of time to adjust camera (defulat: 0)</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   /// <param name="newOrthoSize">Change camera orthographic size (ignored by perspective camera) (default: no change)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, float tweenDuration, Vector2 newViewportPoint, float newOrthoSize ) {
     FrameObject( camera, focusObject, tweenDuration, newViewportPoint, camera.transform.position, newOrthoSize );
   }
   /// <summary>
   /// Tween camera over time to place an object at a specified viewport point, also moves camera
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="tweenDuration">Length of time to adjust camera (defulat: 0)</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   /// <param name="newCameraPosition">Where to move the camera (default: no move)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, float tweenDuration, Vector2 newViewportPoint, Vector3 newCameraPosition ) {
     FrameObject( camera, focusObject, tweenDuration, newViewportPoint, newCameraPosition, camera.orthographicSize );
   }
 
   /// <summary>
   /// Tween camera over time to place an object at a specified viewport point, changing camera orthoSize and moving camera
   /// </summary>
   /// <param name="focusObject">Gameobject to frame</param>
   /// <param name="tweenDuration">Length of time to adjust camera (defulat: 0)</param>
   /// <param name="newViewportPoint">viewport point (Vector2) to place frame object (0->1)</param>
   /// <param name="newCameraPosition">Where to move the camera (default: no move)</param>
   /// <param name="newOrthoSize">Change camera orthographic size (ignored by perspective camera) (default: no change)</param>
   public static void FrameObject ( this Camera camera, Transform focusObject, float tweenDuration, Vector2 newViewportPoint, Vector3 newCameraPosition, float newOrthoSize ) {
     if ( tweenDuration < 0.01f ) {
       FrameObject( camera, focusObject, newViewportPoint, newCameraPosition, newOrthoSize );
     } else {
       CameraTweenBehaviour.Instance.DoTween( camera, focusObject, newViewportPoint, newCameraPosition, tweenDuration, newOrthoSize );
     }
   }
 
}
 
 
//Optionally CameraTweenBehaviour.cs  but can remain here in ViewportFraming.cs
//Coroutines require a MonoBehaviour to run on, cant work from static extensions
public class CameraTweenBehaviour : MonoBehaviour {
   private static CameraTweenBehaviour _instance;
   //Use static instance to prevent creating more than one
   public static CameraTweenBehaviour Instance {
     get {
       if ( _instance == null ) {
         _instance = (CameraTweenBehaviour) FindObjectOfType( typeof( CameraTweenBehaviour ) );
       }
       if ( _instance == null ) {
         _instance = Camera.main.gameObject.AddComponent<CameraTweenBehaviour>();
       }
       return _instance;
     }
     set {
       _instance = value;
     }
   }
 
   private Quaternion endRotation, startRotation;
   private Vector3 endPosition, startPosition;
   private float endOrthoSize, startOrthoSize;
   bool changePos, changeOrtho;
 
   public void DoTween ( Camera camera, Transform focusObject, Vector2 newViewportPoint, Vector3 newCameraPosition, float tweenDuration, float newOrthoSize ) {
     StopAllCoroutines();
 
     changePos = ( camera.transform.position - newCameraPosition ).sqrMagnitude > 0.01f;
     changeOrtho = Mathf.Abs( camera.orthographicSize - newOrthoSize ) > 0.01f;
 
     startRotation = camera.transform.rotation;
     startPosition = camera.transform.position;
     startOrthoSize = camera.orthographicSize;
 
     endPosition = newCameraPosition;
     endOrthoSize = newOrthoSize;
 
     camera.transform.position = newCameraPosition;
     camera.transform.LookAt( focusObject );
     if ( camera.orthographic )
       camera.orthographicSize = newOrthoSize;
     Vector3 viewportPoint = new Vector3( newViewportPoint.x, newViewportPoint.y, 0 );
     Vector3 viewportWorldPosition = camera.ViewportToWorldPoint( viewportPoint );
     float hypotenuse = ( focusObject.position - camera.transform.position ).magnitude;
     float sideA = ( viewportWorldPosition - camera.transform.position ).magnitude;
     float newDistance = Mathf.Sqrt( ( hypotenuse * hypotenuse ) - ( sideA * sideA ) );
 
     viewportPoint.z = Mathf.Max( newDistance, 0.2f );
     Vector3 worldPoint = camera.ViewportToWorldPoint( viewportPoint );
     Vector3 dirToWorldPoint = ( worldPoint - camera.transform.position ).normalized;
     endRotation = Quaternion.FromToRotation( dirToWorldPoint, camera.transform.forward ) * camera.transform.rotation;
 
     camera.transform.rotation = startRotation;
     camera.orthographicSize = startOrthoSize;
 
 
 
     StartCoroutine( RunTween( camera, tweenDuration ) );
   }
 
   IEnumerator RunTween ( Camera camera, float tweenDuration ) {
     float time = 0;
     while ( time < 1 ) {
       time += Time.deltaTime * ( 1f / tweenDuration );
       camera.transform.rotation = Quaternion.Slerp( startRotation, endRotation, time );
       if ( changePos )
         camera.transform.position = Vector3.Lerp( startPosition, endPosition, time );
       if ( changeOrtho )
         camera.orthographicSize = Mathf.Lerp( startOrthoSize, endOrthoSize, time );
       yield return null;
     }
     camera.transform.rotation = endRotation;
     if ( changePos )
       camera.transform.position = endPosition;
     if ( changeOrtho )
       camera.orthographicSize = endOrthoSize;
   }
}