using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YhProj;
using YhProj.Game.Map;
using YhProj.Game.UI;
using YhProj.Game.Character;
using YhProj.Game;

public class GameManager : MonoBehaviour
{
    IFactory factory;

    private void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchFactory<T>() where T : IFactory
    {
        factory = new CharacterFactory();
    }
}
