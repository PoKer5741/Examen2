# Sistema de Mesa de Ayuda TI - SoporteLab

**Autor:** Isaac Alexander Vega Orozco
**Institución:** Universidad de Costa Rica

## Descripción del Proyecto
Este proyecto es una solución basada en arquitectura por capas para registrar y gestionar solicitudes de ticket para el examen. Está compuesto por una API REST que administra los datos y una aplicación cliente web interactiva que consume dicha API. La persistencia de datos se maneja localmente mediante SQLite y Entity Framework Core.

## Puertos Utilizados
* SoporteLab.Api (Backend): http://localhost:5072
* SoporteLab.Web (Frontend): http://localhost:5031

## Pasos para ejecutar la API
1. Abrir una terminal y navegar hacia la carpeta del backend: `cd SoporteLab.Api`
2. Ejecutar el comando: `dotnet run`
3. Mantener la terminal abierta para que el servidor siga escuchando peticiones.

## Pasos para ejecutar la Web
1. Abrir una segunda terminal independiente y navegar hacia la carpeta del frontend: `cd SoporteLab.Web`
2. Ejecutar el comando: `dotnet run`
3. Abrir el navegador en la dirección indicada (usualmente http://localhost:5031).

## Problemas encontrados y solución aplicada
* Problema: Los botones del frontend (como "Marcar como Respondido" o "Eliminar") no ejecutaban ninguna acción al hacer clic, a pesar de estar correctamente enlazados a métodos C#.
* Solución: Se identificó que las páginas de Blazor en .NET 8 se renderizan de forma estática por defecto. La solución fue inyectar la directiva `@rendermode InteractiveServer` al inicio de las páginas para habilitar la comunicación en tiempo real.