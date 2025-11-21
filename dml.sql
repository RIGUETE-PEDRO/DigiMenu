INSERT INTO TipoUsuario (TipoUsuario) VALUES ('User'), ('Admin');


  --insert usuario 

INSERT INTO Usuario (Nome, HashSenha, Telefone, Email, Criacao, TipoUsuarioId)
VALUES
('usuario', '15e2b0d3c33891ebb0f1ef609ec419420c20e320ce94c65fbc8c3312448eb225',
 '28999534886', 'usuario@gmail.com', '2025-11-17', 1),

('admin', '15e2b0d3c33891ebb0f1ef609ec419420c20e320ce94c65fbc8c3312448eb225',
 '28999534886', 'admin@gmail.com', '2025-11-17', 2);


--status de pedido
INSERT INTO Status (Nome) VALUES
('pendente'),
('em preparo'),
('pronto'),
('saiu para entrega'),
('finalizado'),
('pedido recusado');


  --insert de categoria
INSERT INTO Categoria (nome)
VALUES ('Bebida'), ('Pizza'), ('Lanches'), ('Outros');


  --insert de produtos
INSERT INTO Produto (Nome, Categoria, Descricao, Preco, Estoque, Ativo) VALUES
('Suco de Morango', 1, 'Bebida refrescante feita com morangos frescos.', 12.90, 15, 1),
('Suco de Goiaba', 1, 'Bebida tropical doce e suave.', 12.90, 8, 1),
('Água de Coco', 1, 'Leve, doce e hidratante.', 16.75, 7, 1),
('Aperol Spritz', 1, 'Aperitivo italiano com prosecco.', 31.90, 10, 1),
('Moscow Mule', 1, 'Vodka, ginger beer e limão.', 28.80, 21, 1),
('Soda de Maçã Verde', 1, 'Bebida gaseificada sabor maçã verde.', 18.90, 9, 1),
('Suco de Abacaxi com Hortelã', 1, 'Tropical refrescante.', 10.90, 4, 1),
('Limonada', 1, 'Limão, água e açúcar.', 9.90, 10, 1),

('Coxinha', 3, 'Recheada com frango temperado.', 8.50, 13, 1),
('Empada', 3, 'Massa amanteigada.', 7.90, 10, 1),
('Frango Empanado', 3, 'Filé empanado frito.', 21.90, 50, 1),
('Cachorro Quente', 3, 'Completo com molho especial.', 15.90, 8, 1),
('Esfirra', 3, 'Assada com massa macia.', 13.45, 25, 1),
('Pastéis Fritos', 3, 'Massa crocante.', 18.75, 50, 1),
('Batata Frita', 3, 'Porção generosa.', 36.50, 34, 1),
('Hambúrguer', 3, 'Blend suculento.', 42.50, 28, 1),

('Banana com Chocolate', 2, 'Chocolate cremoso com banana.', 80.00, 5, 1),
('Romeu e Julieta', 2, 'Queijo + goiabada.', 82.60, 8, 1),
('Manjericão', 2, 'Molho, mussarela e manjericão.', 76.80, 4, 1),
('Quatro Queijos', 2, 'Mussarela, provolone, parmesão e catupiry.', 79.90, 8, 1),
('Frango com Catupiry', 2, 'Frango desfiado com catupiry.', 85.70, 12, 1),
('Camarão', 2, 'Camarões frescos.', 98.50, 9, 1);

  --insert de carrousel
INSERT INTO Carousel (Nome, Ativo, Ordem) VALUES
('P:3',1,1), ('P:4',1,2), ('P:5',1,3), ('P:6',1,4), ('P:7',1,5),
('P:8',1,6), ('P:9',1,7), ('P:10',0,0), ('P:11',0,0), ('P:12',0,0),
('P:13',0,0), ('P:14',0,0), ('P:15',0,0), ('P:16',0,0), ('P:17',0,0),
('P:18',0,0), ('P:19',0,0), ('P:20',0,0), ('P:21',0,0), ('P:22',0,0),
('P:23',0,0), ('P:24',0,0);

  --insert de imagemProduto


INSERT INTO ImagemProduto (CaminhoImagem, ProdutoId, CarouselId) VALUES
('imgProduto/2cd679912cd94e5da005573da15ce209.png', 1, 1),
('imgProduto/0dbdeb0ee2ba4cd7ab02772247653df1.png', 2, 2),
('imgProduto/310239f3411c47d09530e020562f6f67.png', 3, 3),
('imgProduto/8e8bb358717b409f8c466942346b9653.png', 4, 4),
('imgProduto/f3e50f17589c4a8fae17d66de79f9791.png', 5, 5),
('imgProduto/c771dd0c35e646779a8d607402448d1c.png', 6, 6),
('imgProduto/4e8edd8989bc462bb23a8b27a613717d.png', 7, 7),
('imgProduto/5e1e7e2345034449bcc81d6051739423.png', 8, 8),
('imgProduto/75dcf52a97604b4ab7c9bd9c4b0e0823.png', 9, 9),
('imgProduto/3b52cfba8a53448f97744f7e1f34df75.png', 10, 10),
('imgProduto/68c1167c514a4cc09e76151d4689cf0f.png', 11, 11),
('imgProduto/3eedf5f5526d4e9ab6057168a4085c1c.png', 12, 12),
('imgProduto/d4ffb64435f14d95abd6d4682c98ec45.png', 13, 13),
('imgProduto/c6e03164c5774e59a2292a8b45fe3cbc.png', 14, 14),
('imgProduto/9de54c5f2e10425789a083a9c3172f40.png', 15, 15),
('imgProduto/7d631a15c63245de98f24d55e47333fc.png', 16, 16),
('imgProduto/9f9ca8b45ea54283b10ee02f9302a5dc.png', 17, 17),
('imgProduto/7ddab3f88eac4b1d810531a2f92badf5.png', 18, 18),
('imgProduto/2f34e4906e204e4ca4a69b0e1a83c756.png', 19, 19),
('imgProduto/32a6b408cda2408bb8c7391a9e2d8403.png', 20, 20),
('imgProduto/0083a6e4488b4ae1b04bb1798caae9b5.png', 21, 21),
('imgProduto/6fb471d66810498bb07c6513d2860efb.png', 22, 22);












