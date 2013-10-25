Space Blok XNA
==============

Space Blok is a 1-4 player game where the goal is to gather as much points as
possible by destroying game bloks. The game utilises the open-source 
BEPUphysics library to provide 3D simulation and realistic collision handling 
for the game objects.

For more information about the game, see the Space Blok XNA project pages at
https://github.com/nokia-developer/space-blok-wp


PREREQUISITIES
-------------------------------------------------------------------------------

- C# basics
- Development environment 'Microsoft Visual Studio 2010 Express for Windows
  Phone'

  
KNOWN ISSUES
-------------------------------------------------------------------------------

Sound effects are not implemented.

  
BUILD & INSTALLATION INSTRUCTIONS
-------------------------------------------------------------------------------

Preparations
~~~~~~~~~~~~

Make sure you have the following installed:
 * Windows 7
 * Microsoft Visual Studio 2010 Express for Windows Phone
 * The Windows Phone Developer Tools January 2011 Update:
   http://download.microsoft.com/download/6/D/6/6D66958D-891B-4C0E-BC32-2DFC41917B11/WindowsPhoneDeveloperResources_en-US_Patch1.msp
 * Windows Phone Developer Tools Fix:
   http://download.microsoft.com/download/6/D/6/6D66958D-891B-4C0E-BC32-2DFC41917B11/VS10-KB2486994-x86.exe

Build on Microsoft Visual Studio
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

1. Open the SLN file:
   File > Open Project, select the file BubbleLevel.sln
2. Select the 'Windows Phone 7 Emulator' target.
3. Press F5 to build the project and run it on the Windows Phone Emulator.


Deploy to Windows Phone 7
~~~~~~~~~~~~~~~~~~~~~~~~~

Preparations:
1. Register in the App Hub to get a Windows Live ID:
   http://create.msdn.com/en-us/home/membership
2. Install Zune for Windows Phone 7:
   http://www.zune.net/en-us/products/windowsphone7/default.htm
3. Register your device in your Windows Live account. 
   Select from Windows: Start > Windows Phone Developer Tools > Windows Phone 
   Developer Registration

Deploy:
1. Open the SLN file:
   File > Open Project, select the file Blok.sln
2. Connect the device to Windows via USB.
3. Select the 'Windows Phone 7 Device' target.
4. Press F5 to build the project and run it on your Windows device.

    
RUNNING THE APPLICATION
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


COMPATIBILITY
-------------------------------------------------------------------------------

- Windows Phone 7

Tested on: 
- HTC 7 Mozart
- LG Optimus 7 

Developed with:
- Microsoft Visual Studio 2010 Express for Windows Phone


LICENSE
-------------------------------------------------------------------------------

See the license text file delivered with this project. The license file is 
also available online at:
https://github.com/nokia-developer/space-blok-wp/blob/master/Licence.txt

The BEPUphysics library is hosted at http://bepu.squarespace.com/ and licensed 
under Apache 2.0. The Apache 2.0 license file is available online at
https://github.com/nokia-developer/space-blok-wp/blob/master/bepu_license.txt
and http://bepuphysics.codeplex.com/license.


CHANGE HISTORY
-------------------------------------------------------------------------------

* Version 1.0: The first version.