using System.Collections.Generic;

namespace ECommerceApp.Core;

public class Cart
{
    public List<Product> Items { get; set; } = new();
    public string Status { get; set; } = "Active"; // Active, Checkout

    public void AddProduct(Product product)
    {
        Items.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        Items.Remove(product);
    }

    public decimal CalculateTotal()
    {
        decimal total = 0;
        foreach(var item in Items)
        {
            total += item.Price;
        }

        // Apply %10 discount if total > 100
        if(total > 100)
        {
            total = total * 0.9m;
        }

        // INTENTIONAL BUG: Tax is subtracted instead of added
        decimal tax = total * 0.18m;
        total = total - tax; // BUG! Should be + tax

        return total;
    }
}
