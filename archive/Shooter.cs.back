using Godot;
using System.Linq;

public partial class Shooter : Area2D
{
    [Export]
    private CollisionShape2D collisionShape;

    public TowerModule Module;

    public double currentCooldown = 0;

    public override void _Ready()
    {
        collisionShape.Shape = new CircleShape2D();
    }

    public override void _Process(double delta)
    {
        if (Module.Cooldown >= 0 && currentCooldown > 0)
        {
            currentCooldown-= delta;
            return;
        }

        var enemiesInRange = GetOverlappingBodies();

        if (enemiesInRange.Count == 0)
            return;

        Module.Shoot(enemiesInRange.Select(e => e as Enemy));
        currentCooldown = Module.Cooldown;
    }

    public void SetDetectionRadius(int radius)
    {
        (collisionShape.Shape as CircleShape2D).Radius = radius;
    }

    private void Shoot(Enemy enemy)
    {
        enemy.Damage(2);
    }
}
