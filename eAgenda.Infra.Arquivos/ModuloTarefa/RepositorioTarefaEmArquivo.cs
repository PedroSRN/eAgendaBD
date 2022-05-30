using eAgenda.Dominio.ModuloTarefa;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Infra.Arquivos.ModuloTarefa
{
    public class RepositorioTarefaEmArquivo : RepositorioEmArquivoBase<Tarefa>, IRepositorioTarefa
    {
        public RepositorioTarefaEmArquivo(DataContext dataContext) : base(dataContext)
        {
            if (dataContext.Tarefas.Count > 0)
                contador = dataContext.Tarefas.Max(x => x.Numero);
        }

        public override ValidationResult Inserir(Tarefa novoRegistro)
        {
            var resultadoValidacao = Validar(novoRegistro);

            if (resultadoValidacao.IsValid)
            {
                novoRegistro.Numero = ++contador;

                var registros = ObterRegistros();

                registros.Add(novoRegistro);
            }

            return resultadoValidacao;
        }

        public override ValidationResult Editar(Tarefa registro)
        {
            var resultadoValidacao = Validar(registro);

            if (resultadoValidacao.IsValid)
            {
                var registros = ObterRegistros();

                foreach (var item in registros)
                {
                    if (item.Numero == registro.Numero)
                    {
                        item.Atualizar(registro);
                        break;
                    }
                }
            }

            return resultadoValidacao;
        }

        private ValidationResult Validar(Tarefa registro)
        {
            var validator = ObterValidador();

            var resultadoValidacao = validator.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            var nomeEncontrado = ObterRegistros()
               .Select(x => x.Titulo)
               .Contains(registro.Titulo);

            if (nomeEncontrado && registro.Numero == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Nome já está cadastrado"));

            return resultadoValidacao;
        }

        public override List<Tarefa> ObterRegistros()
        {
            return dataContext.Tarefas;
        }

        public void AdicionarItens(Tarefa tarefaSelecionada, List<ItemTarefa> itens)
        {
            foreach (var item in itens)
            {
                tarefaSelecionada.AdicionarItem(item);
            }
        }

        public void AtualizarItens(Tarefa tarefaSelecionada,
            List<ItemTarefa> itensConcluidos, List<ItemTarefa> itensPendentes)
        {
            foreach (var item in itensConcluidos)
            {
                tarefaSelecionada.ConcluirItem(item);
            }

            foreach (var item in itensPendentes)
            {
                tarefaSelecionada.MarcarPendente(item);
            }

        }

        public List<Tarefa> SelecionarTodos(StatusTarefaEnum status)
        {
            switch (status)
            {
                case StatusTarefaEnum.Todos: return SelecionarTodos();

                case StatusTarefaEnum.Pendentes: return SelecionarTarefasPendentes();

                case StatusTarefaEnum.Concluidas: return SelecionarTarefasConcluidas();

                default: return SelecionarTodos();
            }
        }

        public override List<Tarefa> SelecionarTodos()
        {
            return base.SelecionarTodos()
                .OrderByDescending(x => x.Prioridade)
                .ToList();
        }

        private List<Tarefa> SelecionarTarefasConcluidas()
        {
            return dataContext.Tarefas
                .Where(x => x.PercentualConcluido == 100)
                .OrderByDescending(x => x.Prioridade)
                .ToList();
        }

        private List<Tarefa> SelecionarTarefasPendentes()
        {
            return dataContext.Tarefas
                .Where(x => x.PercentualConcluido < 100)
                .OrderByDescending(x => x.Prioridade)
                .ToList();
        }

        public override AbstractValidator<Tarefa> ObterValidador()
        {
            return new ValidadorTarefa();
        }
    }
}
