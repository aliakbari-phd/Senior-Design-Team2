using System;
using System.Collections.Generic;

public class TrialTracker
{
    public const string flexAt0Trial1 = "Flex At 0 Deg Trial 1";
    public const string flexAt0Trial2 = "Flex At 0 Deg Trial 2";
    public const string flexAt0Trial3 = "Flex At 0 Deg Trial 3";
    public const string flexAt30LeftTrial1 = "Flex At 30 Deg Left Trial 1";
    public const string flexAt30LeftTrial2 = "Flex At 30 Deg Left Trial 2";
    public const string flexAt30LeftTrial3 = "Flex At 30 Deg Left Trial 3";
    public const string flexAt30RightTrial1 = "Flex At 30 Deg Right Trial 1";
    public const string flexAt30RightTrial2 = "Flex At 30 Deg Right Trial 2";
    public const string flexAt30RightTrial3 = "Flex At 30 Deg Right Trial 3";
    public const string spROMTrial = "Sagittal Plane ROM Trial";
    public bool allTrialsComplete;
    List<bool> trialsCompleted;

    public const int totalNumTrials = 10;

    public int flexAt0T1Index;
    public int flexAt0T2Index;
    public int flexAt0T3Index;
    public int flexAt30LeftT1Index;
    public int flexAt30LeftT2Index;
    public int flexAt30LeftT3Index;
    public int flexAt30RightT1Index;
    public int flexAt30RightT2Index;
    public int flexAt30RightT3Index;
    public int sagittalTrialIndex;

    public int flexAt0T1IndexEnd;
    public int flexAt0T2IndexEnd;
    public int flexAt0T3IndexEnd;
    public int flexAt30LeftT1IndexEnd;
    public int flexAt30LeftT2IndexEnd;
    public int flexAt30LeftT3IndexEnd;
    public int flexAt30RightT1IndexEnd;
    public int flexAt30RightT2IndexEnd;
    public int flexAt30RightT3IndexEnd;
    public int sagittalTrialIndexEnd;

    public TrialTracker()
	{
        flexAt0T1Index = 0;
        flexAt0T2Index = 0;
        flexAt0T3Index = 0;
        flexAt30LeftT1Index = 0;
        flexAt30LeftT2Index = 0;
        flexAt30LeftT3Index = 0;
        flexAt30RightT1Index = 0;
        flexAt30RightT2Index = 0;
        flexAt30RightT3Index = 0;
        flexAt0T1IndexEnd = 0;
        flexAt0T2IndexEnd = 0;
        flexAt0T3IndexEnd = 0;
        flexAt30LeftT1IndexEnd = 0;
        flexAt30LeftT2IndexEnd = 0;
        flexAt30LeftT3IndexEnd = 0;
        flexAt30RightT1IndexEnd = 0;
        flexAt30RightT2IndexEnd = 0;
        flexAt30RightT3IndexEnd = 0;
        trialsCompleted = new List<bool>();
        for(int i = 0; i < totalNumTrials; i++)
        {
            trialsCompleted.Add(false);
        }
        allTrialsComplete = false;
    }

    public bool testsCompleted()
    {
        for (int i = 0; i < totalNumTrials; i++)
        {
            allTrialsComplete &= trialsCompleted[i];
        }
        return allTrialsComplete;
    }

    public void setTrialIndexWithTrialString(int index, string trialName, bool isEnd)
    {
        // On first trial, set index to 0
        if(index < 0)
        {
            index = 0;
        }

        switch(trialName)
        {
            case flexAt0Trial1:
                if (!isEnd)
                {
                    flexAt0T1Index = index;
                }
                else
                {
                    flexAt0T1IndexEnd = index;
                }
                break;
            case flexAt0Trial2:
                if (!isEnd)
                {
                    flexAt0T2Index = index;
                }
                else
                {
                    flexAt0T2IndexEnd = index;
                }
                break;
            case flexAt0Trial3:
                if (!isEnd)
                {
                    flexAt0T3Index = index;
                }
                else
                {
                    flexAt0T3IndexEnd = index;
                }
                break;
            case flexAt30LeftTrial1:
                if (!isEnd)
                {
                    flexAt30LeftT1Index = index;
                }
                else
                {
                    flexAt30LeftT1IndexEnd = index;
                }
                break;
            case flexAt30LeftTrial2:
                if (!isEnd)
                {
                    flexAt30LeftT2Index = index;
                }
                else
                {
                    flexAt30LeftT2IndexEnd = index;
                }
                break;
            case flexAt30LeftTrial3:
                if (!isEnd)
                {
                    flexAt30LeftT3Index = index;
                }
                else
                {
                    flexAt30LeftT3IndexEnd = index;
                }
                break;
            case flexAt30RightTrial1:
                if (!isEnd)
                {
                    flexAt30RightT1Index = index;
                }
                else
                {
                    flexAt30RightT1IndexEnd = index;
                }
                break;
            case flexAt30RightTrial2:
                if (!isEnd)
                {
                    flexAt30RightT2Index = index;
                }
                else
                {
                    flexAt30RightT2IndexEnd = index;
                }
                break;
            case flexAt30RightTrial3:
                if (!isEnd)
                {
                    flexAt30RightT3Index = index;
                }
                else
                {
                    flexAt30RightT3IndexEnd = index;
                }
                break;
            case spROMTrial:
                if (!isEnd)
                {
                    sagittalTrialIndex = index;
                }
                else
                {
                    sagittalTrialIndexEnd = index;
                }
                break;
            default:
                break;
        }
    }

    public void getTrialIndexWithTrialString(string trialName, ref int startIndice, ref int endIndice)
    {
        switch (trialName)
        {
            case flexAt0Trial1:
                startIndice = flexAt0T1Index;
                endIndice = flexAt0T1IndexEnd;
                break;
            case flexAt0Trial2:
                startIndice = flexAt0T2Index;
                endIndice = flexAt0T2IndexEnd;
                break;
            case flexAt0Trial3:
                startIndice = flexAt0T3Index;
                endIndice = flexAt0T3IndexEnd;
                break;
            case flexAt30LeftTrial1:
                startIndice = flexAt30LeftT1Index;
                endIndice = flexAt30LeftT1IndexEnd;
                break;
            case flexAt30LeftTrial2:
                startIndice = flexAt30LeftT2Index;
                endIndice = flexAt30LeftT2IndexEnd;
                break;
            case flexAt30LeftTrial3:
                startIndice = flexAt30LeftT3Index;
                endIndice = flexAt30LeftT3IndexEnd;
                break;
            case flexAt30RightTrial1:
                startIndice = flexAt30RightT1Index;
                endIndice = flexAt30RightT1IndexEnd;
                break;
            case flexAt30RightTrial2:
                startIndice = flexAt30RightT2Index;
                endIndice = flexAt30RightT2IndexEnd;
                break;
            case flexAt30RightTrial3:
                startIndice = flexAt30RightT3Index;
                endIndice = flexAt30RightT3IndexEnd;
                break;
            case spROMTrial:
                startIndice = sagittalTrialIndex;
                endIndice = sagittalTrialIndexEnd;
                break;
            default:
                break;
        }
    }
}
