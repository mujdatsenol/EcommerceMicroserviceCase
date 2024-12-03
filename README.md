# Ecommerce Microservice Case Study
Bu uygulama Microservice mimarisi ile geliştirilmiş örnek bir E-Ticaret uygulamasıdır. Bir Case Study uygulamasıdır.

Uygulama .NET 9 ile yazıldı. 3 adet servis bir birinden bağımsız çalışsak şekilde RabbitMQ vasıtası ile iletişim kurabiliyorlar. Servislerin mimarisi CQRS ve Dikey Kesit Mimarisi birlikte kullanılarak tasarlandı. Her serviste Minimal API kullanıldı. Her servis farklı PostgreSQL veri tabanına sahip. Her servisde Loglama kullanılıyor. Servisler Docker'a hazır halde çalışıyorlar. Bağımlı oldukları diğer servislerle birlikte docker da hazır olacak şekilde çalışabiliyor. Servislerin dökümantasyonu OpenApi için hazır hale getirildi. Servis klasörlerin de örnek kullanım ve test için .http dosyaları mevcut

## Kullanılan Teknolojiler

* Servislerin mesajlaşması için **RabbitMQ**
* Veri tabanı için **PostgreSQL**
* Veri tabanı yönetimi için **pgAdmin**
* Log kayıtları için **Elasticsearch**
* Log'ları izlemek için **Kibana**
* Servis dokümantasyonu için **Scalar** arayüzü var
* 3 farklı servis var: **Stock API**, **Order API**, **Notification API**

Case Study mantığına göre Order servise gelen bir sipariş işlendikten sonra RabbitMQ ile diğer servislere exchange mesaj iletiyor. Bu mesajı Stock ve Notification servisleri dinledikleri exchange kuyruğu ile aldıktan sonra, stok servisi ürünlerin stoklarını güncelliyor ve bildirim servisi E-posta gönderecek işlemi yapıyor.

## Uygulama Kullanımı

Projeyi Docker da çalıştırmak için.

```bash
  docker compose up -d
```

Docker ayarlarını değiştirmek için kök dizinde bulunan docker-compose.yml dosyasını kullabilir siniz.
Servislerin container ayarları kendi klasör dizinlerin de ki Dockerfile dosyasında mevcut.

## Stock API Kullanımı

#### Scalar API Document için

```http
  http://<host-address>:7001/scalar/v1
```
Bütün api endpointleri scalar aracılığı ile görülebilir.

Örnek bir kullanım:

#### Tüm ürünleri getir

```http
  GET http://<host-address>:7001/api/products
```

#### Bir ürün getir

```http
  GET http://<host-address>:7001/api/products/${Id}
```

| Parametre | Tip    | Açıklama                                      |
| :-------- |:-------|:----------------------------------------------|
| `id`      | `Guid` | **Zorunlu**. Çağrılacak ürünün anahtar değeri |

## Order API Kullanımı

#### Scalar API Document için

```http
  http://<host-address>:7002/scalar/v1
```

#### Sipariş ver

```http
  POST http://<host-address>:7001/api/orders
```

```json
{
  "customerName": "Ahmet",
  "customerSurname": "Yılmaz",
  "customerEmail": "ahmet.yilmaz@domain.com",
  "orderItems": [
    {
      "productId": "08dd1301-6e1d-5f51-0000-000000000000",
      "productName": "Kazak",
      "productDescription": "Kazak açıklaması",
      "quantity": 2,
      "unitPrice": 799.99
    },
    {
      "productId": "08dd1301-6e1d-6a1e-0000-000000000000",
      "productName": "Hırka",
      "productDescription": "Hırka açıklaması",
      "quantity": 3,
      "unitPrice": 1299.99
    },
    {
      "productId": "08dd1301-6e1d-6a2b-0000-000000000000",
      "productName": "Pantolon",
      "productDescription": "Pantolon açıklaması",
      "quantity": 2,
      "unitPrice": 950.3
    }
  ]
}
```

| Parametre                         | Tip        | Gerekli | Açıklama               |
|:----------------------------------|:-----------|:--------|:-----------------------|
| `customerName`                    | `string`   | √       | Müşteri adı            |
| `customerSurname`                 | `string`   | √       | Müşteri soyadı         |
| `customerEmail`                   | `string`   | √       | Müşteri E-posta adresi |
| `orderItems[]`                    | `dataList` | √       | Ürün bilgileri         |
| `orderItems[].productId`          | `Guid`     | √       | Ürün Id                |
| `orderItems[].productName`        | `string`   | √       | Ürün adı               |
| `orderItems[].productDescription` | `string`   | √       | Ürün açıklaması        |
| `orderItems[].quantity`           | `int`      | √       | Ürün miktarı           |
| `orderItems[].unitPrice`          | `decimal`  | √       | Ürün birim fiyatı      |

## Notification API Kullanımı

#### Scalar API Document için

```http
  http://<host-address>:7003/scalar/v1
```
Bütün api endpointleri scalar aracılığı ile görülebilir.
