namespace eAgenda.Infra.Arquivos
{
    public interface ISerializador
    {
        DataContext CarregarDadosDoArquivo();

        void GravarDadosEmArquivo(DataContext dados);
    }
}
