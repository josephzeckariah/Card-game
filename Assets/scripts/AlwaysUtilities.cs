using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlwaysUtilities 
{
    // Start is called before the first frame update
   public  static Vector3 Vector3ChangeNotAllProperties(Vector3 vectorToChange, Vector3 valuesToChngeToZeroIfStayTheSame)
    {
        Vector3 vector3ToProcess = vectorToChange;


        if (valuesToChngeToZeroIfStayTheSame.x != 0)
        {
            vector3ToProcess.x = valuesToChngeToZeroIfStayTheSame.x;
        }
        if (valuesToChngeToZeroIfStayTheSame.y != 0)
        {
            vector3ToProcess.y = valuesToChngeToZeroIfStayTheSame.y;
        }
        if (valuesToChngeToZeroIfStayTheSame.z != 0)
        {
            vector3ToProcess.z = valuesToChngeToZeroIfStayTheSame.z;
        }


        return vector3ToProcess;
    }

   public  static Vector3 Vector3Divide(Vector3 divided, Vector3 divideBy)
    {
        Vector3 output;
        output.x = divided.x / divideBy.x;
        output.y = divided.y / divideBy.y;
        output.z = divided.z / divideBy.z;
        return output;
    }
}
