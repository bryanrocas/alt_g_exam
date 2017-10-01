using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBubbleDetect
{
	List<IBubble> GetMatchingNeighbors();
	void NotifyOtherNeighbors();
	void RegisterSelf();
	void RegisterNeighbors();
	void CheckAnchors();
}

