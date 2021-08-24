using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;

namespace MyGame
{
    public enum EnemyTypes
    {
        Helicopter,
        Missile,
        Mine,
        Fort
    }

    public struct EnemyPrototype
    {
        public int ScoreValueOnDeath;
        public int MaxHealth;
    }


    public abstract class EnemyBase : GameObject
    {
        public static Dictionary<EnemyTypes, EnemyPrototype> prototypes = new Dictionary<EnemyTypes, EnemyPrototype>{
            { EnemyTypes.Helicopter, new EnemyPrototype{ ScoreValueOnDeath = 1, MaxHealth = 1 } },
            { EnemyTypes.Mine, new EnemyPrototype{ ScoreValueOnDeath = 1, MaxHealth = 1 } },
            { EnemyTypes.Missile, new EnemyPrototype{ ScoreValueOnDeath = 2, MaxHealth = 1 } },
            { EnemyTypes.Fort, new EnemyPrototype{ ScoreValueOnDeath = 3, MaxHealth = 2 } }
        };


        static protected Random rand = new Random();

        protected int ScoreValueOnDeath;
        protected int CurrentHealth;
        public int MaxHealth;

        protected bool HasBeenKilled = false;

        public EnemyBase(EnemyTypes InEnemyType, uint objectWidth_, uint objectHeight_, string textureName_, 
            uint totalFrames_ = 1, uint numberOfRows_ = 1, uint numberOfColumns_ = 1, float frameLifetime_ = 1)
            : base(InEnemyType.ToString(), objectWidth_, objectHeight_, textureName_, totalFrames_, numberOfRows_, numberOfColumns_, frameLifetime_)
        {
            EnemyPrototype p = prototypes[InEnemyType];
            ScoreValueOnDeath = p.ScoreValueOnDeath;
            MaxHealth = p.MaxHealth;
            CurrentHealth = MaxHealth;

            CollisionData.CollisionEnabled = true;

            InitializePosition();
        }

        protected abstract void InitializePosition();

        public override void Update()
        {
            base.Update();

            if (CurrentHealth == 0)
            {
                HandleOnDeath();
            }
        }

        protected virtual void HandleOnDeath()
        {
            if (HasBeenKilled == false)
            {
                Tank.TankInstance.Score += ScoreValueOnDeath;
            }
            HasBeenKilled = true;
            IsDead = true;
        }
    }
}
