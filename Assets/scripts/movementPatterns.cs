using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementPatterns : MonoBehaviour
{
	[SerializeField] public float angularVelocity;
	[SerializeField] public int type;
	
	private Vector3 center;
	private Vector3 pos;
	private float mag;
	private float angle;

	void Start()
	{
		pos= transform.localPosition;

		center= transform.parent != null ? transform.parent.position : new Vector3(0, 0, 0);
		Vector3 currentPos= transform.position;
		float dx= (center.x - currentPos.x);
		float dy= (center.y - currentPos.y);
		angle= Mathf.Atan2(dy, dx) - Mathf.PI;

		if(type == 3)
			angularVelocity= (Random.value * 2) + 0.5f;
	}

	void Update()
	{
		switch(type)
		{
			case 0 : {
				moveInJelifishPattern();
				break;
			}
			case 1 : {
				moveInWeirdpattern();
				break;
			}
			case 2 : {
				moveCircularly();
				break;
			}
			case 3 : {
				oscilateVerticaly();
				break;
			}
		}
	}

	private void moveInJelifishPattern()
	{
		mag= ((Mathf.Cos(angle * 4f) + 1) / 4) + 0.5f;
		float dx= Mathf.Cos(angle) * (pos.magnitude / mag);
		float dy= Mathf.Sin(angle) * (pos.magnitude / mag);
		transform.localPosition= new Vector2(dx, dy);
		angle= (angle + (angularVelocity * Time.deltaTime)) % 360;
	}

	private void moveInWeirdpattern()
	{
		mag= ((Mathf.Cos(angle * 10f) + 1) / 4) + 0.5f;
		float dx= Mathf.Cos(angle) * pos.magnitude;
		float dy= Mathf.Sin(angle) * (pos.magnitude / mag);
		transform.localPosition= new Vector2(dx, dy);
		angle= (angle + (angularVelocity * Time.deltaTime)) % 360;
	}

	private void moveCircularly()
	{
		float dx= Mathf.Cos(angle) * pos.magnitude;
		float dy= Mathf.Sin(angle) * pos.magnitude;
		transform.localPosition= new Vector2(dx, dy);
		angle= (angle + (angularVelocity * Time.deltaTime)) % 360;
	}

	private void oscilateVerticaly()
	{
		float dx= pos.magnitude;
		float dy= Mathf.Sin(angle) * 4;
		transform.localPosition= new Vector2(dx, dy);
		angle= (angle + (angularVelocity * Time.deltaTime)) % 360;
	}

	public IEnumerator smoothMove(Vector2 start, Vector2 end, float duration)
	{
		float t= 0;
		while(t <= 1.0f)
		{
			t+= Time.deltaTime / duration;
			transform.position= Vector2.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
			yield return null;
		}
	}
}
