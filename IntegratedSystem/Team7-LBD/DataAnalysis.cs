using System;
using System.Collections.Generic;

public class DataAnalysis
{
    static double flexAngleROM = 55;

    //LBD Quantification Factors
    static double maleGenderFactor = .93;
    static double femaleGenderFactor = .97;

    public double fpROMFactor = 2;
    public double spROMFactor = 2;
    public double asymCompleteFactor = 1;
    public double twistingROMFactor = 1;
    public double peakAngVelFactor = 1;
    public double peakAngAccFactor = 2;
    public double peakAngJerkFactor = 1;

    //Patient Data
    public int patientID;
    public bool gender;
    public int age;
    public float peakSPAngVelocityAt0;
    public float peakSPAngAccelerationAt0;
    public float peakSPAngJerkAt0;
    public float peakFlexAngVelocityAt0;
    public float peakFlexAngAccelerationAt0;
    public float peakFlexAngJerkAt0;
    public bool fpROM = true;
    public bool spROM15 = true;
    public bool spROM30 = true;
    public bool asymComplete = true;

    //Calculate twisting ROM
    public double twistingROM;
    public double maxSPCWAngle;
    public double maxSPCCWAngle;

    public List<float> kinectSPAngleAt0T1;

    public List<float> angularFlexVelAt0T1;
    public List<float> angularSPVelAt0T1;

    public List<float> angularFlexAccelAt0T1;
    public List<float> angularSPAccelAt0T1;

    public List<float> angularFlexJerkAt0T1;
    public List<float> angularSPJerkAt0T1;

    public List<float> angularFlexVelAt0T2;
    public List<float> angularSPVelAt0T2;

    public List<float> angularFlexAccelAt0T2;
    public List<float> angularSPAccelAt0T2;

    public List<float> angularFlexJerkAt0T2;
    public List<float> angularSPJerkAt0T2;

    public List<float> angularFlexVelAt0T3;
    public List<float> angularSPVelAt0T3;

    public List<float> angularFlexAccelAt0T3;
    public List<float> angularSPAccelAt0T3;

    public List<float> angularFlexJerkAt0T3;
    public List<float> angularSPJerkAt0T3;

    public List<float> angularFlexVelAt30LeftT1;
    public List<float> angularSPVelAt30LeftT1;

    public List<float> angularFlexAccelAt30LeftT1;
    public List<float> angularSPAccelAt30LeftT1;

    public List<float> angularFlexJerkAt30LeftT1;
    public List<float> angularSPJerkAt30LeftT1;

    public List<float> angularFlexVelAt30LeftT2;
    public List<float> angularSPVelAt30LeftT2;

    public List<float> angularFlexAccelAt30LeftT2;
    public List<float> angularSPAccelAt30LeftT2;

    public List<float> angularFlexJerkAt30LeftT2;
    public List<float> angularSPJerkAt30LeftT2;

    public List<float> angularFlexVelAt30LeftT3;
    public List<float> angularSPVelAt30LeftT3;

    public List<float> angularFlexAccelAt30LeftT3;
    public List<float> angularSPAccelAt30LeftT3;

    public List<float> angularFlexJerkAt30LeftT3;
    public List<float> angularSPJerkAt30LeftT3;

    public List<float> angularFlexVelAt30RightT1;
    public List<float> angularSPVelAt30RightT1;

    public List<float> angularFlexAccelAt30RightT1;
    public List<float> angularSPAccelAt30RightT1;

    public List<float> angularFlexJerkAt30RightT1;
    public List<float> angularSPJerkAt30RightT1;

    public List<float> angularFlexVelAt30RightT2;
    public List<float> angularSPVelAt30RightT2;

    public List<float> angularFlexAccelAt30RightT2;
    public List<float> angularSPAccelAt30RightT2;

    public List<float> angularFlexJerkAt30RightT2;
    public List<float> angularSPJerkAt30RightT2;

    public List<float> angularFlexVelAt30RightT3;
    public List<float> angularSPVelAt30RightT3;

