
public interface ISelectable
{
    bool CanSelect { get; set; }
    void HandleSelection();
    void HandleBeingSelected();
    void HandleBeingUnselected();
}
