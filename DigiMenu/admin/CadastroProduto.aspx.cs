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
        private static readonly CultureInfo PtBr = new CultureInfo("pt-BR");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarProdutos();

                btnVoltar.Visible = false;
                btnCadastrar.Visible = true;
                btnAtualizar.Visible = false;

                bool modoVisualizacao = Request.QueryString["view"] == "1";
                PreencherFormulario(modoVisualizacao);

                if (modoVisualizacao)
                {

                    lblMensagem.Text = "Visualizando produto.";
                    lblMensagem.Visible = true;
                }
            }

            if (!IsPostBack && Request.QueryString["salvo"] == "1")
            {
                lblMensagem.Text = "Produto cadastrado com sucesso!";
                lblMensagem.Visible = true;
            }
        }

        private bool TentarObterPreco(out decimal preco)
        {
            preco = 0m;
            var texto = txtPreco.Text.Trim();
            if (string.IsNullOrEmpty(texto)) return false;

            // Normalizações: remove R$, espaços
            texto = texto.Replace("R$", "").Trim();


            // Troca ponto de milhar e garante vírgula decimal
            // Se tiver vírgula e ponto: remove pontos
            if (texto.Contains(",") && texto.Contains("."))
            {
                texto = texto.Replace(".", "");
            }
            // Se não tiver vírgula mas tiver ponto -> ponto vira vírgula
            else if (!texto.Contains(",") && texto.Contains("."))
            {
                texto = texto.Replace('.', ',');
            }

            return decimal.TryParse(texto, NumberStyles.Number, PtBr, out preco) && preco >= 0;
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

            if (!TentarObterPreco(out decimal preco))
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

        // Limpa os campos do formulário
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

        //função para excluir produto
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
            PreencherFormulario(false);
        }

        private void PreencherFormulario(bool visualizar)
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
                    // Exibe formatado pt-BR no campo
                    txtPreco.Text = produto.Preco.ToString("N2", PtBr);
                    txtEstoque.Text = produto.Estoque.ToString();
                    Checkbox1.Checked = produto.Ativo;
                    insereIMG(produto);



                    // Ajusta interface para modo edição
                    btnCadastrar.Visible = false;
                    btnAtualizar.Visible = true;
                    btnAtualizar.Text = "Atualizar";
                    btnVoltar.Visible = true;
                    btnVoltar.InnerText = "Voltar cadastro";

                    if (visualizar == true) {
                        
                        txtNome.Enabled = false;
                        txtDescricao.Enabled = false;
                        txtPreco.Enabled = false;
                        txtEstoque.Enabled = false;
                        Checkbox1.Disabled = true;
                        File1.Enabled = false; // Desabilita upload de imagem
                        File1.Visible = false;  // Oculta controle de upload
                        btnAtualizar.Visible = false; // Oculta botão atualizar

                    }
                }
            }
        }


        private void insereIMG(Produto produto) {

            if (!string.IsNullOrEmpty(produto.imagem))
            {
                imgPreview.ImageUrl = "~/" + produto.imagem; // Caminho relativo da imagem
                imgPreview.Visible = true;                   // Mostra o controle
            }
            else
            {
                imgPreview.Visible = false;                  // Oculta se não tiver imagem
            }
        }

        protected void btnVisualizar_Click(object sender, ImageClickEventArgs e)
        {
            var btn = (ImageButton)sender;
            if (int.TryParse(btn.CommandArgument, out int idProduto))
            {
                // Redireciona com parâmetro de visualização
                Response.Redirect($"~/admin/CadastroProduto.aspx?cod={idProduto}&view=1", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void Atualizar_Click(object sender, EventArgs e)
        {
            string idProdutoStr = Request.QueryString["cod"];
            Produto produto;

            if (!string.IsNullOrEmpty(idProdutoStr) && int.TryParse(idProdutoStr, out int idProduto))
            {
                // Atualizar
                produto = produtoDAO.BuscarPorId(idProduto);
                if (produto == null)
                {
                    ExibirMensagem("Produto não encontrado.", true);
                    return;
                }
            }
            else
            {
                // Novo produto (fallback)
                produto = new Produto();
            }

            if (!TentarObterPreco(out decimal preco))
            {
                ExibirMensagem("Preço inválido.", true);
                return;
            }

            if (!int.TryParse(txtEstoque.Text, out int estoque) || estoque < 0)
            {
                ExibirMensagem("Estoque inválido.", true);
                return;
            }

            produto.Nome = txtNome.Text.Trim();
            produto.Descricao = txtDescricao.Text.Trim();
            produto.Preco = preco;
            produto.Estoque = estoque;
            produto.Ativo = Checkbox1.Checked;

            // --- LÓGICA ATUALIZAÇÃO IMAGEM (mantém se não houver upload) ---
            try
            {
                HttpPostedFile uploaded = File1?.PostedFile;
                if (uploaded != null && uploaded.ContentLength > 0)
                {
                    string originalName = Path.GetFileName(uploaded.FileName);
                    string ext = Path.GetExtension(originalName).ToLowerInvariant();
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
                    {
                        ExibirMensagem("Formato de imagem inválido (use jpg, png, gif, webp).", true);
                        return;
                    }

                    string oldImage = produto.imagem; // guardar antiga
                    string newFileName = Guid.NewGuid().ToString("N") + ext;
                    string folderPhysical = Server.MapPath("~/imgProduto");
                    if (!Directory.Exists(folderPhysical)) Directory.CreateDirectory(folderPhysical);
                    string newPhysicalPath = Path.Combine(folderPhysical, newFileName);
                    uploaded.SaveAs(newPhysicalPath);
                    string newRelativePath = "imgProduto/" + newFileName;

                    produto.imagem = newRelativePath; // define nova

                    if (!string.IsNullOrEmpty(oldImage) && !oldImage.Equals("imgProduto/sem-imagem.png", StringComparison.OrdinalIgnoreCase))
                    {
                        string oldPhysical = Server.MapPath("~/" + oldImage.TrimStart('/'));
                        if (File.Exists(oldPhysical))
                        {
                            try { File.Delete(oldPhysical); } catch { }
                        }
                    }
                }
                // Se não houve upload, não mexe em produto.imagem (mantém a existente por ser entidade rastreada)
            }
            catch (Exception exUpload)
            {
                ExibirMensagem("Erro ao processar imagem: " + exUpload.Message, true);
                return;
            }
            // --- FIM LÓGICA IMAGEM ---

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
            LimparCampos();
            CarregarProdutos();
            Response.Redirect("CadastroProduto.aspx");
        }
    }
}
