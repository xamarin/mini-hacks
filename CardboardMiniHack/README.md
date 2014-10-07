# Cardboard Monkey Mini-Hack

## General Instructions

This mini-hack can be carried out entirely from the MainActivity file. You'll use the following extra APIs from other classes (of which instances are available in the activity):

- `Game.IsLookingAtObject`: tells if the user is currently looking at the object
- `Game.HideObject`: hide the object in a different world position
- `CardboardOverlayView.Show3DToast`: display a toast-like message suitable for Cardboard display
- `Game.DistanceRatioFromObject`: an approximation of how far the camera is from the object
- `CardboardOverlayView.SetTemperatureColor`: set the color displayed by the temperature indicator at the top of the screen

## Part 1

The goal of the game is to find the monkey cube hidden somewhere in the world. As you can notice when you launch the application the first time, the monkey is always in front of you which makes it kind of boring.

We are going to use the Cardboard magnetic input (the metallic latch on the left side of your case) to validate the fact that the monkey has been found and place it somewhere else.

To respond to a user activating the switch, you need to override the `OnCardboardTrigger` method in the main activity (a subclass of `CardboardActivity`). In the starter project, we have already done this to give a some vibration feedback when the event occur.

Your first task is to augment this to:

- Display a 3D toast message when the latch is activated telling if the object has been found (i.e. the user is looking directly at it) or display some form of encouragement otherwise
- Keep track of the number of time the user has found the object and show it too in the previous toast message
- When successfully found, replace the object in a random location. The inline documentation for the `HideObject` method gives you some possible value hints.

## Part 2

To give the user some extra help, there is an round indicator that can be displayed on top of the cardboard screen to indicate how close the user is from finding the monkey with the `CardboardOverlayView.SetTemperatureColor` method.

You should use the value from the `Game.DistanceRatioFromObject` property (conveniently ranging between 0.0 and 1.0) to compute a meaningful color variation after each scene frame has finished drawing.
