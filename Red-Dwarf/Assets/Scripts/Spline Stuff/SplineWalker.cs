using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;
    public GameObject ship;

    public bool lookForward;
    private bool keyhit = false;

    private float lerpTime = 30;
    private float currentLerpTime = 0;
    public float duration;
	public string[] type= {"Queen","General","Walker"};
	public int child=0; 
	public float kamikazeTime= 15f;
	public bool kamikaze;


	

	public SplineWalkerMode mode;

	private float progress;
	private bool goingForward = true;

	

	private void Update () {

		if(kamikaze)
			kamikazeTime -= Time.deltaTime;
		if (kamikaze && kamikazeTime < 0) {
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime >= lerpTime)
				currentLerpTime = lerpTime;
			float perc = currentLerpTime / lerpTime;
			transform.position = Vector3.Lerp (transform.position, ship.transform.position, perc);
		} else {


			if (goingForward) {
				progress += Time.deltaTime / duration;
				if (progress > 1f) {
					if (mode == SplineWalkerMode.Once) {
						progress = 1f;
					} else if (mode == SplineWalkerMode.Loop) {
						progress -= 1f;
					} else {
						progress = 2f - progress;
						goingForward = false;
					}
				}
			} else {
				progress -= Time.deltaTime / duration;
				if (progress < 0f) {
					progress = -progress;
					goingForward = true;
				}
			}

			Vector3 position = spline.GetPoint (progress);
			transform.localPosition = position;
			if (lookForward) {
				transform.LookAt (ship.transform.position);
			}
		}
	}
}