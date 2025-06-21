# Challange-Inoa
Desafio para o Processo Seletivo da Inoa


Este repositório tem como objetivo principal estudar e treinar a utilização de APIs, como fazer a requisição, utilizar o DTO e, em seguida, receber, tratar e analisar os dados recebidos.

O desafio surgiu como parte do Processo Seletivo para a INOA, empresa de Desenvolvimentos de Sistemas voltados para o mercado financeiro. Segue o enunciado do desafio:
O objetivo do sistema é avisar, via e-mail, caso a cotação de um ativo da B3 caia mais do que certo nível, ou suba acima de outro.

O programa deve ser uma aplicação de console (não há necessidade de interface gráfica).
Ele deve ser chamado via linha de comando com 3 parâmetros.
     1. O ativo a ser monitorado
     2. O preço de referência para venda
     3. O preço de referência para compra

Ex.
> stock-quote-alert.exe PETR4 22.67 22.59 

Ele deve ler de um arquivo de configuração com:
     1. O e-mail de destino dos alertas
     2. As configurações de acesso ao servidor de SMTP que irá enviar o e-mail

A escolha da API de cotação é livre.

O programa deve ficar continuamente monitorando a cotação do ativo enquanto estiver rodando.

============================================================================================================================


O repositório é composto por:

     1. Stock.cs
     2. ApiResponse.cs
     3. SendEmail.cs
     4. Project.csproj
     5. .env

Para que o código execute, basta seguir o seguinte passo a passo:

     1. Verifique se as extensões estão corretamente baixadas;
     2. Verifique se o .NET está baixado na versão 8.0
     3. Dentro do .env, modifique as linhas:
          - SMTP_SERVER= // Adicione o email do SMTP (smtp.gmail.com)
          - SMTP_PORT= //Adicione a porta do SMTP (Normalmente 587)
          - SMTP_USER= // Adicione o usuário (Provavelmente o mesmo email que enviará o email)
          - SMTP_PASSWORD=  // Adicione a senha do usuário
          - FROM= // Adicione o email que enviará o email
          - TO= // Adicione o email que receberá a mensagem


Utilizar o seguinte comando no cmd:
> dotnet run [STOCK] [VALOR_DE_VENDA] [VALOR_DE_COMPRA]

