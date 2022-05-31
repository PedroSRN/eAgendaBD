using eAgenda.Dominio.ModuloTarefa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace eAgenda.WinApp.ModuloTarefa
{
    public partial class TelaCadastroItensTarefaForm : Form
    {
        private readonly Tarefa tarefa;

        public TelaCadastroItensTarefaForm(Tarefa tarefa)
        {
            InitializeComponent();

            this.tarefa = tarefa;

            labelTituloTarefa.Text = tarefa.Titulo;

            foreach (ItemTarefa item in tarefa.Itens)
            {
                listItensTarefa.Items.Add(item);
            }
        }

        public List<ItemTarefa> ItensAdicionados
        {
            get
            {
                return listItensTarefa.Items.Cast<ItemTarefa>().ToList();
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            List<string> titulos = ItensAdicionados.Select(x => x.Titulo).ToList();

            if (titulos.Count == 0 || titulos.Contains(txtTituloItem.Text) == false)
            {
                ItemTarefa itemTarefa = new ItemTarefa();

                itemTarefa.Titulo = txtTituloItem.Text;

                listItensTarefa.Items.Add(itemTarefa);
            }

            tarefa.CalcularPercentualConcluido();
        }

      
    }
}
