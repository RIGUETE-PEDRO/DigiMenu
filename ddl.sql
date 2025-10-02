-- Cria o banco de dados se ele não existir
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DigiMenu')
BEGIN
    CREATE DATABASE DigiMenu;
END
GO

USE DigiMenu;
GO

-- ===========================
-- Tabela TipoUsuario
-- ===========================
CREATE TABLE TipoUsuario (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TipoUsuario VARCHAR(100) NULL
);

-- ===========================
-- Tabela Usuario
-- ===========================
CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    HashSenha VARCHAR(300) NOT NULL,
    Telefone CHAR(20) NULL,
    Email VARCHAR(255) NULL,
    Criacao DATE NOT NULL,
    TipoUsuarioId INT NOT NULL,
    FOREIGN KEY (TipoUsuarioId) REFERENCES TipoUsuario(Id)
);

-- ===========================
-- Tabela Tarefas
-- ===========================
CREATE TABLE Tarefas (
    IdTarefas INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Tarefa VARCHAR(200) NULL
);

-- ===========================
-- Tabela Log
-- ===========================
CREATE TABLE Log (
    IdLog INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TarefasId INT NOT NULL,
    DataHora DATETIME NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (TarefasId) REFERENCES Tarefas(IdTarefas),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-- ===========================
-- Tabela Pais
-- ===========================
CREATE TABLE Pais (
    IdPais INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(80) NULL
);

-- ===========================
-- Tabela Estado
-- ===========================
CREATE TABLE Estado (
    IdEstado INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(80) NULL,
    PaisId INT NOT NULL,
    FOREIGN KEY (PaisId) REFERENCES Pais(IdPais)
);

-- ===========================
-- Tabela Cidade
-- ===========================
CREATE TABLE Cidade (
    IdCidade INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(100) NULL,
    EstadoId INT NOT NULL,
    FOREIGN KEY (EstadoId) REFERENCES Estado(IdEstado)
);

-- ===========================
-- Tabela Produto
-- ===========================
CREATE TABLE Produto (
    IdProduto INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Descricao VARCHAR(255) NULL,
    Preco DECIMAL(10,2) NOT NULL,
    Estoque INT NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1
  
);

-- ===========================
-- Tabela Status
-- ===========================
CREATE TABLE Status (
    IdStatus INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(45) NULL
);

-- ===========================
-- Tabela Pedido
-- ===========================
CREATE TABLE Pedido (
    IdPedido INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Data DATETIME NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    UsuarioId INT NOT NULL,
    StatusId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
    FOREIGN KEY (StatusId) REFERENCES Status(IdStatus)
);

-- ===========================
-- Tabela ItemPedido
-- ===========================
CREATE TABLE ItemPedido (
    IdItemPedido INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(9,2) NOT NULL,
    ProdutoId INT NOT NULL,
    PedidoId INT NOT NULL,
    FOREIGN KEY (ProdutoId) REFERENCES Produto(IdProduto),
    FOREIGN KEY (PedidoId) REFERENCES Pedido(IdPedido)
);

-- ===========================
-- Tabela Endereco
-- ===========================
CREATE TABLE Endereco (
    IdEndereco INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CidadeId INT NOT NULL,
    Logradouro VARCHAR(255) NOT NULL,
    Numero VARCHAR(20) NULL,
    Cep VARCHAR(20) NULL,
    Complemento VARCHAR(100) NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (CidadeId) REFERENCES Cidade(IdCidade),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-- ===========================
-- Tabela Carrinho
-- ===========================
CREATE TABLE Carrinho (
    IdCarrinho INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UsuarioId INT NOT NULL,
    DataCriacao DATETIME NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-- ===========================
-- Tabela ItemCarrinho
-- ===========================
CREATE TABLE ItemCarrinho (
    IdItemCarrinho INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Quantidade INT NULL,
    PrecoTotal DECIMAL(10,2) NULL,
    CarrinhoId INT NOT NULL,
    ProdutoId INT NOT NULL,
    FOREIGN KEY (CarrinhoId) REFERENCES Carrinho(IdCarrinho),
    FOREIGN KEY (ProdutoId) REFERENCES Produto(IdProduto)
);

-- ===========================
-- Tabela Carousel
-- ===========================
CREATE TABLE Carousel (
    IdCarousel INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(100) NULL,
    Ativo BIT NOT NULL DEFAULT 0,
    Ordem INT NOT NULL
);

-- ===========================
-- Tabela ImagemProduto
-- ===========================
CREATE TABLE ImagemProduto (
    IdImagemProduto INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CaminhoImagem VARCHAR(300) NULL,
    ProdutoId INT NOT NULL,
    FOREIGN KEY (ProdutoId) REFERENCES Produto(IdProduto)
);

-- ===========================
-- Tabela Carousel_has_ImagemProduto
-- ===========================
CREATE TABLE CarouselHasImagemProduto (
    CarouselId INT NOT NULL,
    ImagemProdutoId INT NOT NULL,
    PRIMARY KEY (CarouselId, ImagemProdutoId),
    FOREIGN KEY (CarouselId) REFERENCES Carousel(IdCarousel),
    FOREIGN KEY (ImagemProdutoId) REFERENCES ImagemProduto(IdImagemProduto)
);


