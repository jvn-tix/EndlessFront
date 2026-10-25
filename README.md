## Developer & Contributions
Jevon (Game Programmer)

## About
Endless Front is a fast-paced action shooter where players move, aim, and shoot their way through waves of enemies. Chasing enemies close in, shooter enemies attack from range, and every hit is backed by juicy feedback like hit stop, camera shake, squash reactions, and floating damage numbers. Survive the onslaught, rack up your score, and stay alive as long as you can.

<br>

## Key Features

**Responsive Shooting System**: Aim and fire projectiles with muzzle flash feedback, bullet projectiles, and satisfying impact effects on every hit.

**Smart Enemy Variety**: Face enemies with different behaviors, from melee chasers that hunt you down to shooters that keep their distance and fire back. An enemy spawner keeps the pressure on.

**Game Feel & Juice**: Hit stop, camera shake, hit squash animations, and damage popups make every hit feel impactful and readable.

**Atmospheric Visuals**: Parallax scrolling backgrounds and flickering lamps give the world depth and mood.

## Scene Flow
```mermaid
flowchart LR
  mm[Main Menu]
  gp[Gameplay]
  go[Game Over]

  mm -- "Start Game" --> gp
  gp -- "Player Dies" --> go
  go -- "Retry" --> gp
  go -- "Main Menu" --> mm
  gp -- "Pause > Quit" --> mm
```

## Layer / Module Design
```mermaid
---
config:
  theme: neutral
  look: neo
---
graph TD
    subgraph "Game Systems"
        SM[Scene Manage]
        AM[Audio Manager]
        Score[Score Manager]
        Menu[Gameplay Menu UI]
    end

    subgraph "Player Systems"
        PM[Player Movement]
        PS[Player Shooting]
        PH[Player Health]
        Muzzle[Muzzle Flash]
    end

    subgraph "Enemy Systems"
        Spawner[Enemy Spawner]
        Chase[Enemy Chase AI]
        Shooter[Enemy Shooter AI]
        EH[Enemy Health]
    end

    subgraph "Combat & Feedback"
        Bullet[Bullet Projectile]
        HitStop[Hit Stop]
        Shake[Camera Shake]
        Squash[Hit Squash]
        Popup[Damage Popup]
    end

    subgraph "UI Systems"
        HealthUI[Health UI]
        EnemyBar[Enemy Health Bar]
    end

    subgraph "Environment"
        Parallax[Parallax Layer]
        Lamp[Lamp Flicker]
    end

    PS --> Bullet
    PS --> Muzzle
    PS --> Shake
    Bullet --> EH
    Bullet --> PH

    Spawner --> Chase
    Spawner --> Shooter
    Shooter --> Bullet
    Chase --> PH

    EH --> EnemyBar
    EH --> Score
    EH --> Popup
    EH --> Squash
    EH --> HitStop

    PH --> HealthUI
    PH --> Shake
    PH --> SM

    Menu --> SM
    AM -.-> PS
    AM -.-> EH

    classDef gameStyle fill:#e1f5fe,stroke:#01579b,stroke-width:2px
    classDef playerStyle fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    classDef enemyStyle fill:#ffebee,stroke:#b71c1c,stroke-width:2px
    classDef fxStyle fill:#fff3e0,stroke:#e65100,stroke-width:2px
    classDef uiStyle fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    classDef envStyle fill:#eceff1,stroke:#37474f,stroke-width:2px

    class SM,AM,Score,Menu gameStyle
    class PM,PS,PH,Muzzle playerStyle
    class Spawner,Chase,Shooter,EH enemyStyle
    class Bullet,HitStop,Shake,Squash,Popup fxStyle
    class HealthUI,EnemyBar uiStyle
    class Parallax,Lamp envStyle
```

## Modules and Features

The action gameplay, enemy AI, combat feedback, and UI are powered by a modular Unity C# scripting system organized into `Player`, `Enemy`, and `Game` folders.

