using System.IO;
using System.Xml.Serialization;

namespace eAgenda.Infra.Arquivos
{
    public class SerializadorDadosEmXml : ISerializador
    {
        private const string arquivo = @"C:\temp\dados.xml";

        public DataContext CarregarDadosDoArquivo()
        {
            if (File.Exists(arquivo) == false)
                return new DataContext();

            XmlSerializer serializador = new XmlSerializer(typeof(DataContext));

            byte[] bytes = File.ReadAllBytes(arquivo);

            MemoryStream ms = new MemoryStream(bytes);

            return (DataContext)serializador.Deserialize(ms);
        }


        public void GravarDadosEmArquivo(DataContext dados)
        {
            XmlSerializer serializador = new XmlSerializer(typeof(DataContext));

            MemoryStream ms = new MemoryStream();

            serializador.Serialize(ms, dados);

            byte[] bytes = ms.ToArray();

            File.WriteAllBytes(arquivo, bytes);
        }
    }
}
