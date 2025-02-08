# TaskMaster
Sistema para Gerenciamento de Tarefas.

##Configurações Necessárias:

Para rodar a aplicação será necessário seguir os seguintes passos após realizar o download do projeto

1. Ir até o arquivo appsettings.json presente em TaskMaster.Presentation e atualizar a string de conexão: "DefaultConnection";
2. Verifique se na pasta .\TaskMaster.Infrastructure\Migrations existem os arquivos relativos a migração.
- Caso não existam será necessário realizar a execução dos seguintes comandos no terminal:

```
cd .\TaskMaster.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ..\TaskMaster.Presentation\
```

- Se ocorrer erro no momento da migração, pode ser necessário instalar o Entity Framework Core globalmente, para isso sexecute:

```
dotnet tool install --global dotnet-ef
```

4. Precisará rodar os seguintes comandos no terminal para realizar a atualização do banco de dados com as tabelas criadas na migração:
```
cd .\TaskMaster.Infrastructure
dotnet ef database update --startup-project ../TaskMaster.Presentation
```

5. Após isso para executar a aplicação execute:
```
cd .\TaskMaster.Presentation
dotnet run
```

6. Para acessar ao front-end, basta executa-lo e abri-lo no seu navegador de preferência, ou até, através do Live Server através do Visual Studio Code.

## Testes

Para realização dos testes é possível realiza-los via Postman, diretamente pelo front-end e pelo Swagger, ou pelo projeto de Testes.

- Para realização dos testes via postman, disponibilizei um arquivo json para que possa ser impostado atraves do seguinte [link](https://drive.google.com/file/d/1zi8nlqCEz7635Qn-YPO_kDS5SO3glXWd/view?usp=drive_link).
- Para acessar o Swagger, poderá acessar localmente com o endereço "[https://localhost:7081/](https://localhost:7081/swagger/index.html)" caso não tenha sido alteradas as configurações de execução no arquivo TaskMaster.Presentation/Properties/launchSettings.json
- Para rodar o projeto de testes, pelo terminal deverá rodar o seguinte código:
```
cd .\TaskMaster.Tests
dotnet test
```
- Para teste via Front-End há alguns pontos:
  ![image](https://github.com/user-attachments/assets/864f860a-e518-4331-9269-52936ce89eeb)
  - Ao abrir a tela, o sistema já tenta comunicar com a API. 
  - Para o teste de cadastro deve-se clicar em "Nova Tarefa".
  - Para o teste de Edição deve-se clicar no ícone com o lápis.
  - Para o teste de Deleção, deve-se clicar no ícone com a lixeira.
  - Para o teste de filtragem deve-se clicar nos check-box acima da tabela.
  - O ícone com o olho serve para visualização de mais detalhes das tarefas.

## Banco de Dados

O banco de dados utilizado foi o SQL Server, e utilizado o SGBD SQL Server Management Studio para acesso e verificações dos registros.
Utilizado também a conexão com usuário do windows e foi necessário na string de conexão informar o TrustServerCertificate=true devido a erros de assinatura ao tentar realizar a migração e o acesso ao banco.
