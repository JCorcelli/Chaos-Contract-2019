using System;
using UnityEngine;

namespace NPCSystem 
{
	
    [RequireComponent(typeof (IMovable))]
    public class UICharacterControl : UpdateBehaviour
    {
		 
        public IMovable character { get; protected set; } // the character we are controlling
        public Transform target; // target to aim for
		
		
		public string _targetName;
		public string _targetTag;

		new protected Rigidbody rigidbody;
		
		protected void Start () {
			
			if (_targetName != "" )
				target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			
			rigidbody = GetComponent<Rigidbody>();
			
			
            character = GetComponent<IMovable>();


        }


        // Update is called once per frame
        protected override void OnUpdate()
        {
            if (target != null)
            {
				
				Vector3 position = rigidbody.position;
				Vector3 direction = target.position - position;
				
				// add force or move rigidbody
					
				
				
                // use the values to move the character
                character.Move(direction);
            }
            else
            {
                // We still need to call the character's move function, but we send zeroed input as the move param.
                character.Move(Vector3.zero);
				
            }

        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
