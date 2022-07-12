using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SAMU192Core.BLL;
using ConfiguradorAreaCobertura.Events;
using SAMU192Core.DTO;

namespace ConfiguradorAreaCobertura
{
    public partial class ucServidor : UserControl
    {
        public List<ServidorDTO> servidores;

        public delegate void ServidorEventHandler(object sender, ServidorEventArgs e);
        public event ServidorEventHandler ServidorSelecionado;

        public ucServidor()
        {
            InitializeComponent();
        }

        public void AtualizaDados()
        {
            lbServidores.DataSource = new BindingList<ServidorDTO>(servidores);
        }

        private ServidorDTO CarregaServidor()
        {
            ServidorDTO resp = new ServidorDTO();
            int cod;
            if (!int.TryParse(txtCodServidor.Text, out cod))
                throw new ApplicationException(string.Format("'{0}' não é um código válido!", txtCodServidor.Text));

            resp.Cod = cod;
            resp.Nome = txtNome.Text;
            resp.Endereco = txtEndereço.Text;
            resp.Usuario = txtUsuario.Text;
            resp.Senha = txtSenha.Text;

            return resp;

        }

        private void InsereServidor(ServidorDTO servidor)
        {
            servidores.Add(servidor);
            AtualizaDados();
        }

        private void AlteraServidor(ServidorDTO servidor)
        {
            if (lbServidores.SelectedItem != null)
            {
                ServidorDTO aux = (ServidorDTO)lbServidores.SelectedItem;
                aux.Cod = servidor.Cod;
                aux.Endereco = servidor.Endereco;
                aux.Nome = servidor.Nome;
                aux.Usuario = servidor.Usuario;
                aux.Senha = servidor.Senha;
                AtualizaDados();
            }
            else
                throw new ApplicationException("Nenhum servidor selecionado para alteração!");
        }
        private void RemoveServidor()
        {
            if (lbServidores.SelectedItem != null)
            {
                servidores.Remove((ServidorDTO)lbServidores.SelectedItem);
                AtualizaDados();
            }
            else
                throw new ApplicationException("Nenhum servidor selecionado para remoção!");
        }

        private void ExibeServidor()
        {
            ServidorDTO servidor = (ServidorDTO)lbServidores.SelectedItem;
            if (servidor != null)
            {
                txtCodServidor.Text = servidor.Cod.ToString();
                txtEndereço.Text = servidor.Endereco;
                txtNome.Text = servidor.Nome;
                txtUsuario.Text = servidor.Usuario;
                txtSenha.Text = servidor.Senha;
            }
            else
            {
                txtCodServidor.Clear();
                txtEndereço.Clear();
                txtNome.Clear();
                txtUsuario.Clear();
                txtSenha.Clear();
            }
        }

        private void lbServidores_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExibeServidor();
            OnServidorSelecionado(new ServidorEventArgs((ServidorDTO)lbServidores.SelectedItem));
        }


        private void pbInserirServidor_Click(object sender, EventArgs e)
        {
            try
            {
                InsereServidor(CarregaServidor());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pbAlterarServidor_Click(object sender, EventArgs e)
        {
            try
            {
                AlteraServidor(CarregaServidor());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void pbRemoverServidor_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveServidor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected virtual void OnServidorSelecionado(ServidorEventArgs e)
        {
            ServidorSelecionado?.Invoke(this, e);
        }

    }
}
