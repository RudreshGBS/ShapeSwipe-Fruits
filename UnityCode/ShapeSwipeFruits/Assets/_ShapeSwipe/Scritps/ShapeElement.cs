
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


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


