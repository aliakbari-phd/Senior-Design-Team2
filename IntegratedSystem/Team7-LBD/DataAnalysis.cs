using System;
using System.Collections.Generic;

public class DataAnalysis
{
    static double flexAngleROM = 55;

    //LBD Quantification Factors
    static double maleGenderFactor = .93;
    static double femaleGenderFactor = .97;

    public double fpROMFactor = 1.5;
    public double spROMFactor = 1.5;
    public double asymCompleteFactor = 1;
    public double twistingROMFactor = 1;
    public double peakAngVelFactor = 1;
    public double peakAngAccFactor = 2;
    public double peakAngJerkFactor = .5;
    public double completeTrialsFactor = 1.5;

    //Patient Data
    public int patientID;
    public bool gender;
    public int age;

    public double peakFlexAngVelocityAvgAt0;
    public double peakFlexAngAccelerationAvgAt0;
    public double peakFlexAngJerkAvgAt0;

    public double peakFlexAngVelocityAvgAt30Left;
    public double peakFlexAngAccelerationAvgAt30Left;
    public double peakFlexAngJerkAvgAt30Left;

    public double peakFlexAngVelocityAvgAt30Right;
    public double peakFlexAngAccelerationAvgAt30Right;
    public double peakFlexAngJerkAvgAt30Right;

    public double peakSPAngVelocity;
    public double peakSPAngAcceleration;
    public double peakSPAngJerk;

    public bool fpROM = true;
    public bool spROM15 = true;
    public bool spROM30 = true;
    public bool asymComplete = true;

    //Calculate twisting ROM
    public double twistingROM;
    public double maxSPCWAngle;
    public double maxSPCCWAngle;

    public List<double> kinectSPAngleAt0T1;

    public List<double> angularFlexVelAt0T1;
    public List<double> angularSPVelAt0T1;

    public List<double> angularFlexAccelAt0T1;
    public List<double> angularSPAccelAt0T1;

    public List<double> angularFlexJerkAt0T1;
    public List<double> angularSPJerkAt0T1;

    public List<double> angularFlexVelAt0T2;
    public List<double> angularSPVelAt0T2;

    public List<double> angularFlexAccelAt0T2;
    public List<double> angularSPAccelAt0T2;

    public List<double> angularFlexJerkAt0T2;
    public List<double> angularSPJerkAt0T2;

    public List<double> angularFlexVelAt0T3;
    public List<double> angularSPVelAt0T3;

    public List<double> angularFlexAccelAt0T3;
    public List<double> angularSPAccelAt0T3;

    public List<double> angularFlexJerkAt0T3;
    public List<double> angularSPJerkAt0T3;

    public List<double> angularFlexVelAt30LeftT1;
    public List<double> angularSPVelAt30LeftT1;

    public List<double> angularFlexAccelAt30LeftT1;
    public List<double> angularSPAccelAt30LeftT1;

    public List<double> angularFlexJerkAt30LeftT1;
    public List<double> angularSPJerkAt30LeftT1;

    public List<double> angularFlexVelAt30LeftT2;
    public List<double> angularSPVelAt30LeftT2;

    public List<double> angularFlexAccelAt30LeftT2;
    public List<double> angularSPAccelAt30LeftT2;

    public List<double> angularFlexJerkAt30LeftT2;
    public List<double> angularSPJerkAt30LeftT2;

    public List<double> angularFlexVelAt30LeftT3;
    public List<double> angularSPVelAt30LeftT3;

    public List<double> angularFlexAccelAt30LeftT3;
    public List<double> angularSPAccelAt30LeftT3;

    public List<double> angularFlexJerkAt30LeftT3;
    public List<double> angularSPJerkAt30LeftT3;

    public List<double> angularFlexVelAt30RightT1;
    public List<double> angularSPVelAt30RightT1;

    public List<double> angularFlexAccelAt30RightT1;
    public List<double> angularSPAccelAt30RightT1;

    public List<double> angularFlexJerkAt30RightT1;
    public List<double> angularSPJerkAt30RightT1;

