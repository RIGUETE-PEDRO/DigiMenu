using DigiMenu.DAL;
using DigiMenu.DAO; // importar DAO
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Globalization; // adicionado para controle de cultura

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
            }

            if (!IsPostBack && Request.QueryString["salvo"] == "1")
            {
                lblMensagem.Text = "Produto cadastrado com sucesso!";
                lblMensagem.Visible = true;
            }
        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
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
        }

        private void ExibirMensagem(string mensagem, bool visivel)
        {
            lblMensagem.Text = mensagem; lblMensagem.Visible = visivel;
        }

        private void CarregarProdutos()
        {
            var produtos = produtoDAO.Listar();

            string html = "";
            foreach (var p in produtos)
            {
                string status = p.Ativo
                    ? "<span class='badge bg-success'>Ativo</span>"
                    : "<span class='badge bg-secondary'>Inativo</span>";

                html += $@"
                <tr>
                    <th scope='row'>{p.IdProduto}</th>
                    <td>{p.Nome}</td>
                    <td>R$ {p.Preco:F2}</td>
                    <td>{status}</td>
                    <td>{p.Estoque}</td>
                    <td></td>
                </tr>";
            }

            tblProdutos.InnerHtml = html;
        }
        private void LimparCampos()
        {
            txtNome.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtPreco.Text = string.Empty;
            txtEstoque.Text = string.Empty;
            Checkbox1.Checked = false;
        }

    }
}
