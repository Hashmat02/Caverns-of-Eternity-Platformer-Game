using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;

public static class ErrorHandling {
	public static void logError(
		string error, 
		[CallerLineNumber] int lineNumber = 0,
		[CallerMemberName] string funcName = null,
		[CallerFilePath] string filePath = null
	) {
		Debug.LogError($"Error: {error} at  {filePath}:{lineNumber} (call: {funcName})");
	}
}
