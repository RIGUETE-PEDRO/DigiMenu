-- Cria o banco de dados se ele não existir
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'mydb')
BEGIN
    CREATE DATABASE mydb;
END
GO

-- Define o contexto para o banco de dados 'mydb'
USE mydb;
GO

-- -----------------------------------------------------
-- Tabela: tipoUsuario
-- -----------------------------------------------------
CREATE TABLE tipoUsuario (
  id INT NOT NULL,
  tipoUsuario VARCHAR(100) NULL,
  PRIMARY KEY (id)
);
GO

-- -----------------------------------------------------
-- Tabela: usuario
-- -----------------------------------------------------
CREATE TABLE usuario (
  id INT NOT NULL,
  nome VARCHAR(255) NOT NULL,
  hashSenha VARCHAR(300) NOT NULL,
  telefone CHAR(20) NULL,
  email VARCHAR(255) NULL,
  criacao DATE NOT NULL,
  tipoUsuarioId INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fkUsuarioTipoUsuario
    FOREIGN KEY (tipoUsuarioId)
    REFERENCES tipoUsuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkUsuarioTipoUsuario_idx ON usuario (tipoUsuarioId ASC);
GO

-- -----------------------------------------------------
-- Tabela: tarefas
-- -----------------------------------------------------
CREATE TABLE tarefas (
  idTarefas INT NOT NULL,
  tarefa VARCHAR(200) NULL,
  PRIMARY KEY (idTarefas)
);
GO

-- -----------------------------------------------------
-- Tabela: log
-- -----------------------------------------------------
CREATE TABLE log (
  idLog INT NOT NULL,
  tarefasIdTarefas INT NOT NULL,
  dataHora DATETIME NULL,
  usuarioId INT NOT NULL,
  PRIMARY KEY (idLog),
  CONSTRAINT fkLogTarefas
    FOREIGN KEY (tarefasIdTarefas)
    REFERENCES tarefas (idTarefas)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fkLogUsuario
    FOREIGN KEY (usuarioId)
    REFERENCES usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkLogTarefas_idx ON log (tarefasIdTarefas ASC);
GO
CREATE INDEX fkLogUsuario_idx ON log (usuarioId ASC);
GO

-- -----------------------------------------------------
-- Tabela: pais
-- -----------------------------------------------------
CREATE TABLE pais (
  idPais INT NOT NULL,
  nome VARCHAR(80) NULL,
  PRIMARY KEY (idPais)
);
GO

-- -----------------------------------------------------
-- Tabela: estado
-- -----------------------------------------------------
CREATE TABLE estado (
  idEstado INT NOT NULL,
  nome VARCHAR(80) NULL,
  paisIdPais INT NOT NULL,
  PRIMARY KEY (idEstado),
  CONSTRAINT fkEstadoPais
    FOREIGN KEY (paisIdPais)
    REFERENCES pais (idPais)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkEstadoPais_idx ON estado (paisIdPais ASC);
GO

-- -----------------------------------------------------
-- Tabela: cidade
-- -----------------------------------------------------
CREATE TABLE cidade (
  idCidade INT NOT NULL,
  nome VARCHAR(100) NULL,
  estadoIdEstado INT NOT NULL,
  PRIMARY KEY (idCidade),
  CONSTRAINT fkCidadeEstado
    FOREIGN KEY (estadoIdEstado)
    REFERENCES estado (idEstado)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkCidadeEstado_idx ON cidade (estadoIdEstado ASC);
GO

-- -----------------------------------------------------
-- Tabela: produto
-- -----------------------------------------------------
CREATE TABLE produto (
  idProduto INT NOT NULL,
  nome VARCHAR(100) NOT NULL,
  descricao VARCHAR(255) NULL,
  preco DECIMAL(10,2) NOT NULL,
  estoque INT NOT NULL,
  ativo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (idProduto)
);
GO

-- -----------------------------------------------------
-- Tabela: status
-- -----------------------------------------------------
CREATE TABLE status (
  idStatus INT NOT NULL,
  nome VARCHAR(45) NULL,
  PRIMARY KEY (idStatus)
);
GO

-- -----------------------------------------------------
-- Tabela: pedido
-- -----------------------------------------------------
CREATE TABLE pedido (
  idPedido INT NOT NULL,
  data DATETIME NOT NULL,
  total DECIMAL(10,2) NOT NULL,
  usuarioId INT NOT NULL,
  statusIdStatus INT NOT NULL,
  PRIMARY KEY (idPedido),
  CONSTRAINT fkPedidoUsuario
    FOREIGN KEY (usuarioId)
    REFERENCES usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fkPedidoStatus
    FOREIGN KEY (statusIdStatus)
    REFERENCES status (idStatus)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkPedidoUsuario_idx ON pedido (usuarioId ASC);
GO
CREATE INDEX fkPedidoStatus_idx ON pedido (statusIdStatus ASC);
GO

-- -----------------------------------------------------
-- Tabela: itemPedido
-- -----------------------------------------------------
CREATE TABLE itemPedido (
  idItemPedido INT NOT NULL,
  quantidade INT NOT NULL,
  precoUnitario DECIMAL(9,2) NOT NULL,
  produtoIdProduto INT NOT NULL,
  pedidoIdPedido INT NOT NULL,
  PRIMARY KEY (idItemPedido),
  CONSTRAINT fkItemPedidoProduto
    FOREIGN KEY (produtoIdProduto)
    REFERENCES produto (idProduto)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fkItemPedidoPedido
    FOREIGN KEY (pedidoIdPedido)
    REFERENCES pedido (idPedido)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkItemPedidoProduto_idx ON itemPedido (produtoIdProduto ASC);
GO
CREATE INDEX fkItemPedidoPedido_idx ON itemPedido (pedidoIdPedido ASC);
GO

-- -----------------------------------------------------
-- Tabela: endereco
-- -----------------------------------------------------
CREATE TABLE endereco (
  idEndereco INT NOT NULL,
  cidadeIdCidade INT NOT NULL,
  logradouro VARCHAR(255) NOT NULL,
  numero VARCHAR(20) NULL,
  cep VARCHAR(20) NULL,
  complemento VARCHAR(100) NULL,
  usuarioId INT NOT NULL,
  PRIMARY KEY (idEndereco),
  CONSTRAINT fkEnderecoCidade
    FOREIGN KEY (cidadeIdCidade)
    REFERENCES cidade (idCidade)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fkEnderecoUsuario
    FOREIGN KEY (usuarioId)
    REFERENCES usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkEnderecoCidade_idx ON endereco (cidadeIdCidade ASC);
GO
CREATE INDEX fkEnderecoUsuario_idx ON endereco (usuarioId ASC);
GO

-- -----------------------------------------------------
-- Tabela: carrinho
-- -----------------------------------------------------
CREATE TABLE carrinho (
  idCarrinho INT NOT NULL,
  usuarioId INT NOT NULL,
  dataCriacao DATETIME NULL,
  PRIMARY KEY (idCarrinho),
  CONSTRAINT fkCarrinhoUsuario
    FOREIGN KEY (usuarioId)
    REFERENCES usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkCarrinhoUsuario_idx ON carrinho (usuarioId ASC);
GO

-- -----------------------------------------------------
-- Tabela: itemCarrinho
-- -----------------------------------------------------
CREATE TABLE itemCarrinho (
  idItemCarrinho INT NOT NULL,
  quantidade INT NULL,
  precoTotal DECIMAL(10,2) NULL,
  carrinhoIdCarrinho INT NOT NULL,
  produtoIdProduto INT NOT NULL,
  PRIMARY KEY (idItemCarrinho),
  CONSTRAINT fkItemCarrinhoCarrinho
    FOREIGN KEY (carrinhoIdCarrinho)
    REFERENCES carrinho (idCarrinho)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fkItemCarrinhoProduto
    FOREIGN KEY (produtoIdProduto)
    REFERENCES produto (idProduto)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkItemCarrinhoCarrinho_idx ON itemCarrinho (carrinhoIdCarrinho ASC);
GO
CREATE INDEX fkItemCarrinhoProduto_idx ON itemCarrinho (produtoIdProduto ASC);
GO

-- -----------------------------------------------------
-- Tabela: carousel
-- -----------------------------------------------------
CREATE TABLE carousel (
  idCarousel INT NOT NULL,
  nome VARCHAR(100) NULL,
  ativo BIT NOT NULL DEFAULT 0,
  ordem INT NOT NULL,
  PRIMARY KEY (idCarousel)
);
GO

-- -----------------------------------------------------
-- Tabela: imagemProduto
-- -----------------------------------------------------
CREATE TABLE imagemProduto (
  idImagemProduto INT NOT NULL,
  caminhoImagem VARCHAR(300) NULL,
  produtoIdProduto INT NOT NULL,
  PRIMARY KEY (idImagemProduto),
  CONSTRAINT fkImagemProdutoProduto
    FOREIGN KEY (produtoIdProduto)
    REFERENCES produto (idProduto)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkImagemProdutoProduto_idx ON imagemProduto (produtoIdProduto ASC);
GO

-- -----------------------------------------------------
-- Tabela: carouselHasImagemProduto
-- -----------------------------------------------------
CREATE TABLE carouselHasImagemProduto (
  carouselIdCarousel INT NOT NULL,
  imagemProdutoIdImagemProduto INT NOT NULL,
  PRIMARY KEY (carouselIdCarousel, imagemProdutoIdImagemProduto),
  CONSTRAINT fkCarouselHasImagemProdutoCarousel
    FOREIGN KEY (carouselIdCarousel)
    REFERENCES carousel (idCarousel)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fkCarouselHasImagemProdutoImagemProduto
    FOREIGN KEY (imagemProdutoIdImagemProduto)
    REFERENCES imagemProduto (idImagemProduto)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);
GO
CREATE INDEX fkCarouselHasImagemProdutoImagemProduto_idx ON carouselHasImagemProduto (imagemProdutoIdImagemProduto ASC);
GO
CREATE INDEX fkCarouselHasImagemProdutoCarousel_idx ON carouselHasImagemProduto (carouselIdCarousel ASC);
GO
