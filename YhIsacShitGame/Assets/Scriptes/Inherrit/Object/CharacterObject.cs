using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using YhProj;

public class CharacterObject : BaseObject
{
    [SerializeField]
    private NavMeshAgent agent;
    CharacterData characterData;

    public override void Load<T>(T _baseData)
    {

    }

    public override void Delete()
    {

    }

}
