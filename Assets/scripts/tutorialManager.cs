using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialManager : MonoBehaviour
{
	[SerializeField] private GameObject blade;
	[SerializeField] private Rigidbody2D spawnRb;
	[SerializeField] private GameObject backDrop;
	[SerializeField] private GameObject helpingHand;
	[SerializeField] private GameObject ing1;
	[SerializeField] private GameObject ing2;
	[SerializeField] private GameObject ing3;
	[SerializeField] private GameObject text1;
	[SerializeField] private GameObject text2;
	[SerializeField] private GameObject text3;
	[SerializeField] private GameObject mask;
	[SerializeField] private GameObject OK;
	[SerializeField] private Transform slotParent;

	private float firerate= 0.25f;
	private float canThrow= -1f;
	private Vector3 touchPos;
	private bool focus= true;
	private bool stage2= false;
	private bool finalStage= false;
	private UiManager UI; 

	void Start()
	{
		UI= gameObject.GetComponent<UiManager>();
		Camera.main.GetComponent<eventSystem>().onRecipeBoardUpdate+= () => {
			if(stage2 == false)
			{
				helpingHand.SetActive(false);
				text1.SetActive(false);
				mask.SetActive(true);
				text2.SetActive(true);
				OK.SetActive(true);
				stage2= true;
			}
		};

		IEnumerator waitfor1second()
		{	
			yield return new WaitForSeconds(1f);
			backDrop.SetActive(true);
			helpingHand.SetActive(true);
			LeanTween.move(helpingHand, ing1.transform.position, 1.5f).setOnComplete(() => {
				text1.SetActive(true);
			});
		}
		StartCoroutine(waitfor1second());
	}

	void Update()
	{
	
		if(UI.invalidIngredientCount > 0 && finalStage == false)
		{
			focus= true;
			finalStage= true;
			mask.SetActive(true);
			text3.SetActive(true);
			OK.SetActive(true);
			backDrop.SetActive(true);
		}

		if(Input.touchCount > 0 && Time.time > canThrow)
		{	
			Touch touch= Input.GetTouch(0);
			touchPos= Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			touchPos.z= 0;

			if(touch.phase == TouchPhase.Ended)
			{
				if(focus == false)
				{
					throwBlade();
				}
				else if(ing1 != null && verifyTouchPoint((Vector2)touchPos, (Vector2)ing1.transform.position) == true)
				{
					throwBlade();
				}
				canThrow= Time.time + firerate;
			}
		}
	}

	public void nextStage()
	{
		if(finalStage == true)
		{
			text3.SetActive(false);
			backDrop.SetActive(false);
		}

		focus= false;
		text2.SetActive(false);
		mask.SetActive(false);
		backDrop.SetActive(false);
		OK.SetActive(false);
	}

	private void throwBlade()
	{
		Vector3 direction= (touchPos - (Vector3)spawnRb.position).normalized;
		float rotation= Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		spawnRb.rotation= rotation;
		Instantiate(blade, spawnRb.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
	}

	private bool verifyTouchPoint(Vector2 b1, Vector2 b2)
	{
		float dx= Mathf.Abs(b1.x - b2.x);
		float dy= Mathf.Abs(b1.y - b2.y);
		//Debug.Log(((dx * dx) + (dy * dy)) + " : " + b1.x + ", " + b1.y + (((dx * dx) + (dy * dy)) <= 2));
		return (((dx * dx) + (dy * dy)) <= 2);
	}
}





