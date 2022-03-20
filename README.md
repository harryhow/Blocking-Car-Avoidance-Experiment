# Blocking Car Alert Experiment

## **Harry's Woven challenge 03.18.2022**

## 🔎 The Ask

Using Unity to demo vehicle notifies the driver to change lanes for avoiding a “blocking car”.

## 📌 **Assumption**

Here are some assumption I’d like to make for this prototyping. 

- Assume there’s overhead display or windshield overlay display that I can use for showing information.
- Assume drive won’t drive opposite.
- Assume the vehicle can easily take extra electronics with some Universal interface.
- Assume there’s inter-vehicle communication or protocol.

## 📋 The **Process**

The overall challenge took around 11 hours after taking advantage of this week's scattered time. The timeline has planned to be utilizing my after hours during the week for around 1-2 hours per day. 

Starting from understanding the problem, the ask and requirements, to get familiar with the existing scene and packages including vehicle and road Unity package used in the project.

I learned that one of the main challenge is to detect obstructing vehicles and the other important one would be able to issue warning messages for changing lanes.

Detecting blocking cars became my starting task, so I spent a few iterations to test the idea and algorithm (mostly Unity’s API) to get the final obstacle detection result.

In addition to the detection logic, I also ideated the various prompts on the overhead windshield display (my assumption) through the overall process, some digital and physical ideas are also experimented.

Here’s the list to break down my overall process into hours, you can find more detailed process from the ‘Detailed Process’ section. 

| Task | Time | Description |
| --- | --- | --- |
| 1 | 0.5 hour | Understand brief and make a plan |
| 2 | 1 hour | Study basic car knowledge (technical term) and VPP package |
| 3 | 0.5 hour | Work with VPP, make a quick logic, alert message and dashboard UI with the Unity scene |
| 4 | 3.5 hour | Start to iterate obstacle detection techniques with Unity’s API and also basic visual and audio alerts.  |
| 5 | 2.5 hour | Experiment creative ideas from different perspective and with current resource limitation.  |
| 6 | 3 hours | Code clean up, documentation and wrap up for the challenge.  |

## 🚨 **Obstacle Detection Alert Experiments**

(For the overall Unity project, I will work on only one scene to experiment alerts. There’s no further need to use multiple scenes for this challenge)

1. Visual (text and image) warning on overhead display

When blocking car is detected, our system will show alerts by text based notification, such as “Changing Lane” and also suggest direction to turn, such as “<< Turn Left” or “Turn Right>>”. Besides text showing on the overhead display or windshield overlay, our system also shows arrow animation to visually indicate the suggested direction to turn. This moving arrow can be strongly enough to draw enough attention for the driver but still keep safe by adapting transparent color. On the opposite, when driver drives away the obstacle, our system will cancel the text and animation alerts but keep monitoring blocking cars.

![Screen Shot 2022-03-21 at 12.56.50 AM.png](Blocking%20C%20c08de/Screen_Shot_2022-03-21_at_12.56.50_AM.png)

1. Audio warning 

When obstacle is detected, our car will notify driver by the sound ‘Blocking car ahead’. When driver drives away the obstacle, the warning sound will then stop playback.( Here I use TTS (Text to Speech) service to generate ‘Blocking car ahead’ audio mp3 file)  

  

1. Beam visualization from the blocking side

Experiment from other car’s perspective, especially from the blocking car. What if the blocking car can actively alert other car by placing a beam on itself? Here I put a beam light on the blocking car, the idea is the blocking car ‘actively’ showing this beam alert through the inte-vehicle comm system so that we can easily visual the blocking event around us and even before our obstacle detecting system is enabled. 

![Screen Shot 2022-03-21 at 1.01.28 AM 2.png](Blocking%20C%20c08de/Screen_Shot_2022-03-21_at_1.01.28_AM_2.png)

1. Low-tech physical experiment: sound to vibrate notification

The rationale here is trying to accommodate my current hardware resource limitation, so that I aim to only use my own laptop to make tangible alert system possible. 

The idea is to leverage in-car speaker’s physical capability generating vibration under certain frequency (~60MHz). This ‘sound-to-vibrate’ alert system will then use the vibration to make physical particles jump. The jumping particles may produce physical, visual notification for the driver.

Imagine there’s a to-go coffee cup accessory which driver can ‘plug in’ into ‘sound-to-vibrate’ alert system. Our system can use vibration to make jumping coffee for warning visualization.

![Screen Shot 2022-03-21 at 1.14.06 AM (2).png](Blocking%20C%20c08de/Screen_Shot_2022-03-21_at_1.14.06_AM_(2).png)

 

