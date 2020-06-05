using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starFxController : MonoBehaviour {

	public GameObject[] starFX;
	public int ea;
	public int currentEa;
	public float delay;
	public float currentDelay;
	public bool isEnd;
	public int idStar;
	public static starFxController myStarFxController;

	void Awake () {
		myStarFxController = this;
	}

	void Start ()
    {
        if (isEnd)
        {
            for(int i=0; i<starFX.Length; i++)
            {
                starFX[i].SetActive(true);
            }
        }
        else
        {
            Reset();
        }
	}

	void Update () {
		if (!isEnd) {
			currentDelay -= Time.deltaTime;
			if (currentDelay <= 0) {
				if (currentEa != ea) {
					currentDelay = delay;
					starFX [currentEa].SetActive (true);
					currentEa++;
                    MainData.instance.AddStar();

                  
                } else {
					isEnd = true;
					currentDelay = delay;
					currentEa = 0;
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			Reset ();
		}
	}

	public void Reset () {
		for (int i = 0; i < 3; i++) {
			starFX [i].SetActive (false);
		}
		currentDelay = delay;
		currentEa = 0;
		isEnd = false;
		for (int i = 0; i < 3; i++) {
			starFX [i].SetActive (false);
		}
	}
}