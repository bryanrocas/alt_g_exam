using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour 
{
	private static T manager;
	public static T Manager
	{
		get
		{
			// referencing blocker in the event where the instance is called while OnDestroy
			if( applicationIsQuitting )
			{
				return null ;
			}
				
			if(!manager)
			{
				manager = (T)FindObjectOfType(typeof(T));
			}

			return manager;
		}
	}

	private static bool _isInitialized;
	private static bool applicationIsQuitting = false ;

	[SerializeField] protected bool isDestroyable = false ;

	protected virtual void Awake()
	{
		if(manager == null) manager = this as T;

		if( !isDestroyable ) DontDestroyOnLoad(this.gameObject);

		_isInitialized = true;
	}

	protected virtual void OnDestroy()
	{
		applicationIsQuitting = true ;
	}
}
