# Space Invaders

I've tried to emulate the original style. The screen represents the resolution (224x256) and proportions of the original arcade machine. The way the enemies move and the positioning of every element on the screen uses a regular grid — as in the original game — and trying to achieve a "pixel-perfect" appearance. Also, all the elements are rendered white. The coloring (red or green) is applied using an overlay that colors everything below it, similar to the original arcade machines, which had a colored layer glued over portions of the screen. 

## New mechanics

Here's where we try to make it even more fun. I've created three additional mechanics: the *Guided Missile* (L), the *Pulse Shield* (C), and the *Omega Ray* (R).

<img alt="GuidedMissile" src="https://user-images.githubusercontent.com/40273816/125150580-34b97980-e117-11eb-966a-148d2ea807f1.gif" width=33%> <img alt="PulseShield" src="https://user-images.githubusercontent.com/40273816/125150610-764a2480-e117-11eb-8720-c55718e9b7c8.gif" width=33%> <img alt="OmegaRay" src="https://user-images.githubusercontent.com/40273816/125150585-3f740e80-e117-11eb-81a4-fb01d7600001.gif" width=33%>

### Guided Missile

A secondary — more fun — weapon. A fire-and-forget missile that allows the player to hunt down enemies while safe under a shelter. The missiles are slower than the regular player's laser cannon but can take down enemies anywhere on the screen. 

The player has a limited supply of missiles. That supply is periodically replenished once a certain number of regular shots is fired. Just be careful not to fire too many missiles at once. They can collide and destroy each other.

### Pulse Shield


This is a defensive mechanic. The player can deploy an energy shield in front of him to block enemy fire and even destroy enemies that are very close.  


The Pulse Shield has a short duration and a long recharge time. Also, the shield stays in the position where it was deployed and doesn't follow the player.

### Omega Ray

The definitive alien killer weapon. This ray destroys everything on its path. However, once activated, the player is immobilized until the ray's long charging time is completed.

The Omega Ray is powerful but forces the player to be vulnerable to incoming enemy fire until the weapon activation finishes.

## Key bindings

| Action              | Key         |
|---------------------|-------------|
| Move right          | Right arrow |
| Move left           | Left arrow  |
| Fire Laser Cannon   | Spacebar    |
| Fire Guided Missile | Left Ctrl   |
| Fire Omega Ray      | Right Ctrl  |
| Deploy Pulse Shield | Up arrow    |