    public List<double> angularFlexVelAt30RightT2;
    public List<double> angularSPVelAt30RightT2;

    public List<double> angularFlexAccelAt30RightT2;
    public List<double> angularSPAccelAt30RightT2;

    public List<double> angularFlexJerkAt30RightT2;
    public List<double> angularSPJerkAt30RightT2;

    public List<double> angularFlexVelAt30RightT3;
    public List<double> angularSPVelAt30RightT3;

    public List<double> angularFlexAccelAt30RightT3;
    public List<double> angularSPAccelAt30RightT3;

    public List<double> angularFlexJerkAt30RightT3;
    public List<double> angularSPJerkAt30RightT3;

    public List<double> sagittalTrialFlexVel;
    public List<double> sagittalTrialSPVel;

    public List<double> sagittalTrialFlexAccel;
    public List<double> sagittalTrialSPAccel;

    public List<double> sagittalTrialFlexJerk;
    public List<double> sagittalTrialSPJerk;

    public List<double> timeStampsAngleAt0;

    public double severityLBD;

    public IMUData imuData;

    public TrialTracker trialTracker;

    public DataAnalysis()
    {
        patientID = 0;
        gender = true;
        age = 20;
        kinectSPAngleAt0T1 = new List<double>();
        angularSPVelAt0T1 = new List<double>();
        angularFlexVelAt0T1 = new List<double>();
        angularSPAccelAt0T1 = new List<double>();
        angularFlexAccelAt0T1 = new List<double>();
        angularSPJerkAt0T1 = new List<double>();
        angularFlexJerkAt0T1 = new List<double>();
        timeStampsAngleAt0 = new List<double>();

        angularSPVelAt0T2 = new List<double>();
        angularFlexVelAt0T2 = new List<double>();
        angularSPAccelAt0T2 = new List<double>();
        angularFlexAccelAt0T2 = new List<double>();
        angularSPJerkAt0T2 = new List<double>();
        angularFlexJerkAt0T2 = new List<double>();

        angularSPVelAt0T3 = new List<double>();
        angularFlexVelAt0T3 = new List<double>();
        angularSPAccelAt0T3 = new List<double>();
        angularFlexAccelAt0T3 = new List<double>();
        angularSPJerkAt0T3 = new List<double>();
        angularFlexJerkAt0T3 = new List<double>();

        angularSPVelAt30LeftT1 = new List<double>();
        angularFlexVelAt30LeftT1 = new List<double>();
        angularSPAccelAt30LeftT1 = new List<double>();
        angularFlexAccelAt30LeftT1 = new List<double>();
        angularSPJerkAt30LeftT1 = new List<double>();
        angularFlexJerkAt30LeftT1 = new List<double>();

        angularSPVelAt30LeftT2 = new List<double>();
        angularFlexVelAt30LeftT2 = new List<double>();
        angularSPAccelAt30LeftT2 = new List<double>();
        angularFlexAccelAt30LeftT2 = new List<double>();
        angularSPJerkAt30LeftT2 = new List<double>();
        angularFlexJerkAt30LeftT2 = new List<double>();

        angularSPVelAt30LeftT3 = new List<double>();
        angularFlexVelAt30LeftT3 = new List<double>();
        angularSPAccelAt30LeftT3 = new List<double>();
        angularFlexAccelAt30LeftT3 = new List<double>();
        angularSPJerkAt30LeftT3 = new List<double>();
        angularFlexJerkAt30LeftT3 = new List<double>();

        angularSPVelAt30RightT1 = new List<double>();
        angularFlexVelAt30RightT1 = new List<double>();
        angularSPAccelAt30RightT1 = new List<double>();
        angularFlexAccelAt30RightT1 = new List<double>();
        angularSPJerkAt30RightT1 = new List<double>();
        angularFlexJerkAt30RightT1 = new List<double>();

        angularSPVelAt30RightT2 = new List<double>();
        angularFlexVelAt30RightT2 = new List<double>();
        angularSPAccelAt30RightT2 = new List<double>();
        angularFlexAccelAt30RightT2 = new List<double>();
        angularSPJerkAt30RightT2 = new List<double>();
        angularFlexJerkAt30RightT2 = new List<double>();

        angularSPVelAt30RightT3 = new List<double>();
        angularFlexVelAt30RightT3 = new List<double>();
        angularSPAccelAt30RightT3 = new List<double>();
        angularFlexAccelAt30RightT3 = new List<double>();
        angularSPJerkAt30RightT3 = new List<double>();
        angularFlexJerkAt30RightT3 = new List<double>();

        sagittalTrialSPVel = new List<double>();
        sagittalTrialFlexVel = new List<double>();
        sagittalTrialSPAccel = new List<double>();
        sagittalTrialFlexAccel = new List<double>();
        sagittalTrialSPJerk = new List<double>();
        sagittalTrialFlexJerk = new List<double>();
    }

