# 🎮 PAIF TEAM - FIAP CLOUD GAMES

Metas: Garantir persistência de dados, qualidade do software e boas práticas de desenvolvimento, preparando a base para futuras funcionalidades como matchmaking e gerenciamento de servidores.

---

## 🐳 Pré-requisitos

- [Docker](https://www.docker.com/get-started) instalado  
- Opcional: [Git](https://git-scm.com/) para clonar o repositório

---

## ⚙️ Como Executar o Projeto

### 🔥 1. Clonar o Repositório

```bash
git clone https://github.com/PAIFteam/PAIFTeam.FCG.git
cd PAIFTeam.FCG
```

### 🔥 2. Iniciar o Ambiente com Docker
* Certifique-se de que o Docker está em execução antes de iniciar o projeto.

Usando o script **```.bat```** (Windows)

Execute o arquivo  localizado na pasta raiz do projeto:

**```start-compose.bat```**

Ou manualmente (Windows/Linux/Mac)

**```docker compose -f docker-compose.yml up --build -d```**

## 🔗 Acessos da Aplicação

* O SQL, backend e frontend estão configurados para iniciar automaticamente com o Docker Compose nas portas:

    - SQL Server: ```localhost:1433```

    - Swagger (Documentação da API): ```http://localhost:8080/api/docs```

---

## 🚀 Funcionalidades Atingidas

**Cadastro de usuários:**
- Identificação do cliente por nome, e-mail e senha. ✅
- Validar formato de e-mail e senha segura (mínimo de 8 caracteres com números, letras e caracteres especiais). ✅

**Autenticação e Autorização:**
- Implementar autenticação via token JWT. ✅
- Dois níveis de acesso:
  - Usuário – acesso à plataforma e biblioteca de jogos. ✅
  - Administrador – acesso total para criação e edição de usuários, jogos e promoções (se existente). ✅

**Persistência de Dados:**
- Utilizar Entity Framework Core para gerenciar os modelos de usuários e jogos. ✅
- Aplicar migrations para a criação do banco de dados. ✅

**Desenvolvimento de API com .NET 8:**
- API seguindo o padrão Minimal API ou Controllers MVC. ✅
- Middleware para tratamento de erros e logs estruturados. ✅
- Documentação com Swagger para expor os endpoints da API. ✅

**Domain-Driven Design (DDD):**
- Modelagem do domínio utilizando Event Storming para mapear os fluxos de usuários e jogos. ✅
- Princípios de DDD na organização das entidades e regras de negócio. ✅

## 🧪 Qualidade de Software

* **Testes unitários e TDD:**  
  Não conseguimos implementar testes unitários nem aplicar TDD neste momento.  
  Futuramente, pretendemos incluir testes para garantir a qualidade e confiabilidade do software.

---

> **Importante:** O host do SQL Server deve ser configurado como **`sqlserver`** no arquivo de configuração da aplicação para funcionar corretamente com o Docker Compose.

---

**Para utilizar o endpoint de registro de usuário, é obrigatório informar a API KEY:**  
> `44e61eab-b85a-455f-84b3-aa4acbc648e1`