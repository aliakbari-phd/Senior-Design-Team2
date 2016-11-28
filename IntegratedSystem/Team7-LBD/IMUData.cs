using System;
using System.Collections.Generic;

public class IMUData
{
    // Index corresponds to a single frame, (ie:
    // gyroXMid[1] corresponds to gyroYMid[1] and timeStampsMid[1])
    public List<double> gyroXMid;
    public List<double> gyroYMid;
    public List<int> timeStampsMid;

    public List<double> transposedTSMid;
    public List<double> transposedTSBase;

    public List<double> flexAnglesMid;
    public List<double> spAnglesMid;

    public List<double> flexAnglesMidCorrected;
    public List<double> kinectFlexAnglesCorrected;

    public List<double> flexAnglesAt0T1;
    public List<double> spAnglesAt0T1;

    public List<double> flexAnglesAt0T2;
    public List<double> spAnglesAt0T2;

    public List<double> flexAnglesAt0T3;
    public List<double> spAnglesAt0T3;

    public List<double> flexAnglesAt30LeftT1;
    public List<double> spAnglesAt30LeftT1;

    public List<double> flexAnglesAt30LeftT2;
    public List<double> spAnglesAt30LeftT2;

    public List<double> flexAnglesAt30LeftT3;
    public List<double> spAnglesAt30LeftT3;

    public List<double> flexAnglesAt30RightT1;
    public List<double> spAnglesAt30RightT1;

    public List<double> flexAnglesAt30RightT2;
    public List<double> spAnglesAt30RightT2;

    public List<double> flexAnglesAt30RightT3;
    public List<double> spAnglesAt30RightT3;

    public List<double> sagittalTrialFlexAngles;
    public List<double> sagittalTrialSPAngles;

    public List<double> transposedTSMidAt0T1;
    public List<double> transposedTSMidAt0T2;
    public List<double> transposedTSMidAt0T3;
    public List<double> transposedTSMidAt30LeftT1;
    public List<double> transposedTSMidAt30LeftT2;
    public List<double> transposedTSMidAt30LeftT3;
    public List<double> transposedTSMidAt30RightT1;
    public List<double> transposedTSMidAt30RightT2;
    public List<double> transposedTSMidAt30RightT3;
    public List<double> transposedTSMidSagittalTrial;

    public List<int> gyroIntPeakElement;
    public List<int> gyroTruePeakElement;
    public double ispeakgyro = 0;
    public double ispeakkin = 0;
    public double peaktimegyro = 0;
    public double peaktimekin = 0;
    public double truepeakgyro = 0;
    public double truepeakkin = 0;
    public double truegyroTS = 0;
    public double truekinTS = 0;
    public double syncvalue = 0;

    public List<double> gyroFlexInterPeaks;      //interim peaks detected
    public List<double> kinectFlexInterPeaks;

    public List<double> gyroFlexTruePeaks;       //true peaks
    public List<double> kinectFlexTruePeaks;

    public List<double> gyroIntPeaktimestamps;   //timestamps lists
    public List<double> kinectIntPeaktimestamps;

    public List<double> gyroTruePeaktimestamps;  //true timestamps lists
    public List<double> kinectTruePeaktimestamps;

    public KinectFeedback kinectFeedback;

    //private static double gyroCorrectionFactor = 32.75;

    public double timeIntervals = 0;

