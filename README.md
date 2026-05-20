# 🎱 BallingBall 3D Game

<div align="center">

![Game View](https://github.com/Chandan-Baskey/BallingBall-3dGame/blob/0f33ba1b64ddb602acaac7e96a6102b130b8950a/GameView.jpg)

<br/>

![Unity](https://img.shields.io/badge/Engine-Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![3D](https://img.shields.io/badge/Type-3D%20Game-blue?style=for-the-badge)
![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20WebGL-orange?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

*A fast-paced 3D ball rolling game built with Unity — dodge obstacles, survive the course, and roll to victory!*

</div>

---

## 📖 Table of Contents

- [About the Game](#-about-the-game)
- [Gameplay](#-gameplay)
- [Features](#-features)
- [Architecture & Scripts](#-architecture--scripts)
- [Script Deep Dive](#-script-deep-dive)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Controls](#-controls)
- [How It Works](#-how-it-works)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎮 About the Game

BallingBall 3D is an endless-runner-style 3D ball game built in Unity using C#. The player controls a rolling ball navigating through an obstacle-filled track. The goal is to survive as long as possible, travel the farthest distance, and beat your own high score — all while the ball continuously rolls forward.

The game draws inspiration from classic mobile ball-rolling games with a focus on:
- Simple one-button input — accessible to all skill levels
- Physics-based movement — realistic momentum and force application
- Score tracking — distance-based scoring system
- Clean, modular code — easy to extend and maintain

---

## 🕹 Gameplay

The ball automatically rolls forward along the Z-axis. The player steers left and right using A / D keys to avoid falling off platforms or hitting obstacles.

The game ends when the ball either:
- Falls below the platform (Y position < -5)
- Travels beyond the finish line (Z position > 300)

A Game Over screen appears with the option to restart.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🏃 Auto-Forward Movement | Ball continuously moves forward without player input |
| ⬅️➡️ Lateral Steering | Player steers left/right using keyboard |
| 📷 Smooth Camera Follow | Camera smoothly trails the ball with a configurable offset |
| 📏 Distance-Based Score | Score updates in real time based on Z-axis position |
| 💀 Fall Detection | Game ends if the ball falls off the platform |
| 🏁 Finish Line | Game ends when the ball reaches the goal distance |
| 🔁 Restart System | Instantly restart from the Game Over screen |

---

## 🏗 Architecture & Scripts

The project is split into two independent game systems — a 3D ball game and a 2D platformer — sharing the same repository. Below is the full breakdown.

Assets/Scripts/
│
├── 🎱  3D Ball Game Scripts
│   ├── Player.cs           — Ball physics & input handling
│   ├── GameManager.cs      — Score, game over, restart logic
│   └── CameraScript.cs     — Simple camera follow system
│
└── 🕹  2D Platformer Scripts
    ├── PlayerControl.cs    — 2D character movement & wall detection
    ├── GameControl.cs      — Respawn, checkpoints, scene loading
    ├── Checkpoint.cs       — Checkpoint activation & sprite swap
    ├── Portal.cs           — Teleportation system
    └── CameraControl.cs    — Smooth 2D camera with clamping

---

## 🔬 Script Deep Dive

### 🎱 3D Ball Game

---

#### Player.cs — Ball Controller

The core input and physics script. Attached to the ball Rigidbody.

public class Player : MonoBehaviour
{
    Rigidbody ball;
    public float ballForce;
    ...
    ball.AddForce(0, 0, ballForce); // Constant forward force
}
Key Mechanics:
- Uses Unity's Rigidbody.AddForce() for physics-based movement — no direct transform manipulation, ensuring natural momentum and friction.
- A / D keys apply lateral forces; the ball doesn't stop instantly, giving it realistic inertia.
- A constant forward force is applied every Update() frame, making the ball auto-roll forward throughout the game.

Design Note: Using AddForce instead of setting velocity directly means the ball accumulates speed over time. Tuning ballForce controls both acceleration and top speed feel.

---

#### GameManager.cs — Game State Controller

Manages score display, win/lose conditions, and scene reloading.

scoreText.text = ball.transform.position.z.ToString("0");

if (ball.transform.position.y < -5 && !over) GameOver();
if (ball.transform.position.z > 300 && !over) GameOver();

Key Mechanics:
- Score is derived directly from the ball's Z position, converting world-space distance into a readable integer score.
- Two loss conditions are checked every frame:
  - Fall detection: Y < -5 — ball has left the platform.
  - Distance cap: Z > 300 — optional finish line / max distance trigger.
- The over boolean flag prevents GameOver() from being called multiple times in the same frame.
- RestartGame() calls SceneManager.LoadScene() with the current scene name — a lightweight scene reload with no additional state to clean up.

---

#### CameraScript.cs — Ball Camera

A minimal but effective camera follow system.

transform.position = ballPos.position - distanceFromBall;

Key Mechanics:
- Sets camera position every Update() frame — no smoothing, resulting in a locked, direct follow.
- distanceFromBall is a Vector3, allowing full control over camera offset in all three axes (behind, above, and to the side).
- Simple and performant — ideal for fast-moving gameplay where lag-free tracking is preferred.

---

### 🕹 2D Platformer System

---

#### PlayerControl.cs — 2D Character Movement

A sophisticated 2D player controller with acceleration, deceleration, platform riding, and wall-flip logic.

speedMultiplier = Mathf.MoveTowards(speedMultiplier, target, acceleration * Time.fixedDeltaTime);
float targetSpeed = speed * speedMultiplier * dir;

Key Mechanics:
- Hold-to-move: Input.GetMouseButton(0) — player holds the screen/mouse to accelerate.
- Acceleration curve: speedMultiplier smoothly ramps from 0 → 1 using Mathf.MoveTowards, avoiding instant full-speed snapping.
- Direction from scale: Movement direction is inferred from transform.localScale.x sign — negative = left, positive = right. This elegantly ties movement to the visual facing direction.
- Wall detection: Physics2D.OverlapBox() checks a small area in front of the player. On wall contact, Flip() is called to reverse direction — creating an auto-bouncing mechanic.
- Moving platforms: If isOnPlatform is true, platformRb.velocity.x is added to the player's velocity so they move naturally with the platform.

---

#### GameControl.cs — 2D Respawn & Scene Management

Handles player death, respawning, checkpoint memory, and scene transitions.

IEnumerator Respawn(float duration)
{
    transform.localScale = new Vector3(0, 0, 0);
    yield return new WaitForSeconds(duration);
    transform.position = checkpointPos;
    transform.localScale = new Vector3(1f, 1f, 1f);
}

Key Mechanics:
- Death animation: On hitting an Obstacle tag, the player is scaled to (0,0,0) — effectively vanishing — before being repositioned at the last checkpoint after a 0.5s delay. Smooth, zero-dependency "death" effect.
- Checkpoint memory: checkpointPos stores the last activated checkpoint position. Defaults to the player's starting position on Start().
- Scene loading: Touching a Finish tag loads buildIndex + 1, enabling sequential level progression.

---

#### Checkpoint.cs — Checkpoint System

Activates checkpoints on player contact, with visual feedback and one-time triggering.

gameController.UpdateCheckpoint(respawnPoint.position);
spriteRenderer.sprite = active;   // Swap to active sprite
coll.enabled = false;             // Prevent re-triggering
Key Mechanics:
- Uses a dedicated respawnPoint Transform (child object) rather than transform.position — allowing precise respawn positioning separate from the checkpoint's visual location.
- Visual state change: Sprite swaps from passive → active on activation, giving the player clear feedback.
- Self-disabling collider: After activation, the Collider2D is disabled — the checkpoint can only be triggered once per run.

---

#### Portal.cs — Teleportation System

Handles bidirectional portals with loop prevention.

private HashSet<GameObject> portalObjects = new HashSet<GameObject>();

destinationPortal.portalObjects.Add(collision.gameObject);
collision.transform.position = destination.position;

Key Mechanics:
- Loop prevention: HashSet<GameObject> tracks objects that just exited a portal. On entry, if the object is in the set, teleportation is skipped — preventing the object from being immediately re-teleported by the destination portal.
- Cross-portal communication: The script fetches the Portal component from the destination and registers the object in *its* set, ensuring the arrival-side portal also ignores the just-teleported object.
- On OnTriggerExit2D, objects are removed from the set, re-enabling normal portal behavior for future entries.

---

#### CameraControl.cs — 2D Smooth Camera

A polished 2D camera with smooth damping and world boundary clamping.

Vector3 targetPosition = target.position + positionOffset;
targetPosition = new Vector3(
    Mathf.Clamp(targetPosition.x, xLimits.x, xLimits.y),
    Mathf.Clamp(targetPosition.y, yLimits.x, yLimits.y),
    -10
);
transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

Key Mechanics:
- Vector3.SmoothDamp() creates organic, lag-free camera following — adjustable via the smoothTime slider (0 = instant, 1 = very slow).
- xLimits / yLimits clamp the camera within level boundaries, preventing the camera from showing empty space beyond the map edges.
- Camera Z is hardcoded to -10 — the standard orthographic camera depth in Unity 2D.
- Runs in LateUpdate() to ensure it reads the player's final position after physics and input have been processed that frame.

---

## 📁 Project Structure

BallingBall-3dGame/
│
├── Assets/
│   ├── Scripts/
│   │   ├── Player.cs
│   │   ├── GameManager.cs
│   │   ├── CameraScript.cs
│   │   ├── PlayerControl.cs
│   │   ├── GameControl.cs
│   │   ├── Checkpoint.cs
│   │   ├── Portal.cs
│   │   └── CameraControl.cs
│   │
│   ├── Scenes/
│   ├── Prefabs/
│   ├── Materials/
│   └── UI/
│
├── GameView.jpg
├── README.md
└── .gitignore

---

## 🚀 Getting Started

### Prerequisites

- [Unity Hub](https://unity.com/download) installed
- Unity 2021.3 LTS or later recommended
- Git

### Clone the Repository

git clone https://github.com/Chandan-Baskey/BallingBall-3dGame.git
cd BallingBall-3dGame

### Open in Unity

1. Open Unity Hub
2. Click Add → select the cloned project folder
3. Open the project with your installed Unity version
4. In the Project panel, navigate to Assets/Scenes/
5. Double-click the main scene to open it
6. Press ▶️ Play to run the game

---

## 🎮 Controls

### 3D Ball Game

| Key | Action |
|---|---|
| A | Move ball left |
| D | Move ball right |
| *(automatic)* | Ball rolls forward |

### 2D Platformer

| Input | Action |
|---|---|
| Hold Mouse / Touch | Accelerate & move in current direction |
| *(automatic)* | Character flips direction on wall contact |

---

## ⚙️ How It Works

### Physics Flow (3D)

Update() Input
    │
    ▼
AddForce (Left / Right / Forward)
    │
    ▼
Rigidbody Physics Simulation
    │
    ▼
Camera Follows Ball Position
    │
    ▼
GameManager Checks Score & Conditions
    │
    ├── Y < -5       → GameOver()
    └── Z > 300      → GameOver()

### Respawn Flow (2D)

Player hits Obstacle tag
    │
    ▼
Die() → StartCoroutine(Respawn(0.5f))
    │
    ▼
Scale → (0,0,0)  [disappear effect]
    │
    ▼
Wait 0.5 seconds
    │
    ▼
Teleport to checkpointPos
    │
    ▼
Scale → (1,1,1)  [reappear]

### Checkpoint Flow (2D)

Player enters Checkpoint trigger
    │
    ▼
UpdateCheckpoint(respawnPoint.position)
    │
    ├── Swap Sprite: passive → active
    └── Disable Collider (one-time trigger)

---

## 🤝 Contributing

Contributions are welcome! Here's how:

1. Fork the repository
2. Create a feature branch: git checkout -b feature/your-feature-name
3. Commit your changes: git commit -m "Add: your feature description"
4. Push to your branch: git push origin feature/your-feature-name
5. Open a Pull Request

### Ideas for Contribution
- 🎵 Add background music and SFX
- 🌟 Particle effects on ball roll / death
- 🏆 Persistent high score with PlayerPrefs
- 📱 Mobile touch controls
- 🎨 New obstacle types and level designs

---

## 👤 Author

Chandan Baskey

[![GitHub](https://img.shields.io/badge/GitHub-Chandan--Baskey-181717?style=for-the-badge&logo=github)](https://github.com/Chandan-Baskey)

---

## 📄 License

This project is licensed under the MIT License — free to use, modify, and distribute.

---

<div align="center">

Made with ❤️ and Unity

⭐️ Star this repo if you found it useful! ⭐️

</div>
