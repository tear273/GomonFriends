using UnityEngine;

namespace AUP{
	public class SensorUtils{
		public static void Message(string tag, string message){
			Debug.LogWarning(tag + message);
		}
	}
}