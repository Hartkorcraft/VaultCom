using Godot;

public interface IHealth
{
    int HealthCap { get; set; }
    int Health { get; set; }
    void Damage(uint dmg);
    void Heal(uint health);
    void Kill();

}
