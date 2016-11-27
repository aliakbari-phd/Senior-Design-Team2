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
    public List<int> gyroIntPeakElement;
    public List<int> gyroTruePeakElement;
    public float ispeakgyro = 0;
    public float ispeakkin = 0;
    public float peaktimegyro = 0;
    public float peaktimekin = 0;
    public float truepeakgyro = 0;
    public float truepeakkin = 0;
    public float truegyroTS = 0;
    public float truekinTS = 0;
    public float syncvalue = 0;

    public List<float> gyroFlexInterPeaks;      //interim peaks detected
    public List<float> kinectFlexInterPeaks;

    public List<float> gyroFlexTruePeaks;       //true peaks
    public List<float> kinectFlexTruePeaks;

    public List<float> gyroIntPeaktimestamps;   //timestamps lists
    public List<float> kinectIntPeaktimestamps;

    public List<float> gyroTruePeaktimestamps;  //true timestamps lists
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

    //filtering

    

    //Correction Code

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
                gyroIntPeakElement.Add(anglesMid.Count);
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
        gyroTruePeakElement.Add(gyroIntPeakElement[0]);

        for (int gyroiter2=0; gyroiter2 < gyroFlexInterPeaks.Count-1; gyroiter2++)
        {
            if (gyroIntPeaktimestamps[gyroiter2 + 1] - gyroIntPeaktimestamps[gyroiter2] > 0.5)
            {
                truepeakgyro = gyroFlexInterPeaks[gyroiter2 + 1];
                truegyroTS = gyroIntPeaktimestamps[gyroiter2 + 1];
                gyroFlexTruePeaks.Add(truepeakgyro);
                gyroTruePeaktimestamps.Add(truegyroTS);
                gyroTruePeakElement.Add(gyroIntPeakElement[gyroiter2 + 1]);
            }
        }

        for (int kiniter2=1; kiniter2 < kinectFlexInterPeaks.Count-1; kiniter2++)
        {
            if (kinectIntPeaktimestamps[kiniter2 + 1] - kinectIntPeaktimestamps[kiniter2] > 0.5)
            {
                truepeakkin = kinectFlexInterPeaks[kiniter2 + 1];
                truekinTS = kinectIntPeaktimestamps[kiniter2 + 1];
                kinectFlexTruePeaks.Add(truepeakkin);
                kinectTruePeaktimestamps.Add(truekinTS);
            }
        }
    }

    void signalSync()
    {
        syncvalue = kinectTruePeaktimestamps[3] - gyroTruePeaktimestamps[3];
        syncvalue = Math.Abs(syncvalue);
        if (kinectTruePeaktimestamps[3] > gyroTruePeaktimestamps[3])
        {
            for(int synciter=0; synciter<transposedTSMid.Count; synciter++)
            {
                transposedTSMid[synciter] = transposedTSMid[synciter] + syncvalue;
            }
        }
        if(gyroTruePeaktimestamps[3] > kinectTruePeaktimestamps[3])
        {
            for(int synciter=0; synciter<kinectFeedback.transposedTSKin.Count; synciter++)
            {
                kinectFeedback.transposedTSKin[synciter] = kinectFeedback.transposedTSKin[synciter] + syncvalue;
            }
        }
    }

    void driftCorrection()
    {
        for(int correctionloop = 0; correctionloop < kinectTruePeaktimestamps.Count - 1; correctionloop++)
        {
            float slopeKin = 0;
            slopeKin = (kinectFlexTruePeaks[correctionloop + 1] - kinectFlexTruePeaks[correctionloop]) / (kinectTruePeaktimestamps[correctionloop + 1] - kinectTruePeaktimestamps[correctionloop]);
            float slopeGyro = 0;
            slopeGyro = (gyroFlexTruePeaks[correctionloop + 1] - gyroFlexTruePeaks[correctionloop]) / (gyroTruePeaktimestamps[correctionloop + 1] - gyroTruePeaktimestamps[correctionloop]);
            int stepcount = gyroTruePeakElement[correctionloop + 1] - gyroTruePeakElement[correctionloop];
            float slopediff = 0;
            for (int slopedrawcount = 0; slopedrawcount<stepcount;slopedrawcount++)
            {
                slopediff = (gyroFlexTruePeaks[correctionloop] + slopeKin * slopedrawcount * timeIntervals) - (gyroFlexTruePeaks[correctionloop] + slopeGyro * slopedrawcount * timeIntervals);
                anglesMid[gyroTruePeakElement[correctionloop] + slopedrawcount] = anglesMid[gyroTruePeakElement[correctionloop] + slopedrawcount] + slopediff;
            }
        }
    }

    public void getAngles()
    {
        transposeTimeStamps();
        cumTrapz();
    }
}
