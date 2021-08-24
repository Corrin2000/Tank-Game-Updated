using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using Engine;

namespace MyGame
{
    class Main_Game : State
    {
        static Random rand = new Random();
        int IncreaseSpawnTimerInMS = 60000;
        int SpawnPowerupTimerInMS = 25000;
        int SpawnHealthTimerInMS = 50000;

        int EnemyHealthBonusTimer = 3600;

        Dictionary<EnemyTypes, System.Timers.Timer> TimerSpawnEnemyDict = new Dictionary<EnemyTypes, System.Timers.Timer>();

        readonly Dictionary<PowerupTypes, string> PowerupTypeToName = new Dictionary<PowerupTypes, string> {
            { PowerupTypes.SpreadShot,  PowerupTypes.SpreadShot.ToString()  },
            { PowerupTypes.SpikeShot,   PowerupTypes.SpikeShot.ToString()   },
            { PowerupTypes.MachineShot, PowerupTypes.MachineShot.ToString() },
            { PowerupTypes.NoneShot,    "" }
        };

        Dictionary<EnemyTypes, int> EnemyTypesToSpawnSpeedInMS = new Dictionary<EnemyTypes, int> {
            {EnemyTypes.Helicopter, 5000 },
            {EnemyTypes.Mine, 8000 },
            {EnemyTypes.Missile, 15000 },
            {EnemyTypes.Fort, 30000 }
        };

        public override void Create()
        {
            base.Create();

            foreach(EnemyTypes EnemyType in Enum.GetValues(typeof(EnemyTypes)))
            {
                CreateTimerSpawnEnemy(EnemyType);
            }

            CreateAndEnableTimer(IncreaseEnemySpawnSpeedCallback, IncreaseSpawnTimerInMS);
            CreateAndEnableTimer(CreatePowerupCallback, SpawnPowerupTimerInMS);
            CreateAndEnableTimer((sender, e) => ObjectManager.AddGameObject(new Heart()), SpawnHealthTimerInMS);

            // Create a background
            GameObject background = new GameObject("Background", 1024, 7680, "BackgroundLevel.png");
            background.Position.Y = 3456;
            background.ZOrder = -10;
            ObjectManager.AddGameObject(background);

            GameObject Health = new GameObject("Health", 96, 32, "Health.png", 4, 1, 4, 0.1f);
            Health.Position.Y = 368;
            Health.Position.X = -460;
            ObjectManager.AddGameObject(Health);

            ObjectManager.AddGameObject(Tank.TankInstance);

            InitGameObjectsOnStartup();

            TextObject Score = TextManager.AddText("Score", "Score: ", FontTypes.Arial32);
            Score.Position.Y = 368;
            Score.Position.X = 360;
            //Engine.Graphics.DrawCollisionData(true);
        }

        private void CreateAndEnableTimer(System.Timers.ElapsedEventHandler InCallbackMethod, int InInterval)
        {
            var TimerSpawnEnemy = new System.Timers.Timer();
            TimerSpawnEnemy.Elapsed += InCallbackMethod;
            TimerSpawnEnemy.Interval = InInterval;
            TimerSpawnEnemy.Enabled = true;
        }


        #region Timer Callbacks
        private void CreateTimerSpawnEnemy(EnemyTypes InEnemyType)
        {
            var TimerSpawnEnemy = new System.Timers.Timer();
            TimerSpawnEnemy.Interval = IncreaseSpawnTimerInMS;
            TimerSpawnEnemy.Elapsed += (sender, e) => { SpawnEnemyOutOfView(InEnemyType); };
            TimerSpawnEnemy.Enabled = true;

            //System.Threading.Timer TimerSpawnEnemy = new System.Threading.Timer((state) => SpawnEnemyOutOfView((EnemyTypes)state), 
            //    InEnemyType, EnemyTypesToSpawnSpeedInMS[InEnemyType], EnemyTypesToSpawnSpeedInMS[InEnemyType]);
            TimerSpawnEnemyDict.Add(InEnemyType, TimerSpawnEnemy);
        }

        void CreatePowerupCallback(object sender, EventArgs e)
        {
            CreatePowerup();
        }

