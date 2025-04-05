# 🧪 API .NET - Teste de Desenvolvedor | JMoraes

Este repositório contém uma API desenvolvida com **ASP.NET Core 8** e **SQLite**, criada como parte de um **teste técnico para a empresa JMoraes**. A aplicação oferece um CRUD completo para **produtos** e **categorias**, com autenticação via **JWT** e documentação **Swagger**.

---

## 🗂️ Estrutura do Projeto

```bash
├── Controllers/              # Endpoints da API
│   ├── AuthController.cs
│   ├── CategoryController.cs
│   ├── ProductController.cs
│   └── Validators/
│       ├── Category/
│       └── Product/
│
├── Data/
│   ├── Mapping/              # Mapeamentos EF Core
│   ├── Migrations/           # Histórico do banco SQLite
│   ├── Repositories/         # Interfaces e implementações de acesso a dados
│   └── AppDbContext.cs
│
├── Helpers/
│   └── PagedResult.cs        # Classe de paginação
│
├── Models/                   # Entidades e DTOs
│   ├── DTO/
│   ├── BaseModel.cs
│   ├── Category.cs
│   ├── Product.cs
│   ├── User.cs
│   └── TokenService.cs
│
├── appsettings.json          # Configurações do projeto
├── Program.cs
├── Startup.cs
