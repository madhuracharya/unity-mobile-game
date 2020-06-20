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
	[SerializeField] private GameObject ingredientGhost;

	void Start()
	{
		rb= GetComponent<Rigidbody2D>();
		canvas= GameObject.Find("Canvas");
		//rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
		revSpeed= Random.Range(-50.0f, 50f);
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

	public void renderGhost()
	{
		GameObject slotParent = GameObject.Find("slotParent");
		foreach (Transform child in slotParent.transform)
		{
			if(child.GetComponent<Alias>().alias == gameObject.GetComponent<Alias>().alias && child.GetChild(2).gameObject.activeSelf == false)
			{
				GameObject ingGhost = Instantiate(ingredientGhost) as GameObject;
				ingGhost.transform.SetParent(canvas.transform, false);
				Vector3 pos= Camera.main.WorldToScreenPoint(gameObject.transform.position);
				ingGhost.transform.position = pos;
				ingredientGhost ingGhostScript= ingGhost.GetComponent<ingredientGhost>();
				ingGhostScript.alias= gameObject.GetComponent<Alias>().alias;
				ingGhostScript.lookAt= child;
				ingGhostScript.ingredient= gameObject;

				Image img= ingGhost.gameObject.GetComponent<Image>();
				img.sprite= gameObject.GetComponent<SpriteRenderer>().sprite;
				img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

				break;
			}
		}
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

			if(sliceBack && sliceFront)
			{
				GameObject sl1= Instantiate(sliceBack, transform.position + new Vector3(-0.5f, 0, 0), transform.rotation);	
				GameObject sl2= Instantiate(sliceFront, transform.position + new Vector3(0.5f, 0, 0), transform.rotation);
				Rigidbody2D srb1= sl1.GetComponent<Rigidbody2D>();
				Rigidbody2D srb2= sl2.GetComponent<Rigidbody2D>();

				/*srb1.AddForce(-other.transform.right * 5, ForceMode2D.Impulse);
				srb2.AddForce(other.transform.right * 5, ForceMode2D.Impulse);*/
				srb1.AddForce(-transform.right * 5, ForceMode2D.Impulse);
				srb2.AddForce(transform.right * 5, ForceMode2D.Impulse);
				if(sl1) Destroy(sl1, 5);
				if(sl2) Destroy(sl2, 5);
			}

			renderGhost();

			Destroy(exp, 2);
			gameObject.SetActive(false);
			GameObject parent= transform.parent.gameObject;
			if(parent!= null && parent.tag == "spawnPoint")
			{
				transform.parent = null;
				Destroy(parent);
			}
		}

		if(other.tag == "blade")
		{
			breakIngredient();
		}
	}
}

