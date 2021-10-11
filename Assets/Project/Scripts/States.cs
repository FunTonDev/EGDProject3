using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class States
{
    public enum InputType { MouseKey, Controller };
    public enum MenuSection { Main, Play, Options, Help, Credits, Quit };
    public enum PlayerMode { Game, UI, Cinematic };
    public enum CameraMode { Platformer, Shooter, RPG };
    public enum GameGenre { None, Platformer, Shooter, RPG };
}
