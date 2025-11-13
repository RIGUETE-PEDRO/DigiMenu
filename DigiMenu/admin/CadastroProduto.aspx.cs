using DigiMenu.DAL;
using DigiMenu.DAO; // importar DAO
using System;
using System.Globalization; // adicionado para controle de cultura
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls; // Adicione esta linha no topo do arquivo

namespace DigiMenu
{
    public partial class CadastroProduto : System.Web.UI.Page
    {
        private ProdutoDAO produtoDAO = new ProdutoDAO();
        private static readonly CultureInfo PtBr = new CultureInfo("pt-BR");
        private ImagemProdutoDAO ImagemProdutoDAO = new ImagemProdutoDAO();
        private Mensagens mensagem = new Mensagens();
        private CategoriaDAO categoriaDAO = new CategoriaDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarProdutos();
                carregarCategorias();

                btnVoltar.Visible = false;
                btnCadastrar.Visible = true;
                btnAtualizar.Visible = false;

                bool modoVisualizacao = Request.QueryString["view"] == "1";
                PreencherFormulario(modoVisualizacao);

                if (modoVisualizacao)
                {
                    PlaceHolderMensagens.Controls.Clear();
                }
            }

            if (Session["UsuarioLogado"] == null)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            // Verifica se é administrador (tipo 2)
            if (Session["TipoUsuario"] == null || Convert.ToInt32(Session["TipoUsuario"]) != 2)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack && Request.QueryString["salvo"] == "1")
            {


                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Produto cadastrado com sucesso!", "sucesso");
                PlaceHolderMensagens.Controls.Add(divSucesso);
            }
        }

        private void carregarCategorias()
        {
            var categorias = categoriaDAO.ListarOrdenado();
            ddlCategoria.Items.Clear();
            ddlCategoria.DataSource = categorias;
            ddlCategoria.DataTextField = "nome";
            ddlCategoria.DataValueField = "id";
            ddlCategoria.DataBind();
            ddlCategoria.Items.Insert(0, new ListItem("Selecione", ""));
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

            PlaceHolderMensagens.Controls.Clear();


            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                    string.IsNullOrWhiteSpace(txtPreco.Text) || string.IsNullOrWhiteSpace(txtEstoque.Text) || string.IsNullOrWhiteSpace(ddlCategoria.SelectedValue))
            {

                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Preencha todos os campos obrigatórios.", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);
                return;
            }

            if (!TentarObterPreco(out decimal preco))
            {

                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Preço inválido.", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);
                return;
            }

            if (!int.TryParse(txtEstoque.Text, out int estoque) || estoque < 0)
            {

                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Estoque inválido.", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);
                return;
            }

            bool ofertar = Checkbox1.Checked;

            try
            {
                // Upload e validação da imagem
                HttpPostedFile uploaded = File1?.PostedFile;
                if (uploaded == null || uploaded.ContentLength == 0)
                {
                    PlaceHolderMensagens.Controls.Clear();
                    var divSucesso = mensagem.MostrarMensagem("Imagem do produto é obrigatória.", "erro");
                    PlaceHolderMensagens.Controls.Add(divSucesso);

                    return;
                }

                string originalName = Path.GetFileName(uploaded.FileName);
                string ext = Path.GetExtension(originalName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
                {
                    PlaceHolderMensagens.Controls.Clear();
                    var divSucesso = mensagem.MostrarMensagem("Formato de imagem inválido (use jpg, png, gif, webp).", "erro");
                    PlaceHolderMensagens.Controls.Add(divSucesso);

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
                    Categoria = int.Parse(ddlCategoria.SelectedValue)
                };

                var imagemProduto = new ImagemProduto
                {
                    CaminhoImagem = caminhoRelativo,
                };

                produtoDAO.Salvar(produto, imagemProduto);


                LogDAO log = new LogDAO();
                int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                log.Registrar(usuarioId, 3); // 3 = Cadastro Produto



                // Post/Redirect/Get para evitar reenvio ao atualizar (F5)
                Response.Redirect("Cadastro-de-produto?salvo=1", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch
            {
                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Erro ao cadastrar produto:", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);

            }
            LimparCampos();
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
            ddlCategoria.ClearSelection();
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
                    PlaceHolderMensagens.Controls.Clear();
                    var divSucesso = mensagem.MostrarMensagem("Produto excluído com sucesso.", "sucesso");
                    PlaceHolderMensagens.Controls.Add(divSucesso);

                }
                catch
                {
                    PlaceHolderMensagens.Controls.Clear();
                    var divSucesso = mensagem.MostrarMensagem("Erro ao excluir produto:", "erro");
                    PlaceHolderMensagens.Controls.Add(divSucesso);

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
                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("ID inválido.", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);


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
                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Produto excluído com sucesso.", "sucesso");
                PlaceHolderMensagens.Controls.Add(divSucesso);
                LogDAO log = new LogDAO();
                int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                log.Registrar(usuarioId, 5);
            }
            catch
            {

                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Erro ao excluir:", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);


            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            var btn = (ImageButton)sender;
            int idProduto;
            if (int.TryParse(btn.CommandArgument, out idProduto))
            {
                Response.Redirect("~/Cadastro-de-produto?cod=" + idProduto);
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
                var imagemProduto = ImagemProdutoDAO.BuscarImagemPorProdutoId(idProduto);
                if (produto != null)
                {
                    txtNome.Text = produto.Nome;
                    txtDescricao.Text = produto.Descricao;
                    txtPreco.Text = produto.Preco.ToString("N2", PtBr);
                    txtEstoque.Text = produto.Estoque.ToString();
                    Checkbox1.Checked = produto.Ativo;
                    // categoria
                    ddlCategoria.SelectedValue = produto.Categoria.ToString();

                    if (imagemProduto != null)
                    {
                        insereIMG(imagemProduto);
                    }

                    btnCadastrar.Visible = false;
                    btnAtualizar.Visible = true;
                    btnAtualizar.Text = "Atualizar";
                    btnVoltar.Visible = true;
                    btnVoltar.InnerText = "Voltar cadastro";

                    if (visualizar == true)
                    {
                        txtNome.Enabled = false;
                        txtDescricao.Enabled = false;
                        txtPreco.Enabled = false;
                        txtEstoque.Enabled = false;
                        Checkbox1.Disabled = true;
                        ddlCategoria.Enabled = false;
                        File1.Enabled = false;
                        File1.Visible = false;
                        btnAtualizar.Visible = false;
                    }
                }
            }
        }

        private void insereIMG(ImagemProduto produto)
        {
            if (produto != null && !string.IsNullOrEmpty(produto.CaminhoImagem))
            {
                imgPreview.ImageUrl = "~/" + produto.CaminhoImagem;
                imgPreview.Visible = true;
            }
            else
            {
                imgPreview.Visible = false;
            }
        }

        protected void btnVisualizar_Click(object sender, ImageClickEventArgs e)
        {
            var btn = (ImageButton)sender;
            if (int.TryParse(btn.CommandArgument, out int idProduto))
            {
                Response.Redirect($"~/Cadastro-de-produto?cod={idProduto}&view=1", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void Atualizar_Click(object sender, EventArgs e)
        {
            string idProdutoStr = Request.QueryString["cod"];
            Produto produto;
            ImagemProduto imagemProduto = null;

            if (!string.IsNullOrEmpty(idProdutoStr) && int.TryParse(idProdutoStr, out int idProduto))
            {
                produto = produtoDAO.BuscarPorId(idProduto);
                if (produto == null)
                {
                    PlaceHolderMensagens.Controls.Clear();
                    var divSucesso = mensagem.MostrarMensagem("Produto não encontrado.", "erro");
                    PlaceHolderMensagens.Controls.Add(divSucesso);


                    return;
                }

                imagemProduto = ImagemProdutoDAO.BuscarImagemPorProdutoId(idProduto);
            }
            else
            {
                produto = new Produto();
            }

            if (!TentarObterPreco(out decimal preco))
            {
                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Preço inválido.", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);

                return;
            }

            if (!int.TryParse(txtEstoque.Text, out int estoque) || estoque < 0)
            {
                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Estoque inválido.", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);

                return;
            }

            produto.Nome = txtNome.Text.Trim();
            produto.Descricao = txtDescricao.Text.Trim();
            produto.Preco = preco;
            produto.Estoque = estoque;
            produto.Ativo = Checkbox1.Checked;
            produto.Categoria = int.Parse(ddlCategoria.SelectedValue);

            try
            {
                HttpPostedFile uploaded = File1?.PostedFile;
                if (uploaded != null && uploaded.ContentLength > 0)
                {
                    string originalName = Path.GetFileName(uploaded.FileName);
                    string ext = Path.GetExtension(originalName).ToLowerInvariant();
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
                    {

                        PlaceHolderMensagens.Controls.Clear();
                        var divSucesso = mensagem.MostrarMensagem("Formato de imagem inválido (use jpg, png, gif, webp).", "erro");
                        PlaceHolderMensagens.Controls.Add(divSucesso);
                        return;
                    }

                    string oldImage = imagemProduto != null ? imagemProduto.CaminhoImagem : null;
                    string newFileName = Guid.NewGuid().ToString("N") + ext;
                    string folderPhysical = Server.MapPath("~/imgProduto");
                    if (!Directory.Exists(folderPhysical)) Directory.CreateDirectory(folderPhysical);
                    string newPhysicalPath = Path.Combine(folderPhysical, newFileName);
                    uploaded.SaveAs(newPhysicalPath);
                    string newRelativePath = "imgProduto/" + newFileName;

                    if (imagemProduto == null)
                    {
                        imagemProduto = new ImagemProduto
                        {
                            ProdutoId = produto.IdProduto,
                            CaminhoImagem = newRelativePath
                        };
                    }
                    else
                    {
                        imagemProduto.CaminhoImagem = newRelativePath;
                    }

                    if (!string.IsNullOrEmpty(oldImage) && !oldImage.Equals("imgProduto/sem-imagem.png", StringComparison.OrdinalIgnoreCase))
                    {
                        string oldPhysical = Server.MapPath("~/" + oldImage.TrimStart('/'));
                        if (File.Exists(oldPhysical))
                        {
                            try { File.Delete(oldPhysical); } catch { }
                        }
                    }
                }
            }
            catch
            {


                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Erro ao processar imagem:", "erro");
                PlaceHolderMensagens.Controls.Add(divSucesso);
                return;
            }

            if (!string.IsNullOrEmpty(idProdutoStr))
            {
                produtoDAO.Atualizar(produto, imagemProduto);
                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Produto atualizado com sucesso!", "sucesso");
                PlaceHolderMensagens.Controls.Add(divSucesso);

                LogDAO log = new LogDAO();
                int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                log.Registrar(usuarioId, 4);
            }
            else
            {
                produtoDAO.Salvar(produto, imagemProduto);
                PlaceHolderMensagens.Controls.Clear();
                var divSucesso = mensagem.MostrarMensagem("Produto cadastrado com sucesso!", "sucesso");
                PlaceHolderMensagens.Controls.Add(divSucesso);
            }

            LimparCampos();
            CarregarProdutos();
            Response.Redirect("Cadastro-de-produto");
        }
    }
}