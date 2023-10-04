# 🚀 Prueba Técnica

Construir una aplicación web .Net Core que permita a usuarios autenticarse (OAuth2) y que consuma una API RESTfull (.NET).

## Requerimientos:

1. Implementar un back-end en .NET para manejar la autenticación y actuar como servidor de la API.
2. Integrar la autenticación mediante OAuth2 con un proveedor popular (Google, Facebook, GitHub, etc.). Los usuarios deberían poder iniciar sesión utilizando sus credenciales de OAuth2 y obtener un token de acceso.
3. Diseñar e implementar una API RESTfull que exponga de forma segura endpoints para recuperar datos relacionados con un dominio específico (perfiles de usuario, productos, pedidos, etc.). Los datos pueden ser simulados o almacenados en una base de datos de tu elección.
4. Crear endpoints que conformen un CRUD, a ser consumidos por el front-end.
5. Desarrollar un front-end que autentique a los usuarios utilizando OAuth2 y consuma la API RESTful para consumir los endpoints creados.
6. Asegurarse de que la aplicación siga las mejores prácticas en cuanto a seguridad, manejo de errores y validación de datos.

Forma de entrega: Enlace a repositorio en Github.

```
git clone https://github.com/edwinet/WebApp.git
```

---

## ⚙️ Requisitos del sistema

- Windows 2012 R2 / 2016 / 2019 / 2022 / 10 / 11.
- [.NET Framework 4.7.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework) (o más reciente).
- [.NET SDK 6.0.405](https://dotnet.microsoft.com/download/dotnet/6.0) (o más reciente).
- [Node.js v16](https://nodejs.org/en/) (o más reciente).
- [SQL Server® 2019 Express](https://www.microsoft.com/en-us/download/details.aspx?id=101064) (o más reciente).
- [Visual Studio Community 2022](https://www.visualstudio.com/vs/community/) (v17.4 o más reciente) proporciona la mejor experiencia de desarrollo para crear aplicaciones ASP.NET Core 6/7 de forma gratuita.
- [Visual Studio Code](https://code.visualstudio.com/) es un editor de código fuente y está disponible para Windows, macOS y Linux. Asegúrese de instalar la [extensión C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp), que es una herramienta de desarrollo ligera para .NET.

### Servidor

- [ASP.NET Core Runtime 6.0.13](https://dotnet.microsoft.com/download/dotnet/6.0) (o más reciente).
- En Windows, asegúrese de instalar el paquete **Hosting Bundle**, que incluye soporte para .NET Runtime y IIS.
 
### Navegador

- Chrome / Edge / Firefox / Safari / Opera (más reciente).
- *Internet Explorer NO es compatible*.

## Base de Datos

- El script para crear la baase de datos estan [./Data/webapp.sql](./Data/webapp.sql)
- La conexión a la base de datos [./appsettings.json](./appsettings.json)

## Back-End

Abrir el projecto con Visual Studio y ejecutar. 

[http://localhost:5000/](http://localhost:5000/)

## API

Para ver los endpoints del API:

[http://localhost:5000/swagger/](http://localhost:5000/swagger/)

Si desea utilizar Postman 

[./wwwroot/swagger/WebApp.postman_collection.json](./wwwroot/swagger/WebApp.postman_collection.json)

## Credenciales

- Admin
	- usuario: admin
	- contraseña: Master0fpuppets
- Editor
	- usuario: editor
	- contraseña: Master0fpuppets
- User
	- usuario: user
	- contraseña: Master0fpuppets
 
## Videos

- Para ver un video del funcionamiento del BackEnd [./Videos/backend.mkv](./Videos/backend.mkv)
- Para ver un video del API [./Videos/api.mkv](./Videos/api.mkv)