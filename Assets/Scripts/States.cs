using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class States
{
    public enum InputType { MouseKey, Controller };
    public enum MenuSection { Main, Play, Options, Help, Credits, Quit, Team };
    public enum GameMode { Game, UI, Cinematic };
    public enum GameGenre { None, Platformer, Shooter, RPG };
    public enum VolumeType { Control, Transition, Damage };
}
