using eAgenda.Dominio.Compartilhado;
using System.Collections.Generic;

namespace eAgenda.Dominio.ModuloTarefa
{
    public interface IRepositorioTarefa : IRepositorio<Tarefa>
    {
        void AdicionarItens(Tarefa tarefaSelecionada, List<ItemTarefa> itens);

        void AtualizarItens(Tarefa tarefaSelecionada, List<ItemTarefa> itensConcluidos, List<ItemTarefa> itensPendentes);

        List<Tarefa> SelecionarTodos(StatusTarefaEnum status);

    }
}