    public List<float> angularFlexAccelAt30RightT3;
    public List<float> angularSPAccelAt30RightT3;

    public List<float> angularFlexJerkAt30RightT3;
    public List<float> angularSPJerkAt30RightT3;

    public List<float> sagittalTrialFlexVel;
    public List<float> sagittalTrialSPVel;

    public List<float> sagittalTrialFlexAccel;
    public List<float> sagittalTrialSPAccel;

    public List<float> sagittalTrialFlexJerk;
    public List<float> sagittalTrialSPJerk;

    public List<float> timeStampsAngleAt0;
    //public List<float> angularSPAccelIMU;
    //public List<float> angularSPJerkIMU;

    public double severityLBD;

    //SensorData correctedData;
    IMUData imuData;

    public DataAnalysis()
    {
        patientID = 0;
        gender = true;
        age = 20;
        kinectSPAngleAt0T1 = new List<float>();
        kinectSPAngleAt0T1 = new List<float>();
        angularSPVelAt0T1 = new List<float>();
        angularFlexVelAt0T1 = new List<float>();
        angularSPAccelAt0T1 = new List<float>();
        angularFlexAccelAt0T1 = new List<float>();
        angularSPJerkAt0T1 = new List<float>();
        angularFlexJerkAt0T1 = new List<float>();
        timeStampsAngleAt0 = new List<float>();
}

    public void InitWithData(List<float> kinectSPAngles, List<float> kinectFlexAngles, IMUData imu)
    {
        kinectSPAngleAt0T1 = kinectSPAngles;
        kinectSPAngleAt0T1 = kinectFlexAngles;
        imuData = imu;
    }

    private void MergeSensorData(SensorData correctedData)
    {
        //Analyze set of kinect and wearable sensor data
        //for each motion set (across sagital plane, trunk flexion, extension)
        //Vel, Accel, Jerk, Angle, etc, and pick most accurate ones
    }


    public double QuantifyLBD()
    {
        double rating = 0;
        //float step = 1;
        float ageFactor = 0;
        float maxVel = 250;
        float maxAcc = 500;
        float maxJerk = 1000;

        float peakSPAngle = 0;
        float peakFlexAngle = 0;
        
        peakSPAngVelocityAt0 = 0;
        peakSPAngAccelerationAt0 = 0;
        peakSPAngJerkAt0 = 0;

        peakFlexAngVelocityAt0 = 0;
        peakFlexAngAccelerationAt0 = 0;
        peakFlexAngJerkAt0 = 0;
        float minFlexAngle = 0;

        angularSPVelAt0T1 = CalcStepDerivative(imuData.spAnglesMid, imuData.timeStampsMid);
        angularFlexVelAt0T1 = CalcStepDerivative(imuData.flexAnglesMid, imuData.timeStampsMid);
        angularSPAccelAt0T1 = CalcStepDerivative(angularSPVelAt0T1, imuData.timeStampsMid);
        angularFlexAccelAt0T1 = CalcStepDerivative(angularFlexVelAt0T1, imuData.timeStampsMid);
        angularSPJerkAt0T1 = CalcStepDerivative(angularSPAccelAt0T1, imuData.timeStampsMid);
        angularFlexJerkAt0T1 = CalcStepDerivative(angularFlexAccelAt0T1, imuData.timeStampsMid);

        List<int> extensionIndices = new List<int>();
        extensionIndices = FindChangeInDirectionIndices(imuData.flexAnglesMid);
        peakFlexAngVelocityAt0 = FindMaxExtension(angularFlexVelAt0T1, extensionIndices)*-1;
        peakFlexAngAccelerationAt0 = FindMaxExtension(angularFlexAccelAt0T1, extensionIndices)*-1;
        peakFlexAngJerkAt0 = FindMaxExtension(angularFlexJerkAt0T1, extensionIndices)*-1;

        peakSPAngle = FindMax(imuData.spAnglesMid);
        peakFlexAngle = FindMax(imuData.flexAnglesMid);
        peakSPAngVelocityAt0 = FindMax(angularSPVelAt0T1);
        peakSPAngAccelerationAt0 = FindMax(angularSPAccelAt0T1);
        peakSPAngJerkAt0 = FindMax(angularSPJerkAt0T1);

        minFlexAngle = FindMin(imuData.flexAnglesMid);


        maxSPCWAngle = peakSPAngle;
        maxSPCCWAngle = FindMin(imuData.spAnglesMid);

        //Normalize peaks to a corresponding rating factor
        rating += QuantifyPeak(maxVel, peakFlexAngVelocityAt0, peakAngVelFactor);
        rating += QuantifyPeak(maxAcc, peakFlexAngAccelerationAt0, peakAngAccFactor);
        rating += QuantifyPeak(maxJerk, peakFlexAngJerkAt0, peakAngJerkFactor);

        //Calculate twisting ROM
        twistingROM = maxSPCWAngle - maxSPCCWAngle;

        if (peakSPAngle < 30)
        {
            rating += spROMFactor*.5;
            spROM30 = false;
            if (peakSPAngle < 15)
            {
                rating += spROMFactor * .5;
                spROM15 = false;
            }
        }



        if(minFlexAngle > flexAngleROM)
        {
            rating += fpROMFactor;
            fpROM = false;
        }

        //Still needs definition
        if(asymComplete == false)
        {
            rating += asymCompleteFactor;
        }


        //If unable to twist more than 90 degrees total
        if(twistingROM < 45)
        {
            rating += twistingROMFactor;
        }

        //Factor in age
        ageFactor = 1 / ((float)age);

        rating += (ageFactor * 10);

        //Normalize accoridng to gender factor
        if (gender == true)
        {
            rating = rating * maleGenderFactor;
        }

        else
        {
            rating = rating * femaleGenderFactor;
        }

        severityLBD = rating;

        return rating;
    }


