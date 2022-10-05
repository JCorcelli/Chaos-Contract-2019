using UnityEngine;
using System.Collections;

namespace Utility.GUI
{
		
	
	public class CatalogueListBuilder : GenericListBuilder {
		// starts with a full catalogue, displays a specific list. 
		// Rule: Elements of catalogue never change
		
		public Transform catalogue; // could be swapped
		
		// public Transform copied; // now it's 
		
		protected override Transform Fill(string content){
			// this is either going to imitate old search or copy content. going to copy
			
			copied = catalogue.Find(content);
			
			if (copied == null) return null;
			
			Transform c = Object.Instantiate(copied); 
			
			c.SetParent( transform ); // and the autosort should cover the position
			
			c.gameObject.SetActive(true);
			
			
			return c;
		} 
		
	}
}