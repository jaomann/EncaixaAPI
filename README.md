# EncaixaAPI

Projeto ASP.NET Core que fornece uma API RESTful para controle e gerenciamento de encaixes em estabelecimentos. Com foco em boas práticas de arquitetura, organização e entrega de software via contêineres.

## 🔧 Tecnologias Utilizadas

- ASP.NET Core
- Entity Framework Core
- SQL Server (via Docker)
- Docker & Docker Compose
- Swagger (OpenAPI)

---

## 🚀 Como executar este projeto via Docker

### Pré-requisitos

- Docker Desktop instalado e em execução
- Git instalado

### Passos para rodar o projeto:

1. **Clone o repositório**

   ```bash
   git clone https://github.com/jaomann/EncaixaAPI.git
   cd EncaixaAPI

2. **(Opcional) Atualize o repositório local**

    Caso já tenha clonado anteriormente:
    ```bash
    git pull

3. **Ajuste nas configurações**
   ```
   Retire o ".example" dos arquivos, appsettings.example.json e .env.example
   Não se esqueça de criar uma senha forte para o SQL Server na variável DB_PASSWORD
   dentro do arquivo .env

3. **Suba os contêineres**
    Acesse o cmd e vá até a pasta com o projeto clonado e atualizado
      ```
      cd diretorio\EncaixaAPI\
      ```
      
    Rode este comando, ele irá compilar a imagem da API e subir os serviços (API + banco de dados):

       docker compose up --build -d

5. **Acesse o Swagger**

Após o Docker finalizar o build, acesse:

http://localhost:8080/swagger

📸 Swagger UI


Exemplos:


![image](https://github.com/user-attachments/assets/09d7f764-df3b-4636-aec1-d5885a889a6a)

Gere o seu login demo

![image](https://github.com/user-attachments/assets/0107e7d0-4883-4027-8d55-f6fa01353457)

Faça o login

![image](https://github.com/user-attachments/assets/108aa5ff-17fa-46e3-8ebc-542d12b7a2a6)

Adicione bearer + seu token para logar

![image](https://github.com/user-attachments/assets/3b270a6a-2cac-4d33-b099-d62ca2e589ac)


📌 Principais Relacionamentos:

  Fluxo de Empacotamento:

    PackingController → PackingService → BinPacker (algoritmo)
                            ↓
                    BoxRepository → Context → Sql Server
  Autenticação:
    
    AuthController → AuthService → JWT

As migrações do EF Core são aplicadas automaticamente na inicialização (via EnsureCreated()).

🤝 Contato
Fique à vontade para entrar em contato para dúvidas ou feedbacks sobre o projeto:

João Emanuel
https://github.com/jaomann
[joaoeemanuel18@gmail.com]
