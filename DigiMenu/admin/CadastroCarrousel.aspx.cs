using DigiMenu.DAL;
using DigiMenu.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace DigiMenu.admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private const string TokenSalvamentoChave = "Carousel_SaveToken";

        public class ProdutoCarrouselDTO
        {
            public int IdProduto { get; set; }
            public bool Ativo { get; set; }
            public int Ordem { get; set; }
        }

        private List<ProdutoCarrouselDTO> ObterProdutosDoRepeater()
        {
            var lista = new List<ProdutoCarrouselDTO>();
            var ids = ViewState["ProdutoIds"] as List<int> ?? new List<int>();
            int index = 0;

            foreach (RepeaterItem item in rptProdutos.Items)
            {
                if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) continue;

                var hfId = item.FindControl("hfId") as HiddenField; // opcional no ASPX
                var chkAtivo = item.FindControl("chkAtivo") as CheckBox;
                var txtOrdem = item.FindControl("txtOrdem") as TextBox;

                // Resolve IdProduto: HiddenField (se existir) ou ViewState por índice
                int idProduto = 0;
                if (hfId != null && int.TryParse(hfId.Value, out var parsedId))
                {
                    idProduto = parsedId;
                }
                else if (index < ids.Count)
                {
                    idProduto = ids[index];
                }

                index++; // avança posição para próximos itens

                if (idProduto <= 0 || chkAtivo == null || txtOrdem == null) continue;

                bool ativo = chkAtivo.Checked;
                int ordem = 0;
                if (ativo)
                {
                    // Quando ativo, tenta ler ordem (> 0). Se inválida, mantém 0; validação é feita antes de salvar.
                    int.TryParse(txtOrdem.Text, out ordem);
                }

                lista.Add(new ProdutoCarrouselDTO
                {
                    IdProduto = idProduto,
                    Ativo = ativo,
                    Ordem = ordem
                });
            }

            return lista;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rptProdutos.ItemCreated += rptProdutos_ItemCreated;
            rptProdutos.ItemDataBound += rptProdutos_ItemDataBound;

            if (!IsPostBack)
            {
                CarregarProdutos();
            }
            else
            {
                AplicarEstadoPosPostback();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ViewState[TokenSalvamentoChave] = Guid.NewGuid().ToString("N");
        }

        private void CarregarProdutos()
        {
            var produtoDAO = new ProdutoDAO();
            var produtosAtivos = produtoDAO.BuscarAtivos();
            var dados = produtoDAO.BuscarDadosCarrossel(produtosAtivos);

            ViewState["ProdutoIds"] = dados.Select(d => d.IdProduto).ToList();
            rptProdutos.DataSource = dados;
            rptProdutos.DataBind();
        }

        private void AplicarEstadoPosPostback()
        {
            foreach (RepeaterItem item in rptProdutos.Items)
            {
                if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) continue;
                var chk = item.FindControl("chkAtivo") as CheckBox;
                var txt = item.FindControl("txtOrdem") as TextBox;
                if (chk != null)
                {
                    chk.AutoPostBack = true;
                }
                if (txt != null)
                {
                    txt.Attributes["type"] = "number";
                    txt.Attributes["min"] = "1";
                }
                if (txt != null && chk != null)
                {
                    txt.Enabled = chk.Checked;
                    if (!chk.Checked)
                    {
                        txt.Text = string.Empty;
                    }
                }
            }
        }

        private void rptProdutos_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var chk = e.Item.FindControl("chkAtivo") as CheckBox;
            if (chk != null)
            {
                chk.AutoPostBack = true;
            }
        }

        private void rptProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var chk = e.Item.FindControl("chkAtivo") as CheckBox;
            var txt = e.Item.FindControl("txtOrdem") as TextBox;
            if (txt != null)
            {
                txt.Attributes["type"] = "number";
                txt.Attributes["min"] = "1";
                txt.BorderColor = Color.Empty;
                txt.ToolTip = string.Empty;
            }
            if (txt != null && chk != null)
            {
                txt.Enabled = chk.Checked;
                if (!chk.Checked)
                {
                    txt.Text = string.Empty;
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            var listaProdutos = ObterProdutosDoRepeater();
            var ordens = new HashSet<int>();
            bool temDuplicado = false;
            var mensagem = new Mensagens();

            foreach (RepeaterItem item in rptProdutos.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var chkAtivo = (CheckBox)item.FindControl("chkAtivo");
                    var txtOrdem = (TextBox)item.FindControl("txtOrdem");

                    if (chkAtivo != null && txtOrdem != null && chkAtivo.Checked)
                    {
                        if (int.TryParse(txtOrdem.Text, out int ordem))
                        {
                            if (!ordens.Add(ordem))
                            {
                                temDuplicado = true;
                                break;
                            }
                        }
                        else
                        {
                            temDuplicado = true;
                            break;
                        }
                    }
                }
            }

            if (temDuplicado)
            {
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("Existem produtos ativos com ordens duplicadas ou sem preencher os campos ativos.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }

            var carouselDAO = new CarouselDAO();
            carouselDAO.AtualizarCarrousel(listaProdutos);

            PlaceHolderMensagens.Controls.Clear();
            var divSucesso = mensagem.MostrarMensagem("Carrousel atualizado com sucesso!", "sucesso");
            PlaceHolderMensagens.Controls.Add(divSucesso);
        }
    }
}