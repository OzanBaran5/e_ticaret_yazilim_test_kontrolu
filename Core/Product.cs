namespace ECommerceApp.Core;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public void DecreaseStock(int amount)
    {
        // INTENTIONAL BUG: Not throwing exception when Stock < amount or Stock <= 0
        Stock -= amount;
    }
}
