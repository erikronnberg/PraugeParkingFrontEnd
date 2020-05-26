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

        /// <summary>
        /// Shows all current vehicles parked in a datagridview
        /// </summary>
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

        /// <summary>
        /// Cleans input data
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Opens a file dialog to load a saved parking lot into memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Takes user input and adds vehicle to parkinglot
        /// </summary>
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

        /// <summary>
        /// Moves a vehicle to the first empty spot in the lot
        /// </summary>
        private void MoveVehicle()
        {
            //User input to decide which vehicle to move
            string regNr = Input();
            //Finds the current spot of the vehicle
            int originalPosition = parking.SearchVehicle(regNr);
            if (originalPosition >= 0)
            {
                //removes the vehicle to get the time of parking
                //and then adds a temporary vehicle in its place to then
                //add the vehicle to its new spot
                int spot = parking.RemoveVehicle(regNr, out Vehicle vehicle);
                Motorcycle _temp = new Motorcycle("_TEMP");
                parking.AddVehicle(_temp, spot);
                if (vehicle.Type == "CAR")
                {
                    Car car = new Car(regNr, vehicle.EntryTime);
                    parking.AddVehicle(car);
                }
                else if (vehicle.Type == "MC")
                {
                    Motorcycle mc = new Motorcycle(regNr, vehicle.EntryTime);
                    parking.AddVehicle(mc);
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

        /// <summary>
        /// Sets the parkingspots in the layout panel to their correct colours
        /// depending on the capacity of the parking spot
        /// </summary>
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

        /// <summary>
        /// Click event for the table layout panels parking spots
        /// Creates a new dialog window for user input and 
        /// either adds, removes or moves vehicles based on
        /// clicked parkingspot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SpotClicked(object sender, EventArgs e)
        {
            Label iconLabel = sender as Label;
            bool success = int.TryParse(iconLabel.Name, out int parkingSpot);
            using (DialogForm dialog = new DialogForm())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK && success == true)
                {
                    if (DialogForm.SelectedButton == "Add")
                    {
                        if (dialog.car != null)
                            parking.AddVehicle(dialog.car, parkingSpot);
                        else if (dialog.mc != null)
                            parking.AddVehicle(dialog.mc, parkingSpot);
                        parking.ExportToFile(file);
                        ClearInput();
                        ShowLot();
                        FillTableLayout();
                    }
                    else if (DialogForm.SelectedButton == "Remove")
                    {
                        foreach (Vehicle vehicle in parking[parkingSpot])
                        {
                            if (vehicle.Type == "CAR")
                            {
                                parking.RemoveVehicle(vehicle.RegNr, out Vehicle _);
                                break;
                            }
                            else if (vehicle.Type == "MC")
                                regNrList.Add(vehicle.RegNr);
                        }
                        if (regNrList.Count == 2)
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
                        else if (regNrList.Count == 1)
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
                            parking.MoveVehicle(dialog.car.RegNr, parkingSpot);
                        if (dialog.mc != null)
                            parking.MoveVehicle(dialog.mc.RegNr, parkingSpot);
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
