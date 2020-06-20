using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
	private Rigidbody2D rb;    
	private float speed= 150f;

	void Start()
	{
		rb= GetComponent<Rigidbody2D>();
		rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
		Destroy(gameObject, 2f); 
	}

	/*public void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "ingredient")
		{
			Destroy(other.gameObject);
		}
	}*/

	/*public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "ingredient")
		{
			Destroy(other.gameObject);
		}
	}*/
}

