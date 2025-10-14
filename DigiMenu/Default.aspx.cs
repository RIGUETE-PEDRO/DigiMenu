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
            }
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
    }
}