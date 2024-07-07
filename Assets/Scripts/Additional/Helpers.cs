using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers {
	public static IEnumerator wait(float seconds) {
		yield return new WaitForSeconds(seconds);
	}

	public static T[] populateArray<T>(T[] arr, T value) {
		for (int i = 0; i < arr.Length; i++) {
			arr[i] = value;
		}
		return arr;
	}

    public static T[] extendArray<T>(T[] arr1, T[] arr2) {
        T[] outArr = new T[arr1.Length + arr2.Length];
        arr1.CopyTo(outArr, 0);
        arr2.CopyTo(outArr, arr1.Length);
        return outArr;
    }
	public static string GetArgs(string name) {
		string[] args = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < args.Length; i++) {
			if (args[i] == name && args.Length > i + 1) {
				return args[i + 1];
			}
		}
		return null;
	}
}
