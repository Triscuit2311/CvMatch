# CvMatch
### Template Matching for any window, in C#.

This project was an exploration of DXGI and OpenCv, intended as a base for some HID development projects. I've used this core for some automation projects as well, and I've left in some example code.

We copy the DXGI buffer information and format it in a way that OpenCV can perform template matching, you can see the custom template matching functionality in [CvSearch.cs](https://github.com/Triscuit2311/CvWindowScanner/blob/master/CvWindowScanner/Modules/CvSearch.cs).

The example here is for the game *'Escape From Tarkov'*, and implements a (quite useless) bot, that repeadly starts a match, and kills the player. This example makes use of all the currently implemented functionality: Template Matching; both for game states and object location and window-specific state machine callbacks (Syncronous and Async).

As you can see in [Program.cs](https://github.com/Triscuit2311/CvWindowScanner/blob/master/CvWindowScanner/Program.cs), setting up a new window is easy. You just need to provide the callbacks, gamestates information, and set the (optional) thread sleep time. (Note that the DXGICapturer always runs in it's own thread.)

While this was built with game windows as the primary targets, it works for anything that's on the screen.
