using System.Collections.Generic;
using UnityEngine;
public class WorldCurvesLengthTable 
{// Dise Klasse fixed die Uvs. Das ist der UVFixed Mode
    private float[] distances; 
    int SmpCount => distances.Length;
    float TotalLength => distances[SmpCount - 1];
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> ExtraPoints = new List<Vector3>();
    public WorldCurvesLengthTable(CatmullRom.CatmullRomPoint[] catmullRomPoints, int precision = 32)
    {
        if (points == null) points = new List<Vector3>();
        else points.Clear();
        
        for (int i = 0; i < catmullRomPoints.Length; i++)
        {
            points.Add(catmullRomPoints[i].position);
        }

        CatmullRom catmullRom = new CatmullRom(points.ToArray(), 100, false);
        var morePoints= catmullRom.GenerateSplinePoints();
        
        for(int i = 0; i < morePoints.Length;i++)
            ExtraPoints.Add(morePoints[i].position);
        
        // Längentabelle generieren
        distances = new float[precision];
        Vector3 prevPoint = catmullRomPoints[0].position;
        distances[0] = 0f;
        for( int i = 1; i < precision; i++ ) 
        {
            float t = i / (precision - 1f);
            Vector3 currentPoint = CatmullRom.GetPointFromT(t, catmullRomPoints).position;
            float delta = (prevPoint-currentPoint).magnitude;
            distances[i] = distances[i - 1] + delta;
            prevPoint = currentPoint;
        }
    }
    public float ToPercentage( float t ) 
    { 	// Umrechnung des t-Werts in Prozent des Abstands entlang der Kurve
        float iFloat = t * (SmpCount-1);
        int idLower = Mathf.FloorToInt(iFloat);
        int idUpper = Mathf.FloorToInt(iFloat + 1);
        if( idUpper >= SmpCount ) idUpper = SmpCount - 1;
        if( idLower < 0 ) idLower = 0;
        return Mathf.Lerp( distances[idLower], distances[idUpper], iFloat - idLower ) / TotalLength;
    }
    
    public Vector3 ToPoints( float t ) 
    { 	// Umrechnung des t-Werts in Prozent des Abstands entlang der Kurve
        float iFloat = t * (SmpCount-1);
        int idLower = Mathf.FloorToInt(iFloat);
        int idUpper = Mathf.FloorToInt(iFloat + 1);
        if( idUpper >= SmpCount ) idUpper = SmpCount - 1;
        if( idLower < 0 ) idLower = 0;
        float percentage = Mathf.Lerp( distances[idLower], distances[idUpper], iFloat - idLower ) / TotalLength;
        int o = (int)MathLibrary.Remap(0, 1, 0, ExtraPoints.Count-1, percentage);
        var n = points.Count;
        if (ExtraPoints.Count - 1 - o < n && ExtraPoints.Count - 1 - o > -1) o -= n*2;
        else if (ExtraPoints.Count - 1 - o > -n&& ExtraPoints.Count - 1 - o < 0) o += n*2;
        return ExtraPoints[o];
        //return Mathf.Lerp( distances[idLower], distances[idUpper], iFloat - idLower ) / TotalLength;
    }
}