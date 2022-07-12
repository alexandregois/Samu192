using System;
using System.Linq;
using System.Collections.Generic;
using SAMU192Core.BLL;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192Core.Interfaces;

namespace SAMU192Core.Facades
{
    public static class FacadeCadastro
    {
        static IStorageProvider StorageProvider;
        public static void Carrega(IStorageProvider storageProvider)
        {
            StorageProvider = storageProvider;
        }
       
        public static bool Salvar<T>(T obj)
        {
            bool result = StorageProvider.Salvar<T>(obj);
            return result;
        }

        public static bool AdicionarEndereco(EnderecoDTO endereco)
        {
            List<EnderecoDTO> enderecos = Recuperar<List<EnderecoDTO>>() ?? new List<EnderecoDTO>();
            enderecos.Add(endereco);
            return SalvarColection(enderecos);
        }

        public static bool SalvarColection<T>(T listDtos)
        {
            bool result = StorageProvider.Salvar(listDtos);
            return result;
        }

        public static bool ValidarTelefone<T>(T obj)
        {
            Cadastro c = new Cadastro(obj as CadastroDTO);
            return c.ValidarTelefones();
        }

        public static bool ValidarCadastro<T>(T obj)
        {
            Cadastro c = new Cadastro(obj as CadastroDTO);
            return c.Validar();
        }

        public static T Recuperar<T>()
        {
            string key = Activator.CreateInstance<T>().GetType().Name;
            return StorageProvider.Recuperar<T>(key);
        }

        public static void ValidaFavorito(string nome, string referencia)
        {
            if (string.IsNullOrEmpty(nome))
                throw new ValidationException("Um Nome para o endereço favorito deve ser informado.");

            //if (string.IsNullOrEmpty(referencia))
            //    throw new ValidationException("Uma Referência para o endereço favorito deve ser informada.");

            var enderecos = Recuperar<List<EnderecoDTO>>();
            if (enderecos.Select(l => l.Nome).ToList().Contains(nome))
                throw new ValidationException("Já existe um endereço favorito com este nome! Informe outro nome.");
        }
    }
}
