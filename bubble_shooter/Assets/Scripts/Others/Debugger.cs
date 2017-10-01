public static class Debugger
{
	public static void Debug( this UnityEngine.MonoBehaviour mono, object log )
	{
		#if DEBUGGER
		UnityEngine.Debug.Log( mono +": "+ log );
		#endif
	}

	public static void DebugError( this UnityEngine.MonoBehaviour mono, object log )
	{
		#if DEBUGGER
		UnityEngine.Debug.LogError( mono +": "+ log );
		#endif
	}
}