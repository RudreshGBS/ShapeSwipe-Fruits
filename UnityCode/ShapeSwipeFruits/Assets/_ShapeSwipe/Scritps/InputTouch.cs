
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

	/// <summary>
	/// Class in charge to listen the touch or click, and send event to subscribers
	/// </summary>
	public class InputTouch : MonoBehaviour 
	{
		/// <summary>
		/// Delegate to listen the touch down or click down, and send event to subscribers
		/// </summary>
		public delegate void OnTouchDown();
		public static event OnTouchDown OnTouchedDown;
		/// <summary>
		/// Delegate to listen the touch up or click up, and send event to subscribers
		/// </summary>
		public delegate void OnTouchUp();
		public static event OnTouchUp OnTouchedUp;

		/// <summary>
		/// Listening for inputs
		/// </summary>
		void Update()
		{
			if(!Application.isMobilePlatform)
			{

				if (Input.GetMouseButtonDown (0))
				{
					if(OnTouchedDown!=null)
						OnTouchedDown();

					return;
				} 
				else if (Input.GetMouseButtonUp (0))
				{
					if(OnTouchedUp!=null)
						OnTouchedUp();

					return;
				}

				return;
			}
			else if(Application.isMobilePlatform || Application.isEditor) 
			{
				int nbTouches = Input.touchCount;;

				if (nbTouches > 0) 
				{
					Touch touch = Input.GetTouch (0);

					TouchPhase phase = touch.phase;

					if (phase == TouchPhase.Began) 
					{
						if(OnTouchedDown!=null)
							OnTouchedDown();
					}
					else if(phase == TouchPhase.Ended)
					{
						if(OnTouchedUp!=null)
							OnTouchedUp();
					}
				}
			}
		}
	}

	/// <summary>
	/// 3 type of touch: left if on the left of the screen, right if on the right of the screen, or none
	/// </summary>
	public enum TouchDirection
	{
		none,
		left,
		right
	}
}