    public IMUData()
    {
        gyroXMid = new List<double>();
        gyroYMid = new List<double>();
        timeStampsMid = new List<int>();
        transposedTSMid = new List<double>();
        flexAnglesMid = new List<double>();
        spAnglesMid = new List<double>();
        flexAnglesAt0T1 = new List<double>();
        spAnglesAt0T1 = new List<double>();
        flexAnglesAt0T2 = new List<double>();
        spAnglesAt0T2 = new List<double>();
        flexAnglesAt0T3 = new List<double>();
        spAnglesAt0T3 = new List<double>();
        flexAnglesAt30LeftT1 = new List<double>();
        spAnglesAt30LeftT1 = new List<double>();
        flexAnglesAt30LeftT2 = new List<double>();
        spAnglesAt30LeftT2 = new List<double>();
        flexAnglesAt30LeftT3 = new List<double>();
        spAnglesAt30LeftT3 = new List<double>();
        flexAnglesAt30RightT1 = new List<double>();
        spAnglesAt30RightT1 = new List<double>();
        flexAnglesAt30RightT2 = new List<double>();
        spAnglesAt30RightT2 = new List<double>();
        flexAnglesAt30RightT3 = new List<double>();
        spAnglesAt30RightT3 = new List<double>();
        sagittalTrialFlexAngles = new List<double>();
        sagittalTrialSPAngles = new List<double>();

        flexAnglesMidCorrected = new List<double>();
        kinectFlexAnglesCorrected = new List<double>();

        transposedTSMidAt0T1 = new List<double>();
        transposedTSMidAt0T2 = new List<double>();
        transposedTSMidAt0T3 = new List<double>();
        transposedTSMidAt30LeftT1 = new List<double>();
        transposedTSMidAt30LeftT2 = new List<double>();
        transposedTSMidAt30LeftT3 = new List<double>();
        transposedTSMidAt30RightT1 = new List<double>();
        transposedTSMidAt30RightT2 = new List<double>();
        transposedTSMidAt30RightT3 = new List<double>();
        transposedTSMidSagittalTrial = new List<double>();
    }

    public void constructTrialList
    (
        List<double> gXMid,
        List<double> gYMid,
        List<int> timeSMid,
        int startIndice,
        int endIndice,
        KinectFeedback kinectFB
    )
    {
        List<double> trialListX = new List<double>();
        List<double> trialListY = new List<double>();
        List<int> trialTimestamps = new List<int>();
        kinectFeedback = kinectFB;
        gyroXMid.Clear();
        gyroYMid.Clear();
        timeStampsMid.Clear();
        for (int i = startIndice; i < endIndice; i++)
        {
            trialListX.Add(gXMid[i]);
            trialListY.Add(gYMid[i]);
            trialTimestamps.Add(timeSMid[i]);
        }
        gyroXMid = trialListX;
        gyroYMid = trialListY;
        timeStampsMid = trialTimestamps;
    }


    // Convert timestamps to relative time starting at 0 and convert to seconds
    void transposeTimeStamps()
    {
        int begTimeStampMid = timeStampsMid[0];
        double time = 0;
        //int begTimeStampBase = timeStampsBase[0];
        //foreach (int ts in timeStampsBase)
        //{
        //    transposedTSBase.Add((ts - begTimeStampBase) / 1000);
        //}
        double timePassed = (timeStampsMid[timeStampsMid.Count - 1] - timeStampsMid[0]) / 1000;
        timeIntervals = timePassed / timeStampsMid.Count;
        for (int i = 1; i < timeStampsMid.Count; i++)
        {
            time = (timeStampsMid[i] - begTimeStampMid);
            time = time / 1000;
            transposedTSMid.Add(time);
        }
    }

    void cumTrapz()
    {
        double flexAngle = 0;
        double spAngle = 0;
        double timeDiff = 0;
        double correctedDiffFlex = 0;
        double correctedDiffSP = 0;
        for (int i = 0; i < transposedTSMid.Count - 1; i++)
        {
            // Trapezoidal rule
            // angle = (b - a)*(f(b)+f(a)/2)
            // Multiply by correction factor
            timeDiff = transposedTSMid[i + 1] - transposedTSMid[i];

            correctedDiffFlex = (gyroYMid[i + 1] + gyroYMid[i]) / (2);
            correctedDiffSP = (gyroXMid[i + 1] + gyroXMid[i]) / (2);
            flexAngle = flexAngle + (timeDiff * correctedDiffFlex);
            flexAnglesMid.Add(flexAngle + 90);
            spAngle = spAngle + (timeDiff * correctedDiffSP);
            spAnglesMid.Add(spAngle);
        }
        filtering();
        //findPeaks();
        //signalSync();
        //driftCorrection();
    }

