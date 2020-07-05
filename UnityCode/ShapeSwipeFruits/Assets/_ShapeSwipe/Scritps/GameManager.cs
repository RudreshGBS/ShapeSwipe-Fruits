
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
#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif
#if VS_SHARE
using AppAdvisory.SharingSystem;
#endif
using AppAdvisory.UI;
#if APPADVISORY_LEADERBOARD
using AppAdvisory.social;
#endif

	public class GameManager : MonoBehaviour 
	{
		public Transform cubePrefab;

		public int numMaxOfCubeInScene = 10; //10
		public int minNumOfCubeInLine = 4; //4
		public int maxNumOfCubeInLine = 7; //7

		public float timeToMoveOnOneBlock = 0.15f;

		public int numMaxOfCube = 5;

		public Color goodShapeColor;
		public Color playerColor;
		public Color[] shapeColors;

		public Text textPoint;

		public AudioClip FX_Start;
		public AudioClip FX_DoorMiss;
		public AudioClip FX_DoorOK;
		public AudioClip FX_Reward;
		public AudioClip FX_NewCharacter;
		public AudioSource audioSource;
	public UIController uiController;
		public void PlaySoundFX_Start()
		{
			audioSource.PlayOneShot(FX_Start);
		}
		public void PlaySoundFX_DoorMiss()
		{
			audioSource.PlayOneShot(FX_DoorMiss);
		}
		public void PlaySoundFX_DoorOK()
		{
			audioSource.PlayOneShot(FX_DoorOK);
		}
		public void PlaySoundFX_Reward()
		{
			audioSource.PlayOneShot(FX_Reward);
		}
		public void PlaySoundFX_NewCharacter()
		{
			audioSource.PlayOneShot(FX_NewCharacter);
		}

		[SerializeField] public int m_point;
		public int point
		{
			get
			{
				return m_point;
			}
			set
			{
				if(textPoint == null)
					textPoint = uiController.scoreIngame;
				
				m_point = value;

				textPoint.text = "" + m_point;

				if(value > 0)
					PlaySoundFX_DoorOK();
			}
		}

	
		public Color GetRandomShapeColor()
		{
			return shapeColors[UnityEngine.Random.Range(0, shapeColors.Length)];
		}

		int GetNumberOfCubeInTheScene()
		{
			var allCubes = FindObjectsOfType<CubeElement>();
			return allCubes.Length;
		}

		Player player;

		List<CubeElement> nextTargets;

		CubeElement InstantiateCube(int x, int z)
		{
			var t = Instantiate(cubePrefab) as Transform;
			var c = t.GetComponent<CubeElement>();
			t.position = new Vector3(-x, -1, -z);
			return c;
		}

		int x = 0;
		int z = 0;

		void Awake()
		{
			nextTargets = new List<CubeElement>();
			player = FindObjectOfType<Player>();

			player.DODestactivate();

			Util.CleanMemory();

			#if AADOTWEEN
			var go = GameObject.Find("[Dotween]");


			if(go == null)
				DOTween.Init();

			#endif


			point = 0;

			audioSource = GetComponent<AudioSource>();

		    uiController.SetBestText(Util.GetBestScore());
		    uiController.SetLastText(Util.GetLastScore());
		}

		void Start()
		{
			StartCoroutine(CreateWorld());
		}

		public void DOGameOver()
		{
			Util.SetLastScore(point);

		    uiController.SetBestText(Util.GetBestScore());
		    uiController.SetLastText(Util.GetLastScore());

			ShowAds();

			#if VS_SHARE
			VSSHARE.DOTakeScreenShot();
			#endif

			#if APPADVISORY_LEADERBOARD
			LeaderboardManager.ReportScore(point);
			#endif

			InputTouch.OnTouchedDown -= OnTouchedDown;

			#if AADOTWEEN

			DOTween.KillAll();

			#endif

			PlaySoundFX_DoorMiss();

			#if AADOTWEEN

			Camera.main.DOOrthoSize(Camera.main.orthographicSize * 0.9f, 0.1f)
				.SetLoops(5, LoopType.Yoyo)
				.OnComplete(() => {
					Camera.main.DOOrthoSize(Camera.main.orthographicSize * 0.01f, 1f)
						.OnComplete(() => {
							Util.ReloadLevel();
						});
			});

			#endif

		}

		void OnEnable()
		{
			InputTouch.OnTouchedDown -= OnTouchedDown;
			InputTouch.OnTouchedDown += OnTouchedDown;
		}

		void OnDisable()
		{
			InputTouch.OnTouchedDown -= OnTouchedDown;
		}

		void OnTouchedDown ()
		{
			player.DOShape();
		}

		public void OnClickedStart()
		{

			#if VS_SHARE
			VSSHARE.DOHideScreenshotIcon();
			#endif

			PlaySoundFX_Start();

			player.EnableShadows(false);

			player.DOActivate();

			Camera.main.transform.parent = null;

			var savePos = player.transform.position;

			player.transform.position = savePos + Vector3.up * 10;

			#if AADOTWEEN

			player.transform.DOMove(savePos, 1)
				.SetEase(Ease.OutBounce)
				.OnComplete(() => {
					Camera.main.transform.parent = player.transform;
					DOPlayerMove();

					player.EnableShadows(true);

				});

			#endif

		}

		void DOPlayerMove()
		{
			#if AADOTWEEN

			DOTween.Kill(player.transform);



			var target = new Vector3(nextTargets[0].transform.position.x, 0, nextTargets[0].transform.position.z);

			player.transform.DOMove(target, timeToMoveOnOneBlock)
				.SetEase(Ease.Linear)

				.OnComplete(() => {
					nextTargets[0].DOAnimOut(timeToMoveOnOneBlock);
					nextTargets.RemoveAt(0);
					DOPlayerMove();
				});

			#endif

		}

		IEnumerator CreateWorld()
		{
			bool firstActivation = true;

			while(true)
			{
				int stopZ = UnityEngine.Random.Range(minNumOfCubeInLine, maxNumOfCubeInLine) + z;

				int activateElementAtPositionZ = UnityEngine.Random.Range(z + 1, stopZ - 2);

				CubeElement c = null;

				while(z <= stopZ)
				{
					while(GetNumberOfCubeInTheScene() > numMaxOfCubeInScene)
					{
						yield return 0;
					}

					c = InstantiateCube(x,z);

					if(z == activateElementAtPositionZ)
					{
						if(firstActivation)
						{
							firstActivation = false;
						}
						else
						{
							c.activateElement();
							c.DORotateElement();
						}
					}

					nextTargets.Add(c);

					c.isRightCube = true;

					z++;

					yield return new WaitForSeconds(timeToMoveOnOneBlock);
				}

				int stopX = UnityEngine.Random.Range(5,10) + x;

				int activateElementAtPositionX = UnityEngine.Random.Range(x + 2, stopX - 2);

				while(x <= stopX)
				{

					while(GetNumberOfCubeInTheScene() > numMaxOfCubeInScene)
					{
						yield return 0;
					}

					c = InstantiateCube(x,z);

					if(x == activateElementAtPositionX)
					{
						c.activateElement();
					}

					nextTargets.Add(c);

					c.isRightCube = false;

					x++;

					yield return new WaitForSeconds(timeToMoveOnOneBlock);
				}
			}
		}

		public int numberOfPlayToShowInterstitial = 5;

		public string VerySimpleAdsURL = "http://u3d.as/oWD";

		/// <summary>
		/// If using Very Simple Ads by App Advisory, show an interstitial if number of play > numberOfPlayToShowInterstitial: http://u3d.as/oWD
		/// </summary>
		public void ShowAds()
		{
			int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
			count++;
			PlayerPrefs.SetInt("GAMEOVER_COUNT",count);
			PlayerPrefs.Save();

			#if APPADVISORY_ADS
			if(count > numberOfPlayToShowInterstitial)
			{
				print("count = " + count + " > numberOfPlayToShowINterstitial = " + numberOfPlayToShowInterstitial);

				if(AdsManager.instance.IsReadyInterstitial())
				{
					print("AdsManager.instance.IsReadyInterstitial() == true ----> SO ====> set count = 0 AND show interstial");
					AdsManager.instance.ShowInterstitial();
					PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
				}
				else
				{
			#if UNITY_EDITOR
					print("AdsManager.instance.IsReadyInterstitial() == false");
			#endif
				}

			}
			else
			{
				PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
			#else
			if(count >= numberOfPlayToShowInterstitial)
			{
			Debug.LogWarning("To show ads, please have a look to Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
			Debug.LogWarning("Very Simple Ad is already implemented in this asset");
			Debug.LogWarning("Just import the package and you are ready to use it and monetize your game!");
			Debug.LogWarning("Very Simple Ad : " + VerySimpleAdsURL);
			PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
			}
			else
			{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
			#endif
		}

	}



