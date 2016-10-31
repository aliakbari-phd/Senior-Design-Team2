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

    public void setTrialIndexWithTrialString(int index, string trialName)
    {
        // On first trial, set index to 0
        if(index < 0)
        {
            index = 0;
        }

        switch(trialName)
        {
            case flexAt0Trial1:
                flexAt0T1Index = index;
                trialsCompleted[0] = true;
                break;
            case flexAt0Trial2:
                flexAt0T2Index = index;
                trialsCompleted[1] = true;
                break;
            case flexAt0Trial3:
                flexAt0T3Index = index;
                trialsCompleted[2] = true;
                break;
            case flexAt30LeftTrial1:
                flexAt30LeftT1Index = index;
                trialsCompleted[3] = true;
                break;
            case flexAt30LeftTrial2:
                flexAt30LeftT2Index = index;
                trialsCompleted[4] = true;
                break;
            case flexAt30LeftTrial3:
                flexAt30LeftT3Index = index;
                trialsCompleted[5] = true;
                break;
            case flexAt30RightTrial1:
                flexAt30RightT1Index = index;
                trialsCompleted[6] = true;
                break;
            case flexAt30RightTrial2:
                flexAt30RightT2Index = index;
                trialsCompleted[7] = true;
                break;
            case flexAt30RightTrial3:
                flexAt30RightT3Index = index;
                trialsCompleted[8] = true;
                break;
            case spROMTrial:
                trialsCompleted[9] = true;
                break;
            default:
                break;
        }
    }
}
