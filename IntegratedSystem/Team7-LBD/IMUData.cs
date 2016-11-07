using System;
using System.Collections.Generic;

public class IMUData
{
    // Index corresponds to a single frame, (ie:
    // gyroXMid[1] corresponds to gyroYMid[1] and timeStampsMid[1])
    public List<float> gyroXMid;
    public List<float> gyroYMid;
    public List<Int16> gyroZMid;
    public List<int> timeStampsMid;
    public List<Int16> gyroXBase;
    public List<Int16> gyroYBase;
    public List<Int16> gyroZBase;
    public List<int> timeStampsBase;

    public List<float> transposedTSMid;
    public List<float> transposedTSBase;

    public List<float> flexAnglesMid;
    public List<float> spAnglesMid;
    public List<float> anglesBase;

    //private static double gyroCorrectionFactor = 32.75;

    public float timeIntervals = 0;

    public IMUData
    (
        List<float> gXMid, 
        List<float> gYMid, 
        List<Int16> gZMid, 
        List<int> timeSMid,
        List<Int16> gXBase,
        List<Int16> gYBase,
        List<Int16> gZBase,
        List<int> timeSBase
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
        flexAnglesMid = new List<float>();
        spAnglesMid = new List<float>();
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
        float flexAngle = 0;
        float spAngle = 0;
        float timeDiff = 0;
        float correctedDiffFlex = 0;
        float correctedDiffSP = 0;
        for(int i = 0; i < transposedTSMid.Count - 1; i++)
        {
            // Trapezoidal rule
            // angle = (b - a)*(f(b)+f(a)/2)
            // Multiply by correction factor
            timeDiff = transposedTSMid[i + 1] - transposedTSMid[i];

            correctedDiffFlex = (gyroYMid[i+1] + gyroYMid[i])/(2);
            correctedDiffSP = (gyroXMid[i + 1] + gyroXMid[i]) / (2);
            flexAngle = flexAngle + (timeDiff*correctedDiffFlex);
            flexAnglesMid.Add(flexAngle + 90);
            spAngle = spAngle + (timeDiff * correctedDiffSP);
            spAnglesMid.Add(spAngle);
        }
    }

    public void getAngles()
    {
        transposeTimeStamps();
        cumTrapz();
    }
}
