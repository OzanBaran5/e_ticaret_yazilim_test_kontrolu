# 🛒 E-Ticaret Yazılım Test ve Kalite Projesi

Bu proje, "Yazılım Test ve Kalitesi" ilkeleri doğrultusunda geliştirilmiş, içerisinde **kasıtlı mantıksal hatalar (bug)** barındıran bir E-Ticaret (C# / .NET 9) konsol uygulamasıdır. 

Projenin temel amacı; kodda bulunan zafiyetlerin Unit Test, Integration Test, White-Box, Black-Box ve Gray-Box test teknikleri kullanılarak **NUnit** framework'ü üzerinden ne şekilde tespit edilebildiğini (Fail) ve sağlam kodların nasıl onaylandığını (Pass) uygulamalı olarak göstermektir.

---

## 🏗️ Proje Mimarisi

Sistem, istenilen e-ticaret akışını (**Ürün Seçimi -> Sepete Ekleme -> Ödeme ve Sipariş**) karşılayacak modüllerle tasarlanmıştır.

* **Core/**: Uygulamanın arka plan iş mantığını yöneten sınıfları içerir (`Product.cs`, `Cart.cs`, `OrderService.cs`). Testlerin patlaması amacıyla sistemin kalbi olan bu sınıflara kasıtlı hatalar enjekte edilmiştir.
* **Tests/**: Test Driven prensiplerine sadık kalarak oluşturulmuş NUnit Test sınıflarıdır.
* **Program.cs**: Sistemin başarılı ve başarısız tüm test sonuçlarının kullanıcı dostu bir arayüzle terminal ekranına (Console) basılmasını sağlayan raporsal giriş noktasıdır. Dışarıdan NUnit Test adapter çalıştırmak yerine, uygulamayı `F5` ile derleyerek Test Sonuçlarını doğrudan renklendirilmiş şekilde izleyebilirsiniz.

---

## 🐛 Enjekte Edilen Bilinçli Hatalar (Bugs)

Projenin test edilmesi analizini derinleştirmek için aşağıdaki zafiyetler sisteme kodlanmıştır:
1. **KDV Hesaplama Hatası (Cart.cs)**: Sepet toplamı hesaplanırken `%18` oranındaki KDV'nin bedele **eklenmesi (`+`)** gerekirken, hatalı bir mantıkla bedelden **çıkarılması (`-`)**.
2. **Stok Zafiyeti (Product.cs)**: Bir kullanıcının ürün satın aldığında, eğer ürünün stoğu `0` ise sistemin `ArgumentException` fırlatarak bunu reddetmemesi ve stoğu eksi (`-`) değerlere indirmesi.
3. **Veri Kaybı / Sepet Silinmesi (OrderService.cs)**: Kullanıcının sepet tutarından daha düşük bir bakiye okuttuğu bir "Hatalı Ödeme" (Fail) senaryosunda uygulamanın satışı reddetmesi doğrudur; fakat exception fırlatılmadan hemen önce kod hatasından dolayı müşterinin içindeki **tüm sepetin sıfırlanması (`Clear`)**.

---

## 🧪 Uygulanan Test Senaryoları ve Sonuçları

`NUnit` aracılığıyla, kodun hem kara kutu hem de dış entegrasyon metodolojilerine uygun **10 farklı test senaryosu** koşturulmuştur. Yukarıdaki hatalara çarpan algoritmalar testten geçememiş ve planlandığı üzere kırmızı (FAIL) dönmüştür.

### 🔴 BAŞARISIZ (FAIL) OLAN TESTLERİN ANALİZİ (4 Test)

#### 1. `[White Box]` CartCalculateTotal_CorrectlyAppliesDiscountAndTax
* **Beklenen:** *212.4m*  |  **Gerçekleşen:** *147.6m*
* **Açıklama:** KDV'nin yanlış çıkartılması (Bug 1) nedeniyle beklenen sepet tutarı yanlış çıkmakta ve uygulamanın en kritik tahsilat iş parçası çökmektedir. Test bu hatayı yakalamıştır.

#### 2. `[Black Box]` ProductDecreaseStock_ThrowsWhenNegative
* **Beklenen:** `<System.ArgumentException>`  |  **Gerçekleşen:** `null` (Sessiz Geçiş)
* **Açıklama:** Negatif stok bug'ı (Bug 2) nedeniyle, sistem exception fırlatmak yerine işlemi onaylamakta ve veri bütünlüğünü bozmaktadır. Input-Output ilişkisini inceleyen Black-Box testimiz bu boşluğu tespit edip Fail almıştır.

#### 3. `[Gray Box]` CartPaymentError_ShouldNotClearCartState
* **Beklenen:** *Sepet Count = 1*   |  **Gerçekleşen:** *Sepet Count = 0*
* **Açıklama:** Sistemin state'ini inceleyen bu testimizde, yetersiz ödeme esnasında kullanıcının var olan sepet verisinin (Bug 3 kaynaklı) kaybolduğu tespit edilmiş ve test Fail olmuştur.

#### 4. `[Integration Test]` MultipleProducts_CalculationAndPayment
* **Beklenen:** *127.44m*  |  **Gerçekleşen:** *88.56m*
* **Açıklama:** `Cart.cs` ve `OrderService.cs` modüllerinin entegre çalışması test edilmiştir. Sepet aşamasındaki KDV çıkarma hatasının, uçtan uca ödeme yetkilendirmesi sırasında sisteme nasıl yanlış veri gönderdiği zincirleme olarak yakalanmıştır.

---

### 🟢 BAŞARILI (PASS) OLAN TESTLER (6 Test)

Buglardan etkilenmeyen olağan pozitif senaryolar da sistemin çalıştığını kanıtlayan şu 6 Pass testiyle raporlanmıştır:
1. `[White Box]` OrderServicePlaceOrder_UpdatesInnerState
2. `[White Box]` CartRemoveProduct_ReducesCount
3. `[Black Box]` CartAddProduct_IncreasesCount
4. `[Black Box]` ProductDecreaseStock_ReducesStockCorrectly
5. `[Gray Box]` OrderServicePlaceOrder_ValidOrderMustBeCheckoutState
6. `[Integration Test]` AddProductToCart_And_PlaceOrderSuccessfully

---

## 🚀 Projeyi Çalıştırma

Projeyi derlemek ve sonuçları teyit etmek için:
1. Depoyu bilgisayarınıza indirin (Klonlayın).
2. Bilgisayarınızda `Visual Studio 2022` yüklü ise `ECommerceApp.csproj` dosyasını çalıştırın.
3. Direkt **Başlat (Start) / F5** tuşlarına bastığınızda, hazırlanan "Özel Konsol Arayüzü" açılacak renkli ve şık bir özetle size test raporunu sunacaktır.
4. Kaynak koddaki detaylar için Visual Studio üst menüsünden **Test > Test Explorer (Test Gezgini)** aracını kullanabilir ve NUnit Categorization `[Category("...")]` ile yapılmış profesyonel test kırılımlarını inceleyebilirsiniz.
