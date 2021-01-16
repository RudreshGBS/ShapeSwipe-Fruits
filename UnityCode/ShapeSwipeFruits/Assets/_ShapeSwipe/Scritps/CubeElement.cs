
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

#if AADOTWEEN

using DG.Tweening;

#endif

	public class CubeElement : MonoBehaviour 
	{

		public bool isRightCube;

		public GameObject SPHERE;
		public GameObject RECTANGLE;
		public GameObject TRIANGLE;

		public GameObject[] trace1;
		public GameObject[] trace2;

		bool isScale = true;

		float timeAnimCubePosition = 0.3f;
		float timeAnimCubeScale = 0.3f;

		Renderer currentRenderer
		{
			get
			{
				if(SPHERE.activeInHierarchy)
					return SPHERE.GetComponent<Renderer>();
				if(RECTANGLE.activeInHierarchy)
					return RECTANGLE.GetComponent<Renderer>();

				return TRIANGLE.GetComponent<Renderer>();
			}
		}

		Material currentMaterial
		{
			get
			{
				return currentRenderer.material;
			}
		}

		public float decalY = 2;

		public SHAPE shape;

		Player player;
		GameManager gameManager;

		void Awake()
		{
			DODesactivateShapes();

			player = FindObjectOfType<Player>();
			gameManager = FindObjectOfType<GameManager>();

			GetComponent<Collider>().enabled = false;

			foreach(var go in trace1)
			{
				go.SetActive(false);
			}

			foreach(var go in trace2)
			{
				go.SetActive(false);
			}
		}

		public void ActivateTrace(int num, bool isTrace1)
		{
			if(isTrace1)
				trace1[num].SetActive(true);
			else
				trace2[num].SetActive(true);
		}

		void DODesactivateShapes()
		{
			SPHERE.SetActive(false);
			RECTANGLE.SetActive(false);
			TRIANGLE.SetActive(false);
		}

		public void DOShapeSelected()
		{
			GetComponent<Collider>().enabled = false;

			//			if(SPHERE.activeInHierarchy)
			//				SPHERE.GetComponent<Renderer>().material.color = gameManager.goodShapeColor;
			//			if(RECTANGLE.activeInHierarchy)
			//				RECTANGLE.GetComponent<Renderer>().material.color = gameManager.goodShapeColor;
			//			if(TRIANGLE.activeInHierarchy)
			//				TRIANGLE.GetComponent<Renderer>().material.color = gameManager.goodShapeColor;

			currentMaterial.color = gameManager.goodShapeColor;
		}

		public void activateElement()
		{
			float rand = Util.GetRandomNumber(0,100);

			if(rand < 33)
				SPHERE.SetActive(true);
			else if (rand < 66)
				RECTANGLE.SetActive(true);
			else
				TRIANGLE.SetActive(true);

			GetComponent<Collider>().enabled = true;

			currentMaterial.color = gameManager.GetRandomShapeColor();
		}

		public void DORotateElement()
		{
		   RotateElement(SPHERE.transform);
		   RotateElement(RECTANGLE.transform);
		   RotateElement(TRIANGLE.transform);
		}
		private void RotateElement(Transform shape) {
			var e = shape.eulerAngles;
			e.y = 90;
			shape.eulerAngles = e;
		}

		void DOAnimIn(Action isCompleted)
		{

			#if AADOTWEEN

			if(isScale)
			{

				transform.localScale = Vector3.zero;
				transform.DOScale(Vector3.one, timeAnimCubeScale)
					.OnComplete(() => {

						if(isCompleted != null)
							isCompleted();

					});


			}
			Vector3 finalPos = transform.position;
			transform.position = new Vector3(finalPos.x, finalPos.y + decalY, finalPos.z);

			transform.DOMove(finalPos, timeAnimCubePosition)
				.OnComplete(() => {

					if(isCompleted != null)
						isCompleted();
				});

			return;
			#endif

			if(isCompleted != null)
				isCompleted();
		}

		void DOAnimOut(Action isCompleted)
		{
			#if AADOTWEEN

			if(isScale)
			{

				transform.DOScale(Vector3.zero, timeAnimCubeScale)
					.OnComplete(() => {


						if(isCompleted != null)
							isCompleted();


					});


			}


			transform.DOMove(new Vector3(transform.position.x, transform.position.y - decalY, transform.position.z), timeAnimCubePosition)
				.OnComplete(() => {


					if(isCompleted != null)
						isCompleted();


				});

			return;
			#endif

			if(isCompleted != null)
				isCompleted();
		}

		void Start()
		{
			DOAnimIn(null);
		}

		public void DOAnimOut(float waitTIme)
		{
			#if AADOTWEEN

			DOVirtual.DelayedCall(waitTIme, () => {


				DOAnimOut(() => {
					Destroy(gameObject);
				});


			});
			#endif


		}
	}


