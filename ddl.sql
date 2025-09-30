-- Cria o banco de dados se ele não existir
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'mydb')
BEGIN
    CREATE DATABASE mydb;
END
GO -- Comando para separar lotes de instruções

-- Define o banco de dados a ser usado para os comandos seguintes
USE mydb;
GO

-- -----------------------------------------------------
-- Tabela TIPO_USUARIO
-- -----------------------------------------------------
CREATE TABLE TIPO_USUARIO (
  id INT NOT NULL,
  [tipo de usuario] VARCHAR(100) NULL,
  PRIMARY KEY (id)
);
GO

-- -----------------------------------------------------
-- Tabela USUARIO
-- -----------------------------------------------------
CREATE TABLE USUARIO (
  ID INT NOT NULL IDENTITY(1,1), -- Usando IDENTITY para auto-incremento
  NOME VARCHAR(255) NOT NULL,
  hashSENHA VARCHAR(300) NOT NULL,
  TELEFONE CHAR(20) NULL,
  EMAIL VARCHAR(255) NULL,
  CRIAÇÃO DATE NOT NULL,
  TIPO_USUARIO_id INT NOT NULL,
  PRIMARY KEY (ID),
  CONSTRAINT fk_DADOS_USUARIO_TIPO_USUARIO1
    FOREIGN KEY (TIPO_USUARIO_id)
    REFERENCES TIPO_USUARIO (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_DADOS_USUARIO_TIPO_USUARIO1_idx ON USUARIO (TIPO_USUARIO_id ASC);
GO

-- -----------------------------------------------------
-- Tabela TAREFAS
-- -----------------------------------------------------
CREATE TABLE TAREFAS (
  idTAREFAS INT NOT NULL IDENTITY(1,1),
  TAREFA VARCHAR(200) NULL,
  PRIMARY KEY (idTAREFAS)
);
GO

-- -----------------------------------------------------
-- Tabela LOG
-- -----------------------------------------------------
CREATE TABLE LOG (
  idLOG INT NOT NULL IDENTITY(1,1),
  TAREFAS_idTAREFAS INT NOT NULL,
  [DATA HORA] DATETIME NULL,
  DADOS_USUARIO_ID INT NOT NULL,
  PRIMARY KEY (idLOG),
  CONSTRAINT fk_LOG_TAREFAS
    FOREIGN KEY (TAREFAS_idTAREFAS)
    REFERENCES TAREFAS (idTAREFAS)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_LOG_DADOS_USUARIO1
    FOREIGN KEY (DADOS_USUARIO_ID)
    REFERENCES USUARIO (ID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_LOG_TAREFAS_idx ON LOG (TAREFAS_idTAREFAS ASC);
CREATE INDEX fk_LOG_DADOS_USUARIO1_idx ON LOG (DADOS_USUARIO_ID ASC);
GO

-- -----------------------------------------------------
-- Tabela PAIS
-- -----------------------------------------------------
CREATE TABLE PAIS (
  idPAIS INT NOT NULL IDENTITY(1,1),
  NOME VARCHAR(80) NULL,
  PRIMARY KEY (idPAIS)
);
GO

-- -----------------------------------------------------
-- Tabela ESTADO
-- -----------------------------------------------------
CREATE TABLE ESTADO (
  idESTADO INT NOT NULL IDENTITY(1,1),
  NOME VARCHAR(80) NULL,
  PAIS_idPAIS INT NOT NULL,
  PRIMARY KEY (idESTADO),
  CONSTRAINT fk_ESTADO_PAIS1
    FOREIGN KEY (PAIS_idPAIS)
    REFERENCES PAIS (idPAIS)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_ESTADO_PAIS1_idx ON ESTADO (PAIS_idPAIS ASC);
GO

-- -----------------------------------------------------
-- Tabela CIDADE
-- -----------------------------------------------------
CREATE TABLE CIDADE (
  idCIDADE INT NOT NULL IDENTITY(1,1),
  NOME VARCHAR(100) NULL,
  ESTADO_idESTADO INT NOT NULL,
  PRIMARY KEY (idCIDADE),
  CONSTRAINT fk_CIDADE_ESTADO1
    FOREIGN KEY (ESTADO_idESTADO)
    REFERENCES ESTADO (idESTADO)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_CIDADE_ESTADO1_idx ON CIDADE (ESTADO_idESTADO ASC);
GO

-- -----------------------------------------------------
-- Tabela PRODUTO
-- -----------------------------------------------------
CREATE TABLE PRODUTO (
  idPRODUTO INT NOT NULL IDENTITY(1,1),
  NOME VARCHAR(100) NOT NULL,
  DESCRICAO VARCHAR(255) NULL,
  PRECO DECIMAL(10,2) NOT NULL,
  ESTOQUE INT NOT NULL,
  Ativo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (idPRODUTO)
);
GO

-- -----------------------------------------------------
-- Tabela Status
-- -----------------------------------------------------
CREATE TABLE Status (
  idStatus INT NOT NULL,
  nome VARCHAR(45) NULL,
  PRIMARY KEY (idStatus)
);
GO

-- -----------------------------------------------------
-- Tabela PEDIDO
-- -----------------------------------------------------
CREATE TABLE PEDIDO (
  idPEDIDO INT NOT NULL IDENTITY(1,1),
  data DATETIME NOT NULL,
  Total DECIMAL(10,2) NOT NULL,
  USUARIO_ID INT NOT NULL,
  Status_idStatus INT NOT NULL,
  PRIMARY KEY (idPEDIDO),
  CONSTRAINT fk_PEDIDO_USUARIO1
    FOREIGN KEY (USUARIO_ID)
    REFERENCES USUARIO (ID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_PEDIDO_Status1
    FOREIGN KEY (Status_idStatus)
    REFERENCES Status (idStatus)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_PEDIDO_USUARIO1_idx ON PEDIDO (USUARIO_ID ASC);
CREATE INDEX fk_PEDIDO_Status1_idx ON PEDIDO (Status_idStatus ASC);
GO

-- -----------------------------------------------------
-- Tabela ItemPedido
-- -----------------------------------------------------
CREATE TABLE ItemPedido (
  idItemPedido INT NOT NULL IDENTITY(1,1),
  quantidade INT NOT NULL,
  precoUnitario DECIMAL(9,2) NOT NULL,
  PRODUTO_idPRODUTO INT NOT NULL,
  PEDIDO_idPEDIDO INT NOT NULL,
  PRIMARY KEY (idItemPedido),
  CONSTRAINT fk_ItemPedido_PRODUTO1
    FOREIGN KEY (PRODUTO_idPRODUTO)
    REFERENCES PRODUTO (idPRODUTO)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_ItemPedido_PEDIDO1
    FOREIGN KEY (PEDIDO_idPEDIDO)
    REFERENCES PEDIDO (idPEDIDO)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_ItemPedido_PRODUTO1_idx ON ItemPedido (PRODUTO_idPRODUTO ASC);
CREATE INDEX fk_ItemPedido_PEDIDO1_idx ON ItemPedido (PEDIDO_idPEDIDO ASC);
GO

-- -----------------------------------------------------
-- Tabela endereço
-- -----------------------------------------------------
CREATE TABLE [endereço] (
  IDidendereço INT NOT NULL IDENTITY(1,1),
  CIDADE_idCIDADE INT NOT NULL,
  logradouro VARCHAR(255) NOT NULL,
  numero VARCHAR(20) NULL,
  cep VARCHAR(20) NULL,
  complemento VARCHAR(100) NULL,
  USUARIO_ID INT NOT NULL,
  PRIMARY KEY (IDidendereço),
  CONSTRAINT fk_endereço_CIDADE1
    FOREIGN KEY (CIDADE_idCIDADE)
    REFERENCES CIDADE (idCIDADE)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_endereço_USUARIO1
    FOREIGN KEY (USUARIO_ID)
    REFERENCES USUARIO (ID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_endereço_CIDADE1_idx ON [endereço] (CIDADE_idCIDADE ASC);
CREATE INDEX fk_endereço_USUARIO1_idx ON [endereço] (USUARIO_ID ASC);
GO

-- -----------------------------------------------------
-- Tabela CARRINHO
-- -----------------------------------------------------
CREATE TABLE CARRINHO (
  idCARRINHO INT NOT NULL IDENTITY(1,1),
  USUARIO_ID INT NOT NULL,
  datacriacao DATETIME NULL,
  PRIMARY KEY (idCARRINHO),
  CONSTRAINT fk_CARRINHO_USUARIO1
    FOREIGN KEY (USUARIO_ID)
    REFERENCES USUARIO (ID)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_CARRINHO_USUARIO1_idx ON CARRINHO (USUARIO_ID ASC);
GO

-- -----------------------------------------------------
-- Tabela ItemCarrinho
-- -----------------------------------------------------
CREATE TABLE ItemCarrinho (
  idItemCarrinho INT NOT NULL IDENTITY(1,1),
  quantidade INT NULL,
  precoTotal DECIMAL(10,2) NULL,
  CARRINHO_idCARRINHO INT NOT NULL,
  PRODUTO_idPRODUTO INT NOT NULL,
  PRIMARY KEY (idItemCarrinho),
  CONSTRAINT fk_ItemCarrinho_CARRINHO1
    FOREIGN KEY (CARRINHO_idCARRINHO)
    REFERENCES CARRINHO (idCARRINHO)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_ItemCarrinho_PRODUTO1
    FOREIGN KEY (PRODUTO_idPRODUTO)
    REFERENCES PRODUTO (idPRODUTO)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_ItemCarrinho_CARRINHO1_idx ON ItemCarrinho (CARRINHO_idCARRINHO ASC);
CREATE INDEX fk_ItemCarrinho_PRODUTO1_idx ON ItemCarrinho (PRODUTO_idPRODUTO ASC);
GO

-- -----------------------------------------------------
-- Tabela Carousel
-- -----------------------------------------------------
CREATE TABLE Carousel (
  idCarousel INT NOT NULL IDENTITY(1,1),
  nome VARCHAR(100) NULL,
  ativo BIT NOT NULL DEFAULT 0,
  ordem INT NOT NULL,
  PRIMARY KEY (idCarousel)
);
GO

-- -----------------------------------------------------
-- Tabela imagemProduto
-- -----------------------------------------------------
CREATE TABLE imagemProduto (
  idimagemProduto INT NOT NULL IDENTITY(1,1),
  caminhoImagem VARCHAR(300) NULL,
  PRODUTO_idPRODUTO INT NOT NULL,
  PRIMARY KEY (idimagemProduto),
  CONSTRAINT fk_imagemProduto_PRODUTO1
    FOREIGN KEY (PRODUTO_idPRODUTO)
    REFERENCES PRODUTO (idPRODUTO)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_imagemProduto_PRODUTO1_idx ON imagemProduto (PRODUTO_idPRODUTO ASC);
GO

-- -----------------------------------------------------
-- Tabela Carousel_has_imagemProduto
-- -----------------------------------------------------
CREATE TABLE Carousel_has_imagemProduto (
  Carousel_idCarousel INT NOT NULL,
  imagemProduto_idimagemProduto INT NOT NULL,
  PRIMARY KEY (Carousel_idCarousel, imagemProduto_idimagemProduto),
  CONSTRAINT fk_Carousel_has_imagemProduto_Carousel1
    FOREIGN KEY (Carousel_idCarousel)
    REFERENCES Carousel (idCarousel)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_Carousel_has_imagemProduto_imagemProduto1
    FOREIGN KEY (imagemProduto_idimagemProduto)
    REFERENCES imagemProduto (idimagemProduto)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fk_Carousel_has_imagemProduto_imagemProduto1_idx ON Carousel_has_imagemProduto (imagemProduto_idimagemProduto ASC);
CREATE INDEX fk_Carousel_has_imagemProduto_Carousel1_idx ON Carousel_has_imagemProduto (Carousel_idCarousel ASC);
GO
