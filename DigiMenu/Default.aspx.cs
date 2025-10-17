using DigiMenu.DAL;
using System;
using System.Linq;
using System.Web.UI;

namespace DigiMenu
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var nomeCompleto = Session["UsuarioNome"] as string;
                if (!string.IsNullOrEmpty(nomeCompleto))
                {
                    string primeiroNome = nomeCompleto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    if (divLogin != null) divLogin.Visible = false;
                    if (divUser != null)
                    {
                        divUser.Visible = true;
                        if (lblUserName != null) lblUserName.InnerText = "Olá, " + primeiroNome + "!";
                    }
                }
                else
                {
                    if (divLogin != null) divLogin.Visible = true;
                    if (divUser != null) divUser.Visible = false;
                }
                CarregarProdutosAtivos();
                iniciarFiltro();
            }
        }

        private void iniciarFiltro()
        {
            ddlCategoria.Visible = rbCategoria.Checked;
            txtPreco.Visible = false;
            ddlOferta.Visible = false;
        }

        private void CarregarProdutosAtivos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                var produtosAtivos = ctx.Produto
                                        .Where(p => p.Ativo)
                                        .Select(p => new
                                        {
                                            p.Nome,
                                            p.Descricao,
                                            p.Preco,
                                            p.Estoque,
                                            Imagem = p.ImagemProduto.FirstOrDefault().CaminhoImagem // se tiver imagens
                                        })
                                        .ToList();

                rptProdutos.DataSource = produtosAtivos;
                rptProdutos.DataBind();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            //pesquisa produto
            string termoPesquisa = txtPesquisa.Text.Trim();

            using (var ctx = new DigiMenuEntities())
            {
                var resultados = ctx.Produto
                                    .Where(p => p.Ativo && (p.Nome.Contains(termoPesquisa) || p.Descricao.Contains(termoPesquisa)))
                                    .Select(p => new
                                    {
                                        p.Nome,
                                        p.Descricao,
                                        p.Preco,
                                        p.Estoque,
                                        Imagem = p.ImagemProduto.FirstOrDefault().CaminhoImagem // se tiver imagens
                                    })
                                    .ToList();
                rptProdutos.DataSource = resultados;
                rptProdutos.DataBind();
            }
        }

        protected void Filtro_CheckedChanged(object sender, EventArgs e)
        {
            // Aqui você vai mostrar/ocultar os inputs conforme o radio selecionado
            ddlCategoria.Visible = rbCategoria.Checked;
            txtPreco.Visible = rbPreco.Checked;
            ddlOferta.Visible = rbOferta.Checked;
        }


        protected void compra_Click(object sender, EventArgs e)
        {

            if (Session["UsuarioId"] == null)
            {
                // Redirecionar para a página de login se o usuário não estiver logado
                Response.Redirect("FrmLogin.aspx");
                return;
            }
            else
            {

                // Lógica para iniciar um novo pedido
                Pedido pedido = new Pedido
                {
                    Data = DateTime.Now,
                    Total = 0, // Calcular o total com base nos itens do pedido
                    UsuarioId = (int)(Session["UsuarioId"] ?? 0), // Certifique-se de que o ID do usuário está na sessão
                    StatusId = 1 // Status "Pendente" ou equivalente
                };


                LogDAO log = new LogDAO();
                int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                log.Registrar(usuarioId, 6); // 6 = adicionar um produto no carrinho

                PedidoDAO pedidoDAO = new PedidoDAO();
                pedidoDAO.Salvar(pedido);
            }
        }

        protected void btnAplicarFiltro_Click(object sender, EventArgs e)
        {

        }
    }
}