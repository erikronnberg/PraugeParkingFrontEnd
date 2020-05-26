using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PraugeParkingFrontEnd
{
    public partial class Form1 : Form
    {
        static Parking parking = new Parking();
        public string file;
        public static List<string> regNrList = new List<string>();
        public Form1()
        {
            InitializeComponent();
            FillTableLayout();
        }
        public void ClearInput()
        {
            txtRegNummer.Clear();
            label2.Text = "";
            rbnCar.Checked = false;
            rbnMC.Checked = false;
        }

        public string SaveDialog()
        {
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Title = "Create a new text file to save your parkinglot";
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                return filename;
            }
            return string.Empty;
        }

        public string OpenDialog()
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Title = "Browse saved parking lot files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                return file;
            }
            return string.Empty;
        }

        public void ShowLot()
        {
            dgwParking.Rows.Clear();
            for (int i = 0; i < parking.NumberOfSquares; i++)
            {
                foreach (Vehicle item in parking[i])
                {
                    if (item != null)
                    {
                        dgwParking.Rows.Add(new object[] { i, item.RegNr, item.Type, item.EntryTime });
                    }
                }
            }
        }

        public string Input()
        {
            if (string.IsNullOrEmpty(file)) MessageBox.Show("No file open, either open one or create a new one", "File not found");
            else if (CleanRead.CleanInput.IsSanitized(txtRegNummer.Text, out string regNr) != true && (rbnCar.Checked == true || rbnMC.Checked == true)) MessageBox.Show("Select a vehicle type or type in a alphanumerical license plate of no more than 10 characters", "Incorret input data");
            else
                return regNr;
            return null;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file = SaveDialog();
            if (string.IsNullOrEmpty(file)) MessageBox.Show("Could not create the file");
        }

        private void öppnaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            file = OpenDialog();
            if (string.IsNullOrEmpty(file)) MessageBox.Show("Could not open file", "Operation failed");
            else
            {
                parking.ImportFromFile(file);
                ShowLot();
                FillTableLayout();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Park()
        {
            string regNr = Input();
            if (rbnCar.Checked == true && string.IsNullOrWhiteSpace(file) != true)
            {
                Car car = new Car(regNr, DateTime.Now);
                parking.AddVehicle(car);
            }
            else if (rbnMC.Checked == true && string.IsNullOrWhiteSpace(file) != true)
            {
                Motorcycle mc = new Motorcycle(regNr, DateTime.Now);
                parking.AddVehicle(mc);
            }
            parking.ExportToFile(file);
            ClearInput();
            ShowLot();
            FillTableLayout();
        }

        private void MoveVehicle()
        {
            string regNr = Input();
            int originalPosition = parking.SearchVehicle(regNr); //kollar om den finns
            if (originalPosition >= 0)
            {
                int spot = parking.RemoveVehicle(regNr, out Vehicle vehicle); //får ut vehicle object genom att remove'a
                Motorcycle _temp = new Motorcycle("_TEMP");
                parking.AddVehicle(_temp, spot);
                if (vehicle.Type == "CAR")
                {
                    Car car = new Car(regNr, vehicle.EntryTime);
                    parking.AddVehicle(car); //parkerar automatiskt
                }
                else if (vehicle.Type == "MC")
                {
                    Motorcycle mc = new Motorcycle(regNr, vehicle.EntryTime);
                    parking.AddVehicle(mc); //parkerar automatiskt
                }
                parking.RemoveVehicle(_temp.RegNr, out Vehicle temp);
                parking.ExportToFile(file);
                ClearInput();
                ShowLot();
                FillTableLayout();
            }
        }

        private void parkNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Park();
        }

        private void moveVehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveVehicle();
        }

        private void searchVehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string regNr = Input();
            int spot = parking.SearchVehicle(regNr);
            label2.Text = regNr + spot.ToString();
        }

        private void removeVehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string regNr = Input();
            parking.RemoveVehicle(regNr, out Vehicle vehicle);
            parking.ExportToFile(file);
            ClearInput();
            ShowLot();
            FillTableLayout();
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            ShowLot();
        }

        public void FillTableLayout()
        {
            int i = 0;
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.Text = "EMPTY";
                    iconLabel.Name = i.ToString();
                    iconLabel.BackColor = Color.Green;
                    foreach (Vehicle item in parking[i])
                    {
                        if (item != null)
                        {
                            if (parking[i].CapacityRemaining == 0)
                            {
                                iconLabel.BackColor = Color.Red;
                            }
                            else if (parking[i].CapacityRemaining == 50)
                            {
                                iconLabel.BackColor = Color.Yellow;
                            }
                            iconLabel.Text = item.Type;
                        }
                    }
                }
                i++;
            }
        }

        public void SpotClicked(object sender, EventArgs e)
        {
            Label iconLabel = sender as Label;
            bool success = int.TryParse(iconLabel.Name, out int x);
            using (DialogForm dialog = new DialogForm())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK && success == true)
                {
                    if (DialogForm.SelectedButton == "Add")
                    {
                        if (dialog.car != null)
                            parking.AddVehicle(dialog.car, x);
                        else if (dialog.mc != null)
                            parking.AddVehicle(dialog.mc, x);
                        parking.ExportToFile(file);
                        ClearInput();
                        ShowLot();
                        FillTableLayout();
                    }
                    else if (DialogForm.SelectedButton == "Remove")
                    {
                        //if (dialog.car != null)
                        //    parking.RemoveVehicle(dialog.car.RegNr, out Vehicle _);
                        //else if (dialog.mc != null)
                        //    parking.RemoveVehicle(dialog.mc.RegNr, out _);
                        bool isCar = false;
                        foreach (Vehicle vehicle in parking[x])
                        {
                            if (vehicle.Type == "CAR")
                            {
                                parking.RemoveVehicle(vehicle.RegNr, out Vehicle _);
                                isCar = true;
                                break;
                            }
                            else
                                regNrList.Add(vehicle.RegNr);
                        }
                        if (isCar == false && regNrList.Count == 2)
                        {
                            using (RemoveDialog removeDialog = new RemoveDialog())
                            {
                                if (removeDialog.ShowDialog(this) == DialogResult.OK)
                                {
                                    foreach (var item in removeDialog.removeVehicleList)
                                    {
                                        parking.RemoveVehicle(item, out Vehicle _);
                                    }
                                }
                            }
                        }
                        else
                            parking.RemoveVehicle(regNrList[0], out Vehicle _);

                        regNrList.Clear();
                        parking.ExportToFile(file);
                        ClearInput();
                        ShowLot();
                        FillTableLayout();
                    }
                    else if (DialogForm.SelectedButton == "Move")
                    {
                        if (dialog.car != null)
                            parking.MoveVehicle(dialog.car.RegNr, x);
                        if (dialog.mc != null)
                            parking.MoveVehicle(dialog.mc.RegNr, x);
                        parking.ExportToFile(file);
                        ClearInput();
                        ShowLot();
                        FillTableLayout();
                    }
                }
                dialog.Dispose();
            }
        }
    }
}
