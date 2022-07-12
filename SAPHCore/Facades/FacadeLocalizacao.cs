using System.Collections.Generic;
using System.Linq;
using SAMU192Core.Exceptions;
using SAMU192Core.Interfaces;
using SAMU192Core.BLL;
using SAMU192Core.DAO;
using SAMU192Core.DTO;
using System;

namespace SAMU192Core.Facades
{
    public static class FacadeLocalizacao
    {
        static bool inicializada = false;
        static Mapeamento cadastro = null;

        public static void Inicializar()
        {
            cadastro = new Mapeamento();
            cadastro.areas = new List<Area>();
            cadastro.quadrantes = new List<Quadrante>();
            cadastro.servidores = new List<Servidor>();
            inicializada = true;
        }

        public static void Carregar(ILeitorDados leitor)
        {
            Inicializar();
            DataAcessObj.Inicializar(leitor);
            CarregarCadastro();
        }

        public static void Gravar(ILeitorDados leitor)
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");
            DataAcessObj.Inicializar(leitor);
            cadastro.Grava();
            cadastro.areas.ForEach(x => x.Grava());
        }

        public static bool ValidarEndereco(EnderecoDTO endereco)
        {
            if (endereco == null)
            {
                return false;
            }

            var servidores = Localizar(endereco.Coordenada);

            if (servidores == null || servidores.Count == 0)
            {
                return false;
            }
            return true;
        }

        public static void GeraQuadrantes(Geometria geo, int maxNiveis, int minAreas )
        {
            GeradorQuadrantes g = new GeradorQuadrantes(cadastro, geo) { MaxNiveis = maxNiveis, MinAreas = minAreas };
            cadastro.quadrantes = g.GeraQuadrantes();
        }

        public static List<ServidorDTO> Localizar(CoordenadaDTO coordenada)
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");

            return cadastro.ServidoresDaCoordenada(coordenada.Coordenada).Select(x=>new ServidorDTO(x)).ToList();
        }

        private static void CarregarCadastro()
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");

            Mapeamento map = new Mapeamento();
            map.Carrega();
            cadastro = map;
        }

        public static List<ServidorDTO> getServidores()
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");
            return cadastro.servidores.Select(x => new ServidorDTO(x)).ToList();
        }
        public static void setServidores(List<ServidorDTO> lista)
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");
            cadastro.servidores = lista.Select(x => x.Servidor).ToList();
        }

        public static List<AreaDTO> getAreas()
        {
           if (!inicializada) throw new ValidationException("Sistema não inicializado!");
           return cadastro.areas.Select(x => new AreaDTO(x)).ToList();
        }
        public static void setAreas(List<AreaDTO> lista)
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");
                cadastro.areas = lista.Select(x => x.Area).ToList();
        }

        public static List<QuadranteDTO> getQuadrantes()
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");
            return cadastro.quadrantes.Select(x => new QuadranteDTO(x)).ToList();
        }

        public static void setQuadrantes(List<QuadranteDTO> lista)
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");
            cadastro.quadrantes = lista.Select(x => x.Quadrante).ToList();
        }

        public static bool PoligonoContem(PoligonoDTO poligonoDto, CoordenadaDTO coordenadaDto)
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");

            return poligonoDto.Poligono.Contem(coordenadaDto.Coordenada);
        }

        public static bool PoligonoProximo(PoligonoDTO poligonoDto, CoordenadaDTO coordenadaDto)
        {
            if (!inicializada) throw new ValidationException("Sistema não inicializado!");

            return poligonoDto.Poligono.Proximo(coordenadaDto.Coordenada);
        }
    }
}
