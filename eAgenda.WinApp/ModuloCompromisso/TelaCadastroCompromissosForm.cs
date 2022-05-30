using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace eAgenda.WinApp.ModuloCompromisso
{
    public partial class TelaCadastroCompromissosForm : Form
    {
        private Compromisso compromisso;

        public TelaCadastroCompromissosForm(List<Contato> contatos)
        {
            InitializeComponent();

            CarregarContatos(contatos);
        }


        private void CarregarContatos(List<Contato> contatos)
        {
            cmbContatos.Items.Clear();

            foreach (var item in contatos)
            {
                cmbContatos.Items.Add(item);
            }
        }

        public Func<Compromisso, ValidationResult> GravarRegistro { get; set; }

        public Compromisso Compromisso
        {
            get { return compromisso; }
            set
            {
                compromisso = value;

                txtNumero.Text = compromisso.Numero.ToString();

                txtAssunto.Text = compromisso.Assunto;

                if (compromisso.TipoLocal == TipoLocalizacaoCompromissoEnum.Remoto)
                {
                    txtLink.Text = compromisso.Link;
                    rdbRemoto_CheckedChanged(null,null);
                }
                else
                {
                    txtLocal.Text = compromisso.Local;
                    rdbPresencial_CheckedChanged(null, null);
                }

                txtData.Value = compromisso.Data;

                txtHoraInicio.Value = compromisso.Data + compromisso.HoraInicio;

                txtHoraTermino.Value = compromisso.Data + compromisso.HoraTermino;

                cmbContatos.Enabled = compromisso.Contato != null;

                checkMarcarContato.Checked = compromisso.Contato != null;

                cmbContatos.SelectedItem = compromisso.Contato;
            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            compromisso.Assunto = txtAssunto.Text;                        
            compromisso.Data = txtData.Value;
            compromisso.HoraInicio = txtHoraInicio.Value.TimeOfDay;
            compromisso.HoraTermino = txtHoraTermino.Value.TimeOfDay;
            compromisso.Contato = (Contato)cmbContatos.SelectedItem;

            if (rdbRemoto.Checked)
            {
                compromisso.TipoLocal = TipoLocalizacaoCompromissoEnum.Remoto;
                compromisso.Link = txtLink.Text;
            }
            else
            {
                compromisso.TipoLocal = TipoLocalizacaoCompromissoEnum.Presencial;
                compromisso.Local = txtLocal.Text;
            }

            var resultadoValidacao = GravarRegistro(compromisso);

            if (resultadoValidacao.IsValid == false)
            {
                string erro = resultadoValidacao.Errors[0].ErrorMessage;

                TelaPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }
        }

        private void TelaCadastroCompromissosForm_Load(object sender, EventArgs e)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void TelaCadastroCompromisssosForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void checkMarcarContato_CheckedChanged(object sender, EventArgs e)
        {
            cmbContatos.Enabled = checkMarcarContato.Checked;
            cmbContatos.SelectedIndex = -1;
        }

        private void rdbRemoto_CheckedChanged(object sender, EventArgs e)
        {
            txtLocal.Text = "";
            txtLocal.Enabled = false;
            txtLink.Enabled = true;
        }

        private void rdbPresencial_CheckedChanged(object sender, EventArgs e)
        {
            txtLink.Text = "";
            txtLink.Enabled = false;
            txtLocal.Enabled = true;
        }
    }
}
