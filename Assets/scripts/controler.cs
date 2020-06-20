using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controler : MonoBehaviour
{
	[SerializeField] private GameObject blade;
	[SerializeField] private Rigidbody2D spawnRb;
	[SerializeField] private GameObject rayTrail;

	private float firerate= 0.25f;
	private float canThrow= -1f;
	private Vector3 touchPos;

	void Start()
	{
		
	}

	void Update()
	{

		if(Input.touchCount > 0 && Time.time > canThrow)
		{	
			Touch touch= Input.GetTouch(0);
			touchPos= Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			touchPos.z= 0;

			if(touch.phase == TouchPhase.Ended)
			{
				//throwRay();
				throwBlade();
				
				canThrow= Time.time + firerate;
			}
		}
	}

	private void throwBlade()
	{
		Vector3 direction= (touchPos - (Vector3)spawnRb.position).normalized;
		float rotation= Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		spawnRb.rotation= rotation;
		Instantiate(blade, spawnRb.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
	}

	private void throwRay()
	{
		Vector3 pointer= (touchPos - spawnRb.transform.position); 

		Debug.DrawRay(spawnRb.transform.position, pointer, Color.green, .5f);
		StartCoroutine(drawRayTrail(pointer.normalized));

		RaycastHit2D[] hits= Physics2D.RaycastAll(spawnRb.transform.position, pointer, 100.0f);

		for (int i = 0; i < hits.Length; i++)
		{
			if(hits[i].transform.gameObject.tag == "ingredient")
			{
				Destroy(hits[i].transform.gameObject);
			}
		}
	}

	public IEnumerator drawRayTrail(Vector3 pos)
	{
		GameObject ray= Instantiate(rayTrail, spawnRb.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.1f);
		ray.transform.position= touchPos;
		Destroy(ray, 0.5f);

	}

}




