using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBubbleDetect
{
	List<IBubble> GetNeighbors ();
	List<IBubble> GetMatchingNeighbors();
	List<IBubble> GetOtherNeighbors ();
	void NotifyOtherNeighbors();
	void RegisterSelf();
	void RegisterNeighbors();
	void CheckAnchors();
}

