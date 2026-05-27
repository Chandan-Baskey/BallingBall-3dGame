# 🎱 BallingBall 3D Game

<div align="center">

![Game View](https://github.com/Chandan-Baskey/BallingBall-3dGame/blob/0f33ba1b64ddb602acaac7e96a6102b130b8950a/GameView.jpg)

<br/>

![Unity](https://img.shields.io/badge/Engine-Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![3D](https://img.shields.io/badge/Type-3D%20Game-blue?style=for-the-badge)
![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20WebGL-orange?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

*A fast-paced 3D ball rolling game built with Unity — dodge walls, collect stars, and roll to victory!*

</div>

---

## 📖 Table of Contents

- [About the Game](#-about-the-game)
- [Screenshots](#-screenshots)
- [Gameplay](#-gameplay)
- [Star Rating System](#-star-rating-system)
- [Features](#-features)
- [Architecture & Scripts](#-architecture--scripts)
- [Script Deep Dive](#-script-deep-dive)
- [Scene & Level Structure](#-scene--level-structure)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Controls](#-controls)
- [How It Works](#-how-it-works)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎮 About the Game

**BallingBall 3D** is an arcade-style 3D ball rolling game built in Unity using C#. The player guides a ball down a narrow track, avoiding walls and reaching the finish line as cleanly as possible. Performance is graded by a **3-star rating system** based on how many walls the player hits during the run.

The game features:
- **Multi-level progression** — 3 handcrafted levels with a dedicated Level Select screen
- **Star-based scoring** — hit fewer walls, earn more stars
- **Pause / Resume system** — in-game menu to stop, resume, or exit
- **Physics-driven movement** — momentum-based steering with `Time.deltaTime` scaling
- **Full UI flow** — Main Menu → Level Select → Gameplay → Win/Loss → Next Level or Retry

---

## 📸 Screenshots

### 🏠 Starting Interface
![Starting Interface](https://github.com/Chandan-Baskey/BallingBall-3dGame/blob/d505cc2a098e5389c09a87c5dfd93b84c7d6e940/Starting%20Interface.jpg)

### 🎮 In-Game View
![In-Game View](https://github.com/Chandan-Baskey/BallingBall-3dGame/blob/d505cc2a098e5389c09a87c5dfd93b84c7d6e940/InGameView.jpg)

### 🏆 Win & Game Over Screen
![Win And Game Over](https://github.com/Chandan-Baskey/BallingBall-3dGame/blob/d505cc2a098e5389c09a87c5dfd93b84c7d6e940/WinAndOver.jpg)

---

## 🕹️ Gameplay

The ball automatically rolls **forward** along the Z-axis. The player steers left and right using `A` / `D` keys to stay on the track and avoid crashing into walls.

- Every **wall collision** increments an internal counter (`counter++`)
- The counter determines your **star rating** at the finish line
- Fall off the platform → **Game Over**
- Reach Z > 200 → **Win** (rated 1–3 stars)

---

## ⭐ Star Rating System

One of the game's standout features is performance-based grading at the end of each level.

| Wall Hits | Rating | Description |
|---|---|---|
| 0 – 2 | ⭐⭐⭐ 3 Stars | Perfect or near-perfect run |
| 3 – 4 | ⭐⭐ 2 Stars | Good run with minor mistakes |
| 5+ | ⭐ 1 Star | Completed but with many wall hits |

The star count is tracked live via `Player.getCounter()` and evaluated the moment the ball crosses Z = 200.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🏃 Auto-Forward Movement | Ball continuously rolls forward with physics force |
| ⬅️➡️ Lateral Steering | Steer left/right with `A` / `D` keys |
| ⭐ 3-Star Rating | Stars awarded based on wall hit count |
| 🧱 Wall Hit Counter | Tracks every collision with tagged "Wall" objects |
| 📷 Camera Follow | Offset-based camera locks onto the ball every frame |
| 📏 Distance Score | Real-time Z-position shown as score |
| 💀 Fall Detection | Game Over when ball drops below Y = -5 |
| ⏸️ Pause / Resume | In-game menu with Stop, Resume, Back to Main |
| 🗺️ Level Select | Choose from 3 levels via a dedicated UI screen |
| 🔁 Restart / Next Level | Restart current scene or load next build index |
| 🏠 Main Menu | Start Game, Level Select, Quit |

---

## 🏗️ Architecture & Scripts

```
Assets/Scripts/
│
├── 🎱  Core Gameplay
│   ├── Player.cs           — Ball physics, input, wall collision counter
│   ├── GameManager.cs      — Game state, star rating, win/lose, UI control
│   └── CameraScript.cs     — Camera follow system
│
└── 🗺️  Navigation & Menus
    ├── Menu.cs             — Main menu (Start, Level Select, Quit)
    └── LvL.cs              — Level Select screen (Levels 1–3, Home)
```

---

## 🔬 Script Deep Dive

---

### `Player.cs` — Ball Controller & Wall Counter

The core physics and input script. Also owns the **wall hit counter** used by the star rating system.

```csharp
[SerializeField] float conrollingForce;
int counter = 0;

ball.AddForce(-conrollingForce * Time.deltaTime, 0, 0); // Left
ball.AddForce( conrollingForce * Time.deltaTime, 0, 0); // Right
ball.AddForce(0, 0, ballForce * Time.deltaTime);         // Forward (always)
```

**Key Mechanics:**

- **`Time.deltaTime` scaling** — All forces are multiplied by `Time.deltaTime`, making movement **frame-rate independent**. The ball behaves consistently at 30fps or 120fps.
- **Separate force values** — `ballForce` controls the constant forward speed; `conrollingForce` controls lateral steering sensitivity — two independent tuning knobs.
- **Wall collision counter** — `OnCollisionEnter` listens for objects tagged `"Wall"`. Each hit increments `counter`. This is the single source of truth for the star rating system.
- **`getCounter()`** — A public getter exposes the counter to `GameManager` cleanly, keeping data encapsulated in the `Player` class.

**Design Note:** Using `AddForce` with `Time.deltaTime` in `Update()` (not `FixedUpdate()`) means the physics accumulates realistically. The ball gains speed gradually and retains momentum when the key is released — creating a skill-based feel.

---

### `GameManager.cs` — Game State & Star Rating System

The central game controller. Manages the full win/lose/pause flow and the 3-star evaluation logic.

```csharp
bool over = false;
bool win  = false;

counrt = ball.getCounter(); // Poll the wall hit counter every frame

if (ball.transform.position.z > 200 && !win)
{
    if      (counrt <= 2)              WinGame3Star();
    else if (counrt > 2 && counrt <= 4) WinGame2Star();
    else if (counrt > 4)               WinGame1Star();
}
```

**Key Mechanics:**

- **Dual state flags** (`over`, `win`) — Prevents multiple simultaneous triggers. Once either flag is set, no further condition checks fire.
- **Real-time counter polling** — `ball.getCounter()` is called every `Update()` frame, ensuring the star evaluation uses the freshest value at the exact moment the finish line is crossed.
- **Star screen GameObjects** — Three separate UI GameObjects (`win3Game`, `win2Game`, `win1Game`) are toggled with `SetActive(true)`. This approach allows designers to style each star screen independently in the Unity Editor.
- **Pause / Resume system** — `ShowMenu()` hides the ball and shows an overlay; `playCurrent()` reverses this, re-enabling the ball. No `Time.timeScale` is used — the ball simply becomes inactive.
- **Navigation methods:**
  - `RestartGame()` — Reloads the current scene by name
  - `nextLvL()` — Loads `buildIndex + 1`
  - `back()` — Loads scene `0` (Main Menu)
  - `stop()` → `ShowMenu()` — Pauses to in-game menu

**Full State Machine:**

```
Start
  │
  ▼
Update Loop ──────────────────────────────────────────┐
  │                                                    │
  ├── Y < -5 ──────────────────────────────► GameOver()
  │                                                    │
  └── Z > 200 ──► counter ≤ 2  ──────────► WinGame3Star()
                   counter 3-4 ──────────► WinGame2Star()
                   counter > 4 ──────────► WinGame1Star()
```

---

### `CameraScript.cs` — Ball Camera

A minimal but effective direct-follow camera.

```csharp
transform.position = ballPos.position - distanceFromBall;
```

**Key Mechanics:**

- Camera position is recalculated every `Update()` frame — **no smoothing**, resulting in a tightly locked follow that feels responsive in fast gameplay.
- `distanceFromBall` is a `Vector3`, giving full axis control: behind (`-Z`), above (`+Y`), and lateral (`X`) offset from the ball.
- No `LateUpdate()` — since the camera doesn't need to wait for physics resolution in this simpler 3D setup.

---

### `Menu.cs` — Main Menu Controller

Handles the three actions available from the main menu screen.

```csharp
public void startGame()   { SceneManager.LoadScene(2); }  // Jump straight to Level 1
public void quitGame()    { Application.Quit(); }          // Exit the application
public void LeveSelect()  { SceneManager.LoadScene(1); }  // Open Level Select screen
```

**Key Mechanics:**

- Scene index `2` = Level 1 (skips index `0` Main Menu and `1` Level Select).
- `Application.Quit()` works in standalone builds; has no effect in the Unity Editor (by design).
- Fully UI-driven — every method is assigned to a Button's `OnClick()` event in the Inspector.

---

### `LvL.cs` — Level Select Controller

Manages navigation from the Level Select screen.

```csharp
public void LvL1() { SceneManager.LoadScene(2); }
public void LvL2() { SceneManager.LoadScene(3); }
public void LvL3() { SceneManager.LoadScene(4); }
public void home() { SceneManager.LoadScene(0); }
public void startGame() { SceneManager.LoadScene(2); } // Quick-start alias
```

**Key Mechanics:**

- Each level maps directly to a **build index**: Level 1 = 2, Level 2 = 3, Level 3 = 4.
- `home()` always returns to scene `0` — the Main Menu.
- `startGame()` is a duplicate of `LvL1()` — useful for a "Play" shortcut button that skips level selection.

---

## 🗺️ Scene & Level Structure

| Build Index | Scene | Description |
|---|---|---|
| `0` | Main Menu | Start Game, Level Select, Quit |
| `1` | Level Select | Choose Level 1, 2, or 3 |
| `2` | Level 1 | First playable level |
| `3` | Level 2 | Second playable level |
| `4` | Level 3 | Third playable level |

---

## 📁 Project Structure

```
BallingBall-3dGame/
│
├── Assets/
│   ├── Scripts/
│   │   ├── Player.cs
│   │   ├── GameManager.cs
│   │   ├── CameraScript.cs
│   │   ├── Menu.cs
│   │   └── LvL.cs
│   │
│   ├── Scenes/
│   │   ├── MainMenu        (index 0)
│   │   ├── LevelSelect     (index 1)
│   │   ├── Level1          (index 2)
│   │   ├── Level2          (index 3)
│   │   └── Level3          (index 4)
│   │
│   ├── Prefabs/
│   ├── Materials/
│   └── UI/
│
├── GameView.jpg
├── Starting Interface.jpg
├── InGameView.jpg
├── WinAndOver.jpg
├── README.md
└── .gitignore
```

---

## 🚀 Getting Started

### Prerequisites

- [Unity Hub](https://unity.com/download) installed
- Unity **2021.3 LTS** or later recommended
- Git

### Clone the Repository

```bash
git clone https://github.com/Chandan-Baskey/BallingBall-3dGame.git
cd BallingBall-3dGame
```

### Open in Unity

1. Open **Unity Hub**
2. Click **Add** → select the cloned project folder
3. Open the project with your installed Unity version
4. In **File → Build Settings**, verify scene order matches the [Scene Structure](#-scene--level-structure) table
5. Navigate to `Assets/Scenes/` → open the **MainMenu** scene
6. Press ▶️ **Play** to run the game

---

## 🎮 Controls

| Key | Action |
|---|---|
| `A` | Steer ball left |
| `D` | Steer ball right |
| *(automatic)* | Ball rolls forward continuously |

> All UI navigation (Start, Restart, Next Level, Back) is handled via on-screen buttons.

---

## ⚙️ How It Works

### Physics Flow

```
Update() every frame
    │
    ├── A key held  → AddForce(-conrollingForce × deltaTime, 0, 0)
    ├── D key held  → AddForce(+conrollingForce × deltaTime, 0, 0)
    └── Always      → AddForce(0, 0, ballForce × deltaTime)
                              │
                              ▼
                    Rigidbody Physics Simulation
                              │
                              ▼
                    Camera mirrors ball position
                              │
                              ▼
                    GameManager polls Z position + counter
```

### Win Evaluation Flow

```
ball.position.z > 200 && !win
    │
    ├── counter ≤ 2  →  ⭐⭐⭐  WinGame3Star()
    ├── counter 3–4  →  ⭐⭐    WinGame2Star()
    └── counter > 4  →  ⭐     WinGame1Star()
                │
                ▼
        ball.SetActive(false)
        win UI panel.SetActive(true)
```

### Full UI Navigation Flow

```
[Main Menu]
    ├── Start Game   → Level 1 (Scene 2)
    ├── Level Select → Level Select Screen (Scene 1)
    └── Quit         → Application.Quit()

[Level Select]
    ├── Level 1 → Scene 2
    ├── Level 2 → Scene 3
    ├── Level 3 → Scene 4
    └── Home    → Main Menu (Scene 0)

[In-Game]
    ├── Stop (Pause) → ShowMenu overlay
    │       ├── Resume   → playCurrent()
    │       └── Back     → Main Menu (Scene 0)
    ├── Win  → Star screen → Restart / Next Level
    └── Lose → Game Over  → Restart / Back
```

---

## 🤝 Contributing

Contributions are welcome! Here's how:

1. **Fork** the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add: your feature description"`
4. Push to your branch: `git push origin feature/your-feature-name`
5. Open a **Pull Request**

### Ideas for Contribution
- 🎵 Add background music and SFX for wall hits, win, and loss
- 💾 Persistent star ratings per level using `PlayerPrefs`
- 📱 Mobile swipe / tilt controls
- ✨ Particle effects on wall collision and win
- 🌍 More levels and obstacle types
- 🏆 Global leaderboard integration

---

## 👤 Author

**Chandan Baskey**

[![GitHub](https://img.shields.io/badge/GitHub-Chandan--Baskey-181717?style=for-the-badge&logo=github)](https://github.com/Chandan-Baskey)

---

## 📄 License

This project is licensed under the **MIT License** — free to use, modify, and distribute.

---

<div align="center">

Made with ❤️ and Unity

⭐ **Star this repo if you found it helpful!** ⭐

</div>
