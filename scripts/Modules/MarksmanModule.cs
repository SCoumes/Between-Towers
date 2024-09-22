using Godot;
using System;
using System.Collections.Generic;

public partial class MarksmanModule : Module
{
	public static readonly new int Cost = 50;
	public readonly new string Name = "Canon";

	private double CoolDown = 0.75;
	private double TimeSinceLastShot = 0;
	private List<Enemy> Enemies = new();
	private Enemy Target = null;

	public override void _Ready()
	{
		GetNode<Area2D>("EnemyDetection").BodyEntered += _on_enemy_detection_body_entered;
		GetNode<Area2D>("EnemyDetection").BodyExited += _on_enemy_detection_body_exited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		TimeSinceLastShot += delta;
		if (TimeSinceLastShot > CoolDown) _Shoot();
	}

	private void _Shoot(){

		if (Target is null) _findTarget();
		if (Target is null) return;
		Target.Damage(20);
		TimeSinceLastShot = 0;
	}

	private void _findTarget(){
		// Find the closest enemy
		Target = null;
		float minDistance = float.MaxValue;
		foreach (Enemy enemy in Enemies){
			if (enemy.Dead) {
				Enemies.Remove(enemy);
				continue;
			}
			float distance = (enemy.GlobalPosition - Tower.GlobalPosition).Length();
			if (distance < minDistance){
				minDistance = distance;
				Target = enemy;
			}
		}
	}

	private void _on_enemy_detection_body_entered(object body){
		if (body is Enemy enemy){
			Enemies.Add(enemy);
		}
	}

	private void _on_enemy_detection_body_exited(object body){
		if (body is Enemy enemy){
			Enemies.Remove(enemy);
			if (enemy == Target) Target = null;
		}
	}
}
