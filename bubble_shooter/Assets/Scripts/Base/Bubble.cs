using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Bubble : MonoBehaviour, IBubble
{
	[SerializeField] int id;
	[SerializeField] BubbleColor bubbleColor;

	#region IBubble implementation
	public int ID 
	{
		get 
		{
			return id;
		}

		protected set
		{
			id = value;
		}
	}
	public BubbleColor BubbleColor 
	{
		get 
		{
			return bubbleColor;
		}

		protected set
		{
			bubbleColor = value;
		}
	}

	public void Init (int id, BubbleColor bubbleColor)
	{
		ID = id;
		BubbleColor = bubbleColor;
	}
	#endregion
}
