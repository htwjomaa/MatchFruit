
// Just a couple Mathfunctions that (could) get used 

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public static class MathLibrary 
{
	// Gebe a und b rein und du kriegst die Prozente eines lerps // (t value)// v ist in dieser range
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	static float InverseLerp( float a, float b, float v ) => (v - a) / (b - a);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Remap( float iMin, float iMax, Vector3 oMin, Vector3 oMax, float v ) 
	{ // Remaps eine komplette Value range zu einer anderen. Wie beim shader. Inverslep und lerp
		float x = Remap(iMin,  iMax, oMin.x, oMax.x,v);
		float y = Remap(iMin,  iMax, oMin.y, oMax.y,v);
		float z = Remap(iMin,  iMax, oMin.z, oMax.z,v);
		return new Vector3(x, y, z);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Remap( float iMin, float iMax, float oMin, float oMax, float v ) 
	{ // Remaps eine komplette Value range zu einer anderen. Wie beim shader. Inverslep und lerp
		float t = InverseLerp(iMin, iMax, v);
		return Mathf.LerpUnclamped( oMin, oMax, t );
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float RemapClamped( float iMin, float iMax, float oMin, float oMax, float v ) 
	{ // positive only
		float t = InverseLerp(iMin, iMax, v);
		return Mathf.Lerp( oMin, oMax, t );
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float CalculateDistance(GameObject Player, GameObject OtherObject)
	{
		Vector3 Verbindungsvector = Player.transform.position - OtherObject.transform.position;
		return Mathf.Sqrt(Mathf.Pow(Verbindungsvector.x, 2) + Mathf.Pow(Verbindungsvector.y, 2) + Mathf.Pow(Verbindungsvector.z, 2));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float CalculateDistance(Transform Player, Transform OtherObject)
	{
		Vector3 Verbindungsvector = Player.transform.position - OtherObject.transform.position;
		return Mathf.Sqrt(Mathf.Pow(Verbindungsvector.x, 2) + Mathf.Pow(Verbindungsvector.y, 2) + Mathf.Pow(Verbindungsvector.z, 2));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float CalculateDistanceSquared(GameObject Player, GameObject OtherObject)
	{
		Vector3 Verbindungsvector = Player.transform.position - OtherObject.transform.position;
		return Mathf.Pow(Verbindungsvector.x, 2) + Mathf.Pow(Verbindungsvector.y, 2) + Mathf.Pow(Verbindungsvector.z, 2);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float CalculateDistance(Vector3 Player, Vector3  OtherObject)
	{
		Vector3 Verbindungsvector = Player - OtherObject;
		return Mathf.Sqrt(Mathf.Pow(Verbindungsvector.x, 2) + Mathf.Pow(Verbindungsvector.y, 2) + Mathf.Pow(Verbindungsvector.z, 2));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float CalculateDistance(Vector2 Player, Vector2  OtherObject)
	{
		Vector3 PlayerVec3 = new Vector3(Player.x, Player.y, 1);
		Vector3 OtherObjectVec3 = new Vector3(OtherObject.x, OtherObject.y, 1);
		return CalculateDistance(PlayerVec3, OtherObjectVec3);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float CalculateDistanceSquared(Vector3 Player, Vector3  OtherObject)
	{
		Vector3 Verbindungsvector = Player - OtherObject;
	
		return Mathf.Pow(Verbindungsvector.x, 2) + Mathf.Pow(Verbindungsvector.y, 2) + Mathf.Pow(Verbindungsvector.z, 2);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float sqrMagnitudeInlined(Vector3 v) => v.x * v.x + v.y * v.y + v.z * v.z;

	public static void boostDirection(Vector3 pointA, Vector3 pointB, float forceAmount, Rigidbody rb)
	{
		var aToB = pointB - pointA;
		var aToBDir = aToB.normalized;
		aToBDir = new Vector3(aToBDir.x, 0, aToBDir.z);
		rb.AddForce(aToBDir * forceAmount, ForceMode.Impulse);
	}

	public static Vector3 FindTheMidPoint(List<Vector3> posExtraSpaces, params GameObject[] objects)
	{
		float xPos = 0, yPos = 0, zPos = 0, objCounter = 0;
		foreach (GameObject Object in objects)
		{
			xPos += Object.transform.position.x;
			yPos += Object.transform.position.y;
			zPos += Object.transform.position.z;
			objCounter++;
		}
		foreach (Vector3 vec3Pos in posExtraSpaces)
		{
			xPos += vec3Pos.x;
			yPos += vec3Pos.y;
			zPos += vec3Pos.z;
			objCounter++;
		}
		xPos /= objCounter; yPos /= objCounter; zPos /= objCounter;
		return new(xPos, yPos, zPos);
	}
	
	public static Vector3 FindTheMidPoint(params GameObject[] objects)
	{
		float xPos = 0, yPos = 0, zPos = 0, objCounter = 0;
		foreach (GameObject Object in objects)
		{
			xPos += Object.transform.position.x;
			yPos += Object.transform.position.y;
			zPos += Object.transform.position.z;
			objCounter++;
		}
		xPos /= objCounter; yPos /= objCounter; zPos /= objCounter;
		return new(xPos, yPos, zPos);
	}
	public static Vector3 FindTheMidPoint(params Vector3[] pos)
	{
		float xPos = 0, yPos = 0, zPos = 0, objCounter = 0;
		foreach (Vector3 vec3Pos in pos)
		{
			xPos += vec3Pos.x;
			yPos += vec3Pos.y;
			zPos += vec3Pos.z;
			objCounter++;
		}
		xPos /= objCounter; yPos /= objCounter; zPos /= objCounter;
		return new(xPos, yPos, zPos);
	}
	public static string TrimToFirstDigitA(float number)//nur bis 9999
	{
		string firstDigit = number.ToString().Remove(1, number.ToString().Length - 1);

		return firstDigit;
	}

	/*
	public static string RemoveDigits(float number, float removeAmount) 
	{
		string digits = "";

		string progress = number.ToString();



		int charAmount = progress.ToCharArray().Length;

		for (int i = 1; i < charAmount-removeAmount+1; i++)
		{
			digits = progress.Remove(progress.Length - 1, 1);
			Debug.Log("Removeing " + i);
		}


		return digits;
	}*/
}
