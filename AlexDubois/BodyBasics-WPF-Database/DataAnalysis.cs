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
    public bool fpROM = true;
    public bool spROM15 = true;
    public bool spROM30 = true;
    public bool asymComplete = true;

    //Calculate twisting ROM
    public double twistingROM;
    public double maxSPCWAngle;
    public double maxSPCCWAngle;

    public List<float> kinectSPAngleAt0;
    public List<float> timeStampsAngleAt0;
    List<float> velTimeStampsAt0;
    List<float> accelTimeStampsAt0;
    List<float> jerkTimeStampsAt0;
    List<float> kinectFlexAngleAt0;
    List<float> angularSPVel;
    List<float> angularFlexVel;
    List<float> angularSPAccel;
    List<float> angularSPJerk;
    List<float> angularFlexAccel;
    List<float> angularFlexJerk;

    public double severityLBD;

    SensorData correctedData;

    public DataAnalysis()
    {
        patientID = 0;
        gender = true;
        age = 0;
        kinectSPAngleAt0 = new List<float>();
        kinectFlexAngleAt0 = new List<float>();
        angularSPVel = new List<float>();
        angularFlexVel = new List<float>();
        angularSPAccel = new List<float>();
        angularFlexAccel = new List<float>();
        angularSPJerk = new List<float>();
        angularFlexJerk = new List<float>();
        correctedData = new SensorData();
        timeStampsAngleAt0 = new List<float>();
        velTimeStampsAt0 = new List<float>();
        accelTimeStampsAt0 = new List<float>();
        jerkTimeStampsAt0 = new List<float>();
    }

    public void InitAngles(List<float> kinectSPAngles, List<float> kinectFlexAngles)
    {
        kinectSPAngleAt0 = kinectSPAngles;
        kinectFlexAngleAt0 = kinectFlexAngles;
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
        float maxVel = 30;
        float maxAcc = 50;
        float maxJerk = 70;

        float peakSPAngle = 0;
        float peakFlexAngle = 0;
        
        peakSPAngVelocityAt0 = 0;
        peakSPAngAccelerationAt0 = 0;
        peakSPAngJerkAt0 = 0;

        float peakFlexAngVelocityAt0 = 0;
        float peakFlexAngAccelerationAt0 = 0;
        float peakFlexAngJerkAt0 = 0;
        float minFlexAngle = 0;
        //Retrieve SP Angular Velocity, Acceleration, and Jerk
        //List<Int16> spAngVelocityAt0 = correctedData.wearableSensor1Data[0];
        //List<Int16> spAngAccelerationAt0 = CalcStepDerivative(correctedData.wearableSensor1Data[0],step);
        //List<Int16> spAngJerkAt0 = CalcStepDerivative(spAngAccelerationAt0,step);
        //Retrieve the peaks of each
        //peakSPAngVelocityAt0 = FindMax(spAngVelocityAt0);
        //peakSPAngAccelerationAt0 = FindMax(spAngAccelerationAt0);
        //peakSPAngJerkAt0 = FindMax(spAngJerkAt0);
        //angularSPVel = CalcStepDerivative(kinectSPAngleAt0, step);
        //angularFlexVel = CalcStepDerivative(kinectFlexAngleAt0, step);
        //angularSPAccel = CalcStepDerivative(angularSPVel, step);
        //angularFlexAccel = CalcStepDerivative(angularFlexVel, step);
        //angularSPJerk = CalcStepDerivative(angularSPAccel, step);
        //angularFlexJerk = CalcStepDerivative(angularFlexAccel, step);


        angularSPVel = CalcStepDerivative(kinectSPAngleAt0, timeStampsAngleAt0, velTimeStampsAt0);
        angularFlexVel = CalcStepDerivative(kinectFlexAngleAt0, timeStampsAngleAt0, velTimeStampsAt0);
        angularSPAccel = CalcStepDerivative(angularSPVel, velTimeStampsAt0, accelTimeStampsAt0);
        angularFlexAccel = CalcStepDerivative(angularFlexVel, velTimeStampsAt0, accelTimeStampsAt0);
        angularSPJerk = CalcStepDerivative(angularSPAccel, accelTimeStampsAt0, jerkTimeStampsAt0);
        angularFlexJerk = CalcStepDerivative(angularFlexAccel, accelTimeStampsAt0, jerkTimeStampsAt0);

        peakSPAngle = FindMax(kinectSPAngleAt0);
        peakFlexAngle = FindMax(kinectFlexAngleAt0);
        peakSPAngVelocityAt0 = FindMax(angularSPVel);
        peakSPAngAccelerationAt0 = FindMax(angularSPAccel);
        peakSPAngJerkAt0 = FindMax(angularSPJerk);
        peakFlexAngVelocityAt0 = FindMax(angularSPVel);
        peakFlexAngAccelerationAt0 = FindMax(angularSPAccel);
        peakFlexAngJerkAt0 = FindMax(angularSPJerk);
        minFlexAngle = FindMin(kinectFlexAngleAt0);


        maxSPCWAngle = peakSPAngle;
        maxSPCCWAngle = FindMin(kinectSPAngleAt0);

        //Normalize peaks to a corresponding rating factor
        rating += QuantifyPeak(maxVel, peakSPAngVelocityAt0, peakAngVelFactor);
        rating += QuantifyPeak(maxAcc, peakSPAngAccelerationAt0, peakAngAccFactor);
        rating += QuantifyPeak(maxJerk, peakSPAngJerkAt0, peakAngJerkFactor);

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

    private List<float> CalcStepDerivative(List<float> floatList, List<float> timeStamps, List<float> derivTimeStamps)
    {
        // Originally the step was passed in
        List<float> derivative = new List<float>();
        derivTimeStamps = new List<float>();
        float derivValue = 0;
        float step = 0;
        for(int i = 0; i < floatList.Count - 1; i ++)
        {
            if (floatList[i + 1] == floatList[i])
            {
                derivative.Add(0);
            }

            else
            {
                step = timeStampsAngleAt0[i + 1] - timeStampsAngleAt0[i];
                derivValue = ((floatList[i + 1] - floatList[i]) / step);
                derivative.Add(derivValue);
                derivTimeStamps.Add(step);
            }
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
