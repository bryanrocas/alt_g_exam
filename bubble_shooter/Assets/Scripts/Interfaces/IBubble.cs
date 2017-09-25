using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BubbleColor
{
	red,
	green,
	blue,
	yellow,
	white
}

public interface IBubble
{
	int ID{ get; }
	BubbleColor BubbleColor{ get; }

	void Init( int id , BubbleColor bubbleColor );
}
