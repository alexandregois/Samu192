using System;
using System.Diagnostics;
using static SAMU192InterfaceService.Utils.Enums;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCSolicitarAtendimentoV1
    {
        enum Colunas
        {
            Versao = 0,
            Telefone1 = 1,
            Telefone2 = 2,
            Nome = 3,
            Sexo = 4,
            DataNascimento = 5,
            OutraPessoa = 6,
            UF = 7,
            Cidade = 8,
            Bairro = 9,
            Logradouro = 10,
            Numero = 11,
            Complemento = 12,
            Referencia = 13,
            Latitude = 14,
            Longitude = 15,
            Queixa = 16,
            LatitudeApp = 17,
            LongitudeApp = 18,
            Sistema = 19,
            Identificador = 20,
            FCMRegistration = 21,
            SolicitacaoPorConversa = 22
        }

        const int NUM_COLUNAS = 23;
        public const string VERSAO = "DCSolicitarAtendimentoV1";

        public DCSolicitarAtendimentoV1()
        {

        }

        public DCSolicitarAtendimentoV1(string[] dados)
        {
            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Solicitação de atendimento: Quantidade incorreta de informações!");

            if (dados[(int)Colunas.Versao] != VERSAO)
                throw new ApplicationException("Solicitação de atendimento: Versão incorreta de informações!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.Identificador]))
                throw new ApplicationException("Solicitação de atendimento: Identificador do dispositivo não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.FCMRegistration]))
                throw new ApplicationException("Solicitação de atendimento: Registro FCM não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.SolicitacaoPorConversa]))
                throw new ApplicationException("Solicitação de atendimento: Solicitação por conversa não informado!");

            Telefone1 = dados[(int)Colunas.Telefone1];
            Telefone2 = dados[(int)Colunas.Telefone2];
            Nome = dados[(int)Colunas.Nome];
            Sexo = dados[(int)Colunas.Sexo];
            DataNascimentoStr = dados[(int)Colunas.DataNascimento];
            OutraPessoaStr = dados[(int)Colunas.OutraPessoa];
            UF = dados[(int)Colunas.UF];
            Cidade = dados[(int)Colunas.Cidade];
            Bairro = dados[(int)Colunas.Bairro];
            Logradouro = dados[(int)Colunas.Logradouro];
            Numero = dados[(int)Colunas.Numero];
            Complemento = dados[(int)Colunas.Complemento];
            Referencia = dados[(int)Colunas.Referencia];
            LatitudeStr = dados[(int)Colunas.Latitude];
            LongitudeStr = dados[(int)Colunas.Longitude];
            Queixa = dados[(int)Colunas.Queixa];
            LatitudeAppStr = dados[(int)Colunas.LatitudeApp];
            LongitudeAppStr = dados[(int)Colunas.LongitudeApp];
            SistemaStr = dados[(int)Colunas.Sistema];
            Identificador = dados[(int)Colunas.Identificador];
            FCMRegistration = dados[(int)Colunas.FCMRegistration];
            SolicitacaoPorConversaStr = dados[(int)Colunas.SolicitacaoPorConversa];
        }

        public string Identificador { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string DataNascimentoStr
        {
            get
            {
                if (DataNascimento.HasValue)
                {
                    return DataNascimento.Value.ToString("dd/MM/yyyy");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None,
                           out DateTime aux))
                    {
                        DataNascimento = aux;
                    }
                    else
                        throw new ApplicationException("DataNascimento deve ser especificada no formato dd/MM/yyyy.");
                    //DataNascimento = null;
                }
                else
                    DataNascimento = null;
            }
        }
        public string FCMRegistration { get; set; }
        public double? Latitude { get; set; }
        public double? LatitudeApp { get; set; }
        public string LatitudeAppStr
        {
            get
            {
                if (LatitudeApp.HasValue)
                {
                    return LatitudeApp.Value.ToString().Replace(".", ",");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string semVirgula = value.Replace(",", ".");
                    if (Double.TryParse(semVirgula,
                            System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign,
                           System.Globalization.CultureInfo.InvariantCulture,
                           out Double aux))
                    {
                        LatitudeApp = aux;
                    }
                    else
                        throw new ApplicationException("LatitudeApp não possui um valor válido.");
                    //LatitudeApp = null;
                }
                else
                    LatitudeApp = null;
            }
        }
        public string LatitudeStr
        {
            get
            {
                if (Latitude.HasValue)
                {
                    return Latitude.Value.ToString().Replace(".", ",");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string semVirgula = value.Replace(",", ".");
                    if (Double.TryParse(semVirgula,
                            System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign,
                           System.Globalization.CultureInfo.InvariantCulture,
                           out Double aux))
                    {
                        Latitude = aux;
                    }
                    else
                        throw new ApplicationException("Latitude não possui um valor válido.");
                    //Latitude = null;
                }
                else
                    Latitude = null;
            }
        }
        public string Logradouro { get; set; }
        public double? Longitude { get; set; }
        public double? LongitudeApp { get; set; }
        public string LongitudeAppStr
        {
            get
            {
                if (LongitudeApp.HasValue)
                {
                    return LongitudeApp.Value.ToString().Replace(".", ",");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string semVirgula = value.Replace(",", ".");
                    if (Double.TryParse(semVirgula,
                            System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign,
                           System.Globalization.CultureInfo.InvariantCulture,
                           out Double aux))
                    {
                        LongitudeApp = aux;
                    }
                    else
                        throw new ApplicationException("LongitudeApp não possui um valor válido.");
                    //LongitudeApp = null;
                }
                else
                    LongitudeApp = null;
            }
        }
        public string LongitudeStr
        {
            get
            {
                if (Longitude.HasValue)
                {
                    return Longitude.Value.ToString().Replace(".", ",");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string semVirgula = value.Replace(",", ".");
                    if (Double.TryParse(semVirgula,
                            System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign,
                           System.Globalization.CultureInfo.InvariantCulture,
                           out Double aux))
                    {
                        Longitude = aux;
                    }
                    else
                        throw new ApplicationException("Longitude não possui um valor válido.");
                    //Longitude = null;
                }
                else
                    Longitude = null;
            }
        }
        public string Nome { get; set; }
        public string Numero { get; set; }
        public bool? OutraPessoa { get; set; }
        public string OutraPessoaStr
        {
            get
            {
                if (OutraPessoa.HasValue)
                {
                    return OutraPessoa.Value ? "1" : "0";
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {

                    if (value == "1")
                        OutraPessoa = true;
                    else
                        if (value == "0")
                        OutraPessoa = false;
                    else
                        OutraPessoa = null;
                }
                else
                    OutraPessoa = null;
            }
        }
        public string Queixa { get; set; }
        public string Referencia { get; set; }
        public string Sexo { get; set; }
        public SistemaOperacional Sistema { get; set; }
        public string SistemaStr
        {
            get
            {
                return ((int)Sistema).ToString();
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        Sistema = (SistemaOperacional)int.Parse(value);
                    }
                    catch
                    {
                        Sistema = SistemaOperacional.Indefinido;
                    }
                }
                else
                    Sistema = SistemaOperacional.Indefinido;
            }
        }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string UF { get; set; }
        public bool? SolicitacaoPorConversa { get; set; }
        public string SolicitacaoPorConversaStr
        {
            get
            {
                if (SolicitacaoPorConversa.HasValue)
                {
                    return SolicitacaoPorConversa.Value ? "1" : "0";
                }
                else
                    return string.Empty;
            }
            set
            {
                SolicitacaoPorConversa = null;

                if (int.TryParse(value, out int auxInt) && (auxInt == 0 || auxInt == 1))
                {
                    SolicitacaoPorConversa = (auxInt == 1);
                }
                else
                    throw new ApplicationException("SolicitacaoPorConversa deve ser 0 ou 1.");
            }
        }
        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Identificador] = Identificador;
                resp[(int)Colunas.Bairro] = Bairro;
                resp[(int)Colunas.Cidade] = Cidade;
                resp[(int)Colunas.Complemento] = Complemento;
                resp[(int)Colunas.DataNascimento] = DataNascimentoStr;
                resp[(int)Colunas.FCMRegistration] = FCMRegistration;
                resp[(int)Colunas.Latitude] = LatitudeStr;
                resp[(int)Colunas.LatitudeApp] = LatitudeAppStr;
                resp[(int)Colunas.Logradouro] = Logradouro;
                resp[(int)Colunas.Longitude] = LongitudeStr;
                resp[(int)Colunas.LongitudeApp] = LongitudeAppStr;
                resp[(int)Colunas.Nome] = Nome;
                resp[(int)Colunas.Numero] = Numero;
                resp[(int)Colunas.OutraPessoa] = OutraPessoaStr;
                resp[(int)Colunas.Queixa] = Queixa;
                resp[(int)Colunas.Referencia] = Referencia;
                resp[(int)Colunas.Sexo] = Sexo;
                resp[(int)Colunas.Sistema] = SistemaStr;
                resp[(int)Colunas.Telefone1] = Telefone1;
                resp[(int)Colunas.Telefone2] = Telefone2;
                resp[(int)Colunas.UF] = UF;
                resp[(int)Colunas.Versao] = VERSAO;
                resp[(int)Colunas.SolicitacaoPorConversa] = SolicitacaoPorConversaStr;

                return resp;
            }
        }
    }
}