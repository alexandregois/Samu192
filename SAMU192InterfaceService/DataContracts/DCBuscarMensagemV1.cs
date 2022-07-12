using System;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCBuscarMensagemV1
    {
        enum Colunas
        {
            Versao = 0,
            FCMRegistration = 1,
            NMaxReg = 2,
            OrderByAsc = 3,
            Sentido = 4,
            HorarioRegistro = 5,
            HorarioRegistroI = 6,
            HorarioRegistroF = 7,
            HorarioLidaNula = 8,
            HorarioLida = 9,
            HorarioLidaI = 10,
            HorarioLidaF = 11,
            Acao = 12,
            AcaoI = 13,
            AcaoF = 14,
            Erros = 15,
            ErrosI = 16,
            ErrosF = 17
        }

        const int NUM_COLUNAS = 18;
        public const string VERSAO = "DCBuscarMensagemV1";

        public DCBuscarMensagemV1()
        {

        }

        public DCBuscarMensagemV1(string[] dados)
        {
            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Busca de Mensagem: Quantidade incorreta de linhas!");

            if (dados[(int)Colunas.Versao] != VERSAO)
                throw new ApplicationException("Busca de Mensagem: Versão incorreta de dados!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.FCMRegistration]))
                throw new ApplicationException("Busca de Mensagem: FCM não informado!");

            FCMRegistration = dados[(int)Colunas.FCMRegistration];
            NMaxRegStr = dados[(int)Colunas.NMaxReg];
            OrderByAscStr = dados[(int)Colunas.OrderByAsc];
            SentidoStr = dados[(int)Colunas.Sentido];
            HorarioRegistroStr = dados[(int)Colunas.HorarioRegistro];
            HorarioRegistroIStr = dados[(int)Colunas.HorarioRegistroI];
            HorarioRegistroFStr = dados[(int)Colunas.HorarioRegistroF];
            HorarioLidaNulaStr = dados[(int)Colunas.HorarioLidaNula];
            HorarioLidaStr = dados[(int)Colunas.HorarioLida];
            HorarioLidaIStr = dados[(int)Colunas.HorarioLidaI];
            HorarioLidaFStr = dados[(int)Colunas.HorarioLidaF];
            AcaoStr = dados[(int)Colunas.Acao];
            AcaoIStr = dados[(int)Colunas.AcaoI];
            AcaoFStr = dados[(int)Colunas.AcaoF];
            ErrosStr = dados[(int)Colunas.Erros];
            ErrosIStr = dados[(int)Colunas.ErrosI];
            ErrosFStr = dados[(int)Colunas.ErrosF];
        }

        public string FCMRegistration { get; set; }

        public int? NMaxReg { get; set; }
        public string NMaxRegStr
        {
            get
            {
                if (NMaxReg.HasValue)
                {
                    return NMaxReg.Value.ToString();
                }
                else
                    return "0";
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        NMaxReg = aux;
                    }
                    else
                        throw new ApplicationException("NMaxReg deve ser um número inteiro.");
                }
                else
                    NMaxReg = 0;
            }
        }

        public bool? OrderByAsc { get; set; }
        public string OrderByAscStr
        {
            get
            {
                if (OrderByAsc.HasValue)
                {
                    return OrderByAsc.Value ? "1" : "0";
                }
                else
                    return "1";
            }
            set
            {
                OrderByAsc = true;

                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    OrderByAsc = (value == "1");
                }
            }
        }

        public int? Sentido { get; set; }
        public string SentidoStr
        {
            get
            {
                if (Sentido.HasValue)
                {
                    return Sentido.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        Sentido = aux;
                    }
                    else
                        throw new ApplicationException("Sentido deve ser um número inteiro.");
                }
                else
                    Sentido = null;
            }
        }

        public DateTime? HorarioRegistro { get; set; }
        public string HorarioRegistroStr
        {
            get
            {
                if (HorarioRegistro.HasValue)
                {
                    return HorarioRegistro.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime aux))
                    {
                        HorarioRegistro = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Registro deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioRegistro = null;
            }
        }

        public DateTime? HorarioRegistroI { get; set; }
        public string HorarioRegistroIStr
        {
            get
            {
                if (HorarioRegistroI.HasValue)
                {
                    return HorarioRegistroI.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime aux))
                    {
                        HorarioRegistroI = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Registro Inicial deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioRegistroI = null;
            }
        }

        public DateTime? HorarioRegistroF { get; set; }
        public string HorarioRegistroFStr
        {
            get
            {
                if (HorarioRegistroF.HasValue)
                {
                    return HorarioRegistroF.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime aux))
                    {
                        HorarioRegistroF = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Registro Final deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioRegistroF = null;
            }
        }

        public bool? HorarioLidaNula { get; set; }
        public string HorarioLidaNulaStr
        {
            get
            {
                if (HorarioLidaNula.HasValue)
                {
                    return HorarioLidaNula.Value ? "1" : "0";
                }
                else
                    return string.Empty;
            }
            set
            {
                HorarioLidaNula = null;

                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    HorarioLidaNula = (value == "1");
                }
            }
        }

        public DateTime? HorarioLida { get; set; }
        public string HorarioLidaStr
        {
            get
            {
                if (HorarioLida.HasValue)
                {
                    return HorarioLida.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime aux))
                    {
                        HorarioLida = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Lida deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioLida = null;
            }
        }

        public DateTime? HorarioLidaI { get; set; }
        public string HorarioLidaIStr
        {
            get
            {
                if (HorarioLidaI.HasValue)
                {
                    return HorarioLidaI.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime aux))
                    {
                        HorarioLidaI = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Lida Inicial deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioLidaI = null;
            }
        }

        public DateTime? HorarioLidaF { get; set; }
        public string HorarioLidaFStr
        {
            get
            {
                if (HorarioLidaF.HasValue)
                {
                    return HorarioLidaF.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime aux))
                    {
                        HorarioLidaF = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Final deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioLidaF = null;
            }
        }

        public int? Acao { get; set; }
        public string AcaoStr
        {
            get
            {
                if (Acao.HasValue)
                {
                    return Acao.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        Acao = aux;
                    }
                    else
                        throw new ApplicationException("Ação deve ser um número inteiro.");
                }
                else
                    Acao = null;
            }
        }

        public int? AcaoI { get; set; }
        public string AcaoIStr
        {
            get
            {
                if (AcaoI.HasValue)
                {
                    return AcaoI.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        AcaoI = aux;
                    }
                    else
                        throw new ApplicationException("Ação Inicial deve ser um número inteiro.");
                }
                else
                    AcaoI = null;
            }
        }

        public int? AcaoF { get; set; }
        public string AcaoFStr
        {
            get
            {
                if (AcaoF.HasValue)
                {
                    return AcaoF.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        AcaoF = aux;
                    }
                    else
                        throw new ApplicationException("Ação Final deve ser um número inteiro.");
                }
                else
                    AcaoF = null;
            }
        }

        public int? Erros { get; set; }
        public string ErrosStr
        {
            get
            {
                if (Erros.HasValue)
                {
                    return Erros.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        Erros = aux;
                    }
                    else
                        throw new ApplicationException("Erros deve ser um número inteiro.");
                }
                else
                    Erros = null;
            }
        }

        public int? ErrosI { get; set; }
        public string ErrosIStr
        {
            get
            {
                if (ErrosI.HasValue)
                {
                    return ErrosI.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        ErrosI = aux;
                    }
                    else
                        throw new ApplicationException("Erros Inicial deve ser um número inteiro.");
                }
                else
                    ErrosI = null;
            }
        }

        public int? ErrosF { get; set; }
        public string ErrosFStr
        {
            get
            {
                if (ErrosF.HasValue)
                {
                    return ErrosF.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    if (int.TryParse(value, out int aux))
                    {
                        ErrosF = aux;
                    }
                    else
                        throw new ApplicationException("Erros Final deve ser um número inteiro.");
                }
                else
                    ErrosF = null;
            }
        }

        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Versao] = VERSAO;
                resp[(int)Colunas.FCMRegistration] = FCMRegistration;
                resp[(int)Colunas.NMaxReg] = NMaxRegStr;
                resp[(int)Colunas.OrderByAsc] = OrderByAscStr;
                resp[(int)Colunas.Sentido] = SentidoStr;
                resp[(int)Colunas.HorarioRegistro] = HorarioRegistroStr;
                resp[(int)Colunas.HorarioRegistroI] = HorarioRegistroIStr;
                resp[(int)Colunas.HorarioRegistroF] = HorarioRegistroFStr;
                resp[(int)Colunas.HorarioLidaNula] = HorarioLidaNulaStr;
                resp[(int)Colunas.HorarioLida] = HorarioLidaStr;
                resp[(int)Colunas.HorarioLidaI] = HorarioLidaIStr;
                resp[(int)Colunas.HorarioLidaF] = HorarioLidaFStr;
                resp[(int)Colunas.Acao] = AcaoStr;
                resp[(int)Colunas.AcaoI] = AcaoIStr;
                resp[(int)Colunas.AcaoF] = AcaoFStr;
                resp[(int)Colunas.Erros] = ErrosStr;
                resp[(int)Colunas.ErrosI] = ErrosIStr;
                resp[(int)Colunas.ErrosF] = ErrosFStr;

                return resp;
            }
        }
    }
}