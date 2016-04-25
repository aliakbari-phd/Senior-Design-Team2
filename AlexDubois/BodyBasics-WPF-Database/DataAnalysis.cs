using System;
using System.Collections.Generic;

public class DataAnalysis
{
    public bool gender;
    public int age;
    public int currentPatientNum;
    List<Int16> angle;
    List<Int16> angularAccel;
    List<Int16> angularJerk;
    SensorData correctedData;

    public DataAnalysis()
    {
        currentPatientNum = 0;
        gender = true;
        age = 0;
        angle = new List<Int16>();
        angularAccel = new List<Int16>();
        angularJerk = new List<Int16>();
        correctedData = new SensorData();
    }

    private void CalcAngle()
    {
        /*for(int i = 0; i < correctedData.GetWearableData().Count; i ++)
        {
            List<Int16> data = correctedData.GetWearableData().FindIndex(i);
            for(int j = 0; j < correctedData.GetWearableData().FindIndex(i).Count; j++)
            {

            }
        }
        */
    }

    private void CalcAngularAccel()
    {

    }

    private void CalcAnglularJerk()
    {

    }
}
