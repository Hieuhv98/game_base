using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : ScriptableObject
{
    private static Config _instance;
    public static Config Instane => _instance ? _instance : Resources.Load<Config>(Constant.CONFIG_PATH);

    [SerializeField] private bool isTest;
    [SerializeField] private GameObject levelTest;

    public static bool IsTest { set => Instane.isTest = value; get => Instane.isTest; }
    public static GameObject LevelTest { set => Instane.levelTest = value; get => Instane.levelTest; }
}
