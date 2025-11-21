using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DigiMenu.DAL
{
    public class PedidoDAO
    {
        public void CarregarPedidos(int clienteId, Repeater rptPedidos, Panel pnlSemPedidos)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var pedidos = ctx.Pedido
                    .Where(p => p.UsuarioId == clienteId)
                    .Select(p => new
                    {
                        p.IdPedido,
                        p.Data,
                        p.Total,
                        Cliente = p.Usuario.Nome,
                        Status = p.Status.Nome,
                        Endereco = p.ItemPedido.Select(i => i.Endereco).FirstOrDefault(), // endereço único
                        Itens = p.ItemPedido.Select(i => new
                        {
                            Produto = i.Produto.Nome,
                            i.Quantidade,
                            i.PrecoUnitario
                        })
                    })
                    .AsEnumerable() // passa para memória para projetar navegáveis
                    .Select(p => new
                    {
                        p.IdPedido,
                        p.Data,
                        p.Total,
                        p.Cliente,
                        p.Status,
                        Cidade = p.Endereco != null ? p.Endereco.Cidade : null,
                        Numero = p.Endereco != null ? p.Endereco.Numero : null,
                        Complemento = p.Endereco != null ? p.Endereco.Complemento : null,
                        Logradouro = p.Endereco != null ? p.Endereco.Logradouro : null,
                        Itens = p.Itens.ToList()
                    })
                    .OrderByDescending(p => p.Data)
                    .ToList();

                if (pnlSemPedidos != null)
                    pnlSemPedidos.Visible = pedidos.Count == 0;

                if (rptPedidos != null)
                {
                    rptPedidos.DataSource = pedidos;
                    rptPedidos.DataBind();

                    int index = 0;
                    foreach (RepeaterItem item in rptPedidos.Items)
                    {
                        var pedido = pedidos[index++];
                        var rptItens = item.FindControl("rptItens") as Repeater;
                        if (rptItens != null)
                        {
                            rptItens.DataSource = pedido.Itens;
                            rptItens.DataBind();
                        }
                    }
                }
            }
        }

        // ADMIN: Carrega pedidos pendentes (status pendente)
        public void AdminCarregarPendentes(Repeater rptPedidos, Panel pnlSemPedidos)
        {
            try
            {
                using (var ctx = new DigiMenuEntities())
                {
                    var pendentes = ctx.Pedido
                        .Where(p => p.StatusId == 1 || p.Status.Nome == "Pendente")
                        .Select(p => new
                        {
                            p.IdPedido,
                            p.Data,
                            p.Total,
                            Cliente = p.Usuario.Nome,
                            Status = p.Status.Nome,
                            Endereco = p.ItemPedido.Select(i => i.Endereco).FirstOrDefault(),
                            Itens = p.ItemPedido.Select(i => new
                            {
                                Produto = i.Produto.Nome,
                                i.Quantidade,
                                i.PrecoUnitario
                            })
                        })
                        .AsEnumerable()
                        .Select(p => new
                        {
                            p.IdPedido,
                            p.Data,
                            p.Total,
                            p.Cliente,
                            p.Status,
                            Cidade = p.Endereco != null ? p.Endereco.Cidade : null,
                            Numero = p.Endereco != null ? p.Endereco.Numero : null,
                            Complemento = p.Endereco != null ? p.Endereco.Complemento : null,
                            Logradouro = p.Endereco != null ? p.Endereco.Logradouro : null,
                            Itens = p.Itens.ToList()
                        })
                        .OrderByDescending(p => p.Data)
                        .ToList();

                    if (pnlSemPedidos != null)
                        pnlSemPedidos.Visible = pendentes.Count == 0;

                    if (rptPedidos != null)
                    {
                        rptPedidos.DataSource = pendentes;
                        rptPedidos.DataBind();

                        int index = 0;
                        foreach (RepeaterItem item in rptPedidos.Items)
                        {
                            var pedido = pendentes[index++];
                            var rptItens = item.FindControl("rptItens") as Repeater;
                            if (rptItens != null)
                            {
                                rptItens.DataSource = pedido.Itens;
                                rptItens.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar pedidos pendentes: " + ex.Message);
            }
        }

        // ADMIN: Carrega pedidos não pendentes (qualquer status != pendente)
        public void AdminCarregarNaoPendentes(Repeater rptPedidosNaoPendentes, CheckBoxList cblPedidosNaoPendentes, Panel pnlSem)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var lista = ctx.Pedido
                    .Where(p => p.StatusId != 1)
                    .Select(p => new
                    {
                        p.IdPedido,
                        p.Data,
                        p.Total,
                        Cliente = p.Usuario.Nome,
                        Status = p.Status.Nome,
                        Endereco = p.ItemPedido.Select(i => i.Endereco).FirstOrDefault()
                    })
                    .AsEnumerable()
                    .Select(p => new
                    {
                        p.IdPedido,
                        p.Data,
                        p.Total,
                        p.Cliente,
                        p.Status,
                        Cidade = p.Endereco != null ? p.Endereco.Cidade : null,
                        Numero = p.Endereco != null ? p.Endereco.Numero : null,
                        Complemento = p.Endereco != null ? p.Endereco.Complemento : null,
                        Logradouro = p.Endereco != null ? p.Endereco.Logradouro : null
                    })
                    .OrderByDescending(p => p.Data)
                    .ToList();

                if (pnlSem != null)
                    pnlSem.Visible = lista.Count == 0;

                if (rptPedidosNaoPendentes != null)
                {
                    rptPedidosNaoPendentes.DataSource = lista;
                    rptPedidosNaoPendentes.DataBind();
                }

                if (cblPedidosNaoPendentes != null)
                {
                    cblPedidosNaoPendentes.Items.Clear();
                    foreach (var p in lista)
                    {
                        string enderecoTxt = string.Join(", ", new[] { p.Cidade, p.Logradouro, p.Numero, p.Complemento }.Where(s => !string.IsNullOrWhiteSpace(s)));
                        string texto = $"#{p.IdPedido} - {p.Cliente} - {p.Data:dd/MM HH:mm} - Total R$ {p.Total:N2} - {p.Status}" + (string.IsNullOrWhiteSpace(enderecoTxt) ? string.Empty : $" - {enderecoTxt}");
                        cblPedidosNaoPendentes.Items.Add(new ListItem(texto, p.IdPedido.ToString()));
                    }
                }
            }
        }

        // ADMIN: Carrega todos os status no dropdown
        public void CarregarStatus(DropDownList ddl)
        {
            if (ddl == null) return;
            using (var ctx = new DigiMenuEntities())
            {
                var lista = ctx.Status
                    .OrderBy(s => s.IdStatus)
                    .Select(s => new { s.IdStatus, s.Nome })
                    .ToList();
                ddl.DataSource = lista;
                ddl.DataTextField = "Nome";
                ddl.DataValueField = "IdStatus";
                ddl.DataBind();
            }
        }

        // ADMIN: Alterar status único
        public void AlterarStatus(int pedidoId, int novoStatusId)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var pedido = ctx.Pedido.FirstOrDefault(p => p.IdPedido == pedidoId);
                if (pedido == null) return;
                pedido.StatusId = novoStatusId;
                ctx.SaveChanges();
            }
        }

        // ADMIN: Alterar status em massa
        public void AlterarStatusEmMassa(IEnumerable<int> pedidosIds, int novoStatusId)
        {
            if (pedidosIds == null) return;
            using (var ctx = new DigiMenuEntities())
            {
                var ids = pedidosIds.ToList();
                var pedidos = ctx.Pedido.Where(p => ids.Contains(p.IdPedido)).ToList();
                foreach (var pedido in pedidos)
                {
                    pedido.StatusId = novoStatusId;
                }
                ctx.SaveChanges();
            }
        }

        internal void Salvar(Pedido pedido)
        {
            using (var ctx = new DigiMenuEntities())
            {
                ctx.Pedido.Add(pedido);
                ctx.SaveChanges();
            }
        }
    }
}
