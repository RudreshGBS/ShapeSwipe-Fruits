
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

#if AADOTWEEN

using DG.Tweening;

#endif

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

namespace AppAdvisory.ShapeSwipe
{
	/// <summary>
	/// Utility class.
	/// </summary>
	public static class Util
	{
		public const string bestScore = "BEST_SCORE";
		public const string lastScore = "LAST_SCORE";

		static System.Random random = new System.Random();

		public static double GetRandomNumber(double minimum, double maximum)
		{ 
			return random.NextDouble() * (maximum - minimum) + minimum;
		}

		public static float GetRandomNumber(float minimum, float maximum)
		{ 
			return (float)random.NextDouble() * (maximum - minimum) + minimum;
		}
		/// <summary>
		/// Real shuffle of List
		/// </summary>
		public static void Shuffle<T>(this IList<T> list)  
		{  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = random.Next(n + 1);  
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}  
		}
		/// <summary>
		/// Set the last score, and eventually save a new best score in rhe PlayerPrefs.
		/// </summary>
		public static void SetLastScore(int score)
		{
			PlayerPrefs.SetInt(lastScore,score);

			int b = GetBestScore();

			if(score > b)
			{
				PlayerPrefs.SetInt(bestScore,score);
			}

			PlayerPrefs.Save();
		}
		/// <summary>
		/// Get best score from PlayerPrefs.
		/// </summary>
		public static int GetBestScore()
		{
			return PlayerPrefs.GetInt(bestScore,0);
		}
		/// <summary>
		/// Get best score from PlayerPrefs.
		/// </summary>
		public static int GetLastScore()
		{
			return PlayerPrefs.GetInt(lastScore,0);
		}
		/// <summary>
		/// Clean the memory and reload the scene
		/// </summary>
		public static void ReloadLevel()
		{
			CleanMemory();

			#if UNITY_5_3_OR_NEWER
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
			#else
			Application.LoadLevel(Application.loadedLevel);
			#endif

			CleanMemory();
		}
		/// <summary>
		/// Clean the memory
		/// </summary>
		public static void CleanMemory()
		{
			#if AADOTWEEN

			DOTween.KillAll();

			#endif

			GC.Collect();
			if(Application.isMobilePlatform)
			{
				Application.targetFrameRate = 60;
				Time.fixedDeltaTime = 1f/60f;
				Time.maximumDeltaTime = 3 * Time.fixedDeltaTime;
			}
			else
			{
				Application.targetFrameRate = 100;
				Time.fixedDeltaTime = 1f/100f;
				Time.maximumDeltaTime = 3 * Time.fixedDeltaTime;
			}
			GC.Collect();
		}
		/// <summary>
		/// Set alpha of UI Text element.
		/// </summary>
		public static void SetAlpha(this Text text, float alpha)
		{
			Color c = text.color;
			c.a = alpha;
			text.color = c;
		}
		/// <summary>
		/// Set scale X of RectTransform element.
		/// </summary>
		public static void SetScaleX(this RectTransform rect, float scale)
		{
			var s = rect.localScale;
			s.x = scale;
			rect.localScale = s;
		}

		public static float GetOrthoMainCamWidth()
		{
			return GetOrthoMainCamHeight() * Camera.main.aspect;
		}

		public static float GetOrthoMainCamHeight()
		{
			Camera cam = Camera.main;
			return 2f * cam.orthographicSize;
		}
	}
}