1. Other ideas/ software or hardware stacks
    
    a. Use OSC (Open Sound Control) or WebSocket communication to control prototyping hardware such as Arduino, Pi. We can use Unity with OSC to control vibration, led lights, motors to make tangible alerts. 
    
    b. Make a virtual or physical assistant to give waving alert. Imagine a lucky cat sit on the dashboard, drive can see lucky cat waves her left hands to indicate warning and also suggested to turn left and vice versa. Research also suggests that more humanized objects can draw more attention to the human. 
    
    ![未命名的作品 2.jpg](Blocking%20C%20c08de/%E6%9C%AA%E5%91%BD%E5%90%8D%E7%9A%84%E4%BD%9C%E5%93%81_2.jpg)
    

## 📖 Detailed Process

Following are some more detailed process and code snippets through the process.

### Task 1 (0.5 hour)

When receiving the challenge, I start from understanding the brief, trying to clarify the ask. I then open the provided Unity template and start to get familiar with essential packages. It looks like this challenge will be highly based on the existing VPP (Vehicle Physical Pro) package for Car control. The most important thing is to setup goal, make a plan and anticipate potential steps.

### Task 2 (1 hour)

Get familiar with some vehicle’s technical term for the VPP package, e.g. Engine, Clutch, GearBox, Driveline, steering, brakes and so on. Mostly importantly I need to understand how to start the engine, drive the card with VPP since this is the first time I use this vehicle Unity package.

### Task 3 (0.5 hour)

1. The first and quick action is to just calculate two object’s distance and show alert by debug message only. This is to help me quickly get familiar the whole template project by just doing. Obviously this way wouldn’t be right since we need to consider the direction of the vehicle not just distance. 
2. Some creative ideas notes: in-car physical avatar assistant, warning from the blocking car’s perspective.  
3. Known Issue: only know distance in between but not knowing if two cars are in the same lane (hitting area) 

![Screen Shot 2022-03-14 at 2.50.31 AM.png](Blocking%20C%20c08de/Screen_Shot_2022-03-14_at_2.50.31_AM.png)

```csharp
////////////////////////////////////////////////////////////////////////////
// Ways to detect obstacle - #1: Vector3 distance (not so good way)
////////////////////////////////////////////////////////////////////////////
private void ScanObstacle()
{
    distanceToObstacle = Vector3.Distance(heroCar.transform.position, blockingCar.transform.position);
    if (distanceToObstacle < 30)
    {
        Debug.Log("** Alert to change lane!! **");
        _isObstacleDetected = true;
        // Show visual to indicate changing direction
        //txtLaneChange.gameObject.SetActive(true);
    }
    else{
        _isObstacleDetected = false;
    }
}
```

### Task 4 (3.5 hour)

1. Add a new script called *DetectObstacle.cs* with *VehicleObstacleDetect* Class
2. Implement *GetDetectStatus* member function
3. Integrate warning label into Dashboard component
4. Get status update from detector (using *VehicleObstacleDetect* namespace) object, such as

```csharp
private void ScanObstacle()
{
  distanceToObstacle = Vector3.Distance(heroCar.transform.position, blockingCar.transform.position);
  if (distanceToObstacle < 30)
  {
      Debug.Log("** Alert to change lane!! **");
      _isObstacleDetected = true;
      // Show visual to indicate changing direction
      //txtLaneChange.gameObject.SetActive(true);
  }
  else{
      _isObstacleDetected = false;
  }
}
```

1. Have some study about better detect obstacle, such as Raycast.hit, OverlapSphere, SphereCast, velocity obstacle, etc. 
2. Have more study on VPP package’s dashboard, UI source code. Adding warning label by leveraging VPP’s dashboard component. (*Dashboard.cs*)

![Screen Shot 2022-03-15 at 12.44.33 PM.png](Blocking%20C%20c08de/Screen_Shot_2022-03-15_at_12.44.33_PM.png)

1. Iterate to use *OverlapSphere* to detect obstacle.

![Screen Shot 2022-03-15 at 9.49.00 PM.png](Blocking%20C%20c08de/Screen_Shot_2022-03-15_at_9.49.00_PM.png)

```csharp
// private int radius = 10;

////////////////////////////////////////////////////////////////////////////
// Ways to detect obstacle - #2: OverlapSphere using radius with collider
////////////////////////////////////////////////////////////////////////////
private void ScanObstacle(Transform myCar, int radius)
{
    // Setup collider
    Collider[] cols = Physics.OverlapSphere(myCar.position, radius);
    foreach (Collider col in cols) 
    {
        if (col.tag.Equals("BlockingCar")){
            _isObstacleDetected = true;
        }
        else{
            _isObstacleDetected = false;
        }
    }
 }
```

The obstacle detection becomes better when we set up a radius area with the targeting obstacle.   

1.  Use *RayCastHit and hit info’s* normal to be more precisely detecting obstacles’s relative position, with this normal value I hope I can give some directions suggestion. 

