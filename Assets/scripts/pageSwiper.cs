using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
	private Vector3 panelLocation;
	private Transform firstChild;
	private Transform lastChild;
	private float firstChildOffset;
        
	void Start()
	{
		panelLocation= transform.position;
		IEnumerator getChildren()
		{	
			yield return null;
			firstChild= transform.GetChild(0);
			lastChild= transform.GetChild(transform.childCount - 1);

			firstChildOffset= firstChild.position.x;
		}
		StartCoroutine(getChildren());
	}

	public void OnDrag(PointerEventData data)
	{
		//float difference= data.pressPosition.x - data.position.x;
		Camera camera= Camera.main;
		float difference= camera.ScreenToWorldPoint(data.pressPosition).x - camera.ScreenToWorldPoint(data.position).x;
		transform.position= panelLocation - new Vector3(difference, 0, 0);
	}

	public void OnEndDrag(PointerEventData data)
	{
		if((firstChild.position.x - firstChildOffset) > 0 || (lastChild.position.x - firstChildOffset) < 0)
		{
			LeanTween.moveX(gameObject, panelLocation.x, 0.3f).setOnComplete(() => {
				panelLocation= transform.position;
			});
		}
		else
		{
			panelLocation= transform.position;
		}
	}

}



