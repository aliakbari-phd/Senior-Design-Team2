using System;
using System.Collections.Generic;

public class SensorData
{
    //Each list contains set of a specific joint's data, first element denotes the Enum
    public List<List<float>> kinectData;
    //Each list contains data at a certain angle
    public List<List<Int16>> wearableSensor1Data;
    public List<List<Int16>> wearableSensor2Data;

    //Enum corresponds to the index within the wearable sensor data
    public enum sensorDataIndices
    {
        Zero,
        FifteenCW,
        ThirtyCW,
        FifteenCCW,
        ThirtyCCW,
        TwistingROM
    }

    public SensorData(List<List<float>> kinectJoints, List<List<Int16>> wearable1, List<List<Int16>> wearable2)
	{
        kinectData = kinectJoints;
        wearableSensor1Data = wearable1;
        wearableSensor1Data = wearable2;
    }

    public SensorData()
    {
        kinectData = new List<List<float>>();
        wearableSensor1Data = new List<List<Int16>>();
        wearableSensor2Data = new List<List<Int16>>();
    }

}