```csharp
////////////////////////////////////////////////////////////////////////////
// Ways to detect obstacle - #3: RaycastHit using raycast to detect hit info
////////////////////////////////////////////////////////////////////////////

private void ScanObstacle(Transform myCar)
{
    // Ray ray = new Ray(heroCar.transform.position, heroCar.transform.forward * 10); 
    Debug.DrawRay(myCar.transform.position, myCar.transform.forward * 30, Color.green);
    RaycastHit hitInfo;
    if (Physics.Raycast(myCar.transform.position, myCar.transform.forward * 30, out hitInfo, 30))//
    {

        Debug.Log("hit point: " + hitInfo.point + " transform pos: " + hitInfo.transform.position + " collider: " + hitInfo.collider.gameObject);
        if (hitInfo.collider.gameObject.CompareTag("BlockingCar")){
            Debug.Log("Hit blocking car");
            Debug.DrawRay(myCar.transform.position, myCar.transform.forward * 30, Color.red);
            _isObstacleDetected = true; 
        }
        else{
            _isObstacleDetected = false;
        }
    }
}
```

 

1. Try the idea of Raycast’s normal to determine possible suggestion for direction
    1. Try get normal value and add suggested left and right direction result 
    2. Search some suitable 3D direction sign asset from Asset store, and also consider to make 3D sign by myself using Blender. However, due to time constraint I decided to grab one from Asset Store. 

```csharp
private void ScanObstacle(Transform myCar) 
{
    RaycastHit hitInfo;

    Debug.DrawRay(myCar.position, myCar.forward * 30, Color.green);
  
    if (Physics.Raycast(heroCar.transform.position, heroCar.transform.forward * 30, out hitInfo, 30)) 
    {
         // Only instersts in object tagging with BlockingCar
         if (hitInfo.collider.gameObject.CompareTag("BlockingCar")) 
         {
						// Intentionally use ebug dray ray to better visualize car's intersection
            Debug.DrawRay(myCar.position, myCar.forward * 30, Color.red);
            Debug.Log(hitInfo.normal.x);
            if (hitInfo.normal.x < 0)
            {
                // Suggest to turn left
                _directionToTurn = leftDirection;
            }
            else
            {
                // Suggest to turn right
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
// Get obstacle detection status
// Returns: 
//     bool true: obstacle detected
//     bool false: obstable not detected
public bool GetDetectStatus() 
{
    return _isObstacleDetected;
}
// Get suggested direction when detecting obstacle
// Returns: 
//     int 1: direction to left
//     int -1: direction to right
public int GetSuggestedDirection() 
{
    return _directionToTurn;
}
```

### Task 5 (2.5 hour)

Here I want to experiment some ideas other than basic visual, audio warning and to show alerts in either digital or physical ways. Also spend some time to imagine possible prototyping hardware, software stacks and possible application, such as lucky cat assistant.  

Due to limited hardware resource I have at the moment, I decided to just document some ideas may use extra prototyping electronics such as Arduino, Raspberry Pi, mobile phone or other communication software stacks such as OSC, WebSocket, etc. 

I make two experiments other than basic alerts, one is trying to experiment from other car’s perspective. What if the blocking car can actively alert other card by placing a beam on itself? The other experiment maybe a little wild, I want to see if I can use in-car speaker to give some physical visualization.  See “Obstacle Detection Alert Experiments” section above.

 

### Task 6 (2 hour)

Spend time to organize documentation and clean up, final review codes. Edit video for deliverable requirements. Documentation is always critical to serve as a tool for team communication, document feedbacks and retrospectives over time. 

To sum up for the Unity project I’ve developed, 

- I add 2 new scrips under *Assets/Script/DetectObstacle.cs*, *Assets/Script/LineBeamer.cs*.
- Import one arrow animation package called ‘Arrow Animations’.
- Add two audios under *Assets/Sound/*
- Make some changes on VPP’s *Dasboard.cs* for visual alerts.
- Associate LineBeamer script to Obstacle object for beam effect
- Add ObstacleDetector object with DetectObstacle script for obstacle detection
- Add some new object on Dashboard scrips that need attention from Unity editor’s drag-and-set.

![Screen Shot 2022-03-21 at 3.08.38 AM.png](Blocking%20C%20c08de/Screen_Shot_2022-03-21_at_3.08.38_AM.png)

## 📎 Notes

1. This Unity challenge project is only tested under Mac OS V11.5.2 (M1), Unity V2020.3.30f1
2. Project code can be also found via my GitHub [https://github.com/harryhow/Blocking-Car-Avoidance-Experiment](https://github.com/harryhow/Blocking-Car-Avoidance-Experiment)

### 🔗 Reference

1. Unity Documentation V2020.3 [https://docs.unity3d.com/Manual/index.html](https://docs.unity3d.com/Manual/index.html)
2. TTS [https://freetts.com/](https://freetts.com/)
3. C# coding style [https://google.github.io/styleguide/csharp-style.html](https://google.github.io/styleguide/csharp-style.html)