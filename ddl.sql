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
-- Tabela categoria
-- ===========================
create table Categoria(
	id int identity primary key,
	nome varchar (100) not null 
)

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
-- Tabela Produto
-- ===========================
CREATE TABLE Produto (
    IdProduto INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Categoria int NOT NULL,
    Descricao VARCHAR(255) NULL,
    Preco DECIMAL(10,2) NOT NULL,
    Estoque INT NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (Categoria) references Categoria (id)
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
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE,
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
    FOREIGN KEY (ProdutoId) REFERENCES Produto(IdProduto) ON DELETE CASCADE,
    FOREIGN KEY (PedidoId) REFERENCES Pedido(IdPedido) ON DELETE CASCADE
);

-- ===========================
-- Tabela Endereco
-- ===========================
CREATE TABLE Endereco (
    IdEndereco INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Cidade varchar(255) NOT NULL,
    Logradouro VARCHAR(255) NOT NULL,
    Numero VARCHAR(20) NULL,
    Cep VARCHAR(20) NULL,
    Complemento VARCHAR(100) NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);

-- ===========================
-- Tabela Carrinho
-- ===========================
CREATE TABLE Carrinho (
    IdCarrinho INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UsuarioId INT NOT NULL,
    DataCriacao DATETIME NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
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
    FOREIGN KEY (CarrinhoId) REFERENCES Carrinho(IdCarrinho) ON DELETE CASCADE,
    FOREIGN KEY (ProdutoId) REFERENCES Produto(IdProduto) ON DELETE CASCADE
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
    CaminhoImagem VARCHAR(500) NOT NULL,
    ProdutoId INT NOT NULL,
    CarouselId INT NULL, 
    FOREIGN KEY (ProdutoId) REFERENCES Produto(IdProduto) ON DELETE CASCADE,
    FOREIGN KEY (CarouselId) REFERENCES Carousel(IdCarousel)
);