    //filtering
    void filtering()
    {
        MathNet.Filtering.OnlineFilter lowPass = MathNet.Filtering.OnlineFilter.CreateLowpass(MathNet.Filtering.ImpulseResponse.Finite, 200000, 5000, 10);
        double[] flexAnglesMidArray = new double[flexAnglesMid.Count];

        for (int i = 0; i < flexAnglesMid.Count; ++i)
        {
            flexAnglesMidArray[i] = flexAnglesMid[i];
        }

        flexAnglesMidArray = lowPass.ProcessSamples(flexAnglesMidArray);
        
        for (int i = 0; i < flexAnglesMidArray.Length; i++)
        {
            flexAnglesMidCorrected.Add(flexAnglesMidArray[i]);
        }
        flexAnglesMid.Clear();
        copyListByValue(flexAnglesMidCorrected, flexAnglesMid);

        //Kinect
        //double[] KinectAnglesArray = new double[kinectFeedback.flexAngles.Count];

        //for (int i = 0; i < kinectFeedback.flexAngles.Count; ++i)
        //{
        //    KinectAnglesArray[i] = kinectFeedback.flexAngles[i];
        //}

        //KinectAnglesArray = lowPass.ProcessSamples(KinectAnglesArray);

        //for (int i = 0; i < KinectAnglesArray.Length; i++)
        //{
        //    kinectFlexAnglesCorrected.Add(KinectAnglesArray[i]);
        //}
        //kinectFeedback.flexAngles.Clear();
        //copyListByValue(kinectFlexAnglesCorrected, kinectFeedback.flexAngles);
    }




    //Correction Code

    void findPeaks()
    {
        for (int gyroiter = 1; gyroiter < flexAnglesMid.Count - 1; gyroiter++)
        {
            if (flexAnglesMid[gyroiter] > flexAnglesMid[gyroiter - 1] && flexAnglesMid[gyroiter + 1] < flexAnglesMid[gyroiter])
            {
                ispeakgyro = flexAnglesMid[gyroiter];
                peaktimegyro = transposedTSMid[gyroiter];
                gyroFlexInterPeaks.Add(ispeakgyro);
                gyroIntPeaktimestamps.Add(peaktimegyro);
                gyroIntPeakElement.Add(flexAnglesMid.Count);
            }
        }

        for (int kiniter = 1; kiniter < kinectFeedback.flexAngles.Count - 1; kiniter++)
        {
            if (kinectFeedback.flexAngles[kiniter] > kinectFeedback.flexAngles[kiniter - 1] && kinectFeedback.flexAngles[kiniter + 1] < kinectFeedback.flexAngles[kiniter])
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

        for (int gyroiter2 = 0; gyroiter2 < gyroFlexInterPeaks.Count - 1; gyroiter2++)
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

        for (int kiniter2 = 1; kiniter2 < kinectFlexInterPeaks.Count - 1; kiniter2++)
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
            for (int synciter = 0; synciter < transposedTSMid.Count; synciter++)
            {
                transposedTSMid[synciter] = transposedTSMid[synciter] + syncvalue;
            }
        }
        if (gyroTruePeaktimestamps[3] > kinectTruePeaktimestamps[3])
        {
            for (int synciter = 0; synciter < kinectFeedback.transposedTSKin.Count; synciter++)
            {
                kinectFeedback.transposedTSKin[synciter] = kinectFeedback.transposedTSKin[synciter] + syncvalue;
            }
        }
    }

    void driftCorrection()
    {
        for (int correctionloop = 0; correctionloop < kinectTruePeaktimestamps.Count - 1; correctionloop++)
        {
            double slopeKin = 0;
            slopeKin = (kinectFlexTruePeaks[correctionloop + 1] - kinectFlexTruePeaks[correctionloop]) / (kinectTruePeaktimestamps[correctionloop + 1] - kinectTruePeaktimestamps[correctionloop]);
            double slopeGyro = 0;
            slopeGyro = (gyroFlexTruePeaks[correctionloop + 1] - gyroFlexTruePeaks[correctionloop]) / (gyroTruePeaktimestamps[correctionloop + 1] - gyroTruePeaktimestamps[correctionloop]);
            int stepcount = gyroTruePeakElement[correctionloop + 1] - gyroTruePeakElement[correctionloop];
            double slopediff = 0;
            for (int slopedrawcount = 0; slopedrawcount < stepcount; slopedrawcount++)
            {
                slopediff = (gyroFlexTruePeaks[correctionloop] + slopeKin * slopedrawcount * timeIntervals) - (gyroFlexTruePeaks[correctionloop] + slopeGyro * slopedrawcount * timeIntervals);
                flexAnglesMid[gyroTruePeakElement[correctionloop] + slopedrawcount] = flexAnglesMid[gyroTruePeakElement[correctionloop] + slopedrawcount] + slopediff;
            }
        }
    }

