using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BubbleColor
{
	red=0,
	green=1,
	blue=2,
	yellow=3,
	white=4
}

public interface IBubble
{
	int ID{ get; }
	BubbleColor BubbleColor{ get; }

	void Init( int id , BubbleColor bubbleColor );
}
