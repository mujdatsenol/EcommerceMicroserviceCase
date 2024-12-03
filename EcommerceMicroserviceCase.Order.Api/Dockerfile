# Derleme ve migration için SDK imajı kullanılır
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyasını ve tüm dosyaları kopyalar
COPY . .

# Bağımlılıklar yüklenir
RUN dotnet restore "EcommerceMicroserviceCase.Order.Api/EcommerceMicroserviceCase.Order.Api.csproj"

# Uygulama build alınır.
WORKDIR "/src/EcommerceMicroserviceCase.Order.Api"
RUN dotnet build "EcommerceMicroserviceCase.Order.Api.csproj" -c Release -o /app/build

# Publish aşaması
FROM build AS publish
RUN dotnet publish "EcommerceMicroserviceCase.Order.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Migration aşaması
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migration

# Migration komutunu çalışacağı dizin
WORKDIR /app/publish

# Publish edilen dosyalar kopyalanır
COPY --from=publish /app/publish /app/publish

# Veritabanı migration işlemini gerçekleşir
RUN dotnet ef database update

# Base image (Uygulamanın çalışacağı imaj)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 7002

# Publish edilmiş uygulamayı son image'a kopyalanması
COPY --from=publish /app/publish .

# wait-for-it.sh scriptini kopyalayıp ve çalıştırılabilir hale getirilmesi
COPY EcommerceMicroserviceCase.Order.Api/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

# Veritabanının hazır olmasını bekleyip API servisinin başlatılması
CMD /wait-for-it.sh ${POSTGRES_DB_ORDER_HOST}:5432 --timeout=0 -- /wait-for-it.sh ${RABBITMQ_HOST}:${RABBITMQ_PORT} -- dotnet EcommerceMicroserviceCase.Order.Api.dll
