# ws-client-dotnet
TurkBelge ENTEGRASYON PROJESİ
Bu proje e-Fatura ve e-Arşiv  ürünlerinin türk belge web servis metodlarının nasıl kullanılması gerektiği ile ilgili örnek olması için oluşturulmuştur. 
Projede beş farklı ürünün web servis metodlarının ve Kimlik Doğrulama web servisinin kullanımları  bulunmaktadır. Yalnızca test web servis sisteminde çalışmakta ve web servislere bağlantı ayarları da projede bulunmaktadır.

PROJEDE KULLANILAN TEKNOLOJİLER:
Bu projede C# Windows Form teknolojisi ile yapıldı.
Projede MVC mimari yapısı kullanıldı.
Projede Singleton Pattern kullanıldı.
Projede veritabanı olarak SqLite kullanıldı.
Projede Entitiy Freamework kullanıldı.
Proje arayüzü İngilizce ve Türkçe dil desteği sağlamaktadır.

PROJEDE KULLANILAN ÜRÜNLER:

Kimlik Doğrulama Servisi İçin:
-Giriş İşlemlerini Yapma
-Gib Mükellef Listesi  Sorgulama

e-Fatura Ürünü İçin:
-Gelen veya Giden veya Taslak Faturaları Alma 
-Fatura Durumlarını Sorgulama      
-Fatura Yanıtı Gönderme
-Fatura Durumu Okundu veya Okunmadı Olarak İşaretleme
-Fatura UBL-Document Oluşturma
-Fatura Taslak Yükleme
-Fatura Gönderme
-Gelen veya Gönderilen Faturaların HTML,PDF,XML Belgelerini İndirme

e-Arşiv Ürünü İçin:
-Gelen veya Giden veya Taslak e-Arşiv Faturaları Alma 
-e-Arşiv Rapor listesini Alma 
-e-Arşiv Fatura Durum Sorgulama
-e-Arşiv Fatura İptali
-e-Arşiv Faturasını E-Posta Olarak Tekrar Gönderme 
-e-Arşiv Durumu Okundu veya Okunmadı Olarak İşaretleme
-e-Arşiv  UBL-Document Oluşturma
-e-Arşiv  Fatura Gönderme
-Gelen veya Gönderilen Faturaların HTML,PDF,XML Belgelerini İndirme

KURULUM
-Solution Explorer’dan TurkBelge.UI projesine sağ tıklayıp ‘Set As Start Up’ seçeneğine tıklayıp Başlangıç projesini seçiyoruz.Daha sonra projeyi Build ediyoruz.Son olarak da web servislere erişim için  bilgisayarın internet erişimi olduğundan emin olunuz.
