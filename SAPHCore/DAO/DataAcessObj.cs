using System;
using SAMU192Core.Interfaces;

namespace SAMU192Core.DAO
{
    internal static class DataAcessObj
    {
        private static ILeitorDados leitor;
        internal static ILeitorDados Leitor
        {
            get => leitor;
            set{ leitor = value; }
        }

        internal static void Inicializar(ILeitorDados leitor)
        {
            Leitor = leitor;
        }
    }
}
