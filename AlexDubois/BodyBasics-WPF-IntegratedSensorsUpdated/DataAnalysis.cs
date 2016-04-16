using System;
using System.Collections.Generic;

public class DataAnalysis
{
    List<Int16> angle;
    List<Int16> angularAccel;
    List<Int16> angularJerk;
    SensorData correctedData;

    public DataAnalysis(SensorData cData)
	{
        angle = default(List<Int16>);
        angularAccel = default(List<Int16>);
        angularJerk = default(List<Int16>);
        correctedData = cData;
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
