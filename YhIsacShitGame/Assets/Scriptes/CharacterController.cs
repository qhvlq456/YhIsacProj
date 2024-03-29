using System.Collections;
using System.Collections.Generic;

namespace YhProj.Game.Character
{
    public abstract class CharacterController<T> : IController where T : CharacterObject
    {
        protected CharacterManager manager;

        protected List<T> charObjectList = new List<T>();
        protected IFactory factory;

        public CharacterController(CharacterManager _manager, IFactory _factory)
        {
            manager = _manager;
            factory = _factory;

            Initialize();
        }
        public void Initialize()
        {

        }

        public virtual void Update()
        {

        }
        public abstract T LoadCharacter(CharacterData _charData);

        public abstract void DeleteCharacter(CharacterData _charData);
        public void Dispose()
        {
            for (int i = 0; i < charObjectList.Count; i++)
            {
                charObjectList[i].Delete();
            }

            charObjectList.Clear();
        }
    }
    public sealed class HeroController : CharacterController<HeroObject>
    {
        public HeroController(CharacterManager _manager, IFactory _factory) : base(_manager, _factory) { }
        public override HeroObject LoadCharacter(CharacterData _charData)
        {
            HeroObject heroObj = factory.Create<HeroObject>(_charData);
            charObjectList.Add(heroObj);

            return heroObj;
        }
        public override void DeleteCharacter(CharacterData _charData)
        {
            throw new System.NotImplementedException();
        }
    }
    public sealed class EnemyController : CharacterController<EnemyObject>
    {
        public EnemyController(CharacterManager _manager, IFactory _factory) : base(_manager, _factory) { }
        public override EnemyObject LoadCharacter(CharacterData _charData)
        {
            EnemyObject enemyObj = factory.Create<EnemyObject>(_charData);
            charObjectList.Add(enemyObj);

            return enemyObj;
        }
        public override void DeleteCharacter(CharacterData _charData)
        {
            throw new System.NotImplementedException();
        }
    }
}
