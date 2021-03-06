# Space Invaders

I've tried to emulate the original style of the arcade game. The screen represents the resolution (224×256) and proportions of the original arcade machine. The way the enemies move and the positioning of every element on the screen uses a regular grid — as in the original game — and trying to achieve a “pixel-perfect” appearance. Also, all the elements are rendered white. The coloring (red or green) is applied using an overlay that colors everything below it, similar to the original arcade machines, which had a colored layer glued over portions of the screen. All is done just for educational purposes.

## New mechanics

Here's where we try to make it even more fun. I've created three additional mechanics: the *Guided Missile* (L), the *Pulse Shield* (C), and the *Omega Ray* (R).

<img alt="GuidedMissile" src="https://user-images.githubusercontent.com/40273816/125726689-2ef67782-4717-4ab3-9ba8-306da127b206.gif" width=32%> <img alt="PulseShield" src="https://user-images.githubusercontent.com/40273816/125150610-764a2480-e117-11eb-8720-c55718e9b7c8.gif" width=32%> <img alt="OmegaRay" src="https://user-images.githubusercontent.com/40273816/125150585-3f740e80-e117-11eb-81a4-fb01d7600001.gif" width=32%>

### Guided Missile

A secondary — more fun — weapon. A fire-and-forget missile that allows the player to hunt down enemies while safe under a shelter. The missiles are slower than the regular player's laser cannon, but can take down enemies anywhere on the screen. 

The player has a limited supply of missiles. Firing a certain number of regular shots replenishes the missiles. Just be careful not to fire too many missiles at once. They can collide and destroy each other.

### Pulse Shield

This is a defensive mechanic. The player can deploy an energy shield in front of him to block enemy fire and even destroy enemies that are very close.  

The Pulse Shield has a short duration and a long recharge time. Also, the shield stays in the position where it was deployed and doesn't follow the player.

### Omega Ray

The definitive alien killer weapon. This ray destroys everything on its path. However, once activated, the player is immobilized until the ray's long charging and firing action completes.

The Omega Ray is powerful, but forces the player to be vulnerable to incoming enemy fire and should be correctly timed. You can fire it from behind a shelter, but then you won't have that shelter anymore.

## Key bindings

| Action              | Key         |
|---------------------|-------------|
| Move right          | Right arrow |
| Move left           | Left arrow  |
| Fire Laser Cannon   | Space bar   |
| Fire Guided Missile | Left Ctrl   |
| Fire Omega Ray      | Right Ctrl  |
| Deploy Pulse Shield | Up arrow    |

## Assets

I've found a few assets throughout the internet. Most of the sprites of the invaders I managed to get [from a sprite sheet](https://www.deviantart.com/gooperblooper22/art/Space-Invaders-Sprite-Sheet-135338373). The sprites of the new mechanics and the rest of the explosions I've made myself. I was also able to find [audio assets](https://www.classicgaming.cc/classics/space-invaders/sounds), but they're not implemented in the game yet.

## References

So far, most of the information I've got about the original game mechanics and quirks comes from the following sources:
- [Player guide for Space Invaders](https://www.classicgaming.cc/classics/space-invaders/play-guide)
- [Emulating the Space Invaders look and feel](https://tobiasvl.github.io/blog/space-invaders/)
- [Wikipedia entry](https://en.wikipedia.org/wiki/Space_Invaders)

The References folder contains mostly screenshots from YouTube videos.
