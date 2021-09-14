using Godot;

public interface IMouseable
{
    Area2D area2D { get; }
    void _on_Area2D_input_event(Node viepoint, InputEvent inputEvent, int local_shape);
    void _on_Area2D_mouse_entered();
    void _on_Area2D_mouse_exited();
    void On_Left_Mouse_Click();
    void On_Right_Mouse_Click();
}
