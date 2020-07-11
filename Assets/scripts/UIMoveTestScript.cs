using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoveTestScript : MonoBehaviour
{
	public RectTransform rb1;
	public GameObject text;

	// Start is called before the first frame update
	void Start()
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
		TMPro.TextMeshProUGUI txt= text.GetComponent<TMPro.TextMeshProUGUI>();
		txt.text= " camera : " + Camera.main.pixelWidth + " x " + Camera.main.pixelHeight + "\n";
		txt.text= txt.text + "screen : " + Screen.width + " x " + Screen.height + "\n";
		txt.text= txt.text + "canvas : " + objectRectTransform.rect.width + " x " + objectRectTransform.rect.height;

		//objectRectTransform.SetSizeWithCurrentAnchors(RectTranform.Axis.Vertical, Screen.width);
		//objectRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

		Debug.Log(rb1.rect.x);
		resetRecipeBoard();
	}

	void OnGUI()
	{
		Rect rc= new Rect(100, 150, 200, 80);
		GUI.Label(rc, "Rect : " + rb1.rect);
	}

	private void resetRecipeBoard()
	{
		float pos= rb1.rect.x;
		LeanTween.moveX(rb1, -rb1.rect.width, .5f).setOnComplete(() => {
			LeanTween.moveX(rb1, -pos, .5f);
		});
	}
}
