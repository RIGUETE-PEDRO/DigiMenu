INSERT INTO TipoUsuario(TipoUsuario) VALUES ('User');
INSERT INTO TipoUsuario(TipoUsuario) VALUES ('Admin');

INSERT INTO Tarefas(Tarefa) VALUES ('Cadastrar Usuario');
INSERT INTO Tarefas(Tarefa) VALUES ('login');
insert into Tarefas (Tarefa) values ('Cadastro de Produto');
insert into Tarefas (Tarefa) values ('Atualização de Produto');
insert into Tarefas (Tarefa) values ('Delete de produto ');
insert into Tarefas (Tarefa) values ('adicionar produto no carrinho');
insert into Tarefas (Tarefa) values ('interar com as configuraçoes do carrousel');

--status de pedido
insert into status values ('pendente')
insert into status values ('em preparo')
insert into status values ('pronto')
insert into status values ('saiu para entrega')
insert into status values ('finalizado')
insert into status values ('pedido recusado')

  --insert de categoria
insert into categoria (nome) values ('Bebida'),('Pizza'),('Lanches'),('Outros')

--insert de paises 

INSERT INTO Pais (Nome)
VALUES 
('Afeganistão'), ('África do Sul'), ('Albânia'), ('Alemanha'), ('Andorra'),
('Angola'), ('Antígua e Barbuda'), ('Arábia Saudita'), ('Argélia'), ('Argentina'),
('Armênia'), ('Austrália'), ('Áustria'), ('Azerbaijão'), ('Bahamas'),
('Bahrein'), ('Bangladesh'), ('Barbados'), ('Bélgica'), ('Belize'),
('Benin'), ('Bielorrússia'), ('Bolívia'), ('Bósnia e Herzegovina'), ('Botsuana'),
('Brasil'), ('Brunei'), ('Bulgária'), ('Burkina Faso'), ('Burundi'),
('Cabo Verde'), ('Camarões'), ('Camboja'), ('Canadá'), ('Catar'),
('Cazaquistão'), ('Chade'), ('Chile'), ('China'), ('Chipre'),
('Colômbia'), ('Comores'), ('Coreia do Norte'), ('Coreia do Sul'), ('Costa do Marfim'),
('Costa Rica'), ('Croácia'), ('Cuba'), ('Dinamarca'), ('Djibuti');
