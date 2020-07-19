using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
	private Vector3 panelLocation;
	private float threshold= 0.2f;
	private int totalPages= 0;
	private int currentPage= 0;

	void Start()
	{
		panelLocation= transform.position;
		IEnumerator getChildren()
		{	
			yield return null;
			totalPages= transform.childCount - 1;
		}
		StartCoroutine(getChildren());
	}

	public void OnDrag(PointerEventData data)
	{
		float difference= Camera.main.ScreenToWorldPoint(new Vector2(data.pressPosition.x, 0)).x - Camera.main.ScreenToWorldPoint(new Vector2(data.position.x, 0)).x; 
		transform.position= panelLocation - new Vector3(difference, 0, 0);
	}

	public void OnEndDrag(PointerEventData data)
	{
		float percentage= (data.pressPosition.x - data.position.x) / Screen.width;
		float nudge= Camera.main.ScreenToWorldPoint(new Vector2(transform.GetChild(0).GetComponent<RectTransform>().rect.width, 0)).x / 2;

		if(Mathf.Abs(percentage) > threshold)
		{
			Vector3 newLocation= panelLocation;
			if(percentage < 0)
			{
				if(currentPage <= 0)
				{
					LeanTween.moveX(transform.gameObject, panelLocation.x, 0.3f);
					return;
				}
				newLocation= panelLocation + new Vector3(nudge, 0, 0);
				currentPage--;
			}
			else if(percentage > 0)
			{
				if(currentPage >= totalPages)
				{
					LeanTween.moveX(transform.gameObject, panelLocation.x, 0.3f);
					return;
				}
				newLocation= panelLocation - new Vector3(nudge, 0, 0);
				currentPage++;
			}

			LeanTween.moveX(transform.gameObject, newLocation.x, 0.3f).setOnComplete(() => {
				panelLocation= newLocation;
			});			
		}
		else
		{
			LeanTween.moveX(transform.gameObject, panelLocation.x, 0.3f);
		}
	}

}
