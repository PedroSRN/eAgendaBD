using eAgenda.Dominio.ModuloDespesa;
using FluentValidation.Results;
using System;
using System.Windows.Forms;

namespace eAgenda.WinApp.ModuloDespesa
{
    public partial class TelaCadastroDespesasForm : Form
    {
        public TelaCadastroDespesasForm()
        {
            InitializeComponent();

            CarregarFormaPgto();

            CarregarCategorias();
        }

        private void CarregarCategorias()
        {
            var categorias = Enum.GetValues(typeof(CategoriaDespesaEnum));

            foreach (var item in categorias)
            {
                cmbCategoria.Items.Add(item);
            }
        }

        private void CarregarFormaPgto()
        {
            var formas = Enum.GetValues(typeof(FormaPgtoDespesaEnum));

            foreach (var item in formas)
            {
                cmbFormaPgto.Items.Add(item);
            }
        }

        private Despesa despesa;

        public Func<Despesa, ValidationResult> GravarRegistro { get; set; }

        public Despesa Despesa
        {
            get
            {
                return despesa;
            }
            set
            {
                despesa = value;

                txtNumero.Text = despesa.Numero.ToString();
                txtDescricao.Text = despesa.Descricao;
                txtValor.Text = despesa.Valor.ToString();
                txtData.Value = despesa.Data;
                cmbFormaPgto.SelectedItem = despesa.FormaPagamento;
                cmbCategoria.SelectedItem = despesa.Categoria;
            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            despesa.Descricao = txtDescricao.Text;
            despesa.Valor = Convert.ToDecimal(txtValor.Text);
            despesa.Data = txtData.Value;
            despesa.FormaPagamento = (FormaPgtoDespesaEnum)cmbFormaPgto.SelectedItem;
            despesa.Categoria = (CategoriaDespesaEnum)cmbCategoria.SelectedItem;

            ValidationResult resultadoValidacao = GravarRegistro(despesa);

            if (resultadoValidacao.IsValid == false)
            {
                string primeiroErro = resultadoValidacao.Errors[0].ErrorMessage;

                TelaPrincipalForm.Instancia.AtualizarRodape(primeiroErro);

                DialogResult = DialogResult.None;
            }
        }
    }
}
