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
        for (int i = 0; i < timeStampsMid.Count; i++)
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
    }

    private void parseTrialString
    (
        string trialName
    )
    {
        switch (trialName)
        {
            case TrialTracker.flexAt0Trial1:
                flexAnglesAt0T1 = flexAnglesMid;
                spAnglesAt0T1 = spAnglesMid;
                break;
            case TrialTracker.flexAt0Trial2:
                flexAnglesAt0T2 = flexAnglesMid;
                spAnglesAt0T2 = spAnglesMid;
                break;
            case TrialTracker.flexAt0Trial3:
                flexAnglesAt0T3 = flexAnglesMid;
                spAnglesAt0T3 = spAnglesMid;
                break;
            case TrialTracker.flexAt30LeftTrial1:
                flexAnglesAt30LeftT1 = flexAnglesMid;
                spAnglesAt30LeftT1 = spAnglesMid;
                break;
            case TrialTracker.flexAt30LeftTrial2:
                flexAnglesAt30LeftT2 = flexAnglesMid;
                spAnglesAt30LeftT2 = spAnglesMid;
                break;
            case TrialTracker.flexAt30LeftTrial3:
                flexAnglesAt30LeftT3 = flexAnglesMid;
                spAnglesAt30LeftT3 = spAnglesMid;
                break;
            case TrialTracker.flexAt30RightTrial1:
                flexAnglesAt30RightT1 = flexAnglesMid;
                spAnglesAt30RightT1 = spAnglesMid;
                break;
            case TrialTracker.flexAt30RightTrial2:
                flexAnglesAt30RightT2 = flexAnglesMid;
                spAnglesAt30RightT2 = spAnglesMid;
                break;
            case TrialTracker.flexAt30RightTrial3:
                flexAnglesAt30RightT3 = flexAnglesMid;
                spAnglesAt30RightT3 = spAnglesMid;
                break;
            case TrialTracker.spROMTrial:
                sagittalTrialFlexAngles = flexAnglesMid;
                sagittalTrialSPAngles = spAnglesMid;
                break;
            default:
                break;
        }
    }
}
