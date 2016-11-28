using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Globalization;
using System.IO;

public class KinectFeedback
{
    public bool isInitial;
    public List<double> sagittalAngles;
    public List<double> flexAngles;
    public double currentSagittalAngle;
    public double currentFlexAngle;
    public int index;
    public List<double> initialPosRS;
    public List<double> initialPosSS;
    public List<double> initialPosSM;
    public List<double> initialPosSB;
    public string flexAngleTxt;
    public string sagittalAngleTxt;
    public string isZero;
    public string isFifteen;
    public string isThirty;
    public string isFlex;
    public List<double> transposedTSKin;
    public List<int> timestamp;

    public KinectFeedback()
    {
        index = 0;
        isInitial = true;
        isZero = "False";
        isFifteen = "False";
        isThirty = "False";
        isFlex = "False";
        initialPosRS = new List<double>();
        initialPosSS = new List<double>();
        initialPosSB = new List<double>();
        initialPosSM = new List<double>();
        sagittalAngles = new List<double>();
        flexAngles = new List<double>();
    }

    public double CalcSagittalAngleWithRespectToInitialPos(List<double> jointPos)
    {
        List<double> vector0 = new List<double>();
        List<double> vector1 = new List<double>();

        for (int i = 0; i < jointPos.Count; i++)
        {
            double posDiff = initialPosRS[i] - initialPosSS[i];
            vector0.Add(posDiff);
        }

        for (int i = 0; i < jointPos.Count; i++)
        {
            double posDiff = jointPos[i] - initialPosSS[i];
            vector1.Add(posDiff);
        }

        double dotProduct = DotProduct(vector0, vector1);
        double magnitudeVector0 = CalcMagnitude(vector0);
        double magnitudeVector1 = CalcMagnitude(vector1);
        double cos = dotProduct / (magnitudeVector0 * magnitudeVector1);
        double angle = Math.Acos(cos) * (180 / Math.PI);
        sagittalAngleTxt = ((double)(angle)).ToString();
        currentSagittalAngle = (double)(angle);
        sagittalAngles.Add(currentSagittalAngle);
        return (double)(angle);
    }

    public double CalcFlexAngleWithRespectToInitialPos(List<double> jointPos)
    {
        List<double> vector0 = new List<double>();
        List<double> vector1 = new List<double>();

        for (int i = 0; i < jointPos.Count; i++)
        {
            double posDiff = jointPos[i] - initialPosSB[i];
            vector0.Add(posDiff);
        }

        for (int i = 0; i < jointPos.Count; i++)
        {
            //0 is the reference point of the Kinect
            double posDiff = 0 - initialPosSS[i];
            vector1.Add(posDiff);
        }

        double dotProduct = DotProduct(vector0, vector1);
        double magnitudeVector0 = CalcMagnitude(vector0);
        double magnitudeVector1 = CalcMagnitude(vector1);
        double cos = dotProduct / (magnitudeVector0 * magnitudeVector1);
        double angle = Math.Acos(cos) * (180 / Math.PI);
        flexAngleTxt = ((double)(angle)).ToString();
        currentFlexAngle = (double)(angle);
        flexAngles.Add(currentFlexAngle);
        return (double)(angle);

    }

    private double DotProduct(List<double> vector1, List<double> vector2)
    {
        double dotProduct = 0;
        for (int i = 0; i < vector1.Count; i++)
        {
            dotProduct += (vector1[i] * vector2[i]);
        }
        return dotProduct;
    }

    private double CalcMagnitude(List<double> vector)
    {
        double magnitude = 0;
        foreach (double pos in vector)
        {
            magnitude += pos * pos;
        }
        magnitude = Math.Sqrt(magnitude);
        return (double)(magnitude);
    }

    void transposeTimeStampskin()
    {
        int begTimeStampMid = timestamp[0];
        float time = 0;
        //int begTimeStampBase = timeStampsBase[0];
        //foreach (int ts in timeStampsBase)
        //{
        //    transposedTSBase.Add((ts - begTimeStampBase) / 1000);
        //}
        float timePassed = (timestamp[timestamp.Count - 1] - timestamp[0]) / 1000;
        for (int i = 0; i < timestamp.Count; i++)
        {
            time = (timestamp[i] - begTimeStampMid);
            time = time / 1000;
            transposedTSKin.Add(time);
        }
    }

    public void Reset()
    {
        initialPosRS.Clear();
        initialPosSS.Clear();
        initialPosSB.Clear();
        sagittalAngles.Clear();
        flexAngles.Clear();
        //transposedTSKin.Clear();
        isInitial = true;
        index = 0;
    }
}