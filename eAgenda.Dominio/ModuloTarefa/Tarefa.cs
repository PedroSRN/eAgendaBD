using eAgenda.Dominio.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Dominio.ModuloTarefa
{
    [Serializable]
    public class Tarefa : EntidadeBase<Tarefa>
    {
        private List<ItemTarefa> itens = new List<ItemTarefa>();

        public Tarefa()
        {
            Prioridade = PrioridadeTarefaEnum.Baixa;
            DataCriacao = DateTime.Now;
        }

        public Tarefa(int n, string t) : this()
        {
            Numero = n;
            Titulo = t;
            DataConclusao = null;
        }

        public string Titulo { get; set; }

        public PrioridadeTarefaEnum Prioridade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public List<ItemTarefa> Itens { get { return itens; } }
        public decimal PercentualConcluido { get; set; }
      
        public void CalcularPercentualConcluido()
        {
                if (itens.Count == 0)
                {
                    PercentualConcluido = 0;
                    return;
                }

                int qtdConcluidas = itens.Count(x => x.Concluido);

                var percentualConcluido = (qtdConcluidas / (decimal)itens.Count()) * 100;

                PercentualConcluido = Math.Round(percentualConcluido, 2);
        }

        public override string ToString()
        {
            var percentual = PercentualConcluido;

            if (DataConclusao.HasValue)
            {
                return $"Número: {Numero}, Título: {Titulo}, Percentual: {percentual}, Prioridade: {Prioridade} " +
                    $"Concluída: {DataConclusao.Value.ToShortDateString()}";
            }

            return $"Número: {Numero}, Título: {Titulo}, Percentual: {percentual}, Prioridade: {Prioridade}";
        }

        public bool AdicionarItem(ItemTarefa item)
        {
            if (Itens.Exists(x => x.Equals(item)) == false)
            {
                item.Tarefa = this;
                itens.Add(item);
                DataConclusao = null;
                return true;
            }

            return false;
        }

    

    public void ConcluirItem(ItemTarefa item)
        {
            ItemTarefa itemTarefa = itens.Find(x => x.Equals(item));

            itemTarefa?.Concluir();

            if (PercentualConcluido == 100)
                DataConclusao = DateTime.Now;
        }

        public void MarcarPendente(ItemTarefa item)
        {
            ItemTarefa itemTarefa = itens.Find(x => x.Equals(item));

            itemTarefa?.MarcarPendente();
        }

        public override void Atualizar(Tarefa registro)
        {
            Numero = registro.Numero;
            Titulo = registro.Titulo;
            Prioridade = registro.Prioridade;
        }
    }
}