        private void IncreaseEnemySpawnSpeedCallback(object sender, EventArgs e)
        {
            //Reduce all the spawn speeds to 90% the previous value
            foreach (EnemyTypes EnemyType in Enum.GetValues(typeof(EnemyTypes)))
            {
                EnemyTypesToSpawnSpeedInMS[EnemyType] = (int)(0.9 * EnemyTypesToSpawnSpeedInMS[EnemyType]);
                TimerSpawnEnemyDict[EnemyType].Interval = EnemyTypesToSpawnSpeedInMS[EnemyType];
            }
        }
        #endregion

        public override void Update()
        {
            //reference objects
            GameObject health = ObjectManager.GetObjectByName("Health");

            base.Update();

            //timer decrements
            --EnemyHealthBonusTimer;

            //timer checks
            if (EnemyHealthBonusTimer <= 0)
            {
                //ShootingEnemy.MaxHealth *= 2;
                //EnemyCompound.MaxHealth *= 2;
                //HomingEnemy.MaxHealth *= 2;
                EnemyHealthBonusTimer = 7200;
            }

            // Check if the player is still alive
            if (Tank.TankInstance.PlayerHealth > 0)
            {
                TextObject Score = TextManager.GetTextObjectByName("Score");
                Score.Text = "Score: " + Tank.TankInstance.Score.ToString();
                

                // This works because you can only have 1, 2, or 3 health
                health.AnimationData.GoToFrame(3 - (uint)Tank.TankInstance.PlayerHealth); 
            }
            else
            {
                int FinalScore = Tank.TankInstance.Score;

                health.AnimationData.GoToFrame(3);

                //death
                GameObject GameOverText = new GameObject("GameOver", 1024, 768, "GameOver.png");
                GameOverText.Position.Y = Camera.Position.Y - 50;
                GameOverText.ZOrder = 20;
                ObjectManager.AddGameObject(GameOverText);

                GameObject RestartButton = new RestartButton();
                RestartButton.Position.Y = Camera.Position.Y - 200;
                ObjectManager.AddGameObject(RestartButton);

                GameObject EndGameCursor = new cursor();
                EndGameCursor.Position.Y = Tank.TankInstance.Position.Y;
                ObjectManager.AddGameObject(EndGameCursor);

                TextObject TotalScore = TextManager.AddText("FinalScore", "Score: ", FontTypes.Arial64);
                TotalScore.Text = "Final Score: " + FinalScore.ToString();
                TotalScore.Position.X -= 125; // = -125 ?
                TotalScore.Position.Y = Camera.Position.Y;
                Tank.TankInstance.PlayerHealth = -1;
            }



            if (InputManager.IsTriggered(Keys.Escape))
            {
                Game.quit = true;
            }
        }

        void CreatePowerup()
        {
            ObjectManager.RemoveAllObjectsByName("spreadShot");
            ObjectManager.RemoveAllObjectsByName("spikeBall");
            ObjectManager.RemoveAllObjectsByName("machineGun");
            PowerupTypes PowerupType = (PowerupTypes)rand.Next(1, Enum.GetNames(typeof(PowerupTypes)).Length); //0 is NoneType
            ObjectManager.AddGameObject(new Powerup(PowerupTypeToName[PowerupType], PowerupType));
        }

        private void SpawnEnemyOutOfView(EnemyTypes TypeToSpawn)
        {
            while (true)
            {
                GameObject g = SpawnEnemyFactory(TypeToSpawn);
                if (!g.IsInViewport())
                {
                    ObjectManager.AddGameObject(g);
                    break;
                }
            }
        }

        private GameObject SpawnEnemyFactory(EnemyTypes TypeToSpawn)
        {
            switch(TypeToSpawn)
            {
                case EnemyTypes.Helicopter:
                    return new Helicopter();
                case EnemyTypes.Mine:
                    return new Mine();
                case EnemyTypes.Missile:
                    return new Missile();
                case EnemyTypes.Fort:
                    return new EnemyFort();
                default:
                    return null;
            }
        }

        private void InitGameObjectsOnStartup()
        {
            SpawnEnemyOutOfView(EnemyTypes.Helicopter);
            SpawnEnemyOutOfView(EnemyTypes.Mine);
            SpawnEnemyOutOfView(EnemyTypes.Mine);
            SpawnEnemyOutOfView(EnemyTypes.Mine);
            SpawnEnemyOutOfView(EnemyTypes.Mine);
            CreatePowerup();
        }
    }
}
