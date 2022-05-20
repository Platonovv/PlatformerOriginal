namespace PixelCrew.Model.Data
{
    public interface ICanAddInInventory
    {
        void AddInInventory(string id, int value);
        void AddInInventoryBig(string id, int count);
    }
}