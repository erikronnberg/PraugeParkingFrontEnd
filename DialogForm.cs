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
    public partial class DialogForm : Form
    {
        public static string SelectedButton { get; set; }
        public Car car { get; set; }
        public Motorcycle mc { get; set; }

        public DialogForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SelectedButton = btnAdd.Text;
            if (rbnMC.Checked == true)
                mc = new Motorcycle(txtReg.Text, DateTime.Now);
            else if (rbnCar.Checked == true)
                car = new Car(txtReg.Text, DateTime.Now);
            btnAdd.DialogResult = DialogResult.OK;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            SelectedButton = btnRemove.Text;
            if (rbnMC.Checked == true)
                mc = new Motorcycle(txtReg.Text, DateTime.Now);
            else if (rbnCar.Checked == true)
                car = new Car(txtReg.Text, DateTime.Now);
            btnRemove.DialogResult = DialogResult.OK;
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            SelectedButton = btnMove.Text;
            if (rbnMC.Checked == true)
                mc = new Motorcycle(txtReg.Text, DateTime.Now);
            else if (rbnCar.Checked == true)
                car = new Car(txtReg.Text, DateTime.Now);
            btnMove.DialogResult = DialogResult.OK;
        }
    }
}
