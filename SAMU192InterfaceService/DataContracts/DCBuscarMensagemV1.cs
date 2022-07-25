using System;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCBuscarMensagemV1
    {
        enum Colunas
        {
            Versao = 0,
            Identificador = 1,
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
            Timestamp = 12
        }

        const int NUM_COLUNAS = 13;
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

            if (string.IsNullOrEmpty(dados[(int)Colunas.Identificador]))
                throw new ApplicationException("Busca de Mensagem: Identificador não informado!");

            Identificador = dados[(int)Colunas.Identificador];
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
            TimestampStr = dados[(int)Colunas.Timestamp];
        }

        public string Identificador { get; set; }

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

        public int? Timestamp { get; set; }
        public string TimestampStr
        {
            get
            {
                if (Timestamp.HasValue)
                {
                    return Timestamp.Value.ToString();
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
                        Timestamp = aux;
                    }
                    else
                        throw new ApplicationException("Ação deve ser um número inteiro.");
                }
                else
                    Timestamp = null;
            }
        }

        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Versao] = VERSAO;
                resp[(int)Colunas.Identificador] = Identificador;
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
                resp[(int)Colunas.Timestamp] = TimestampStr;

                return resp;
            }
        }
    }
}