using System;
using System.Collections.Generic;

public class IMUData
{
    // Index corresponds to a single frame, (ie:
    // gyroXMid[1] corresponds to gyroYMid[1] and timeStampsMid[1])
    public List<float> gyroXMid;
    public List<float> gyroYMid;
    public List<int> timeStampsMid;

    public List<float> transposedTSMid;
    public List<float> transposedTSBase;

    public List<float> flexAnglesMid;
    public List<float> spAnglesMid;

    public List<float> flexAnglesAt0T1;
    public List<float> spAnglesAt0T1;

    public List<float> flexAnglesAt0T2;
    public List<float> spAnglesAt0T2;

    public List<float> flexAnglesAt0T3;
    public List<float> spAnglesAt0T3;

    public List<float> flexAnglesAt30LeftT1;
    public List<float> spAnglesAt30LeftT1;

    public List<float> flexAnglesAt30LeftT2;
    public List<float> spAnglesAt30LeftT2;

    public List<float> flexAnglesAt30LeftT3;
    public List<float> spAnglesAt30LeftT3;

    public List<float> flexAnglesAt30RightT1;
    public List<float> spAnglesAt30RightT1;

    public List<float> flexAnglesAt30RightT2;
    public List<float> spAnglesAt30RightT2;

    public List<float> flexAnglesAt30RightT3;
    public List<float> spAnglesAt30RightT3;

    public List<float> sagittalTrialFlexAngles;
    public List<float> sagittalTrialSPAngles;

    public List<float> transposedTSMidAt0T1;
    public List<float> transposedTSMidAt0T2;
    public List<float> transposedTSMidAt0T3;
    public List<float> transposedTSMidAt30LeftT1;
    public List<float> transposedTSMidAt30LeftT2;
    public List<float> transposedTSMidAt30LeftT3;
    public List<float> transposedTSMidAt30RightT1;
    public List<float> transposedTSMidAt30RightT2;
    public List<float> transposedTSMidAt30RightT3;
    public List<float> transposedTSMidSagittalTrial;

    //private static double gyroCorrectionFactor = 32.75;

    public float timeIntervals = 0;

    public IMUData()
    {
        gyroXMid = new List<float>();
        gyroYMid = new List<float>();
        timeStampsMid = new List<int>();
        transposedTSMid = new List<float>();
        flexAnglesMid = new List<float>();
        spAnglesMid = new List<float>();
        flexAnglesAt0T1 = new List<float>();
        spAnglesAt0T1 = new List<float>();
        flexAnglesAt0T2 = new List<float>();
        spAnglesAt0T2 = new List<float>();
        flexAnglesAt0T3 = new List<float>();
        spAnglesAt0T3 = new List<float>();
        flexAnglesAt30LeftT1 = new List<float>();
        spAnglesAt30LeftT1 = new List<float>();
        flexAnglesAt30LeftT2 = new List<float>();
        spAnglesAt30LeftT2 = new List<float>();
        flexAnglesAt30LeftT3 = new List<float>();
        spAnglesAt30LeftT3 = new List<float>();
        flexAnglesAt30RightT1 = new List<float>();
        spAnglesAt30RightT1 = new List<float>();
        flexAnglesAt30RightT2 = new List<float>();
        spAnglesAt30RightT2 = new List<float>();
        flexAnglesAt30RightT3 = new List<float>();
        spAnglesAt30RightT3 = new List<float>();
        sagittalTrialFlexAngles = new List<float>();
        sagittalTrialSPAngles = new List<float>();

        transposedTSMidAt0T1 = new List<float>();
        transposedTSMidAt0T2 = new List<float>();
        transposedTSMidAt0T3 = new List<float>();
        transposedTSMidAt30LeftT1 = new List<float>();
        transposedTSMidAt30LeftT2 = new List<float>();
        transposedTSMidAt30LeftT3 = new List<float>();
        transposedTSMidAt30RightT1 = new List<float>();
        transposedTSMidAt30RightT2 = new List<float>();
        transposedTSMidAt30RightT3 = new List<float>();
        transposedTSMidSagittalTrial = new List<float>();
    }

    public void constructTrialList
    (
        List<float> gXMid,
        List<float> gYMid,
        List<int> timeSMid,
        int startIndice,
        int endIndice
    )
    {
        List<float> trialListX = new List<float>();
        List<float> trialListY = new List<float>();
        List<int> trialTimestamps = new List<int>();
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
        float time = 0;
        //int begTimeStampBase = timeStampsBase[0];
        //foreach (int ts in timeStampsBase)
        //{
        //    transposedTSBase.Add((ts - begTimeStampBase) / 1000);
        //}
        float timePassed = (timeStampsMid[timeStampsMid.Count - 1] - timeStampsMid[0]) / 1000;
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
        float flexAngle = 0;
        float spAngle = 0;
        float timeDiff = 0;
        float correctedDiffFlex = 0;
        float correctedDiffSP = 0;
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
    }

    public void getAngles
    (
        List<float> gXMid,
        List<float> gYMid,
        List<int> timeSMid,
        int startIndice,
        int endIndice,
        string trialName
    )
    {
        constructTrialList(gXMid, gYMid, timeSMid, startIndice, endIndice);
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

    private void copyListByValue(List<float> listToCopy, List<float> listCopy)
    {
        for(int i = 0; i < listToCopy.Count; i++)
        {
            listCopy.Add(listToCopy[i]);
        }
    }
}
