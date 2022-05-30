using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace eAgenda.Infra.Arquivos
{
    public class SerializadorDadosEmBinario : ISerializador
    {
        private const string arquivo = @"C:\temp\dados.bin";

        public DataContext CarregarDadosDoArquivo()
        {
            if (File.Exists(arquivo) == false)
                return new DataContext();

            BinaryFormatter serializador = new BinaryFormatter();

            byte[] bytesTarefas = File.ReadAllBytes(arquivo);

            MemoryStream ms = new MemoryStream(bytesTarefas);

            return (DataContext)serializador.Deserialize(ms);
        }

        public void GravarDadosEmArquivo(DataContext dados)
        {
            BinaryFormatter serializador = new BinaryFormatter();

            MemoryStream ms = new MemoryStream();

            serializador.Serialize(ms, dados);

            byte[] bytes = ms.ToArray();

            File.WriteAllBytes(arquivo, bytes);
        }
    }
}