    //potentially change to sensor data, keep a running max in main program
    private float FindMax(List<float> list)
    {
        float max = 0;
        for(int i = 0; i < list.Count; i++)
        {
            if(max < list[i])
            {
                max = list[i];
            }
        }
        return max;
    }

    private List<int> FindChangeInDirectionIndices(List<float> list)
    {
        List<int> directionChangeIndices = new List<int>();
        int index = 1;
        float diff = 0;
        for(; index < list.Count; index++)
        {
            diff = list[index] - list[index - 1];
            if(diff < 0)
            {
                directionChangeIndices.Add(index);
            }
        }
        return directionChangeIndices;
    }

    private float FindMaxExtension(List<float> list, List<int> changeInDirectionIndices)
    {
        float min = 0;
        foreach (int i in changeInDirectionIndices)
        {
            if (min > list[i])
            {
                min = list[i];
            }
        }
        return min;
    }

    //potentially change to sensor data, keep a running min in main program
    private float FindMin(List<float> list)
    {
        float min = 100;
        for (int i = 0; i < list.Count; i++)
        {
            if (min > list[i])
            {
                min = list[i];
            }
        }
        return min;
    }

    private void Integrate()
    {
        //Define integration alg
    }

    private List<float> CalcStepDerivative(List<float> floatList, List<int> timeStamps)
    {
        // Originally the step was passed in
        List<float> derivative = new List<float>();
        float derivValue = 0;
        float step = 0;
        // Add a leading 0 in order to make the sizes of the new
        // derivative list the same as the corresponding time stamps
        derivative.Add(0);
        for (int i = 0; i < floatList.Count - 1; i ++)
        {
            step = imuData.timeIntervals;
            derivValue = ((floatList[i + 1] - floatList[i]) / step);
            derivative.Add(derivValue);
        }
        return derivative;
    }
    private double QuantifyPeak(float max, float peakMeasurement, double peakFactor)
    {
        double quantifiedPeak = 0;
        if(max < peakMeasurement)
        {
            return 0;
        }

        quantifiedPeak = (peakMeasurement / max);
        quantifiedPeak = (quantifiedPeak * peakFactor);
        quantifiedPeak = peakFactor - quantifiedPeak;
        return quantifiedPeak;
    }
}
