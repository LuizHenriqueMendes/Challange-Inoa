# Challenge-Inoa
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


O repositório é composto principalmente, por:

     1. Stock.cs
     2. ApiResponse.cs
     3. SendEmail.cs
     4. GraphGen.cs
     5. Project.csproj
     5. .env

Para que o código execute, basta seguir o seguinte passo a passo:

     1. Verifique se o .NET está baixado na versão 8.0
          - recomendo baixar através do link: https://dotnet.microsoft.com/pt-br/download/dotnet/8.0
     2. Verifique se as bibliotecas estão corretamente baixadas;
          - utilize o comando: dotnet restore Project.csproj
     3. Dentro do .env, modifique as linhas:
          - SMTP_SERVER= smtp.gmail.com
          - SMTP_PORT= 587
          - SMTP_USER= // Adicione o usuário (Provavelmente o mesmo email que enviará o email)
          - SMTP_PASSWORD=  // Adicione a senha do usuário (*)
          - FROM= // Adicione o email que enviará o email
          - TO= // Adicione o email que receberá a mensagem

(*) Caso utilize verificação em dois fatores:
     - acesse o link: https://myaccount.google.com/apppasswords?continue=https://myaccount.google.com/security?origin%3D3%26rapt%3DAEjHL4N0tUNUERiQuuK3uLgv0biNjLgcFt_6qoDl_GA8AHtw1OH-sQEg2st4rhu_oxI-DAF2CBb3DFJPKSCVbNFdJUb4tQlQ5klJrq1sbuPfiq0fdBLn9Zw&rapt=AEjHL4NYJkRwixB5gWi2tyh3sp8FR7-EQ0Iu0TcGgVKTqjH3AGX9lgC1hdmX4nkPUXETs1JqvZaiPhfmBoYzhVm7k1klwCBgJVn5MvFagGa7CUcWNBK6Y8Y
     - Em seguida, escreva um nome para identificar qual será o app que usará a senha gerada e clique em "Criar";
     - Copie o código e insira no campo "SMTP_PASSWORD" (sem remover os espaços);

Utilizar o seguinte comando no cmd:
> dotnet run [STOCK] [VALOR_DE_VENDA] [VALOR_DE_COMPRA]
ex:
> dotnet run PETR4 22.67 22.59

obs: Caso tenha qualquer tipo de dúvida sobre como rodar o código ou algum problema encontrado, por favor entre em contato comigo através de: luiz.mendescastro@gmail.com
Estarei mais do que disposto para melhorar este repositório e fazê-lo mais eficiente!
