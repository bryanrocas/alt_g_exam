public static class Debugger
{
	public static void Debug( this UnityEngine.MonoBehaviour mono, object log )
	{
		#if DEBUG
		UnityEngine.Debug.Log( mono +": "+ log );
		#endif
	}
}