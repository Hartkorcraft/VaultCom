using Godot;
using System;

public class PickUp : SpriteMapObject
{


    public void _on_Area2D_area_entered(Area2D area)
    {
        GD.Print(area, " Entered!");
    }

    public void _on_Area2D_area_exited(Area2D area)
    {

    }
}
