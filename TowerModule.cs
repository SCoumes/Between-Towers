using Godot;
using Godot.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class TowerModule
{
    public Vector2I TextureCoordinates;
    public double Cooldown;
    public int Radius = 64;
    public int Level = 1;

    public readonly string Name;
    //public Action<IEnumerable> Shoot;

    public abstract void Shoot(IEnumerable<Enemy> enemies);

    public TowerModule(string name) 
    {
        Name = name;
    }

    public class PrincessModule : TowerModule
    {
        public int HP = 10;

        public PrincessModule() : base("PRINCESS")
        {
            Level = 5;
            TextureCoordinates = new Vector2I(0,0);
            Cooldown = -1;
            Radius = 32;
        }

        public override void Shoot(IEnumerable<Enemy> enemies)
        {

            foreach(var enemy in enemies)
            {
                HP -= enemy.Template.HPCost;
                enemy.CallDeferred(Enemy.MethodName.QueueFree);
            }

            if (HP < 0)
            {
                GD.Print("Game over !");
            }
        }
    }

    public class ArcherModule : TowerModule
    {
        public int Damage = 2;

        public ArcherModule() : base("ARCHER")
        {
            Level = 1;
            Cooldown = 1;
            TextureCoordinates = new Vector2I(1,0);
        }

        public override void Shoot(IEnumerable<Enemy> enemies)
        {
            enemies.First().Damage(Damage);
        }
    }

    public class MarksmanModule : TowerModule
    {
        public int Damage = 10;

        public MarksmanModule() : base("MARKSMAN")
        {
            Level = 2;
            Cooldown = 2;
            TextureCoordinates = new Vector2I(3, 0);
        }

        public override void Shoot(IEnumerable<Enemy> enemies)
        {
            enemies.First().Damage(Damage);
        }
    }
    
    public class CannonModule : TowerModule
    {
        public int Damage = 6;

        public CannonModule() : base("CANNON")
        {
            Level = 2;
            Cooldown = 4;
            TextureCoordinates = new Vector2I(4, 0);
        }

        public override void Shoot(IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                enemy.Damage(Damage);
            }
        }
    } 
    
    public class MagicModule : TowerModule
    {
        public int Damage = 6;

        public MagicModule() : base("MAGIC")
        {
            Level = 3;
            Cooldown = 1;
            TextureCoordinates = new Vector2I(2, 0);
        }

        public override void Shoot(IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                enemy.Damage(Damage);
            }
        }
    }
}