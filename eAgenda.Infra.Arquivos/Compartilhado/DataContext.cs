using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloTarefa;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Infra.Arquivos
{
    [Serializable]
    public class DataContext //Container
    {
        private readonly ISerializador serializador;

        public DataContext()
        {
            Tarefas = new List<Tarefa>();

            Contatos = new List<Contato>();

            Compromissos = new List<Compromisso>();

            Despesas = new List<Despesa>();
        }

        public DataContext(ISerializador serializador) : this()
        {
            this.serializador = serializador;

            CarregarDados();
        }

        public List<Tarefa> Tarefas { get; set; }

        public List<Contato> Contatos { get; set; }

        public List<Compromisso> Compromissos { get; set; }

        public List<Despesa> Despesas { get; set; }

        public void GravarDados()
        {
            serializador.GravarDadosEmArquivo(this);
        }

        private void CarregarDados()
        {
            var ctx = serializador.CarregarDadosDoArquivo();

            if (ctx.Tarefas.Any())
                this.Tarefas.AddRange(ctx.Tarefas);

            if (ctx.Contatos.Any())
                this.Contatos.AddRange(ctx.Contatos);

            if (ctx.Compromissos.Any())
                this.Compromissos.AddRange(ctx.Compromissos);

            if (ctx.Despesas.Any())
                this.Despesas.AddRange(ctx.Despesas);
        }
    }
}
