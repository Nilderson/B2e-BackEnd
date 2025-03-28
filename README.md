# B2e - Backend (Microservices with .NET Core 8)

Este projeto é um backend baseado em **.NET Core 8** utilizando arquitetura de **microserviços**. Ele gerencia autenticação, produtos e permite a exportação de produtos para um arquivo Excel.

## 🚀 Tecnologias Utilizadas
- **.NET Core 8**
- **Entity Framework Core**
- **SQL Server**
- **JWT (JSON Web Token)**
- **ClosedXML** (Geração de Excel)
- **Swagger**
- **BCrypt** Para Criptografar as Senhas

## 🔧 Configuração e Instalação

### 1️⃣ **Configurar Banco de Dados**
O projeto utiliza **SQL Server**. :


2️⃣ Executar Migrations

Abra o console do gerenciador de pacotes e execute a seguinte linha 
	- Update-Database -Project Produtos.Infra -StartupProject 1.Produtos.Auth.API

acesse seu banco com SQL Server 
- Pegue os dados para conexao

Atualize a ConnectionStrings nas duas APIs nos dois arquivos appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ProdutosDB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
}


3️⃣ Rodar a Aplicação
Configure o projeto para Rodar mais de um projeto Auth.Api e Products.Api
Ao rodar pegue as Urls para configurar a comunicação com do Front



📦 Endpoints Principais
Método	        	Endpoint	                Descrição
POST	        	/api/auth/login	            	Autenticação e geração de token
GET	            	/api/produtos	            	Lista produtos
POST	        	/api/produtos	            	Adiciona um novo produto
PUT	            	/api/produtos/{id}	        Edita um produto
DELETE	        	/api/produtos/{id}	        Remove um produto
GET	            	/api/produtos/export	    	Exporta produtos para Excel

✅ Testando com Postman ou Frontend
1️⃣ Autentique-se (POST /api/auth/login) e copie o token.
2️⃣ Use o token nas chamadas da API (Authorization: Bearer <TOKEN>).
3️⃣ Consuma a API via Postman, React Frontend ou outra ferramenta.

Arquivo Json para PostMan junto ao arquivo ReadMe

Para cadastrar um usurio pode usar o Swaguer, o Postman ou executar o Script abaixo no SQL Server

USE [ProdutosDB]
GO

INSERT INTO [dbo].[Users]
           ([Login]
           ,[Senha]
           ,[DataCreate])
VALUES('Admin', '$2a$11$uxj0A3dymnKIKqg6Hz4b6OcOFbYEgsk3iXh9c6j/vQEDnQH91BeMq','2025-03-26 01:12:16.8315341')

GO

User Admin
Senha 1234

