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
    class Gun : GameObject
    {        
        private int _CanFireTimer = 15;
        
        private const int SHOT_ROTATION_QUARTER_TURN = 22;      // 90/4
        private const int SHOT_ROTATION_HALF_TURN = 45;         // 90/2
        private const int SHOT_ROTATION_THREE_QUARTER_TURN = 67; //90 * 3/4


        public Gun()
            : base("TankGun", 128, 128, "TankGun.png")
        {
            ZOrder = 2;
        }


        public override void Update()
        {
            base.Update();
            Position.X = Tank.TankInstance.Position.X;
            Position.Y = Tank.TankInstance.Position.Y;

            CanFireTimer--;            

            // If the game is not over
            // Clean this up
            if (Tank.TankInstance.PlayerHealth > 0)
            {
                // Is firing off cooldown?
                if (CanFireTimer > 0)
                {
                    return;
                }

                bool ShouldFire = false;
                int DirectionToFire = 0;
                bool IsKeyTriggered = false;

                #region Keypresses
                // Check for keypress
                // Unfortunately I can't check for keypresses together
                if (InputManager.IsPressed(Keys.Left))
                {
                    if(InputManager.IsTriggered(Keys.Left))
                    {
                        IsKeyTriggered = true;
                    }
                    Rotation = 90;
                    ShouldFire = true;
                    DirectionToFire = (int)Keys.Left;
                }
                else if (InputManager.IsPressed(Keys.Right))
                {
                    if (InputManager.IsTriggered(Keys.Right))
                    {
                        IsKeyTriggered = true;
                    }
                    Rotation = 270;
                    ShouldFire = true;
                    DirectionToFire = (int)Keys.Right;
                }
                else if (InputManager.IsPressed(Keys.Up))
                {
                    if (InputManager.IsTriggered(Keys.Up))
                    {
                        IsKeyTriggered = true;
                    }
                    Rotation = 0;
                    ShouldFire = true;
                    DirectionToFire = (int)Keys.Up;
                }
                else if (InputManager.IsPressed(Keys.Down))
                {
                    if (InputManager.IsTriggered(Keys.Down))
                    {
                        IsKeyTriggered = true;
                    }
                    Rotation = 180;
                    ShouldFire = true;
                    DirectionToFire = (int)Keys.Down;
                }
                #endregion

                if (ShouldFire)
                {
                    PowerupTypes CurrentTankPower = Tank.TankInstance.Power;
                    if (CurrentTankPower == PowerupTypes.MachineShot || IsKeyTriggered)
                    {
                        FireInDirection(DirectionToFire, CurrentTankPower);
                    }
                }
            }           
        }

        private void FireInDirection(int Direction, PowerupTypes Power)
        {
            if(Power != PowerupTypes.NoneShot)
            {
                Tank.TankInstance.PowerTotal--;
            }

            switch (Power)
            {
                case PowerupTypes.MachineShot:
                    ObjectManager.AddGameObject(new TankShell(Position, Rotation));
                    if(Tank.TankInstance.PowerLevel == 3)
                    {
                        CanFireTimer = 3;
                    }
                    else
                    {
                        CanFireTimer = 5;
                    }
                    break;
                case PowerupTypes.SpreadShot:
                    if (Tank.TankInstance.PowerLevel == 1)
                    {
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation - SHOT_ROTATION_QUARTER_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation + SHOT_ROTATION_QUARTER_TURN));
                    }
                    else if (Tank.TankInstance.PowerLevel == 2)
                    {
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation - SHOT_ROTATION_HALF_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation - SHOT_ROTATION_QUARTER_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation + SHOT_ROTATION_QUARTER_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation + SHOT_ROTATION_HALF_TURN));
                    }
                    else if (Tank.TankInstance.PowerLevel > 2)
                    {
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation - SHOT_ROTATION_THREE_QUARTER_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation - SHOT_ROTATION_HALF_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation - SHOT_ROTATION_QUARTER_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation + SHOT_ROTATION_QUARTER_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation + SHOT_ROTATION_HALF_TURN));
                        ObjectManager.AddGameObject(new TankShell(Position, Rotation + SHOT_ROTATION_THREE_QUARTER_TURN));
                    }
                    CanFireTimer = 15;
                    break;
                case PowerupTypes.SpikeShot:
                    ObjectManager.AddGameObject(new TankShell(Position, Rotation));
                    CanFireTimer = 15;
                    break;
                default: // No powerup
                    ObjectManager.AddGameObject(new TankShell(Position, Rotation));
                    CanFireTimer = 15;
                    break;
            }
        }


        public int CanFireTimer { get => _CanFireTimer; set => _CanFireTimer = Math.Max(value,0) ; }


    }
}
