using Godot;

public static class Wind{
    public static Vector2 Blow(Vector2 velocity, double delta){
        return 40 * (float)delta * Vector2.Right + velocity;
    }
}