    public void InitWithData(List<double> kinectSPAngles, List<double> kinectFlexAngles, IMUData imu, TrialTracker tt)
    {
        kinectSPAngleAt0T1 = kinectSPAngles;
        kinectSPAngleAt0T1 = kinectFlexAngles;
        imuData = imu;
        trialTracker = tt;
    }


    public double QuantifyLBD()
    {
        double rating = 0;
        //double step = 1;
        double ageFactor = 0;
        double maxVel = 250;
        double maxAcc = 500;
        double maxJerk = 1000;

        double peakSPAngle = 0;
        double peakFlexAngle = 0;

        //peakSPAngVelocityAt0 = 0;
        //peakSPAngAccelerationAt0 = 0;
        //peakSPAngJerkAt0 = 0;

        //peakFlexAngVelocityAt0 = 0;
        //peakFlexAngAccelerationAt0 = 0;
        //peakFlexAngJerkAt0 = 0;

        peakFlexAngVelocityAvgAt0 = 0;
        peakFlexAngAccelerationAvgAt0 = 0;
        peakFlexAngJerkAvgAt0 = 0;

        double minFlexAngle = 0;

        if (imuData.spAnglesAt0T1.Count > 0)
        {
            angularSPVelAt0T1 = CalcStepDerivative(imuData.spAnglesAt0T1, imuData.transposedTSMidAt0T1);
            angularFlexVelAt0T1 = CalcStepDerivative(imuData.flexAnglesAt0T1, imuData.transposedTSMidAt0T1);    
            angularSPAccelAt0T1 = CalcStepDerivative(angularSPVelAt0T1, imuData.transposedTSMidAt0T1);
            angularFlexAccelAt0T1 = CalcStepDerivative(angularFlexVelAt0T1, imuData.transposedTSMidAt0T1);
            angularSPJerkAt0T1 = CalcStepDerivative(angularSPAccelAt0T1, imuData.transposedTSMidAt0T1);
            angularFlexJerkAt0T1 = CalcStepDerivative(angularFlexAccelAt0T1, imuData.transposedTSMidAt0T1);
            trialTracker.setTestFinished(0);
        }

        if (imuData.spAnglesAt0T2.Count > 0)
        {
            angularSPVelAt0T2 = CalcStepDerivative(imuData.spAnglesAt0T2, imuData.transposedTSMidAt0T2);
            angularFlexVelAt0T2 = CalcStepDerivative(imuData.flexAnglesAt0T2, imuData.transposedTSMidAt0T2);
            angularSPAccelAt0T2 = CalcStepDerivative(angularSPVelAt0T2, imuData.transposedTSMidAt0T2);
            angularFlexAccelAt0T2 = CalcStepDerivative(angularFlexVelAt0T2, imuData.transposedTSMidAt0T2);
            angularSPJerkAt0T2 = CalcStepDerivative(angularSPAccelAt0T2, imuData.transposedTSMidAt0T2);
            angularFlexJerkAt0T2 = CalcStepDerivative(angularFlexAccelAt0T2, imuData.transposedTSMidAt0T2);
            trialTracker.setTestFinished(1);
        }

        if (imuData.spAnglesAt0T3.Count > 0)
        {
            angularSPVelAt0T3 = CalcStepDerivative(imuData.spAnglesAt0T3, imuData.transposedTSMidAt0T3);
            angularFlexVelAt0T3 = CalcStepDerivative(imuData.flexAnglesAt0T3, imuData.transposedTSMidAt0T3);
            angularSPAccelAt0T3 = CalcStepDerivative(angularSPVelAt0T3, imuData.transposedTSMidAt0T3);
            angularFlexAccelAt0T3 = CalcStepDerivative(angularFlexVelAt0T3, imuData.transposedTSMidAt0T3);
            angularSPJerkAt0T3 = CalcStepDerivative(angularSPAccelAt0T3, imuData.transposedTSMidAt0T3);
            angularFlexJerkAt0T3 = CalcStepDerivative(angularFlexAccelAt0T3, imuData.transposedTSMidAt0T3);
            trialTracker.setTestFinished(2);
        }

        if (imuData.spAnglesAt30LeftT1.Count > 0)
        {
            angularSPVelAt30LeftT1 = CalcStepDerivative(imuData.spAnglesAt30LeftT1, imuData.transposedTSMidAt30LeftT1);
            angularFlexVelAt30LeftT1 = CalcStepDerivative(imuData.flexAnglesAt30LeftT1, imuData.transposedTSMidAt30LeftT1);
            angularSPAccelAt30LeftT1 = CalcStepDerivative(angularSPVelAt30LeftT1, imuData.transposedTSMidAt30LeftT1);
            angularFlexAccelAt30LeftT1 = CalcStepDerivative(angularFlexVelAt30LeftT1, imuData.transposedTSMidAt30LeftT1);
            angularSPJerkAt30LeftT1 = CalcStepDerivative(angularSPAccelAt30LeftT1, imuData.transposedTSMidAt30LeftT1);
            angularFlexJerkAt30LeftT1 = CalcStepDerivative(angularFlexAccelAt30LeftT1, imuData.transposedTSMidAt30LeftT1);
            trialTracker.setTestFinished(3);
        }

        if (imuData.spAnglesAt30LeftT2.Count > 0)
        {
            angularSPVelAt30LeftT2 = CalcStepDerivative(imuData.spAnglesAt30LeftT2, imuData.transposedTSMidAt30LeftT2);
            angularFlexVelAt30LeftT2 = CalcStepDerivative(imuData.flexAnglesAt30LeftT2, imuData.transposedTSMidAt30LeftT2);
            angularSPAccelAt30LeftT2 = CalcStepDerivative(angularSPVelAt30LeftT2, imuData.transposedTSMidAt30LeftT2);
            angularFlexAccelAt30LeftT2 = CalcStepDerivative(angularFlexVelAt30LeftT2, imuData.transposedTSMidAt30LeftT2);
            angularSPJerkAt30LeftT2 = CalcStepDerivative(angularSPAccelAt30LeftT2, imuData.transposedTSMidAt30LeftT2);
            angularFlexJerkAt30LeftT2 = CalcStepDerivative(angularFlexAccelAt30LeftT2, imuData.transposedTSMidAt30LeftT2);
            trialTracker.setTestFinished(4);
        }

        if (imuData.spAnglesAt30LeftT3.Count > 0)
        {
            angularSPVelAt30LeftT3 = CalcStepDerivative(imuData.spAnglesAt30LeftT3, imuData.transposedTSMidAt30LeftT3);
            angularFlexVelAt30LeftT3 = CalcStepDerivative(imuData.flexAnglesAt30LeftT3, imuData.transposedTSMidAt30LeftT3);
            angularSPAccelAt30LeftT3 = CalcStepDerivative(angularSPVelAt30LeftT3, imuData.transposedTSMidAt30LeftT3);
            angularFlexAccelAt30LeftT3 = CalcStepDerivative(angularFlexVelAt30LeftT3, imuData.transposedTSMidAt30LeftT3);
            angularSPJerkAt30LeftT3 = CalcStepDerivative(angularSPAccelAt30LeftT3, imuData.transposedTSMidAt30LeftT3);
            angularFlexJerkAt30LeftT3 = CalcStepDerivative(angularFlexAccelAt30LeftT3, imuData.transposedTSMidAt30LeftT3);
            trialTracker.setTestFinished(5);
        }

        if (imuData.spAnglesAt30RightT1.Count > 0)
        {
            angularSPVelAt30RightT1 = CalcStepDerivative(imuData.spAnglesAt30RightT1, imuData.transposedTSMidAt30RightT1);
            angularFlexVelAt30RightT1 = CalcStepDerivative(imuData.flexAnglesAt30RightT1, imuData.transposedTSMidAt30RightT1);
            angularSPAccelAt30RightT1 = CalcStepDerivative(angularSPVelAt30RightT1, imuData.transposedTSMidAt30RightT1);
            angularFlexAccelAt30RightT1 = CalcStepDerivative(angularFlexVelAt30RightT1, imuData.transposedTSMidAt30RightT1);
            angularSPJerkAt30RightT1 = CalcStepDerivative(angularSPAccelAt30RightT1, imuData.transposedTSMidAt30RightT1);
            angularFlexJerkAt30RightT1 = CalcStepDerivative(angularFlexAccelAt30RightT1, imuData.transposedTSMidAt30RightT1);
            trialTracker.setTestFinished(6);
        }

        if (imuData.spAnglesAt30RightT2.Count > 0)
        {
            angularSPVelAt30RightT2 = CalcStepDerivative(imuData.spAnglesAt30RightT2, imuData.transposedTSMidAt30RightT2);
            angularFlexVelAt30RightT2 = CalcStepDerivative(imuData.flexAnglesAt30RightT2, imuData.transposedTSMidAt30RightT2);
            angularSPAccelAt30RightT2 = CalcStepDerivative(angularSPVelAt30RightT2, imuData.transposedTSMidAt30RightT2);
            angularFlexAccelAt30RightT2 = CalcStepDerivative(angularFlexVelAt30RightT2, imuData.transposedTSMidAt30RightT2);
            angularSPJerkAt30RightT2 = CalcStepDerivative(angularSPAccelAt30RightT2, imuData.transposedTSMidAt30RightT2);
            angularFlexJerkAt30RightT2 = CalcStepDerivative(angularFlexAccelAt30RightT2, imuData.transposedTSMidAt30RightT2);
            trialTracker.setTestFinished(7);
        }

        if (imuData.spAnglesAt30RightT3.Count > 0)
        {
            angularSPVelAt30RightT3 = CalcStepDerivative(imuData.spAnglesAt30RightT3, imuData.transposedTSMidAt30RightT3);
            angularFlexVelAt30RightT3 = CalcStepDerivative(imuData.flexAnglesAt30RightT3, imuData.transposedTSMidAt30RightT3);
            angularSPAccelAt30RightT3 = CalcStepDerivative(angularSPVelAt30RightT3, imuData.transposedTSMidAt30RightT3);
            angularFlexAccelAt30RightT3 = CalcStepDerivative(angularFlexVelAt30RightT3, imuData.transposedTSMidAt30RightT3);
            angularSPJerkAt30RightT3 = CalcStepDerivative(angularSPAccelAt30RightT3, imuData.transposedTSMidAt30RightT3);
            angularFlexJerkAt30RightT3 = CalcStepDerivative(angularFlexAccelAt30RightT3, imuData.transposedTSMidAt30RightT3);
            trialTracker.setTestFinished(8);
        }

        if (imuData.sagittalTrialSPAngles.Count > 0)
        {
            sagittalTrialSPVel = CalcStepDerivative(imuData.sagittalTrialSPAngles, imuData.transposedTSMidSagittalTrial);
            sagittalTrialFlexVel = CalcStepDerivative(imuData.sagittalTrialFlexAngles, imuData.transposedTSMidSagittalTrial);
            sagittalTrialSPAccel = CalcStepDerivative(sagittalTrialSPVel, imuData.transposedTSMidSagittalTrial);
            sagittalTrialFlexAccel = CalcStepDerivative(sagittalTrialFlexVel, imuData.transposedTSMidSagittalTrial);
            sagittalTrialSPJerk = CalcStepDerivative(sagittalTrialSPAccel, imuData.transposedTSMidSagittalTrial);
            sagittalTrialFlexJerk = CalcStepDerivative(sagittalTrialFlexAccel, imuData.transposedTSMidSagittalTrial);
            trialTracker.setTestFinished(9);
        }

        if (trialTracker.testsFinished())
        {
            List<int> extensionIndicesAt0T1 = new List<int>();
            extensionIndicesAt0T1 = FindChangeInDirectionIndices(imuData.flexAnglesAt0T1);
            peakFlexAngVelocityAvgAt0 += FindMaxExtension(angularFlexVelAt0T1, extensionIndicesAt0T1) * -1;
            peakFlexAngAccelerationAvgAt0 += FindMaxExtension(angularFlexAccelAt0T1, extensionIndicesAt0T1) * -1;
            peakFlexAngJerkAvgAt0 += FindMaxExtension(angularFlexJerkAt0T1, extensionIndicesAt0T1) * -1;

            peakFlexAngle += FindMax(imuData.flexAnglesAt0T1);
            minFlexAngle += FindMin(imuData.flexAnglesAt0T1);


            List<int> extensionIndicesAt0T2 = new List<int>();
            extensionIndicesAt0T2 = FindChangeInDirectionIndices(imuData.flexAnglesAt0T2);
            peakFlexAngVelocityAvgAt0 += FindMaxExtension(angularFlexVelAt0T2, extensionIndicesAt0T2) * -1;
            peakFlexAngAccelerationAvgAt0 += FindMaxExtension(angularFlexAccelAt0T2, extensionIndicesAt0T2) * -1;
            peakFlexAngJerkAvgAt0 += FindMaxExtension(angularFlexJerkAt0T2, extensionIndicesAt0T2) * -1;

            peakFlexAngle += FindMax(imuData.flexAnglesAt0T2);
            minFlexAngle += FindMin(imuData.flexAnglesAt0T2);


            List<int> extensionIndicesAt0T3 = new List<int>();
            extensionIndicesAt0T3 = FindChangeInDirectionIndices(imuData.flexAnglesAt0T3);
            peakFlexAngVelocityAvgAt0 += FindMaxExtension(angularFlexVelAt0T3, extensionIndicesAt0T3) * -1;
            peakFlexAngAccelerationAvgAt0 += FindMaxExtension(angularFlexAccelAt0T3, extensionIndicesAt0T3) * -1;
            peakFlexAngJerkAvgAt0 += FindMaxExtension(angularFlexJerkAt0T3, extensionIndicesAt0T3) * -1;

            peakFlexAngle += FindMax(imuData.flexAnglesAt0T3);
            minFlexAngle += FindMin(imuData.flexAnglesAt0T3);

            peakFlexAngVelocityAvgAt0 = peakFlexAngVelocityAvgAt0 / 3;
            peakFlexAngAccelerationAvgAt0 = peakFlexAngAccelerationAvgAt0 / 3;
            peakFlexAngJerkAvgAt0 = peakFlexAngJerkAvgAt0 / 3;
            peakFlexAngle = peakFlexAngle / 3;
            minFlexAngle = peakFlexAngle / 3;

            peakSPAngle = FindMax(imuData.sagittalTrialSPAngles);
            peakSPAngVelocity = FindMax(sagittalTrialFlexVel);
            peakSPAngAcceleration = FindMax(sagittalTrialFlexAccel);
            peakSPAngJerk = FindMax(sagittalTrialFlexJerk);

            maxSPCWAngle = peakSPAngle;
            maxSPCCWAngle = FindMin(imuData.sagittalTrialSPAngles);

            //Normalize peaks to a corresponding rating factor
            rating += QuantifyPeak(maxVel, peakFlexAngVelocityAvgAt0, peakAngVelFactor);
            rating += QuantifyPeak(maxAcc, peakFlexAngAccelerationAvgAt0, peakAngAccFactor);
            rating += QuantifyPeak(maxJerk, peakFlexAngJerkAvgAt0, peakAngJerkFactor);

            //Calculate twisting ROM
            twistingROM = maxSPCWAngle - maxSPCCWAngle;

            if (peakSPAngle < 30)
            {
                rating += spROMFactor * .5;
                spROM30 = false;
                if (peakSPAngle < 15)
                {
                    rating += spROMFactor * .5;
                    spROM15 = false;
                }
            }



            if (minFlexAngle > flexAngleROM)
            {
                rating += fpROMFactor;
                fpROM = false;
            }
            else
            {
                trialTracker.setTestComplete(0);
                trialTracker.setTestComplete(1);
                trialTracker.setTestComplete(2);
            }


            //If unable to twist more than 90 degrees total
            if (twistingROM < 45)
            {
                rating += twistingROMFactor;
            }
            else
            {
                trialTracker.setTestComplete(9);
            }

            if(FindMin(imuData.flexAnglesAt30LeftT1) <= flexAngleROM)
            {
                trialTracker.setTestComplete(3);
            }
            if (FindMin(imuData.flexAnglesAt30LeftT2) <= flexAngleROM)
            {
                trialTracker.setTestComplete(4);
            }
            if (FindMin(imuData.flexAnglesAt30LeftT3) <= flexAngleROM)
            {
                trialTracker.setTestComplete(5);
            }
            if (FindMin(imuData.flexAnglesAt30RightT1) <= flexAngleROM)
            {
                trialTracker.setTestComplete(6);
            }
            if (FindMin(imuData.flexAnglesAt30RightT2) <= flexAngleROM)
            {
                trialTracker.setTestComplete(7);
            }
            if (FindMin(imuData.flexAnglesAt30RightT3) <= flexAngleROM)
            {
                trialTracker.setTestComplete(8);
            }


            asymComplete = trialTracker.testsCompleted();

            if (asymComplete == false)
            {
                if(trialTracker.trialsCompleted[0] == true)
                {
                    rating += asymCompleteFactor * .25;
                }
                if (trialTracker.trialsCompleted[3] == true)
                {
                    rating += asymCompleteFactor * .25;
                }
                if (trialTracker.trialsCompleted[6] == true)
                {
                    rating += asymCompleteFactor * .25;
                }
                if (trialTracker.trialsCompleted[9] == true)
                {
                    rating += asymCompleteFactor * .25;
                }
            }

            //Factor in age
            ageFactor = 1 / ((double)age);

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
        }

        return rating;
    }


