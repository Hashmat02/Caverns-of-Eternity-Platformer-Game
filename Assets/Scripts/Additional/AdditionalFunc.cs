using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;

public class Helpers {
	static public IEnumerator wait(float seconds) {
		yield return new WaitForSeconds(seconds);
	}

    static public T[] extendArray<T>(T[] arr1, T[] arr2) {
        T[] outArr = new T[arr1.Length + arr2.Length];
        arr1.CopyTo(outArr, 0);
        arr2.CopyTo(outArr, arr1.Length);
        return outArr;
    }
}

public class ErrorHandling {
	static public void logError(
		string error, 
		[CallerLineNumber] int lineNumber = 0,
		[CallerMemberName] string funcName = null,
		[CallerFilePath] string filePath = null
	) {
		Debug.LogError($"Error: {error} at  {filePath}:{lineNumber} (call: {funcName})");
	}
}