| 📂 Name | 📁 Folder | 📋 Responsibility |
|---------|-----------|-------------------|
| **PlayerMovement** | **Player** | - Handle player movement input<br/>- Control speed and direction<br/>- Handle facing and ground/air logic |
| **PlayerShooting** | **Player** | - Handle aiming and fire input<br/>- Spawn bullet projectiles<br/>- Manage fire rate and cooldown<br/>- Trigger muzzle flash and shoot effects |
| **PlayerHealth** | **Player** | - Track player health (current/max)<br/>- Handle damage and invincibility<br/>- Trigger death and game over<br/>- Update health UI |
| **MuzzleFlash** | **Player** | - Display muzzle flash on shooting<br/>- Control flash duration and visibility |
| **EnemyChaseAI** | **Enemy** | - Pursue the player<br/>- Deal contact damage<br/>- Handle movement toward target |
| **EnemyShooterAI** | **Enemy** | - Keep distance from the player<br/>- Fire projectiles at the player<br/>- Manage attack cooldown |
| **EnemyHealth** | **Enemy** | - Track enemy health points<br/>- Handle damage from player bullets<br/>- Trigger death, score, and cleanup |
| **EnemyHealthBar** | **Enemy** | - Display enemy health bar<br/>- Update bar fill in real time |
| **EnemySpawner** | **Enemy** | - Spawn enemies over time<br/>- Control spawn positions and rates<br/>- Manage enemy types and wave pacing |
| **HitSquash** | **Enemy** | - Play squash and stretch reaction on hit<br/>- Give impact feedback to the player |
| **AudioManager** | **Game** | - Play music and sound effects<br/>- Manage volume and audio clips |
| **BulletProjectile** | **Game** | - Move projectiles and detect collisions<br/>- Apply damage on hit<br/>- Handle lifetime and cleanup |
| **CameraShake** | **Game** | - Shake the camera on impacts<br/>- Control intensity and duration |
| **DamagePopup** | **Game** | - Show floating damage numbers<br/>- Animate and fade out popups |
| **GameplayMenuUI** | **Game** | - Handle pause and game over menus<br/>- Manage resume, retry, and quit buttons |
| **HealthUI** | **Game** | - Display player health in the HUD<br/>- Update when health changes |
| **HitStop** | **Game** | - Briefly freeze time on impact<br/>- Add weight to hits |
| **SceneManage** | **Game** | - Load and reload scenes<br/>- Handle transitions between menu and gameplay |
| **ScoreManager** | **Game** | - Track and display score<br/>- Add points when enemies are defeated |
| **ParallaxLayer** | **Root** | - Scroll background layers at different speeds<br/>- Create depth in the scene |
| **LampFlicker** | **Root** | - Randomly vary lamp light intensity<br/>- Add atmosphere to the environment |

<br>

## Game Flow Chart
```mermaid
---
config:
  theme: redux
  look: neo
---
flowchart TD
  start([Game Start])
  start --> move{Player Input}

  move -->|WASD| walk[Move Character]
  move -->|Mouse| aim[Aim Weapon]
  move -->|Left Click| shoot[Shoot Bullet]
  move -->|Esc| pause[Pause Menu]

  shoot --> muzzle[Muzzle Flash + Sound]
  muzzle --> fire[Spawn Bullet Projectile]
  fire --> hit{Bullet Hits Enemy?}

  hit -->|Yes| dmg[Enemy Takes Damage]
  hit -->|No| miss[Bullet Expires]

  dmg --> fx[Hit Stop + Squash + Damage Popup]
  fx --> dead{Enemy Health = 0?}
  dead -->|Yes| score[Add Score + Remove Enemy]
  dead -->|No| move
  score --> move

  start --> spawn[Enemy Spawner]
  spawn --> type{Enemy Type}
  type -->|Chaser| chase[Chase Player]
  type -->|Shooter| ranged[Shoot Player]

  chase --> hazard{Player Hit?}
  ranged --> hazard
  hazard -->|Yes| hurt[Player Takes Damage + Camera Shake]
  hazard -->|No| move

  hurt --> hp{Health > 0?}
  hp -->|Yes| move
  hp -->|No| gameover[Game Over]

  gameover --> retry{Retry?}
  retry -->|Yes| start
  retry -->|No| menu[Main Menu]
```

<br>

## Event Signal Diagram
```mermaid
classDiagram
    %% --- Player Systems ---
    class PlayerShooting {
        +OnShoot()
        +OnReload()
    }

    class PlayerHealth {
        +OnHealthChanged(current: int, max: int)
        +OnDamageTaken(amount: int)
        +OnDeath()
    }

    class PlayerMovement {
        +OnMove(direction: Vector2)
        +OnJump()
    }

    %% --- Enemy Systems ---
    class EnemySpawner {
        +OnEnemySpawned(enemy: GameObject)
    }

    class EnemyHealth {
        +OnHealthChanged(current: int, max: int)
        +OnDamageTaken(amount: int)
        +OnDeath()
    }

    class EnemyChaseAI {
        +OnTargetReached()
    }

    class EnemyShooterAI {
        +OnFire()
    }

    %% --- Game Systems ---
    class BulletProjectile {
        +OnHit(target: GameObject)
    }

    class ScoreManager {
        +OnScoreChanged(score: int)
    }

    class HitStop {
        +OnHitStopStart()
        +OnHitStopEnd()
    }

    class CameraShake {
        +OnShakeStart(intensity: float)
    }

    class SceneManage {
        +OnSceneLoad(name: string)
    }

    %% --- Relations ---
    PlayerShooting --> BulletProjectile : creates
    EnemyShooterAI --> BulletProjectile : creates
    BulletProjectile --> EnemyHealth : damages
    BulletProjectile --> PlayerHealth : damages

    EnemySpawner --> EnemyChaseAI : spawns
    EnemySpawner --> EnemyShooterAI : spawns
    EnemyChaseAI --> PlayerHealth : damages

    EnemyHealth --> ScoreManager : adds score
    EnemyHealth --> HitStop : triggers
    PlayerShooting --> CameraShake : triggers
    PlayerHealth --> CameraShake : triggers
    PlayerHealth --> SceneManage : on death
```

<br>

## Play The Game
<a href="YOUR_ITCH_IO_OR_GAME_LINK">Play Now</a>

<br>

## Installation & Setup
1. Clone this repository
2. Open the project in Unity (version used in your project recommended)
3. Open the main gameplay scene
4. Press Play to start testing

## Controls
- **WASD** - Movement
- **Mouse** - Aim
- **Left Click** - Shoot
- **Escape** - Pause Menu

## Credits
Game developed as a Game Programming Project Individually.
