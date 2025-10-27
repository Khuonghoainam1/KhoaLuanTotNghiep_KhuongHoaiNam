using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneName
{
    SplashScene,
    MenuScene,
    GameScene,
    HomeScene,
    BossWorldScene,
    OperaKingMiniGame
}

public class MiniGameScene
{
    public static List<SceneName> scenes = new List<SceneName> { SceneName.OperaKingMiniGame };
}