    //potentially change to sensor data, keep a running max in main program
    public double FindMax(List<double> list)
    {
        double max = 0;
        for(int i = 0; i < list.Count; i++)
        {
            if(max < list[i])
            {
                max = list[i];
            }
        }
        return max;
    }

    private List<int> FindChangeInDirectionIndices(List<double> list)
    {
        List<int> directionChangeIndices = new List<int>();
        int index = 1;
        double diff = 0;
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

    private double FindMaxExtension(List<double> list, List<int> changeInDirectionIndices)
    {
        double min = 0;
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
    public double FindMin(List<double> list)
    {
        double min = 100;
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

    private List<double> CalcStepDerivative(List<double> doubleList, List<double> timeStamps)
    {
        // Originally the step was passed in
        List<double> derivative = new List<double>();
        double derivValue = 0;
        double step = (timeStamps[(timeStamps.Count)-1] - timeStamps[0])/timeStamps.Count;
        // Add a leading 0 in order to make the sizes of the new
        // derivative list the same as the corresponding time stamps
        derivative.Add(0);
        for (int i = 0; i < doubleList.Count - 1; i ++)
        {
            derivValue = ((doubleList[i + 1] - doubleList[i]) / step);
            derivative.Add(derivValue);
        }
        return derivative;
    }
    private double QuantifyPeak(double max, double peakMeasurement, double peakFactor)
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
