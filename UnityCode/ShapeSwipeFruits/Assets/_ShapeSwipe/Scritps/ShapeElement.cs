
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
	public class ShapeElement : MonoBehaviour 
	{
		public SHAPE shape;

		Player player;

		CubeElement cube;

		void Awake()
		{
			player = GetComponentInParent<Player>();
			cube = GetComponentInParent<CubeElement>();
		}

		void OnEnable()
		{
			if(player != null)
				player.shape = this.shape;

			if(cube != null)
				cube.shape = this.shape;
		}
	}
}

