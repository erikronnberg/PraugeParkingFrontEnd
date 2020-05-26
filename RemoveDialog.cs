using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PraugeParkingFrontEnd
{
    public partial class RemoveDialog : Form
    {
        public List<string> removeVehicleList = new List<string>();
        public RemoveDialog()
        {
            InitializeComponent();
            cbx1.Text = Form1.regNrList[0];
            cbx2.Text = Form1.regNrList[1];
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cbx1.Checked == true)
                removeVehicleList.Add(cbx1.Text);
            if (cbx2.Checked == true)
                removeVehicleList.Add(cbx2.Text);
            DialogResult = DialogResult.OK;
        }
    }
}
