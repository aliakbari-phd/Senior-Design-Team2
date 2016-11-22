using System;
using System.Collections.Generic;

public class IMUData
{
    // Index corresponds to a single frame, (ie:
    // gyroXMid[1] corresponds to gyroYMid[1] and timeStampsMid[1])
    public List<Int16> gyroXMid;
    public List<float> gyroYMid;
    public List<Int16> gyroZMid;
    public List<int> timeStampsMid;
    public List<Int16> gyroXBase;
    public List<Int16> gyroYBase;
    public List<Int16> gyroZBase;
    public List<int> timeStampsBase;
    public float ispeakgyro = 0;
    public float ispeakkin = 0;
    public float peaktimegyro = 0;
    public float peaktimekin = 0;

    public List<float> gyroFlexInterPeaks;      //interim peaks detected
    public List<float> kinectFlexInterPeaks;

    public List<float> gyroFlexTruePeaks;       //true peaks
    public List<float> kinectFlexTruePeaks;

    public List<float> gyroIntPeaktimestamps;
    public List<float> kinectIntPeaktimestamps;

    public List<float> gyroTruePeaktimestamps;
    public List<float> kinectTruePeaktimestamps;

    public KinectFeedback kinectFeedback;

    public List<float> transposedTSMid;
    public List<double> transposedTSBase;

    public List<float> anglesMid;
    public List<double> anglesBase;

    private static double gyroCorrectionFactor = 32.75;

    public float timeIntervals = 0;

    public IMUData
    (
        List<Int16> gXMid, 
        List<float> gYMid, 
        List<Int16> gZMid, 
        List<int> timeSMid,
        List<Int16> gXBase,
        List<Int16> gYBase,
        List<Int16> gZBase,
        List<int> timeSBase,
        KinectFeedback kinectFB
    )
	{
        gyroXMid = gXMid;
        gyroYMid = gYMid;
        gyroZMid = gZMid;
        timeStampsMid = timeSMid;
        gyroXBase = gXBase;
        gyroYBase = gYBase;
        gyroZBase = gZBase;
        timeStampsBase = timeSBase;
        transposedTSMid = new List<float>();
        anglesMid = new List<float>();
        kinectFeedback = kinectFB;
    }


    // Convert timestamps to relative time starting at 0 and convert to seconds
    void transposeTimeStamps()
    {
        int begTimeStampMid = timeStampsMid[0];
        float time = 0;
        //int begTimeStampBase = timeStampsBase[0];
        //foreach (int ts in timeStampsBase)
        //{
        //    transposedTSBase.Add((ts - begTimeStampBase) / 1000);
        //}
        float timePassed = (timeStampsMid[timeStampsMid.Count - 1] - timeStampsMid[0])/1000;
        timeIntervals = timePassed / timeStampsMid.Count;
        for (int i = 0; i < timeStampsMid.Count; i++)
        {
            time = (timeStampsMid[i] - begTimeStampMid);
            time = time/1000;
            transposedTSMid.Add(time);
        }
    }


    void cumTrapz()
    {
        float angle = 0;
        for(int i = 0; i < transposedTSMid.Count - 1; i++)
        {
            // Trapezoidal rule
            // angle = (b - a)*(f(b)+f(a)/2)
            // Multiply by correction factor
            float timeDiff = transposedTSMid[i + 1] - transposedTSMid[i];
            float correctedDiff = (gyroYMid[i+1] + gyroYMid[i])/(2);
            angle = angle + (timeDiff*correctedDiff);
            anglesMid.Add(angle + 90);
        }
    }

    void findPeaks()
    {
        for (int gyroiter=1; gyroiter < anglesMid.Count-1; gyroiter++)
        {
            if (anglesMid[gyroiter]>anglesMid[gyroiter-1] && anglesMid[gyroiter+1]<anglesMid[gyroiter])
            {
                ispeakgyro = anglesMid[gyroiter];
                peaktimegyro = transposedTSMid[gyroiter];
                gyroFlexInterPeaks.Add(ispeakgyro);
                gyroIntPeaktimestamps.Add(peaktimegyro);
            }
        }

        for (int kiniter=1; kiniter < kinectFeedback.flexAngles.Count-1; kiniter++)
        {
            if (kinectFeedback.flexAngles[kiniter] > kinectFeedback.flexAngles[kiniter-1] && kinectFeedback.flexAngles[kiniter+1] < kinectFeedback.flexAngles[kiniter])
            {
                ispeakkin = kinectFeedback.flexAngles[kiniter];
                peaktimekin = kinectFeedback.transposedTSKin[kiniter];
                kinectFlexInterPeaks.Add(ispeakkin);
                kinectIntPeaktimestamps.Add(peaktimekin);
            }
        }

        gyroFlexTruePeaks.Add(gyroFlexInterPeaks[0]);
        kinectFlexTruePeaks.Add(kinectFlexInterPeaks[0]);

        for (int gyroiter2=1; gyroiter2 < gyroFlexInterPeaks.Count; gyroiter2++)
        {

        }
    }

    void signalSync()
    {

    }

    void driftCorrection()
    {

    }

    public void getAngles()
    {
        transposeTimeStamps();
        cumTrapz();
    }
}
