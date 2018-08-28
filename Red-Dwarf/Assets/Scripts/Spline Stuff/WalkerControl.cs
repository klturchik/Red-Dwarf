using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerControl : MonoBehaviour {
	readonly float spawntimer=15;

	public GameObject queenpre;
	public GameObject generalpre;
	public GameObject pawnpre;
	GameObject[] spline;
	public GameObject[] currentEnemies;
	bool[] splinecheck;
	int[] splineindex;
	int[] enemyType; // 0 for queen, 1 for Gen, 2 for Pawn;
	int maxEnemies;
	int currGen;
	int currQueen;
	int currPawn;
	public GameObject ship;
	GameObject[] test;
	GameObject[] generals;
	GameObject[] pawns;
	GameObject[] queen;


	private PlayerEvents playerEvents;
	int diff =1; 
	public int genCount =8;
	int wcount;
	bool startSpawn;
	float time;

	void Start(){
		wcount = genCount * 4;
		spline = GameObject.FindGameObjectsWithTag ("spline");
		maxEnemies = spline.GetLength (0);
		currentEnemies = new GameObject[maxEnemies];
		time= spawntimer;
		splinecheck = new bool[spline.GetLength(0)];
		splineindex = new int[spline.GetLength (0)];
		enemyType = new int[spline.GetLength (0)];
		CreateEnemies ();

	}
	// Update is called once per frame
	void Update ()  {
		time -= Time.deltaTime;
		if (startSpawn)
			checkSplines ();

		if (Input.GetKeyDown (KeyCode.C)) {
			DestroyEnemy (0);
			DestroyEnemy (1);
			DestroyEnemy (2);
			DestroyEnemy (3);
		}
		else if (Input.GetKeyDown (KeyCode.B)) {
			DestroyEnemy (4);
			DestroyEnemy (5);
			DestroyEnemy (6);
		}		
		else if (Input.GetKeyDown (KeyCode.D)) {

		}		
		if (time <0)
			time =spawntimer;

	} 

	public void DestroyEnemy(int splineid){
		currentEnemies [splineid] = null;
		if (enemyType [splineid] == 2) {
			Destroy (pawns [splineindex[splineid]]);
			splinecheck [splineid] = false;


		}
		else if (enemyType [splineid] == 1) {
			Destroy (generals [splineindex[splineid]]);
			splinecheck [splineid] = false;

		}
		else {
			Destroy (queen [splineindex[splineid]]);
			splinecheck [splineid] = false;

		}

	}

	void checkSplines()
	{


		if (time>spawntimer-1&& queen[0]!=null) {
			for(int x=0;x<spline.GetLength(0);x++)
			{

				if (currentEnemies [x] == null) {
					if( queen [currQueen].GetComponent<SplineWalker> ().child == 0) {
						if (x == 6) {
							queen [currQueen].GetComponent<SplineWalker> ().spline = spline [x].GetComponent<BezierSpline> ();
							currentEnemies [x] = queen [currQueen];
							splineindex [x] = currQueen;
							enemyType [x] = 0;
							queen [currQueen--].SetActive (true);
						}


					}
					else{
						if (generals [currGen].GetComponent<SplineWalker> ().child == 0) {
							generals[currGen].GetComponent<SplineWalker> ().spline = spline[x].GetComponent<BezierSpline> ();	
							splineindex [x] = currGen;
							currentEnemies [x] = generals [currGen];
							enemyType [x] = 1;
							queen [currQueen].GetComponent<SplineWalker> ().child--;
							generals [currGen--].SetActive (true);

						} else {
							pawns[currPawn].GetComponent<SplineWalker> ().spline = spline[x].GetComponent<BezierSpline> ();
							splineindex [x] = currPawn;
							currentEnemies [x] = pawns [currPawn];
							float rand = Random.value * 10 % 3;
							enemyType [x] = 2;
							if((int)rand==0)
								pawns [currPawn].GetComponent<SplineWalker> ().kamikaze = true;
							generals[currGen].GetComponent<SplineWalker>().child--;
							pawns [currPawn--].SetActive (true);
						}

					}

				}


				Wait (1);
				splinecheck[x]=true;


			} 
		}

		else if(queen[0]==null){
			diff++;
			CreateEnemies ();

		}
	}

	void CreateEnemies(){
		wcount = genCount * 4 * diff;
		currGen = genCount * diff;
		currPawn = wcount * diff;
		currQueen=diff-1;
		generals = new GameObject[currGen--];
		pawns = new GameObject[currPawn--];
		queen = new GameObject[diff];
		for (int x = 0; x < diff; x++) {
			queen[x] = Instantiate (queenpre);
			queen[x].GetComponent<SplineWalker> ().spline = spline[0].GetComponent<BezierSpline> ();
			queen[x].GetComponent<SplineWalker> ().ship = ship;
			queen[x].GetComponent<SplineWalker> ().child = genCount/diff;
		}

		for (int x = 0; x < wcount*diff; x++) {
			pawns [x] = Instantiate (pawnpre);
			pawns[x].GetComponent<SplineWalker>().spline= spline[1].GetComponent<BezierSpline>();
			pawns [x].GetComponent<SplineWalker> ().ship = ship;

		}

		for (int x = 0; x < genCount*diff; x++) {
			int pcount = wcount;
			generals [x] = Instantiate (generalpre);
			generals[x].GetComponent<SplineWalker> ().ship = ship;
			if (pcount > 4) {	
				generals [x].GetComponent<SplineWalker> ().child = 4;
				pcount -= 4;
			} else {
				generals[x].GetComponent< SplineWalker>().child=pcount;

			}


			wcount -= generals [x].GetComponent<SplineWalker> ().child;
			generals[x].GetComponent<SplineWalker>().spline= spline[2].GetComponent<BezierSpline>();


		}
	}
	void OnEnable()
	{
		

		playerEvents = ship.GetComponent<PlayerEvents>();
		initListeners(true);
	}

	void OnDisable()
	{
		initListeners(false);
	}


	private void initListeners(bool state)
	{
		if (state)
		{
			playerEvents.OnSpawn += SpawnStart;
		}
		else
		{
			playerEvents.OnSpawn -= SpawnStart;
		}
	}

	void SpawnStart()
	{
		startSpawn=true;
	}

	IEnumerable Wait(float l)
	{
		yield return new WaitForSeconds (l);
	}
}
