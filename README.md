# 🛠️ API RESTful - Teste Desenvolvedor Pleno (.NET 8 + SQLite)

Este projeto consiste em uma API RESTful desenvolvida com **C# ASP.NET Core 8.0** e **SQLite**. O objetivo é demonstrar domínio de boas práticas de arquitetura, autenticação JWT, validações e operações básicas de CRUD para produtos e categorias.

---

## 🚀 Como executar o projeto

### ✅ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) ou outro editor compatível
- Git (opcional, para clonar o repositório)

### ⚙️ Passo a passo

1. Clone o projeto:
   ```bash
   git clone https://github.com/seu-usuario/seu-repositorio.git
   cd seu-repositorio
   ```
2. Restaure os pacotes e compile:
   ```bash
   dotnet clean
   dotnet build
   ```
3. Configure o UserSecrets para armazenar a chave JWT (já configurado no projeto):
   ```bash
   dotnet user-secrets set "Jwt:Key" "sua-chave"
   ```
4. Execute a API:
   ```bash
   dotnet run
   ```
A API estará disponível em: https://localhost:5001

## 🔐 Autenticação

A autenticação é feita via JWT. Para obter um token, use:

### 🔓 ```POST /api/auth/login```
**Body:**
```bash
{
  "username": "admin",
  "password": "admin"
}
```
Resposta:
```bash
{
  "token": "<seu_token_jwt>"
}
```

Use este token em chamadas protegidas no cabeçalho:

```bash
Authorization: Bearer <seu_token_jwt>
```

## 📦 Endpoints principais

### 📁 Categorias

#### 🔍 GET ```/api/category```
Retorna todas as categorias.

#### ➕ POST ```/api/category```
**Body:**
```bash
{
  "name": "Eletrônicos"
}
```
#### ✏️ PUT ```/api/category```
```bash
{
  "id": 1,
  "name": "Livros"
}
```
#### ❌ DELETE ```/api/product/{id}```

### 📦 Produtos

#### 🔍 GET ```/api/product```
Lista todos os produtos.

#### 📄 GET ```/api/product/paged?pageNumber=1&pageSize=10&categoryId=2```
Paginação por categoria.

#### ➕ POST ```/api/product```
```bash
{
  "name": "Notebook",
  "description": "Notebook Dell XPS",
  "price": 8999.99,
  "categoryId": 2
}
```

#### ✏️ PUT ```/api/product```
```bash
{
  "id": 5,
  "name": "Notebook Atualizado",
  "description": "Descrição atualizada",
  "price": 7999.99,
  "categoryId": 2
}
```

#### ❌ DELETE ```/api/product/{id}```

## 🧰 Tecnologias Utilizadas
- ASP.NET Core 8.0
- SQLite (via Entity Framework Core)
- Autenticação JWT
- Flunt (validações)
- Swagger (documentação automática)
- Newtonsoft.Json
