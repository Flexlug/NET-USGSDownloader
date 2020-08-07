# NET-USGSDownloader

Приложение позволяет скачивать множество архивов со снмиками с web-ресурса USGS.

Для работы приложения требуется .NET Core версией не ниже 3.1.

## 1. Авторизация

В папке рядом с исполняемым файлом необходимо создать файл с расширением json и следующей структурой:
```json
{
  "login": "your login",
  "password": "your password"
}
```

В представленные поля необходимо ввести логин и пароль от аккаунта USGS. Если аккаунт отсутствует, то необходимо его [создать](https://ers.cr.usgs.gov/register).

## 2. Параметры загрузки

В параметрах запуска приложения необходимо указать путь к файлу geojson, внутри которого должны храниться координаты выделенной области (без ключа). Координаты должны быть в поле features.
Тип: Polygon. В поле features должен быть только один примитив. Возможно указать несколько файлов в параметрах запуска.


Пример:

```
USGSDownloader-Console.exe file.geojson
```

file.geojson - входной файл с параметрами для загрузки.
