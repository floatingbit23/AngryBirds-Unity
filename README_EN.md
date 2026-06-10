**🇬🇧 English** | [🇪🇸 Español](README.md)

# 🐦 Angry Birds

> [!NOTE]
> This game is a simple clone of the classic videogame [Angry Birds](https://en.wikipedia.org/wiki/Angry_Birds_(video_game)), and was created with the purpose of learning the **C#** programming language through game development with the **Unity** game engine.

![frame](/images/gameplay.png)

## Description

Built with **Unity 6**.

## 🎮 Implemented mechanics

- **Functional slingshot** with 2D physics and elastic animation (_DOTween_).
- **Damage system** based on the impact velocity on the pigs.
- **Camera management** with _Cinemachine_ (_idle_ camera and follow camera).
- **Turn-based system** with a limited number of shots (3 by default).
- **Sound effects** for the slingshot, collisions and pig deaths.
- **Multiple levels** with transition to the next one upon winning.
- **Icon UI** showing remaining shots.

## 🗂️ Project structure

```
Assets/
├── Prefabs/         # AngryBird, Baddie, building blocks, particles
├── Scenes/          # MainScene, Scene2
├── Scripts/         # 9 C# scripts
├── Sounds/          # Sound effects
└── Sprites/         # 2D sprites
```

### Main scripts

| Script | Responsibility |
|---|---|
| `GameManager.cs` | _Singleton_ (single instance) that controls game flow: shots, win and restart. |
| `SlingShotHandler.cs` | Slingshot logic: elastic band drawing, aiming and bird launching. |
| `AngryBird.cs` | Bird behaviour: launch, in-flight rotation and collision. |
| `Baddie.cs` | Enemy pig health: taking damage and dying. |
| `InputManager.cs` | Mouse input capture (_New Input System_). |
| `CameraManager.cs` | Transitions between static camera and scene follow camera. |
| `SlingShotArea.cs` | Detection of whether the cursor is within the slingshot action area. |
| `SoundManager.cs` | _Singleton_ for playing sound effects. |
| `IconHandler.cs` | Visual update of remaining shot icons. |


## Requirements

- **Unity 6** (6000.0.25f1 or higher)
- **DOTween** (via Unity Asset Store)
- **Cinemachine** (via Unity Package Manager)
- **Input System** (via Unity Package Manager)

## How to run

1. Open the project with Unity Hub.
2. Open `Assets/Scenes/MainScene.unity`.
3. Press ▶️ Play in the editor.
