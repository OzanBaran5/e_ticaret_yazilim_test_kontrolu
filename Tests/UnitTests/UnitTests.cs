using System;
using NUnit.Framework;
using ECommerceApp.Core;

namespace ECommerceApp.Tests.UnitTests;

[TestFixture]
public class UnitTests
{
    private Product _testProduct;
    private Cart _testCart;
    private OrderService _orderService;

    [SetUp]
    public void Setup()
    {
        _testProduct = new Product { Id = 1, Name = "Laptop", Price = 1000m, Stock = 5 };
        _testCart = new Cart();
        _orderService = new OrderService();
    }

    // --- 1. UNIT TEST (WHITE BOX) ---
    [Test]
    [Category("White Box")]
    public void CartCalculateTotal_CorrectlyAppliesDiscountAndTax()
    {
        // Arrange
        _testCart.AddProduct(new Product { Id = 2, Name = "Mouse", Price = 200m, Stock = 10 });
        
        // Act
        decimal total = _testCart.CalculateTotal();
        
        // Assert
        // Expected Logic: 200 > 100 -> %10 discount -> 180. Tax %18 -> +32.4 = 212.4
        // Due to the INTENTIONAL BUG, tax is subtracted: 180 - 32.4 = 147.6
        // This test will FAIL because it expects the correct business value (212.4)
        Assert.That(total, Is.EqualTo(212.4m));
    }

    [Test]
    [Category("White Box")]
    public void OrderServicePlaceOrder_UpdatesInnerState()
    {
        // Arrange
        _testCart.AddProduct(_testProduct);

        // Act
        // Initializing with big amount to bypass payment check
        var order = _orderService.PlaceOrder(_testCart, 2000m);

        // Assert
        Assert.That(_testCart.Status, Is.EqualTo("Checkout"));
    }

    [Test]
    [Category("White Box")]
    public void CartRemoveProduct_ReducesCount()
    {
         // Arrange
         _testCart.AddProduct(_testProduct);
         
         // Act
         _testCart.RemoveProduct(_testProduct);

         // Assert
         Assert.That(_testCart.Items.Count, Is.EqualTo(0));
    }

    // --- 2. BLACK BOX TEST ---
    [Test]
    [Category("Black Box")]
    public void CartAddProduct_IncreasesCount()
    {
        // Act
        _testCart.AddProduct(_testProduct);

        // Assert
        Assert.That(_testCart.Items.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("Black Box")]
    public void ProductDecreaseStock_ReducesStockCorrectly()
    {
        // Act
        _testProduct.DecreaseStock(3);

        // Assert
        Assert.That(_testProduct.Stock, Is.EqualTo(2));
    }

    [Test]
    [Category("Black Box")]
    public void ProductDecreaseStock_ThrowsWhenNegative()
    {
        // Arrange
        var product = new Product { Id = 3, Name = "Keyboard", Price = 100m, Stock = 0 };

        // Act & Assert
        // INTENTIONAL BUG: System allows negative stocks without throwing. 
        // This test will FAIL.
        Assert.Throws<ArgumentException>(() => product.DecreaseStock(1));
    }

    // --- 3. GRAY BOX TEST ---
    [Test]
    [Category("Gray Box")]
    public void OrderServicePlaceOrder_ValidOrderMustBeCheckoutState()
    {
         // Arrange
         _testCart.AddProduct(new Product { Id = 4, Name = "Cable", Price = 50m, Stock = 10 });

         // Act
         var order = _orderService.PlaceOrder(_testCart, 100m);

         // Assert
         Assert.That(order.IsSuccessful, Is.True);
         Assert.That(_testCart.Status, Is.EqualTo("Checkout"));
    }

    [Test]
    [Category("Gray Box")]
    public void CartPaymentError_ShouldNotClearCartState()
    {
        // Arrange (Assuming the state needs to be maintained on payment failure)
        _testCart.AddProduct(new Product { Id = 5, Name = "Desk", Price = 500m, Stock = 1 });

        // Act & Assert
        // Payment amount < Total throws Exception
        Assert.Throws<ArgumentException>(() => _orderService.PlaceOrder(_testCart, 10m));
        
        // INTENTIONAL BUG: The cart is cleared before exception thrown.
        // This test will FAIL.
        Assert.That(_testCart.Items.Count, Is.EqualTo(1), "Cart state was lost during failed payment!");
    }
}
