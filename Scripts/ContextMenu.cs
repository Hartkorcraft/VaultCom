using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class ContextMenu : PopupMenu
{

    //public bool Showing { get; set; } = false;
    List<IContextable> iContextChoices = new List<IContextable>();
    public override void _Ready()
    {

    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("Right_Mouse") && Main.game_Manager.AllowWorldInput)
        {
            //GD.Print(Main.game_Manager.Previous_State);
            //Main.game_Manager.SetState(new ContextSelectionState());
            var mouse_pos = GetGlobalMousePosition();
            var mouse_grid_pos = Main.Mouse_Grid_Pos;

            AddContextOptions();

            if (Visible is false)
            {
                Popup_(new Rect2(mouse_pos.x, mouse_pos.y, RectSize.x, RectSize.y));
            }
            else { RectGlobalPosition = new Vector2(mouse_pos); }
        }
    }

    private void AddContextOptions(bool add_close_button = true)
    {
        var mouse_grid_pos = Main.Mouse_Grid_Pos;
        this.Clear();
        var icontextable = Get_Context_Objects(new Vector2i(mouse_grid_pos));

        for (int i = 0; i < icontextable.Count; i++)
        {
            var name = icontextable[i].GetType().ToString();
            if (icontextable[i] is INameable)
            {
                name = ((INameable)icontextable[i]).ObjectName;
            }
            AddItem(name, i);
        }
        iContextChoices = icontextable;

        if (add_close_button)
        {
            AddItem("Close", 999);
        }
        SetAsMinsize();
    }
    
    private List<IContextable> Get_Context_Objects(Vector2i mouse_grid_pos)
    {
        var collider_dicts = Main.Get_Collider_Dicts_From_GridPos(mouse_grid_pos);
        var contextable = new List<IContextable>();

        for (int i = 0; i < collider_dicts.Count; i++)
        {
            var col = ((CollisionObject2D)collider_dicts[i]["collider"]).GetParent();

            if (col is IContextable)
            {
                contextable.Add((IContextable)col);
            }
        }
        return contextable;
    }

    public void _on_ContextMenu_id_pressed(int id)
    {

        if (id >= 0 && id < iContextChoices.Count)
        {
            iContextChoices[id]?.Act_On_Context_Selection();
        }

        switch (id)
        {
            case 999:
                GD.Print("Closed!");
                Visible = false;
                break;
            default:
                //GD.PushWarning("No switch option!");
                break;
        }
    }
    public void _on_ContextMenu_about_to_show()
    {
        GD.Print("Showed!");
    }
    public void _on_ContextMenu_popup_hide()
    {

    }

    public void _on_ContextMenu_visibility_changed()
    {
        // if (Visible is false)
        // {
        //     Main.game_Manager.EndContextSelectionState();
        // }
    }

}
