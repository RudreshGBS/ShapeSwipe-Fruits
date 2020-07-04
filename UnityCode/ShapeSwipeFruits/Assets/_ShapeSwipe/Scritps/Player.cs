
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


namespace AppAdvisory.ShapeSwipe
{
	public class Player : MonoBehaviour 
	{
		public SHAPE shape;

		public GameObject[] shapes;

		int count = 0;

		public GameObject[] shadows;
		public void EnableShadows(bool _enable)
		{
			foreach(var go in shadows)
			{
				go.SetActive(_enable);
			}
		}

		GameManager gameManager;

		void Awake()
		{
			gameManager = FindObjectOfType<GameManager>();
	
			DisableAll();

			foreach(GameObject obj in shapes)
			{
				obj.GetComponent<Renderer>().material.color = gameManager.playerColor;
			}
		}

		public void DODestactivate()
		{
			DisableAll();
		}

		public void DOActivate()
		{
			DisableAll();
			count = 0;
			shapes[count].SetActive(true);
		}

		void DisableAll()
		{
			foreach(GameObject obj in shapes)
			{
				obj.SetActive(false);
			}
		}

		public void DOShape()
		{
			DisableAll();

			count ++;

			if(count >= shapes.Length)
			{
				count = 0;
			}

			shapes[count].SetActive(true);

		}

		void OnTriggerEnter(Collider other) 
		{
			CubeElement cubeElement = other.GetComponent<CubeElement>();

			if(cubeElement.shape == this.shape)
			{
				cubeElement.DOShapeSelected();
				gameManager.point++;
			}
			else
			{
				gameManager.DOGameOver();
			}
		}
	}
}

