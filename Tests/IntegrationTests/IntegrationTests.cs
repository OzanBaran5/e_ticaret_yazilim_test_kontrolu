using System;
using NUnit.Framework;
using ECommerceApp.Core;

namespace ECommerceApp.Tests.IntegrationTests;

[TestFixture]
public class IntegrationTests
{
    [Test]
    [Category("Integration Test")]
    public void AddProductToCart_And_PlaceOrderSuccessfully()
    {
        // Multi-module testing setup (Cart + Product + OrderService)
        var product = new Product { Id = 10, Name = "Monitor", Price = 50m, Stock = 2 };
        var cart = new Cart();
        var service = new OrderService();

        // Execution span
        cart.AddProduct(product);
        var orderInfo = service.PlaceOrder(cart, 100m);

        // Verification
        Assert.That(orderInfo.IsSuccessful, Is.True);
        Assert.That(cart.Status, Is.EqualTo("Checkout"));
        Assert.That(product.Stock, Is.EqualTo(1));
    }

    [Test]
    [Category("Integration Test")]
    public void MultipleProducts_CalculationAndPayment()
    {
        // Arrange
        var p1 = new Product { Id = 1, Name = "Item A", Price = 60m, Stock = 5 };
        var p2 = new Product { Id = 2, Name = "Item B", Price = 60m, Stock = 5 }; 
        var cart = new Cart();
        cart.AddProduct(p1);
        cart.AddProduct(p2);
        
        var service = new OrderService();
        
        // Act
        // Total price logic without bugs: 120 -> %10 disc -> 108. Tax 18% -> 127.44
        // The service should calculate the required payment amount as 127.44m
        decimal expectedTotal = 127.44m;
        
        decimal actualTotal = cart.CalculateTotal();
        
        // INTENTIONAL BUG: Will calculate tax subtraction: 108 - 19.44 = 88.56
        // This test will FAIL.
        Assert.That(actualTotal, Is.EqualTo(expectedTotal), "The total amount with tax should correctly be 127.44");
        
        var result = service.PlaceOrder(cart, actualTotal);
        Assert.That(result.IsSuccessful, Is.True);
    }
}
