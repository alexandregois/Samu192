using System;
using static SAMU192InterfaceService.Utils.Enums;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCSolicitarAtendimentoChatV1
    {

        enum Colunas
        {
            versao = 0,
            telefone1 = 1,
            telefone2 = 2,
            nome = 3,
            sexo = 4,
            dataNascimento = 5,
            outraPessoa = 6,
            uf = 7,
            cidade = 8,
            bairro = 9,
            logradouro = 10,
            numero = 11,
            complemento = 12,
            referencia = 13,
            latitude = 14,
            longitude = 15,
            queixa = 16,
            latitudeApp = 17,
            longitudeApp = 18,
            sistema = 19,
            identificador = 20,
            FCMRegistration = 21
        }

        const int NUM_COLUNAS = 22;
        public const string VERSAO = "DCSolicitarAtendimentoChatV1";

        public DCSolicitarAtendimentoChatV1()
        {

        }

        public DCSolicitarAtendimentoChatV1(string[] dados)
        {

            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Solicitação de atendimento por chat: Quantidade incorreta de informações!");

            if (dados[(int)Colunas.versao] != VERSAO)
                throw new ApplicationException("Solicitação de atendimento por chat: Versão incorreta de informações!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.identificador]))
                throw new ApplicationException("Solicitação de atendimento por chat: Identificador do dispositivo não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.FCMRegistration]))
                throw new ApplicationException("Solicitação de atendimento por chat: Registro FCM não informado!");

            Identificador = dados[(int)Colunas.identificador];
            Bairro = dados[(int)Colunas.bairro];
            Cidade = dados[(int)Colunas.cidade];
            Complemento = dados[(int)Colunas.complemento];
            DataNascimentoStr = dados[(int)Colunas.dataNascimento];
            FCMRegistration = dados[(int)Colunas.FCMRegistration];
            LatitudeStr = dados[(int)Colunas.latitude];
            LatitudeAppStr = dados[(int)Colunas.latitudeApp];
            Logradouro = dados[(int)Colunas.logradouro];
            LongitudeStr = dados[(int)Colunas.longitude];
            LongitudeAppStr = dados[(int)Colunas.longitudeApp];
            Nome = dados[(int)Colunas.nome];
            Numero = dados[(int)Colunas.numero];
            OutraPessoaStr = dados[(int)Colunas.outraPessoa];
            Queixa = dados[(int)Colunas.queixa];
            Referencia = dados[(int)Colunas.referencia];
            Sexo = dados[(int)Colunas.sexo];
            SistemaStr = dados[(int)Colunas.sistema];
            Telefone1 = dados[(int)Colunas.telefone1];
            Telefone2 = dados[(int)Colunas.telefone2];
            UF = dados[(int)Colunas.uf];

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
        public Double? Latitude { get; set; }
        public Double? LatitudeApp { get; set; }
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
        public Double? Longitude { get; set; }
        public Double? LongitudeApp { get; set; }
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
        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.identificador] = Identificador;
                resp[(int)Colunas.bairro] = Bairro;
                resp[(int)Colunas.cidade] = Cidade;
                resp[(int)Colunas.complemento] = Complemento;
                resp[(int)Colunas.dataNascimento] = DataNascimentoStr;
                resp[(int)Colunas.FCMRegistration] = FCMRegistration;
                resp[(int)Colunas.latitude] = LatitudeStr;
                resp[(int)Colunas.latitudeApp] = LatitudeAppStr;
                resp[(int)Colunas.logradouro] = Logradouro;
                resp[(int)Colunas.longitude] = LongitudeStr;
                resp[(int)Colunas.longitudeApp] = LongitudeAppStr;
                resp[(int)Colunas.nome] = Nome;
                resp[(int)Colunas.numero] = Numero;
                resp[(int)Colunas.outraPessoa] = OutraPessoaStr;
                resp[(int)Colunas.queixa] = Queixa;
                resp[(int)Colunas.referencia] = Referencia;
                resp[(int)Colunas.sexo] = Sexo;
                resp[(int)Colunas.sistema] = SistemaStr;
                resp[(int)Colunas.telefone1] = Telefone1;
                resp[(int)Colunas.telefone2] = Telefone2;
                resp[(int)Colunas.uf] = UF;
                resp[(int)Colunas.versao] = VERSAO;

                return resp;
            }
        }

    }
}