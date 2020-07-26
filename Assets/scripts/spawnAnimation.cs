using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAnimation : MonoBehaviour
{
	public void spawnAnimationEnded()
	{
		if(transform.parent != null)
		{
			GameObject parent= transform.parent.gameObject;
			parent.GetComponent<SpriteRenderer>().enabled = true;
			parent.GetComponent<ingredient>().enabled = true;
			CircleCollider2D collider= parent.GetComponent<CircleCollider2D>();
			if(collider != null)
			{
				collider.enabled= true;
			}
			else
			{
				CapsuleCollider2D col= parent.GetComponent<CapsuleCollider2D>();
				if(col != null)
				{
					col.enabled= true;
				}
			}
		}
		
		Destroy(gameObject);
	}

	public void despawnAnimationEnded()
	{
		Destroy(gameObject);
	}
}
