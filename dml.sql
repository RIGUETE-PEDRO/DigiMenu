INSERT INTO TipoUsuario(TipoUsuario) VALUES ('User');
INSERT INTO TipoUsuario(TipoUsuario) VALUES ('Admin');

  --insert usuario 

INSERT INTO DigiMenu.dbo.Usuario
(Id, Nome, HashSenha, Telefone, Email, Criacao, TipoUsuarioId)
VALUES(1, N'usuario', N'15e2b0d3c33891ebb0f1ef609ec419420c20e320ce94c65fbc8c3312448eb225', N'28999534886         ', N'usuario@gmail.com', '2025-11-17', 1);
INSERT INTO DigiMenu.dbo.Usuario
(Id, Nome, HashSenha, Telefone, Email, Criacao, TipoUsuarioId)
VALUES(2, N'admin', N'15e2b0d3c33891ebb0f1ef609ec419420c20e320ce94c65fbc8c3312448eb225', N'28999534886         ', N'admin@gmail.com', '2025-11-17', 2);


--status de pedido
insert into status values ('pendente')
insert into status values ('em preparo')
insert into status values ('pronto')
insert into status values ('saiu para entrega')
insert into status values ('finalizado')
insert into status values ('pedido recusado')

  --insert de categoria
insert into categoria (nome) values ('Bebida'),('Pizza'),('Lanches'),('Outros')

  --insert de produtos
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(3, N'Suco de Morango', 1, N'Bebida refrescante feita com morangos frescos.', 12.90, 15, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(4, N'Suco de Goiaba', 1, N'Bebida de fruta tropical de sabor doce e suave.', 12.90, 8, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(5, N'Água de Coco', 1, N'É leve, doce, hidratante e com baixo teor calórico.', 16.75, 7, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(6, N'Aperol Spritz', 1, N'Aperitivo italiano de cor laranja vibrante, feito com Aperol, Prosecco e um toque de água com gás.', 31.90, 10, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(7, N'Moscow Mule', 1, N'Coquetel feito com vodka, cerveja de gengibre e suco de limão.', 28.80, 21, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(8, N'Soda de Maçã Verde', 1, N'Bebida gaseificada com sabor artificial de maçã verde.', 18.90, 9, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(9, N'Suco de Abacaxi com Hortelã', 1, N'Suco tropical revigorante que combina a doçura e acidez.', 10.90, 4, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(10, N'Limonada', 1, N'Bebida clássica feita com suco de limão, água e açúcar.', 9.90, 10, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(11, N'Coxinha', 3, N'Recheado com frango desfiado temperado.', 8.50, 13, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(12, N'Empada', 3, N'Empada individual, com massa fofinha e amanteigada.', 7.90, 10, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(13, N'Frango Empanado', 3, N'Porção de filé de frango empanado frito.', 21.90, 50, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(14, N'Cachorro Quente', 3, N'Pão macio, salsicha de qualidade, servido com nosso molho de tomate, milho, ervilha.', 15.90, 8, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(15, N'Esfirra', 3, N'Salgado árabe de massa macia, assado em forno quente.', 13.45, 25, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(16, N'Pastéis Fritos', 3, N'Massa fina e crocante, frita na hora, com recheios tradicionais.', 18.75, 50, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(17, N'Batata Frita', 3, N'Porção de batatas fritas, perfeitas para acompanhar qualquer lanche.', 36.50, 34, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(18, N'Hambúrguer', 3, N'Pão de brioche/gergelim, com suculento blend de carne, acompanhado de queijo, alface, tomate e molho especial.', 42.50, 28, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(19, N'Banana com Chocolate', 2, N'Base de chocolate cremoso, fatias de banana fresca, canela e açúcar.', 80.00, 5, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(20, N'Romeu e Julieta', 2, N'A união perfeita entre o salgado do queijo e o doce da goiabada derretida.', 82.60, 8, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(21, N'Manjericão', 2, N'A simplicidade perfeita: molho de tomate, mussarela, frescas folhas de manjericão.', 76.80, 4, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(22, N'Quatro Queijos', 2, N'Combinação harmoniosa de mussarela, provolone, parmesão e catupiry.', 79.90, 8, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(23, N'Frango com Catupiry', 2, N'Saboroso frango desfiado e temperado coberto com a original e cremosa requeijão Catupiry.', 85.70, 12, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(24, N'Camarão', 2, N'Delicados camarões frescos salteados no azeite, sobre uma base de mussarela.', 98.50, 9, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(25, N'Portuguesa', 2, N'Uma combinação rica: mussarela, presunto, ovos cozidos, cebola e azeitonas.', 88.50, 27, 1);
INSERT INTO DigiMenu.dbo.Produto
(IdProduto, Nome, Categoria, Descricao, Preco, Estoque, Ativo)
VALUES(26, N'Pepperoni', 2, N'Molho de tomate, queijo mussarela e generosas fatias de pepperoni.', 90.00, 11, 1);

  --insert de carrousel
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(3, N'P:3', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(4, N'P:4', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(5, N'P:5', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(6, N'P:6', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(7, N'P:7', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(8, N'P:8', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(9, N'P:9', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(10, N'P:10', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(11, N'P:11', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(12, N'P:12', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(13, N'P:13', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(14, N'P:14', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(15, N'P:15', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(16, N'P:16', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(17, N'P:17', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(18, N'P:18', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(19, N'P:19', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(20, N'P:22', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(21, N'P:23', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(22, N'P:24', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(23, N'P:25', 1, 0);
INSERT INTO DigiMenu.dbo.Carousel
(IdCarousel, Nome, Ativo, Ordem)
VALUES(24, N'P:26', 1, 0);

  --insert de imagemProduto

INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(3, N'imgProduto/2cd679912cd94e5da005573da15ce209.png', 3, 3);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(4, N'imgProduto/0dbdeb0ee2ba4cd7ab02772247653df1.png', 4, 4);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(5, N'imgProduto/310239f3411c47d09530e020562f6f67.png', 5, 5);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(6, N'imgProduto/8e8bb358717b409f8c466942346b9653.png', 6, 6);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(7, N'imgProduto/f3e50f17589c4a8fae17d66de79f9791.png', 7, 7);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(8, N'imgProduto/c771dd0c35e646779a8d607402448d1c.png', 8, 8);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(9, N'imgProduto/4e8edd8989bc462bb23a8b27a613717d.png', 9, 9);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(10, N'imgProduto/5e1e7e2345034449bcc81d6051739423.png', 10, 10);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(11, N'imgProduto/75dcf52a97604b4ab7c9bd9c4b0e0823.png', 11, 11);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(12, N'imgProduto/3b52cfba8a53448f97744f7e1f34df75.png', 12, 12);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(13, N'imgProduto/68c1167c514a4cc09e76151d4689cf0f.png', 13, 13);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(14, N'imgProduto/3eedf5f5526d4e9ab6057168a4085c1c.png', 14, 14);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(15, N'imgProduto/d4ffb64435f14d95abd6d4682c98ec45.png', 15, 15);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(16, N'imgProduto/c6e03164c5774e59a2292a8b45fe3cbc.png', 16, 16);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(17, N'imgProduto/9de54c5f2e10425789a083a9c3172f40.png', 17, 17);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(18, N'imgProduto/7d631a15c63245de98f24d55e47333fc.png', 18, 18);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(19, N'imgProduto/9f9ca8b45ea54283b10ee02f9302a5dc.png', 19, 19);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(20, N'imgProduto/7ddab3f88eac4b1d810531a2f92badf5.png', 20, 25);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(21, N'imgProduto/2f34e4906e204e4ca4a69b0e1a83c756.png', 21, 26);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(22, N'imgProduto/32a6b408cda2408bb8c7391a9e2d8403.png', 22, 20);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(23, N'imgProduto/0083a6e4488b4ae1b04bb1798caae9b5.png', 23, NULL);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(24, N'imgProduto/6fb471d66810498bb07c6513d2860efb.png', 24, NULL);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(25, N'imgProduto/26f06788d88e4561b409508fc48a4a65.png', 25, NULL);
INSERT INTO DigiMenu.dbo.ImagemProduto
(IdImagemProduto, CaminhoImagem, ProdutoId, CarouselId)
VALUES(26, N'imgProduto/5329d9ff52404835a6d9a729922299e9.png', 26, NULL);





