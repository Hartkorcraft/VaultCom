using Godot;
using System;

public class PopupInfoWindow : PopupNew
{


    public void _on_PopupInfoWindow_mouse_entered()
    {
        GD.Print("Entered popup ");
    }
    public void _on_PopupInfoWindow_mouse_exited()
    {
        GD.Print("Exited popup");
    }

    public void _on_PopupInfoWindow_popup_hide(){
        QueueFree();
    }
}
