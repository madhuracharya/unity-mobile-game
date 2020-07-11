using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ingredient : MonoBehaviour
{
	private Rigidbody2D rb;
	private float speed= 3f;
	private float revSpeed;
	private GameObject canvas;
	[SerializeField] private GameObject explosion;
	[SerializeField] private GameObject sliceBack;
	[SerializeField] private GameObject sliceFront;
	[SerializeField] private Sprite sliceBackImage;
	[SerializeField] private Sprite sliceFrontImage;


	//private eventSystem eventSystem;

	void Start()
	{
		rb= GetComponent<Rigidbody2D>();
		canvas= GameObject.Find("Canvas");
		revSpeed= Random.Range(-50.0f, 50f);
		//eventSystem= Camera.main.GetComponent<eventSystem>();
	}

	void Update()
	{
		transform.Rotate(0, 0, revSpeed * Time.deltaTime);
	}

	Dictionary<string, Color[]> getColors()
	{
		Dictionary<string, Color[]> IngredientColors = new Dictionary<string, Color[]>();
		IngredientColors["greenApple"]= new Color[]{new Color(0.8f, 0.8f, 0.8f, 1f), new Color(0.5576454f, 1f, 0f, 1f)};
		IngredientColors["redApple"]= new Color[]{new Color(.8f, 0.8f, 0.8f, 1f), new Color(1f, 0f, 0f, 1f)};
		IngredientColors["orange"]= new Color[]{new Color(1f, 0.5907618f, 0f, 1f), new Color(1f, 0.8353392f, 0f, 1f)};
		IngredientColors["pineapple"]= new Color[]{new Color(1f, 0.8873908f, 0f, 1f), new Color(0.7608987f, 1f, 0f, 1f)};

		return IngredientColors;
	}

	public void renderSlices()
	{
		GameObject slotParent = GameObject.Find("slotParent");
		GameObject animationCanvas= GameObject.Find("AnimationCanvas");

		GameObject sl1= Instantiate(sliceBack) as GameObject;	
		GameObject sl2= Instantiate(sliceFront) as GameObject;
		sl1.transform.SetParent(animationCanvas.transform, false);
		sl2.transform.SetParent(animationCanvas.transform, false);

		/*Vector3 pos= Camera.main.WorldToScreenPoint(gameObject.transform.position);
		sl1.transform.position= pos;
		sl2.transform.position= pos;*/

		sl1.transform.position= transform.position;
		sl2.transform.position= transform.position;

		ingredientSlices is1= sl1.GetComponent<ingredientSlices>();
		ingredientSlices is2= sl2.GetComponent<ingredientSlices>();

		foreach (Transform child in slotParent.transform)
		{
			if(child.GetComponent<Alias>().alias == gameObject.GetComponent<Alias>().alias && child.GetChild(2).gameObject.activeSelf == false)
			{
				if(is1 != null)
				{
					is1.ingredientName= gameObject.GetComponent<Alias>().alias;
					is1.lookAt= child.GetComponent<RectTransform>();
					is1.alias= "sliceBack";
					Image img= is1.GetComponent<Image>();
					img.sprite= sliceFrontImage;
					//img.color = new Color(img.color.r, img.color.g, img.color.b, 0.7f);
				}

				if(is2 != null)
				{
					is2.ingredientName= gameObject.GetComponent<Alias>().alias;
					is2.lookAt= child.GetComponent<RectTransform>();
					is2.alias= "sliceFront";
					Image img= is2.GetComponent<Image>();
					img.sprite= sliceBackImage;
					//img.color = new Color(img.color.r, img.color.g, img.color.b, 0.7f);
				}
				Destroy(gameObject);
				return;
			}
		}

		if(is1 != null)
		{
			is1.ingredientName= gameObject.GetComponent<Alias>().alias;
			is1.alias= "sliceBack";
			Image img= is1.GetComponent<Image>();
			img.sprite= sliceFrontImage;
			//img.color = new Color(img.color.r, img.color.g, img.color.b, 0.7f);
		}

		if(is2 != null)
		{
			is2.ingredientName= gameObject.GetComponent<Alias>().alias;
			is2.alias= "sliceFront";
			Image img= is2.GetComponent<Image>();
			img.sprite= sliceBackImage;
			//img.color = new Color(img.color.r, img.color.g, img.color.b, 0.7f);
		}

		Destroy(gameObject);
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		void breakIngredient()
		{
			GameObject exp= Instantiate(explosion, transform.position, transform.rotation);		
			if(getColors().ContainsKey(gameObject.name.Replace("(Clone)", "")))
			{
				ParticleSystem.MainModule exMain= exp.GetComponent<ParticleSystem>().main;
				Color[] col= getColors()[(string)gameObject.name.Replace("(Clone)", "")];
				exMain.startColor= new ParticleSystem.MinMaxGradient(col[0], col[1]);
			}

			renderSlices();

			Destroy(exp, 2);
			gameObject.SetActive(false);
			if(transform.parent != null)
			{
				GameObject parent= transform.parent.gameObject;
				if(parent!= null && parent.tag == "spawnPoint")
				{
					transform.parent = null;
					Destroy(parent);
				}
			}
		}

		if(other.tag == "blade")
		{
			breakIngredient();
		}
	}
}

