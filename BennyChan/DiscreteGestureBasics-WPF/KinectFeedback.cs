using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Globalization;
using System.IO;

public class KinectFeedback
{
    public bool isInitial;
    public List<double> sagittalAngle;
    public List<float> initialPosRS;
    public List<float> initialPosSS;

    public KinectFeedback()
	{
        isInitial = true;
        initialPosRS = new List<float>();
        initialPosSS = new List<float>();
        sagittalAngle = new List<double>();
    }
    
    public double CalcAngleWithRespectToInitialPos(List<float>jointPos)
    {
        List<float> vector0 = new List<float>();
        List<float> vector1 = new List<float>();

        for (int i = 0; i < jointPos.Count; i++)
        {
            float posDiff = initialPosRS[i] - initialPosSS[i];
            vector0.Add(posDiff);
        }

        for (int i = 0; i < jointPos.Count; i++)
        {
            float posDiff = jointPos[i] - initialPosSS[i];
            vector1.Add(posDiff);
        }

        float dotProduct = DotProduct(vector0, vector1);
        double magnitudeVector0 = CalcMagnitude(vector0);
        double magnitudeVector1 = CalcMagnitude(vector1);
        double cos = dotProduct / (magnitudeVector0 * magnitudeVector1);
        sagittalAngle.Add(Math.Acos(cos)*(180/Math.PI));
        double angle = Math.Acos(cos) * (180 /Math.PI);
        return angle;

    }

    private float DotProduct(List<float>vector1, List<float>vector2)
    {
        float dotProduct = 0;
        for (int i = 0; i < vector1.Count; i++)
        {
            dotProduct += (vector1[i] * vector2[i]);
        }
        return dotProduct;
    }

    private double CalcMagnitude(List<float>vector)
    {
        double magnitude = 0;
        foreach(float pos in vector)
        {
            magnitude += pos * pos;
        }
        magnitude = Math.Sqrt(magnitude);
        return magnitude;
    }
}
