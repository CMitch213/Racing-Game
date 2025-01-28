# Racing-Game
#### (Work in progress title)

<br>

## Description
This is a simple physics based racing game that I have been working on for a few months. It was made with C# and the Unity Game Engine. I did the coding, however the assets were not created by me. This game right now is only playable with a controller or a racing wheel but I might eventually add keyboard support. 
Install a build of the game here:
https://cmitch213.itch.io/racing-game

<br>

## Features
- Manual Transmission
- Handbraking
- Curise Control
- Toggleable headlights
- Simulated RPM and Exhaust sound
- Realistic Power and Speed
- Controller Support
- Wheel Support (Work in progress)
- Random Map / Vehicle Selection
- Toggleable FOV


  ### Cars
  - Chevrolet Camaro ZL1
  - Audi RS6 Avant
  - Porsche 911 Dakar
  - Porsche 911 Carrera (930)
  - Ford Raptor R
  - Lightning McQueen
  - NA Mazda Miata
  - Generic Motorcycle (Very buggy)
  - CaseIH Tractor
 
 
  ### Tracks
  - 1/4 Mile Drag Strip
  - Nurburgring
  - Oval Race Track
  - Rainbow Road from Mario Kart
  - Toad's Harbour from Mario Kart

<br>

## Controls
|  **Control**  | **Input (Controller)** | **Input (Keyboard** |
|-------------- | ---------------------- | ------------------- |
| Drive         | Right Trigger          | N/A                 |
| Brake         | Left Trigger           | N/A                 |
| Steer         | Left Stick             | N/A                 |
| Shift Up      | Right Bumper           | N/A                 |
| Shift Down    | Left Bumper            | N/A                 |
| Handbrake     | A (Xbox) X (PS)        | N/A                 |
| Headlights    | Y (Xbox) Tri (PS)      | N/A                 |
| Ignition      | Left D-Pad             | N/A                 |
| CruiseControl | Right D-Pad            | N/A                 |
| Horn          | Up D-Pad               | N/A                 |
| Zoom Out      | N/A                    | ,                   |
| Zoom In       | N/A                    | .                   |

  * Racing wheel currently a work in progress

<br>

## Issues
 - [x] Weird glitch where accelerator gets stuck if you're accelerating too fast
 - [ ] Racing wheel only sort of works
 - [x] Audio too loud
 - [ ] [#1](https://github.com/CMitch213/Racing-Game/issues/1)
 - If you find an issue report it on the issues section of the github

<br>

## How to work on this game yourself
Feel free to download the file, and open it as a new project in unity. Everything should work out of the box. Just don't claim it as your own. (:
It was made in Unity 2022.3.24

<br>

## Complex Stuff

<details>

<summary> Complex stuff that is in the code and not fully documented. </summary>

## RPM Calculations

### For auto
This is the simpler one

**rpm = $(kph * (90 / gear * 2)) + car.idleRPM$**
- You start with your speed bc you want the faster you drive to be the more rpm
- $* 90 / gear * 2$ makes it so the lower the gear you are in the higher your rpm
- $+ car.idleRPM$ makes it so you do not go below your idle rpms


### For Manual
This one is a doozy

**targetRPM = $((kph / car.topSpeed * gearRatios[gearNum-1]^2 * 6000) + car.idleRPM)$**
**rpm = $Mathf.Lerp(rpm, targetRPM, Time.deltaTime)$

- you slowly Lerp your rpm to your target so it is a smooth transition over time
- $speed / car.topSpeed$ so it is dependent on how close you are to your top speed
- $* gearRatios^2$ so your gear ratios have a severe effect on what rpm you are in
- $* 6000$ bc otherwise it would be bad
- $+ car.idleRPM$ makes it so you do not go below your idle rpms

any other question you can DM me on discord at CMitch

</details>
