# TaskMaster
Sistema para Gerenciamento de Tarefas.

##Configurações Necessárias:

Para rodar a aplicação será necessário seguir os seguintes passos após realizar o download do projeto

  1. Ir até o arquivo appsettings.json presente em TaskMaster.Presentation e atualizar a string de conexão: "DefaultConnection";
  2. Precisará rodar os seguintes comandos no terminal para realizar a atualização do banco de dados com as tabelas criadas na migração:
    ``` cd TaskMaster.Infrastructure
    dotnet ef database update --startup-project ../TaskMaster.Presentation ```
