using System;

namespace ECommerceApp;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Yazılım Test ve Kalitesi - E-Ticaret Test Raporu";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("  E-TİCARET SİSTEMİ - TEST VE BUG RAPORU ARAYÜZÜ  ");
        Console.WriteLine("==================================================\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Proje: E-Ticaret App - Unit, Black, Gray, Integration Testleri");
        Console.WriteLine("Toplam Test Senaryosu: 10\n");

        // Başarılı (Pass) Olan Testler
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("--- \u2714 BAŞARILI (PASS) OLAN TESTLER (6 Test) ---");
        Console.WriteLine(" 1. [White Box] OrderServicePlaceOrder_UpdatesInnerState");
        Console.WriteLine(" 2. [White Box] CartRemoveProduct_ReducesCount");
        Console.WriteLine(" 3. [Black Box] CartAddProduct_IncreasesCount");
        Console.WriteLine(" 4. [Black Box] ProductDecreaseStock_ReducesStockCorrectly");
        Console.WriteLine(" 5. [Gray Box]  OrderServicePlaceOrder_ValidOrderMustBeCheckoutState");
        Console.WriteLine(" 6. [Integration] AddProductToCart_And_PlaceOrderSuccessfully\n");

        // Başarısız (Fail) Olan Testler (Bug'lar)
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("--- \u274c BAŞARISIZ (FAIL) OLAN TESTLER / BİLİNÇLİ EKLENEN BUGLAR (4 Test) ---");
        
        Console.WriteLine(" 7. [White Box] CartCalculateTotal_CorrectlyAppliesDiscountAndTax");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("    - Bug Nedeni: Sepetteki KDV'nin (+) yerine yanlışlıkla (-) ile çıkarılması.\n");
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine(" 8. [Black Box] ProductDecreaseStock_ThrowsWhenNegative");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("    - Bug Nedeni: Eksi stok engellemesinin (Exception) çalışmaması.\n");
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine(" 9. [Gray Box]  CartPaymentError_ShouldNotClearCartState");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("    - Bug Nedeni: Yetersiz bakiyede ödeme patlamadan önce müşterinin sepetinin sıfırlanması.\n");
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine(" 10.[Integration] MultipleProducts_CalculationAndPayment");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("    - Bug Nedeni: Toplam fiyattaki KDV hatasının sipariş entegrasyonunu da patlatması.\n");

        Console.ResetColor();
        Console.WriteLine("==================================================");
        Console.WriteLine("Test Explorer'dan da bu sonuçların birebir aynısını görüntüleyebilirsiniz.");
        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}
