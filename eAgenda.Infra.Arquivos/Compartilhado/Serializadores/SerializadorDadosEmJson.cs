using System.IO;
using System.Text.Json;

namespace eAgenda.Infra.Arquivos
{
    internal class SerializadorDadosEmJson : ISerializador
    {
        private const string arquivo = @"C:\temp\dados.json";

        public DataContext CarregarDadosDoArquivo()
        {
            if (File.Exists(arquivo) == false)
                return new DataContext();

            string json = File.ReadAllText(arquivo);

            return JsonSerializer.Deserialize<DataContext>(json);
        }

        public void GravarDadosEmArquivo(DataContext dados)
        {
            var config = new JsonSerializerOptions { WriteIndented = true };

            string json = JsonSerializer.Serialize(dados, config);

            File.WriteAllText(arquivo, json);
        }
    }
}
