using DigiMenu.DAL;
using DigiMenu.DAO; // importar DAO
using System;
using System.Globalization; // adicionado para controle de cultura
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; // Adicione esta linha no topo do arquivo

namespace DigiMenu
{
    public partial class CadastroProduto : System.Web.UI.Page
    {
        private ProdutoDAO produtoDAO = new ProdutoDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarProdutos();

                // Configuração padrão (modo cadastro)
                btnVoltar.Visible = false;
                btnCadastrar.Visible = true;
                btnAtualizar.Visible = false;

                // Verifica se é edição e ajusta interface
                PreencherFormularioSeEditar();
            }

            if (!IsPostBack && Request.QueryString["salvo"] == "1")
            {
                lblMensagem.Text = "Produto cadastrado com sucesso!";
                lblMensagem.Visible = true;
            }
        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            // Se por algum motivo estiver em modo edição, redireciona para fluxo de atualização
            if (!string.IsNullOrEmpty(Request.QueryString["cod"]))
            {
                Atualizar_Click(sender, e);
                return;
            }

            lblMensagem.Visible = false;
            lblMensagem.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                    string.IsNullOrWhiteSpace(txtPreco.Text) || string.IsNullOrWhiteSpace(txtEstoque.Text))
            {
                ExibirMensagem("Preencha todos os campos obrigatórios.", true);
                return;
            }

            // Normalização e parse seguro do preço em pt-BR
            string precoTexto = txtPreco.Text.Trim();
            precoTexto = precoTexto.Replace("R$", "").Trim();
            if (precoTexto.Contains(",") && precoTexto.Contains("."))
            {
                // Ex: 1.234,56 -> remover pontos (milhar)
                precoTexto = precoTexto.Replace(".", "");
            }
            else if (!precoTexto.Contains(",") && precoTexto.Contains("."))
            {
                // Ex: 5.25 -> 5,25
                precoTexto = precoTexto.Replace('.', ',');
            }

            decimal preco;
            if (!decimal.TryParse(precoTexto, NumberStyles.Number, new CultureInfo("pt-BR"), out preco) || preco < 0)
            {
                ExibirMensagem("Preço inválido.", true);
                return;
            }

            if (!int.TryParse(txtEstoque.Text, out int estoque) || estoque < 0)
            {
                ExibirMensagem("Estoque inválido.", true);
                return;
            }

            bool ofertar = Checkbox1.Checked;

            try
            {
                // Upload e validação da imagem
                HttpPostedFile uploaded = File1?.PostedFile;
                if (uploaded == null || uploaded.ContentLength == 0)
                {
                    ExibirMensagem("Imagem do produto é obrigatória.", true);
                    return;
                }

                string originalName = Path.GetFileName(uploaded.FileName);
                string ext = Path.GetExtension(originalName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
                {
                    ExibirMensagem("Formato de imagem inválido (use jpg, png, gif, webp).", true);
                    return;
                }

                string newFileName = Guid.NewGuid().ToString("N") + ext;
                string folderPhysical = Server.MapPath("~/imgProduto");
                if (!Directory.Exists(folderPhysical)) Directory.CreateDirectory(folderPhysical);
                uploaded.SaveAs(Path.Combine(folderPhysical, newFileName));
                string caminhoRelativo = "imgProduto/" + newFileName;

                var produto = new Produto
                {
                    Nome = txtNome.Text.Trim(),
                    Descricao = txtDescricao.Text.Trim(),
                    Preco = preco,
                    Estoque = estoque,
                    Ativo = ofertar,
                    imagem = caminhoRelativo
                };

                produtoDAO.Salvar(produto);

                // Post/Redirect/Get para evitar reenvio ao atualizar (F5)
                Response.Redirect("CadastroProduto.aspx?salvo=1", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao cadastrar produto: " + ex.Message;
                lblMensagem.Visible = true;
            }
            LimparCampos();
        }
        // Exibe mensagem no label de mensagem 
        private void ExibirMensagem(string mensagem, bool visivel)
        {
            lblMensagem.Text = mensagem; lblMensagem.Visible = visivel;
        }

        // Carrega produtos na tabela
        private void CarregarProdutos()
        {
            var produtos = produtoDAO.Listar();
            rptProdutos.DataSource = produtos;
            rptProdutos.DataBind();
        }

        private void LimparCampos()
        {
            txtNome.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtPreco.Text = string.Empty;
            txtEstoque.Text = string.Empty;
            Checkbox1.Checked = false;
        }

        // Novo handler para o Repeater
        protected void rptProdutos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                //id do produto a ser excluído
                int idProduto = Convert.ToInt32(e.CommandArgument);
                try
                {
                    // Delete retorna caminho relativo da imagem
                    string caminhoRelativo = produtoDAO.Delete(idProduto);

                    if (!string.IsNullOrEmpty(caminhoRelativo))
                    {
                        // Garantir que não haja barras iniciais duplicadas
                        string caminhoFisico = Server.MapPath("~/'".Replace("'", "") + caminhoRelativo.TrimStart('/'));
                        // Ajustar: caminhoRelativo esperado: imgProduto/arquivo.ext
                        if (!File.Exists(caminhoFisico))
                        {
                            // Tentar montagem alternativa (caso acima falhe)
                            caminhoFisico = Server.MapPath("~/" + caminhoRelativo.TrimStart('/'));
                        }
                        //se existir arquivo ele deleta
                        if (File.Exists(caminhoFisico))
                        {
                            File.Delete(caminhoFisico);
                        }
                    }

                    CarregarProdutos();
                    ExibirMensagem("Produto excluído com sucesso.", true);
                }
                catch (Exception ex)
                {
                    ExibirMensagem("Erro ao excluir: " + ex.Message, true);
                }
            }
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            var btn = (ImageButton)sender;
            ExcluirProduto(btn.CommandArgument);
        }

        private void ExcluirProduto(string commandArgument)
        {
            int idProduto;
            if (!int.TryParse(commandArgument, out idProduto))
            {
                ExibirMensagem("ID inválido.", true);
                return;
            }

            try
            {
                string caminhoRelativo = produtoDAO.Delete(idProduto);
                if (!string.IsNullOrWhiteSpace(caminhoRelativo))
                {
                    string caminhoFisico = Server.MapPath("~/" + caminhoRelativo.TrimStart('/'));
                    if (File.Exists(caminhoFisico))
                    {
                        File.Delete(caminhoFisico);
                    }
                }
                CarregarProdutos();
                ExibirMensagem("Produto excluído com sucesso.", true);
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro ao excluir: " + ex.Message, true);
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            var btn = (ImageButton)sender;
            int idProduto;
            if (int.TryParse(btn.CommandArgument, out idProduto))
            {
                Response.Redirect("~/admin/CadastroProduto.aspx?cod=" + idProduto);
            }
        }

        private void PreencherFormularioSeEditar()
        {
            string idProdutoStr = Request.QueryString["cod"];
            int idProduto;
            if (!string.IsNullOrEmpty(idProdutoStr) && int.TryParse(idProdutoStr, out idProduto))
            {
                var produto = produtoDAO.BuscarPorId(idProduto);
                if (produto != null)
                {
                    // Preenche campos
                    txtNome.Text = produto.Nome;
                    txtDescricao.Text = produto.Descricao;
                    txtPreco.Text = produto.Preco.ToString(CultureInfo.InvariantCulture); // ex: 1234.56
                    txtEstoque.Text = produto.Estoque.ToString();
                    Checkbox1.Checked = produto.Ativo;

                    // Ajusta interface para modo edição
                    btnCadastrar.Visible = false;
                    btnAtualizar.Visible = true;
                    btnAtualizar.Text = "Atualizar";
                    btnVoltar.Visible = true;
                    btnVoltar.InnerText = "Voltar cadastro";
                }
            }
        }


        protected void btnVisualizar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

        }

        protected void Atualizar_Click(object sender, EventArgs e)
        {
            string idProdutoStr = Request.QueryString["cod"];
            Produto produto;

            if (!string.IsNullOrEmpty(idProdutoStr) && int.TryParse(idProdutoStr, out int idProduto))
            {
                // Atualizar
                produto = produtoDAO.BuscarPorId(idProduto);
                if (produto == null) return;
            }
            else
            {
                // Novo produto (fallback)
                produto = new Produto();
            }

            produto.Nome = txtNome.Text;
            produto.Descricao = txtDescricao.Text;
            produto.Preco = decimal.Parse(txtPreco.Text, CultureInfo.InvariantCulture);
            produto.Estoque = int.Parse(txtEstoque.Text);
            produto.Ativo = Checkbox1.Checked;

            if (!string.IsNullOrEmpty(idProdutoStr))
            {
                produtoDAO.Atualizar(produto);
                lblMensagem.Text = "Produto atualizado com sucesso!";
            }
            else
            {
                produtoDAO.Salvar(produto);
                lblMensagem.Text = "Produto cadastrado com sucesso!";
            }

            lblMensagem.Visible = true;

            // Limpar formulário e recarregar tabela
            LimparCampos();
            CarregarProdutos();

            // Volta para modo cadastro (remove querystring)
            Response.Redirect("CadastroProduto.aspx");
        }
    }
}
