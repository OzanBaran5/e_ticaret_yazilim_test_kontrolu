using System;

namespace ECommerceApp.Core;

public class OrderService
{
    public class Order
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public Order PlaceOrder(Cart cart, decimal paymentAmount)
    {
        decimal total = cart.CalculateTotal();

        if (paymentAmount < total)
        {
            // INTENTIONAL BUG: Clears cart state before throwing exception, causing data loss!
            cart.Items.Clear(); 
            throw new ArgumentException("Insufficient payment!");
        }

        foreach (var item in cart.Items)
        {
            item.DecreaseStock(1);
        }

        cart.Status = "Checkout";

        return new Order { IsSuccessful = true, Message = "Order placed successfully." };
    }
}
