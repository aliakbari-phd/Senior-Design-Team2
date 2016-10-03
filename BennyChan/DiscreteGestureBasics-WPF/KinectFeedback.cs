using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Globalization;
using System.IO;

public class KinectFeedback
{
    public bool isInitial;
    public List<float> sagittalAngle;
    public List<float> flexAngle;
    public float currentSagittalAngle;
    public float currentFlexAngle;
    public List<float> initialPosRS;
    public List<float> initialPosSS;
    public List<float> initialPosSM;
    public List<float> initialPosSB;
    public string flexAngleTxt;
    public string sagittalAngleTxt;
    public string isZero;
    public string isFifteen;
    public string isThirty;
    public string isFlex;

    public KinectFeedback()
    {
        isInitial = true;
        isZero = "False";
        isFifteen = "False";
        isThirty = "False";
        isFlex = "False";
        initialPosRS = new List<float>();
        initialPosSS = new List<float>();
        initialPosSM = new List<float>();
        initialPosSB = new List<float>();
        sagittalAngle = new List<float>();
        flexAngle = new List<float>();
    }

    public float CalcSagittalAngleWithRespectToInitialPos(List<float> jointPos)
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
        float magnitudeVector0 = CalcMagnitude(vector0);
        float magnitudeVector1 = CalcMagnitude(vector1);
        float cos = dotProduct / (magnitudeVector0 * magnitudeVector1);
        sagittalAngle.Add((float)(Math.Acos(cos) * (180 / Math.PI)));
        double angle = Math.Acos(cos) * (180 / Math.PI);
        sagittalAngleTxt = ((float)(angle)).ToString();
        currentSagittalAngle = (float)(angle);
        return (float)(angle);
    }

    public float CalcFlexAngleWithRespectToInitialPos(List<float> jointPos)
    {
        List<float> vector0 = new List<float>();
        List<float> vector1 = new List<float>();

        for (int i = 0; i < jointPos.Count; i++)
        {
            float posDiff = jointPos[i] - initialPosSB[i];
            vector0.Add(posDiff);
        }

        for (int i = 0; i < jointPos.Count; i++)
        {
            //0 is the reference point of the Kinect
            float posDiff = 0 - initialPosSM[i];
            vector1.Add(posDiff);
        }

        float dotProduct = DotProduct(vector0, vector1);
        float magnitudeVector0 = CalcMagnitude(vector0);
        float magnitudeVector1 = CalcMagnitude(vector1);
        float cos = dotProduct / (magnitudeVector0 * magnitudeVector1);
        flexAngle.Add((float)(Math.Acos(cos) * (180 / Math.PI)));
        double angle = Math.Acos(cos) * (180 / Math.PI);
        flexAngleTxt = ((float)(angle)).ToString();
        currentFlexAngle = (float)(angle);
        return (float)(angle);

    }

    private float DotProduct(List<float> vector1, List<float> vector2)
    {
        float dotProduct = 0;
        for (int i = 0; i < vector1.Count; i++)
        {
            dotProduct += (vector1[i] * vector2[i]);
        }
        return dotProduct;
    }

    private float CalcMagnitude(List<float> vector)
    {
        double magnitude = 0;
        foreach (float pos in vector)
        {
            magnitude += pos * pos;
        }
        magnitude = Math.Sqrt(magnitude);
        return (float)(magnitude);
    }
}
