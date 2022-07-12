using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfiguradorAreaCobertura
{
    public partial class frmLista : Form
    {

        private List<object> lista;

        public List<object> ItemsSelecionados
        {
            get
            {
                List<object> resp = new List<object>();
                foreach (object x in lbLista.CheckedItems)
                    resp.Add(x);
                return resp;
            }
        }

        public frmLista(List<object> list)
        {
            InitializeComponent();

            this.lista = list;

        }

        private void pbCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmLista_Load(object sender, EventArgs e)
        {
            lbLista.DataSource = new BindingList<Object>(lista);
        }

        private void pbOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
