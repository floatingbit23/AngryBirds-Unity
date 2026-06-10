**🇪🇸 Español** | [🇬🇧 English](README_EN.md)

# 🐦 Angry Birds

>[!info]
>Este juego es un clon simple del clásico videojuego [Angry Birds](https://es.wikipedia.org/wiki/Angry_Birds_(videojuego)), y fue creado con la intención de aprender el lenguaje de programación **C#** a través del desarrollo de videojuegos con el motor gráfico **Unity**.

## Descripción

Desarrollado en **Unity 6**.

## 🎮 Mecánicas implementadas

- **Tirachinas funcional** con físicas 2D y animación elástica (_DOTween_).
- **Sistema de daño** basado en la velocidad de impacto sobre los cerdos.
- **Gestión de cámara** con _Cinemachine_ (cámara _idle_ y cámara de seguimiento).
- **Sistema de turnos** con número limitado de lanzamientos (3 por defecto).
- **Efectos de sonido** para el tirachinas, las colisiones y las muertes de los cerdos.
- **Múltiples niveles** con transición al siguiente al ganar.
- **UI de iconos** que refleja los lanzamientos restantes.

## 🗂️ Estructura del proyecto

```
Assets/
├── Prefabs/         # AngryBird, Baddie, bloques de construcción, partículas
├── Scenes/          # MainScene, Scene2
├── Scripts/         # 9 scripts en C#
├── Sounds/          # Efectos de sonido
└── Sprites/         # Sprites (imagenes 2D) del juego
```

### Scripts principales

| Script | Responsabilidad |
|---|---|
| `GameManager.cs` | _Singleton_ (1 sola instancia) que controla el flujo del juego: lanzamientos, victoria y reinicio. |
| `SlingShotHandler.cs` | Lógica del tirachinas: dibujo de las cuerdas elásticas, apuntado y lanzamiento del pájaro. |
| `AngryBird.cs` | Comportamiento del pájaro: lanzamiento, rotación en vuelo y colisión. |
| `Baddie.cs` | Salud del cerdo enemigo: recibir daño y morir. |
| `InputManager.cs` | Captura de input del ratón (_New Input System_) |
| `CameraManager.cs` | Transiciones entre cámara estática y cámara de seguimiento de escena. |
| `SlingShotArea.cs` | Detección de si el cursor está dentro del área de acción del tirachinas. |
| `SoundManager.cs` | _Singleton_ para reproducir los efectos de sonido. |
| `IconHandler.cs` | Actualización visual de los iconos de lanzamientos restantes. |


## Requisitos

- **Unity 6** (6000.0.25f1 o superior)
- **DOTween** (via _Asset Store_ de Unity)
- **Cinemachine** (via _Package Manager_ de Unity)
- **Input System** (via _Package Manager_ de Unity)

## Cómo ejecutarlo

1. Abrir el proyecto con Unity Hub.
2. Abrir `Assets/Scenes/MainScene.unity`.
3. Pulsar ▶️ Play en el editor.
