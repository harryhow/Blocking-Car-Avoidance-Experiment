// Detect Obstacle: detect current and target car's distance 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VehicleObstacleDetect{

    public class DetectObstacle : MonoBehaviour
    {
        //[Header("Distance Info")]
        public GameObject heroCar;
        public GameObject blockingCar;


        // Define data member
        private bool _isObstacleDetected = false;
        private int _directionToTurn = 0;
    
        [HideInInspector] public int rightDirection = -1; //TODO: move to Sign class
        [HideInInspector] public int leftDirection = 1;// TODO: move to Sing class

        void Awake()
        { 

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {    
            ScanObstacle(heroCar.transform);
        }

        private void ScanObstacle(Transform myCar) 
        {
            RaycastHit hitInfo;

            Debug.DrawRay(myCar.position, myCar.forward * 30, Color.green);
          
            if (Physics.Raycast(heroCar.transform.position, heroCar.transform.forward * 30, out hitInfo, 30)) 
            {
                 
                 if (hitInfo.collider.gameObject.CompareTag("BlockingCar")) 
                 {
                    Debug.DrawRay(myCar.position, myCar.forward * 30, Color.red);
                    Debug.Log(hitInfo.normal.x);
                    if (hitInfo.normal.x < 0)
                    {
                        // suggest to turn left
                        _directionToTurn = leftDirection;
                    }
                    else
                    {
                        // suggest to turn right
                        _directionToTurn = rightDirection;
                    }
                    _isObstacleDetected = true;
                 }
            }
            else
            {
                _isObstacleDetected = false;
            }
        }


        // public member functions
        public bool GetDetectStatus() 
        {
            return _isObstacleDetected;
        }

        public int GetSuggestedDirection() 
        {
            return _directionToTurn;
        }

        //////////////////////////////////////////////////////////////////////////////
        // Ways to detect obstacle - #3: RaycastHit using raycast to detect hit info
        //////////////////////////////////////////////////////////////////////////////
        
        // private void ScanObstacle(Transform myCar)
        // {
        //     // Ray ray = new Ray(heroCar.transform.position, heroCar.transform.forward * 10); 
        //     Debug.DrawRay(myCar.transform.position, myCar.transform.forward * 30, Color.green);
        //     RaycastHit hitInfo;
        //     if (Physics.Raycast(myCar.transform.position, myCar.transform.forward * 30, out hitInfo, 30))//
        //     {

        //         Debug.Log("hit point: " + hitInfo.point + " transform pos: " + hitInfo.transform.position + " collider: " + hitInfo.collider.gameObject);
        //         if (hitInfo.collider.gameObject.CompareTag("BlockingCar")){
        //             Debug.Log("Hit blocking car");
        //             Debug.DrawRay(myCar.transform.position, myCar.transform.forward * 30, Color.red);
        //             _isObstacleDetected = true; 
        //         }
        //         else{
        //             _isObstacleDetected = false;
        //         }
        //     }
        // }

        //////////////////////////////////////////////////////////////////////////////
        // Ways to detect obstacle - #2: OverlapSphere using radius with collider
        //////////////////////////////////////////////////////////////////////////////
        // private void ScanObstacle(Transform myCar, int radius)
        // {
        //     // Setup collider
        //     Collider[] cols = Physics.OverlapSphere(myCar.position, radius);
        //     foreach (Collider col in cols) 
        //     {
        //         if (col.tag.Equals("BlockingCar")){
        //             _isObstacleDetected = true;
        //         }
        //         else{
        //             _isObstacleDetected = false;
        //         }
        //     }
        //  }


        //////////////////////////////////////////////////////////////////////////////
        // Ways to detect obstacle - #1: Vector3 distance (not so good way)
        //////////////////////////////////////////////////////////////////////////////
        // private void ScanObstacle()
        // {
        //     distanceToObstacle = Vector3.Distance(heroCar.transform.position, blockingCar.transform.position);
        //     if (distanceToObstacle < 30)
        //     {
        //         Debug.Log("** Alert to change lane!! **");
        //         _isObstacleDetected = true;
        //         // Show visual to indicate changing direction
        //         //txtLaneChange.gameObject.SetActive(true);
        //     }
        //     else{
        //         _isObstacleDetected = false;
        //     }
        // }


    }
}

