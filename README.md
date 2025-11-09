# DigiMenu

**DigiMenu** é um sistema de administração de páginas web voltado para o setor alimentício — sendo mais específico, **lanchonetes**.  
O sistema serve para atender clientes com páginas de compras e cardápios interativos, além da parte administrativa, que tem como principais funções:

---

| Função |
|--|
| Cadastro de produto |
| Cadastro de carrossel |
| Recebimento de pedidos |
| Gerenciar status dos pedidos |
| Logins com senhas criptografadas |
| Capacidade de adicionar mais administradores |

---

## Ferramentas Usadas
<div align="center">
  <img src="ImgReadme/sqlServer.webp" alt="SQL Server" width="120" style="margin: 10px;">
  <img src="ImgReadme/dotNET.png" alt=".NET" width="120" style="margin: 10px;">
</div>

---

## COMO CONFIGURAR
<details>
  <summary>Passo 1</summary>
atravez da tecnologia git utilizaremos para baixar o projeto utilizando a tecnologia git 

  ```git
     git clone https://github.com/RIGUETE-PEDRO/DigiMenu.git
```
</details>
<details>
<summary>Passo 2</summary>

Para o primeiro passo, iremos pegar o **script do banco de dados**, que se encontra dentro do projeto.

Logo após, copie o script para a criação do banco de dados [ddl.sql](./ddl.sql)

Execute o script no **SQL Server**, e em seguida, rode também o [dml.sql](./dml.sql)

Assim teremos os dados populados e as tabelas criadas para a inicialização do projeto.
</details>

<details>
<summary>Passo 3</summary>

Podemos agora entrar no **Visual Studio 2022** e clicar onde aparecer **"Abrir uma pasta local"**.  
Procure o diretório onde está o projeto **DigiMenu**, e ao entrar, dê dois cliques em **DigiMenuSolution**.  
Por fim, basta executar o arquivo **Default.aspx** para iniciar o projeto 
</details>
