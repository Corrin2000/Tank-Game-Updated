using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Windows.Forms;
using System.Drawing;


namespace MyGame
{
    class Tank : GameObject
    {
        //Singleton
        private static readonly Tank _TankInstance = new Tank();

        public static Tank TankInstance
        {
            get
            {
                return _TankInstance;
            }
        }
        
        const int _MaxHealth = 3;
        const int _TankSpeed = 4;

        private int _score = 0;
        private int _PlayerHealth = 3;
        private PowerupTypes _Power;
        private int _PowerTotal = 200;
        private int _PowerLevel = 1;
        int _CanBeHurtTimer = 0;

        //bool first = true;
        int counter = 0;
        bool upframe = false;

        Dictionary<PowerupTypes, int> _AmmoPowerTotals = new Dictionary<PowerupTypes, int> {
            { PowerupTypes.MachineShot, 100},
            { PowerupTypes.SpreadShot,  25},
            { PowerupTypes.SpikeShot,   25}
        };

        private Tank()
            : base("TankBase", 128, 128, "TankBottom.png", 3, 1, 3, .05f)
        {
            CollisionData.CollisionEnabled = true;
            CollisionData.SetCollisionData(32);
            ZOrder = 1;
            AnimationData.GoToFrame(0);
            
            //_Power = PowerupTypes.SpreadShot;

            ObjectManager.AddGameObject(new Gun());

            //SoundManager.AddSoundEffect("move", "tank-move.mp3");
            //SoundManager.PlaySoundEffect("move");
        }


        public override void Update()
        {
            // # Cheatz #
            if (InputManager.IsTriggered(Keys.K))
            {
                PowerLevel++;
                _AmmoPowerTotals.TryGetValue(Power, out _PowerTotal);
            }

            base.Update();
            --CanBeHurtTimer;
            TextManager.RemoveTextObjectByName("power" + (counter - 1));

            int TempPowerUsedPercent;
            if (_AmmoPowerTotals.TryGetValue(Power, out TempPowerUsedPercent))
            {
                //Fix this enum naming. Dictionary?
                TextObject PowerTypeText = TextManager.AddText("power" + counter, "Current Power Is: " + Power.ToString(), FontTypes.Arial32);
                PowerTypeText.Position = new PointF(-500, 350 + Camera.Position.Y);
                PowerTypeText.SetColor(1, 0, 1);

                TextManager.RemoveTextObjectByName("powerRemain" + (counter - 1));
                TextObject PowerPercentRemainText = TextManager.AddText("powerRemain" + counter, "Power Percent Remaining: " + (100 * ((double)PowerTotal / TempPowerUsedPercent)), FontTypes.Arial32);
                PowerPercentRemainText.Position = new PointF(-500, 300 + Camera.Position.Y);
                PowerPercentRemainText.SetColor(1, 0, 1);
            }
            else
            {
                TextManager.RemoveTextObjectByName("powerRemain" + counter);
                TextManager.RemoveTextObjectByName("powerRemain" + (counter + 1));
                TextManager.RemoveTextObjectByName("powerRemain" + (counter - 1));
            }
            counter++;
            if (upframe == true)
            {
                AnimationData.Play();
            }
            else
            {
                AnimationData.Stop();
            }
            upframe = false;
            //move tank up
            //SoundManager.PauseSoundEffect("move");
            if (PlayerHealth > 0)
            {
                /*if (first == true)
                {
                    SoundManager.PlaySoundEffect("move");
                    first = false;
                }
                SoundManager.UnpauseSoundEffect("move");
                */

                #region Keypress
                //movement
                if (InputManager.IsPressed(Keys.W) && Position.Y <= 7234)
                {
                    upframe = true;
                    Position.Y += _TankSpeed;
                    Rotation = 0;
                }
                if (InputManager.IsPressed(Keys.S) && Position.Y >= -320)
                {
                    upframe = true;
                    Position.Y -= _TankSpeed;
                    Rotation = 180;
                }
                if (InputManager.IsPressed(Keys.D) && Position.X <= 450)
                {
                    upframe = true;
                    Position.X += _TankSpeed;
                    Rotation = 270;
                }
                if (InputManager.IsPressed(Keys.A) && Position.X >= -450)
                {
                    upframe = true;
                    Position.X -= _TankSpeed;
                    Rotation = 90;
                }
                if (InputManager.IsPressed(Keys.D) && InputManager.IsPressed(Keys.W))
                {
                    upframe = true;
                    Rotation = 315;
                }
                if (InputManager.IsPressed(Keys.A) && InputManager.IsPressed(Keys.W))
                {
                    upframe = true;
                    Rotation = 45;
                }
                if (InputManager.IsPressed(Keys.S) && InputManager.IsPressed(Keys.A))
                {
                    upframe = true;
                    Rotation = 135;
                }
                if (InputManager.IsPressed(Keys.S) && InputManager.IsPressed(Keys.D))
                {
                    upframe = true;
                    Rotation = 225;
                }
                #endregion
            }
            if (PlayerHealth != 0)
            {
                if (Position.Y >= 0 && Position.Y < 6912)
                {
                    Camera.Position = new PointF(0, Position.Y);
                    GameObject health = ObjectManager.GetObjectByName("Health");
                    TextObject Score = TextManager.GetTextObjectByName("Score");
                    Score.Position.Y = Camera.Position.Y + 378;
                    health.Position.Y = Camera.Position.Y + 368;
                }
            }

            //Change color if you're hurt
            if (CanBeHurtTimer <= 60 && CanBeHurtTimer >= 0)
            {
                SetModulationColor(1, 0, 0, 0.5f);
            }
            else
            {
                SetModulationColor(0, 0, 0, 0f);
            }

        }

        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            if (collisionInfo_.collidedWithGameObject.Name == "Bullet" || collisionInfo_.collidedWithGameObject.Name == EnemyTypes.Mine.ToString())
            {
                if (CanBeHurtTimer <= 0)
                {
                    --PlayerHealth;
                    CanBeHurtTimer = 60;
                }
            }
            else if (collisionInfo_.collidedWithGameObject.Name == "Heart")
            {
                ++PlayerHealth;
            }
            else if (collisionInfo_.collidedWithGameObject.Name.Contains("Powerup"))
            {
                foreach (PowerupTypes type in Enum.GetValues(typeof(PowerupTypes)))
                {
                    if (collisionInfo_.collidedWithGameObject.Name.Contains(type.ToString()))
                    {
                        if (Power == type)
                        {
                            PowerLevel++;
                            if(Power == PowerupTypes.MachineShot)
                            {
                                _AmmoPowerTotals[Power] = 100 * Math.Min(PowerLevel, 2);
                            }
                        }
                        else
                        {
                            PowerLevel = 1;
                        }
                        _AmmoPowerTotals.TryGetValue(type, out _PowerTotal);
                        break;
                    }
                }
            }
        }


        #region Properties

        public int PlayerHealth { get => _PlayerHealth; set => _PlayerHealth = Math.Min(_MaxHealth, value); }

        public int Score { get => _score; set => _score = value; }

        public PowerupTypes Power { get => _Power; set => _Power = value; }

        public int PowerTotal
        {
            get { return _PowerTotal; }
            set 
            { 
                _PowerTotal = value;
                if(_PowerTotal <= 0)
                {
                    PowerTotal = 0;
                    Power = PowerupTypes.NoneShot;
                }
            }
        }
        public int PowerLevel { get => _PowerLevel; set => _PowerLevel = Math.Min(3, value); }
        public int CanBeHurtTimer { get => _CanBeHurtTimer; set => _CanBeHurtTimer = value; }

        #endregion
    }
}
