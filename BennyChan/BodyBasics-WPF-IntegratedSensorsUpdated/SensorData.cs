using System;
using System.Collections.Generic;

public class SensorData
{
    //Each list contains set of a specific joint's data, first element denotes the Enum
    List<List<float>> kinectData;
    //Each list connected to a wearable sensor
    List<List<Int16>> wearableData;


    public SensorData(List<List<float>> kinectJoints, List<List<Int16>> wearable)
	{
        kinectData = kinectJoints;
        wearableData = wearable;
    }

    public SensorData()
    {
        kinectData = new List<List<float>>();
        wearableData = new List<List<Int16>>();
    }



    public List<List<float>> GetKinectData()
    {
        return kinectData;
    }

    public List<List<Int16>> GetWearableData()
    {
        return wearableData;
    }
}
