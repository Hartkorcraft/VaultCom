using Godot;

public interface IHealth
{
    int HealthCap { get; set; }
    int Health { get; set; }
    void Damage(int dmg);
    void Heal(int health);
    void Kill();
}
