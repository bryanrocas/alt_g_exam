using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game> 
{
	public float projectileSpeed = 10f;
	public float bubbleRadius = 1f;
	public int horBubbles = 6 ;
	public int verBubbles = 6 ;

	public GameObject bubblePrefab ;
}
