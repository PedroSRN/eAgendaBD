using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infra.BancoDados.ModuloCompromissos
{
    public class RepositorioCompromissoEmBancoDados : IRepositorioCompromisso
    {
        private const string enderecoBanco = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=e-agendaDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        private const string sqlInserir =
            @"INSERT INTO [TB_COMPROMISSO]
                (
                       [ASSUNTO],
	                   [LOCAL],
	                   [LINK],
	                   [DATA],
	                   [HORAINICIO],
	                   [HORATERMINO],
	                   [CONTATO_NUMERO]
                )       
            VALUES
                (
                    @ASSUNTO,
                    @LOCAL,
                    @LINK,
                    @DATA,
                    @HORAINICIO,
                    @HORATERMINO,
                    @CONTATO_NUMERO
                ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @" UPDATE [TB_COMPROMISSO]
                    SET 
                       [ASSUNTO] = @ASSUNTO,
	                   [LOCAL] =  @LOCAL,
	                   [LINK] =  @LINK,
	                   [DATA] =  @DATA,
	                   [HORAINICIO] = @HORAINICIO,
	                   [HORATERMINO] = @HORATERMINO,
	                   [CONTATO_NUMERO] = @CONTATO_NUMERO

                    WHERE [NUMERO] = @NUMERO";

        private const string sqlExcluir =
           @"DELETE FROM [TB_COMPROMISSO] 
                WHERE [NUMERO] = @NUMERO";

        private const string sqlSelecionarTodos =
            @"SELECT
                    CP.[NUMERO],
                    CP.[ASSUNTO],
	                CP.[LOCAL],
	                CP.[LINK],
	                CP.[DATA],
	                CP.[HORAINICIO],
	                CP.[HORATERMINO],
	                CP.[CONTATO_NUMERO],
                    CT.[NOME],
	                CT.[EMAIL],
	                CT.[TELEFONE],
	                CT.[EMPRESA],
	                CT.[CARGO]
                FROM
                     [TB_COMPROMISSO] AS CP LEFT JOIN 
                     [TB_CONTATO] AS CT
                ON
                CT.NUMERO = CP.CONTATO_NUMERO";

        private const string sqlSelecionarPorNumero =
            @"SELECT
                    CP.[NUMERO],
                    CP.[ASSUNTO],
	                CP.[LOCAL],
	                CP.[LINK],
	                CP.[DATA],
	                CP.[HORAINICIO],
	                CP.[HORATERMINO],
	                CP.[CONTATO_NUMERO],
                    CT.[NOME],
	                CT.[EMAIL],
	                CT.[TELEFONE],
	                CT.[EMPRESA],
	                CT.[CARGO]
                FROM
                     [TB_COMPROMISSO] AS CP LEFT JOIN 
                     [TB_CONTATO] AS CT
                ON
                     CT.NUMERO = CP.CONTATO_NUMERO
                WHERE
                     CP.[NUMERO] = @NUMERO";

        private const string sqlSelecionarCompromissosPassados =
            @"SELECT   
                    CP.[NUMERO],
                    CP.[ASSUNTO],
	                CP.[LOCAL],
	                CP.[LINK],
	                CP.[DATA],
	                CP.[HORAINICIO],
	                CP.[HORATERMINO],
	                CP.[CONTATO_NUMERO],
                    CT.[NOME],
	                CT.[EMAIL],
	                CT.[TELEFONE],
	                CT.[EMPRESA],
	                CT.[CARGO]
                FROM
                     [TB_COMPROMISSO] AS CP LEFT JOIN 
                     [TB_CONTATO] AS CT
                ON
                     CT.NUMERO = CP.CONTATO_NUMERO
                WHERE
                    CP.[DATA] <= @DATA";

        private const string sqlSelecionarCompromissosFuturos =
        @"SELECT   
                    CP.[NUMERO],
                    CP.[ASSUNTO],
	                CP.[LOCAL],
	                CP.[LINK],
	                CP.[DATA],
	                CP.[HORAINICIO],
	                CP.[HORATERMINO],
	                CP.[CONTATO_NUMERO],
                    CT.[NOME],
	                CT.[EMAIL],
	                CT.[TELEFONE],
	                CT.[EMPRESA],
	                CT.[CARGO]
                FROM
                     [TB_COMPROMISSO] AS CP LEFT JOIN 
                     [TB_CONTATO] AS CT
                ON
                     CT.NUMERO = CP.CONTATO_NUMERO
                WHERE  
                       CP.[DATA] BETWEEN @DATAINICIAL AND @DATAFINAL";
          

        public ValidationResult Inserir(Compromisso novoRegistro)
        {
            var validador = new ValidadorCompromisso();

            var resultadoValidacao = validador.Validate(novoRegistro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosCompromisso(novoRegistro, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoRegistro.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        private void ConfigurarParametrosCompromisso(Compromisso compromisso, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", compromisso.Numero);
            comando.Parameters.AddWithValue("ASSUNTO", compromisso.Assunto);
            comando.Parameters.AddWithValue("LOCAL", string.IsNullOrEmpty(compromisso.Local) ? DBNull.Value : compromisso.Local);
            comando.Parameters.AddWithValue("LINK", string.IsNullOrEmpty(compromisso.Link) ? DBNull.Value : compromisso.Link);
            comando.Parameters.AddWithValue("DATA", compromisso.Data);
            comando.Parameters.AddWithValue("HORAINICIO", compromisso.HoraInicio.Ticks);
            comando.Parameters.AddWithValue("HORATERMINO", compromisso.HoraTermino.Ticks);

            comando.Parameters.AddWithValue("CONTATO_NUMERO", compromisso.Contato == null ? DBNull.Value : compromisso.Contato.Numero);
        }

        public ValidationResult Editar(Compromisso registro)
        {
            var validador = new ValidadorCompromisso();

            var resultadoValidacao = validador.Validate(registro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosCompromisso(registro, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Compromisso registro)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("NUMERO", registro.Numero);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Compromisso> SelecionarCompromissosFuturos(DateTime dataInicial, DateTime dataFinal)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarCompromissosFuturos, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("DATAINICIAL", dataInicial);
            comandoSelecao.Parameters.AddWithValue("DATAFINAL", dataFinal);

            conexaoComBanco.Open();
            SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();

            List<Compromisso> compromissos = new List<Compromisso>();

            while (leitorCompromisso.Read())
            {
                Compromisso compromisso = ConverterParaCompromisso(leitorCompromisso);

                compromissos.Add(compromisso);
            }

            conexaoComBanco.Close();

            return compromissos;
        }

        public List<Compromisso> SelecionarCompromissosPassados(DateTime dataDeHoje)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarCompromissosPassados, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("DATA", dataDeHoje);

            conexaoComBanco.Open();
            SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();

            List<Compromisso> compromissos = new List<Compromisso>();

            while (leitorCompromisso.Read())
            {
                Compromisso compromisso = ConverterParaCompromisso(leitorCompromisso);

                compromissos.Add(compromisso);
            }

            conexaoComBanco.Close();

            return compromissos;
        }

        public Compromisso SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoComBanco.Open();
            SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();

            Compromisso compromisso = null;
            if (leitorCompromisso.Read())
                compromisso = ConverterParaCompromisso(leitorCompromisso);

            conexaoComBanco.Close();

            return compromisso;
        }

        public List<Compromisso> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();

            List<Compromisso> compromissos = new List<Compromisso>();

            while (leitorCompromisso.Read())
            {
                Compromisso compromisso = ConverterParaCompromisso(leitorCompromisso);

                compromissos.Add(compromisso);
            }

            conexaoComBanco.Close();

            return compromissos;
        }

        private Compromisso ConverterParaCompromisso(SqlDataReader leitorCompromisso)
        {
            var numero = Convert.ToInt32(leitorCompromisso["NUMERO"]);
            var assunto = Convert.ToString(leitorCompromisso["ASSUNTO"]);

            string local = "", link = "";
            TipoLocalizacaoCompromissoEnum tipoLocalizacao = TipoLocalizacaoCompromissoEnum.Presencial;

            if (leitorCompromisso["LOCAL"] != DBNull.Value)
                local = Convert.ToString(leitorCompromisso["LOCAL"]);

            if (leitorCompromisso["LINK"] != DBNull.Value)
            {
                link = Convert.ToString(leitorCompromisso["LINK"]);
                tipoLocalizacao = TipoLocalizacaoCompromissoEnum.Remoto;
            }

           var data = Convert.ToDateTime(leitorCompromisso["DATA"]);

            var horaInicio = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HORAINICIO"]));
            var horaTermino = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HORATERMINO"]));

            Compromisso compromisso = new Compromisso();
            compromisso.Numero = numero;
            compromisso.Assunto = assunto;
            compromisso.Local = local;
            compromisso.Link = link;
            compromisso.Data = data;
            compromisso.HoraInicio = horaInicio;
            compromisso.HoraTermino = horaTermino;
            compromisso.TipoLocal = tipoLocalizacao;

            if (leitorCompromisso["CONTATO_NUMERO"] != DBNull.Value)
            {
                var numeroContato = Convert.ToInt32(leitorCompromisso["CONTATO_NUMERO"]);
                var email = Convert.ToString(leitorCompromisso["EMAIL"]);
                var nome = Convert.ToString(leitorCompromisso["NOME"]);
                var telefone = Convert.ToString(leitorCompromisso["TELEFONE"]);
                var empresa = Convert.ToString(leitorCompromisso["EMPRESA"]);
                var cargo = Convert.ToString(leitorCompromisso["CARGO"]);

                compromisso.Contato = new Contato
                {
                    Numero = numeroContato,
                    Nome = nome,
                    Telefone = telefone,
                    Email = email,
                    Cargo = cargo,
                    Empresa = empresa
                };
            }

            return compromisso;
        }

    }
}
