// Harry:
// Show beam light from the obstacle side

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleObstacleDetect;

public class LineBeamer : MonoBehaviour
{

    LineRenderer beam;
    public GameObject blockingCar;
    public DetectObstacle detector;


    // Start is called before the first frame update
    void Start()
    {
        beam = GetComponent<LineRenderer>();
        beam.positionCount = 2;
        beam.startWidth = 0.5f;
        beam.endWidth = 2.0f;

        // Setup a beam from the blocking car   
        Vector3 toPos = blockingCar.transform.position;
        toPos.y += 5;
        beam.SetPosition(0, blockingCar.transform.position);
        beam.SetPosition(1, toPos);
        beam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        // Show beam when obstacle is detected on the blocking car side
        if (detector.GetDetectStatus() == true)
		{
            beam.enabled = true;

		}
        else if (detector.GetDetectStatus() == false)
        {
            beam.enabled = false;
        }
        
    }
}