    public void getAngles
    (
        List<double> gXMid,
        List<double> gYMid,
        List<int> timeSMid,
        int startIndice,
        int endIndice,
        string trialName
    )
    {
        constructTrialList(gXMid, gYMid, timeSMid, startIndice, endIndice, kinectFeedback);
        transposeTimeStamps();
        cumTrapz();
        parseTrialString(trialName);
        transposedTSMid.Clear();
        flexAnglesMid.Clear();
        spAnglesMid.Clear();
    }

    private void parseTrialString
    (
        string trialName
    )
    {
        switch (trialName)
        {
            case TrialTracker.flexAt0Trial1:
                //flexAnglesAt0T1 = flexAnglesMid;
                //spAnglesAt0T1 = spAnglesMid;
                //transposedTSMidAt0T1 = transposedTSMid;
                copyListByValue(flexAnglesMid, flexAnglesAt0T1);
                copyListByValue(spAnglesMid, spAnglesAt0T1);
                copyListByValue(transposedTSMid, transposedTSMidAt0T1);
                break;
            case TrialTracker.flexAt0Trial2:
                //flexAnglesAt0T2 = flexAnglesMid;
                //spAnglesAt0T2 = spAnglesMid;
                //transposedTSMidAt0T2 = transposedTSMid;
                copyListByValue(flexAnglesMid, flexAnglesAt0T2);
                copyListByValue(spAnglesMid, spAnglesAt0T2);
                copyListByValue(transposedTSMid, transposedTSMidAt0T2);
                break;
            case TrialTracker.flexAt0Trial3:
                copyListByValue(flexAnglesMid, flexAnglesAt0T3);
                copyListByValue(spAnglesMid, spAnglesAt0T3);
                copyListByValue(transposedTSMid, transposedTSMidAt0T3);
                break;
            case TrialTracker.flexAt30LeftTrial1:
                copyListByValue(flexAnglesMid, flexAnglesAt30LeftT1);
                copyListByValue(spAnglesMid, spAnglesAt30LeftT1);
                copyListByValue(transposedTSMid, transposedTSMidAt30LeftT1);
                break;
            case TrialTracker.flexAt30LeftTrial2:
                copyListByValue(flexAnglesMid, flexAnglesAt30LeftT2);
                copyListByValue(spAnglesMid, spAnglesAt30LeftT2);
                copyListByValue(transposedTSMid, transposedTSMidAt30LeftT2);
                break;
            case TrialTracker.flexAt30LeftTrial3:
                copyListByValue(flexAnglesMid, flexAnglesAt30LeftT3);
                copyListByValue(spAnglesMid, spAnglesAt30LeftT3);
                copyListByValue(transposedTSMid, transposedTSMidAt30LeftT3);
                break;
            case TrialTracker.flexAt30RightTrial1:
                copyListByValue(flexAnglesMid, flexAnglesAt30RightT1);
                copyListByValue(spAnglesMid, spAnglesAt30RightT1);
                copyListByValue(transposedTSMid, transposedTSMidAt30RightT1);
                break;
            case TrialTracker.flexAt30RightTrial2:
                copyListByValue(flexAnglesMid, flexAnglesAt30RightT2);
                copyListByValue(spAnglesMid, spAnglesAt30RightT2);
                copyListByValue(transposedTSMid, transposedTSMidAt30RightT2);
                break;
            case TrialTracker.flexAt30RightTrial3:
                copyListByValue(flexAnglesMid, flexAnglesAt30RightT3);
                copyListByValue(spAnglesMid, spAnglesAt30RightT3);
                copyListByValue(transposedTSMid, transposedTSMidAt30RightT3);
                break;
            case TrialTracker.spROMTrial:
                copyListByValue(flexAnglesMid, sagittalTrialFlexAngles);
                copyListByValue(spAnglesMid, sagittalTrialSPAngles);
                copyListByValue(transposedTSMid, transposedTSMidSagittalTrial);
                break;
            default:
                break;
        }
    }

    private void copyListByValue(List<double> listToCopy, List<double> listCopy)
    {
        for(int i = 0; i < listToCopy.Count; i++)
        {
            listCopy.Add(listToCopy[i]);
        }
    }
}
