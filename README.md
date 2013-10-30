Space Blok XNA
==============

Space Blok is a 1-4 player game where the goal is to gather as much points as
possible by destroying game bloks. The game utilises the open-source 
BEPUphysics library to provide 3D simulation and realistic collision handling 
for the game objects.

This project is hosted in GitHub:
https://github.com/nokia-developer/space-blok-wp

For more information about the game, see the wiki pages:
https://github.com/nokia-developer/space-blok-wp/wiki

This project is compatible with Windows Phone 7 and Windows Phone 8.


1. Building and deploying with Microsoft Visual Studio
-------------------------------------------------------------------------------

1. Open the Solution file (.sln): File > Open Project, select the file
   `Blok.sln`
2. Select the target, either emulator or device.
3. Press F5 to build the project and run it in the selected target. If the
   selected target is device, the app will deployed on the phone.


2. Running the game
-------------------------------------------------------------------------------

Launch Space Blok on your device. Select New Game on the main menu to start 
the game.

Place the phone on the table between the players so that one corner of the
phone is towards each player. The balls can be shot by swiping on top of each
platform. The direction and the velocity of the shot depends on the direction
and the length of the swipe. Each shot will have a slight trajectory towards 
the camera to make it possible to hit the closest bloks. The bloks closest to 
the center of the compound are stronger and need severals hits before they are 
destroyed. The outer bloks only require a single hit.

You may pause the game by clicking the Back button during the game. The game 
is resumed by tapping Resume. You may also exit to the main menu by tapping 
Main menu, but this will end the current game.

The game ends when all of the bloks are destroyed. The Results view is then 
shown with scores for each player. Returning to the main menu is done by 
clicking the Back button.

You may look at the info view with some tips about the game by tapping Info in 
the main menu.


3. Known issues
-------------------------------------------------------------------------------

Sound effects are not implemented.


4. License
-------------------------------------------------------------------------------

See the license text file delivered with this project. The license file is 
also available online at:
https://github.com/nokia-developer/space-blok-wp/blob/master/Licence.txt

The BEPUphysics library is hosted at http://bepu.squarespace.com/ and licensed 
under Apache 2.0. The Apache 2.0 license file is available online at
https://github.com/nokia-developer/space-blok-wp/blob/master/bepu_license.txt
and http://bepuphysics.codeplex.com/license.


5. Version history
-------------------------------------------------------------------------------

* Version 1.0: The first version.
