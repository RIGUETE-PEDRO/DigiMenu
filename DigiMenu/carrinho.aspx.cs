using DigiMenu.DAL;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu
{
    public partial class carrinho : System.Web.UI.Page
    {
        ItemCarrinhoDAO itemCarrinhoDAO;
        public void Page_Load(object sender, EventArgs e)
        {
            if (itemCarrinhoDAO == null)
                itemCarrinhoDAO = new ItemCarrinhoDAO(control: this);

            if (!IsPostBack)
            {
                itemCarrinhoDAO.CarregarCarrinho();
                int addId;
                if (int.TryParse(Request.QueryString["add"], out addId))
                {
                    itemCarrinhoDAO.AdicionarProduto(addId);
                }

                var rpt = FindControl("rptCarrinho") as Repeater;
                if (rpt != null)
                {
                    rpt.ItemDataBound += rptCarrinho_ItemDataBound;
                }
                itemCarrinhoDAO.CarregarCarrinho();
            }
        }

        public void rptCarrinho_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var txtQtd = e.Item.FindControl("txtQuantidade") as TextBox;
            if (txtQtd != null)
            {
                var dataItem = e.Item.DataItem;
                var propEstoque = dataItem.GetType().GetProperty("Estoque");
                int estoque = propEstoque != null ? (int)propEstoque.GetValue(dataItem, null) : int.MaxValue;
                txtQtd.Attributes["type"] = "number";
                txtQtd.Attributes["min"] = "1";
                txtQtd.Attributes["max"] = estoque.ToString();
            }
        }

        public void rptCarrinho_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int usuarioId = itemCarrinhoDAO.ObterUsuarioId(Session["UsuarioId"]);
            if (usuarioId == 0)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            int idItemCarrinho;
            if (!int.TryParse(e.CommandArgument.ToString(), out idItemCarrinho)) return;

            int? novaQuantidade = null;
            if (e.CommandName == "Atualizar")
            {
                var txtQtd = e.Item.FindControl("txtQuantidade") as TextBox;
                int qtd;
                if (txtQtd != null && int.TryParse(txtQtd.Text, out qtd) && qtd > 0)
                {
                    novaQuantidade = qtd;
                }
            }

            var sucesso = itemCarrinhoDAO.ProcessarItemCarrinho(idItemCarrinho, usuarioId, e.CommandName, novaQuantidade);
            itemCarrinhoDAO.CarregarCarrinho();
        }

        protected void btnContinuarComprando_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        public void btnFinalizar_Click(object sender, EventArgs e)
        {
            int usuarioId = itemCarrinhoDAO.ObterUsuarioId(Session["UsuarioId"]);
            if (usuarioId == 0)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }
            ItemCarrinhoDAO itemCarrinho = new ItemCarrinhoDAO(control: this);

            Endereco endereco = new Endereco
            {
                Cidade = cidadeEntrega.Value,
                Logradouro = logadouroEntrega.Value,
                Numero = numeroEntrega.Value,
                Complemento = complementoEntrega.Value
            };

            if (string.IsNullOrWhiteSpace(endereco.Cidade) || string.IsNullOrWhiteSpace(endereco.Logradouro))
            {
                return;
            }

            EnderecoDAO enderecoDAO = new EnderecoDAO(control: this);
            int idEndereco = enderecoDAO.SalvarEndereco(usuarioId, endereco);
            if (idEndereco <= 0)
            {
                return;
            }

            int idPedido = itemCarrinho.FinalizarCarrinho(usuarioId, idEndereco);
            if (idPedido > 0)
            {
                Response.Redirect("StatusPedido.aspx");
            }
        }

        protected void PedirEndereco_Click(object sender, EventArgs e)
        {
            endereco.Style["display"] = "block";
        }
    }
}