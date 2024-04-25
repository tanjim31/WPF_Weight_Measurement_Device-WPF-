using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Data.SqlClient;
using System.Data;

namespace WPFUSB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            LoadUSBDevices();
            //LoadSerialPortDevices();
            //LoadUSBDevices("USB Root Hub (USB 3.0)");
            LoadCombinedData();
            //weightTextBox.TextChanged += weightTextBox_TextChanged;
            datagrid1.SelectionChanged += datagrid1_SelectionChanged;

        }


        SqlConnection con = new SqlConnection("Data Source=STL-SOFT-RAHAT\\SQLEXPRESS;Initial Catalog=Vehicle;Integrated Security=True; ");

        //private void LoadUSBDevices() //It retrieves a broad range of USB-related devices, including controllers, hubs, and peripherals.
        //{
        //    try
        //    {
        //        // Query for USB devices
        //        ManagementObjectSearcher searcher =
        //            new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(USB%'");

        //        // Extract device names from the query results
        //        List<string> deviceNames = new List<string>();
        //        foreach (ManagementObject queryObj in searcher.Get())
        //        {
        //            string deviceName = queryObj["Caption"] as string;
        //            if (deviceName != null)
        //            {
        //                deviceNames.Add(deviceName);
        //            }
        //        }

        //        // Set the ListBox item source to the list of device names
        //        deviceListBox.ItemsSource = deviceNames;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading USB devices: " + ex.Message);
        //    }
        //}


        //private void LoadSerialPortDevices()  //This is for serial ports
        //{
        //    try
        //    {
        //        // Get available serial port names (retrieves the names of serial ports available on the system)
        //        string[] portNames = SerialPort.GetPortNames();


        //        // Set the ListBox item source to the list of serial port names
        //        deviceListBox.ItemsSource = portNames;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading serial port devices: " + ex.Message);
        //    }
        //}




        //Information about USB controllers, excluding other USB devices like peripherals and hubs.

        //private void LoadUSBDevices()
        //{
        //    try
        //    {
        //        // Query for USB controllers
        //        ManagementObjectSearcher searcher =
        //            new ManagementObjectSearcher("SELECT * FROM Win32_USBController");
        //        //USB ports are typically associated with USB controllers in the system
        //        //It selects all instances of USB controllers directly.
        //        //This queries provide information about USB controllers, not individual ports.
        //        //It provides detailed information about each USB controller, such as manufacturer, device ID, status, and more.

        //        // Extract device names from the query results
        //        List<string> deviceNames = new List<string>();
        //        foreach (ManagementObject queryObj in searcher.Get())
        //        {
        //            // Extract information about the USB controller, such as its caption
        //            string deviceName = queryObj["Caption"] as string;
        //            if (deviceName != null)
        //            {
        //                deviceNames.Add(deviceName);
        //            }
        //        }

        //        // Set the ListBox item source to the list of USB controller names
        //        deviceListBox.ItemsSource = deviceNames;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading USB devices: " + ex.Message);
        //    }
        //}



        //using for the first USB device found, regardless of whether it's a controller or a port.
        //private void LoadUSBDevices() // finding the first USB device
        //{
        //    try
        //    {
        //        // Query for the first USB controller or USB port
        //        ManagementObjectSearcher searcher =
        //            new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice");

        //        // Extract device names from the query results
        //        List<string> deviceNames = new List<string>();
        //        foreach (ManagementObject queryObj in searcher.Get())
        //        {
        //            // Extract information about the USB device, such as its caption
        //            ManagementObject usbDevice = new ManagementObject((string)queryObj["Dependent"]);
        //            string deviceName = usbDevice["Caption"] as string;
        //            if (deviceName != null)
        //            {
        //                deviceNames.Add(deviceName);
        //                break; // Break after finding the first USB device
        //            }
        //        }

        //        // Set the ListBox item source to the list of USB device names
        //        deviceListBox.ItemsSource = deviceNames;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading USB devices: " + ex.Message);
        //    }
        //}


        //Specific USB Device 
        //private void LoadUSBDevices(string targetDeviceName)
        //{
        //    try
        //    {
        //        // Query for the specific USB device by its name
        //        ManagementObjectSearcher searcher =
        //            new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice");

        //        // Extract device names from the query results
        //        List<string> deviceNames = new List<string>();
        //        foreach (ManagementObject queryObj in searcher.Get())
        //        {
        //            // Extract information about the USB device, such as its caption
        //            ManagementObject usbDevice = new ManagementObject((string)queryObj["Dependent"]);
        //            string deviceName = usbDevice["Caption"] as string;
        //            if (deviceName != null && deviceName.Contains(targetDeviceName))
        //            {
        //                deviceNames.Add(deviceName);
        //            }
        //        }

        //        // Set the ListBox item source to the list of USB device names
        //        deviceListBox.ItemsSource = deviceNames;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading USB devices: " + ex.Message);
        //    }
        //}



        //deviceDescription
        private void LoadUSBDevices()
        {
            try
            {
                // Clear existing items in the ListBox
                deviceListBox.ItemsSource = null;

                // Query for all USB devices
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice");
                List<string> deviceNames = new List<string>();

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    // Extract information about the USB device, such as its caption and description
                    ManagementObject usbDevice = new ManagementObject((string)queryObj["Dependent"]);
                    string deviceName = usbDevice["Caption"] as string;
                    string deviceDescription = usbDevice["Description"] as string;

                    // Check if the device matches the characteristics of the weight detect machine
                    // You might need to adjust this condition based on the specific characteristics of your weight detect machine
                    if (!string.IsNullOrEmpty(deviceName) && !string.IsNullOrEmpty(deviceDescription) &&
                        deviceDescription.Contains(/*"Weight Detect Machine"*/ "USB Root Hub (USB 3.0)"))
                    {
                        deviceNames.Add(deviceName);
                    }
                }

                // Set the ListBox item source to the list of USB device names
                deviceListBox.ItemsSource = deviceNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading USB devices: " + ex.Message);
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void clearData()  //This is for clear text box data
        {

            // Clear all text boxes
            vehicleNo_txt.Clear();
            ChalanNo_txt.Clear();
            Date_txt.SelectedDate = null;
            weightTextBox.Clear(); // Clear the weightTextBox
            datagrid1.ItemsSource = null;  // Clear the data grid



        }

        //without checkbox
        //private async void DeviceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //    //column based
        //    try
        //    {
        //        // Clear existing items in the DataGrid
        //        datagrid1.ItemsSource = null;

        //        // Get the selected USB device name
        //        string selectedDeviceName = deviceListBox.SelectedItem as string;

        //        // Retrieve information associated with the selected USB device
        //        List<DeviceInfo> deviceInfoList = GetDeviceInfo(selectedDeviceName);

        //        // Set the ItemsSource of the DataGrid to the list of device information
        //        datagrid1.ItemsSource = deviceInfoList;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error connecting to USB device: " + ex.Message);
        //    }

        //}


        //with Weight
        private async void DeviceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear existing items in the DataGrid
            datagrid1.ItemsSource = null;

            // Get the selected USB device name
            string selectedDeviceName = deviceListBox.SelectedItem as string;

            // Retrieve information associated with the selected USB device
            List<DeviceInfo> deviceInfoList = GetDeviceInfo(selectedDeviceName);

            // Set the ItemsSource of the DataGrid to the list of device information
            datagrid1.ItemsSource = deviceInfoList;

            // Update the weightTextBox with the weight value of the first item in deviceInfoList
            if (deviceInfoList.Count > 0)
            {
                weightTextBox.Text = deviceInfoList[0].Weight.ToString();
            }
            else
            {
                // Clear weightTextBox if no device info is available
                weightTextBox.Text = "";
            }
        }



        private List<DeviceInfo> GetDeviceInfo(string deviceName)
        {

            List<DeviceInfo> deviceInfoList = new List<DeviceInfo>();

            // Create a new DeviceInfo object and populate its properties
            DeviceInfo deviceInfo = new DeviceInfo
            {
                DeviceName = deviceName,
                Weight = 0, // Sample weight value
                Unit = "KG"  // Sample unit value
            };

            deviceInfoList.Add(deviceInfo);
            return deviceInfoList;
        }


        //With Weight
        private string previousWeightValue = ""; // Variable to store the previous weight value
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction
                transaction = con.BeginTransaction("SaveTransaction");

                // Assign transaction to the command object
                cmd.Connection = con;
                cmd.Transaction = transaction;

                try
                {
                    // Insert into VehicleInfo table
                    cmd.CommandText = "INSERT INTO VehicleInfo (VehicleNo, ChalanNo, Date) VALUES (@VehicleNo, @ChalanNo, @Date);";
                    cmd.Parameters.AddWithValue("@VehicleNo", vehicleNo_txt.Text);
                    cmd.Parameters.AddWithValue("@ChalanNo", ChalanNo_txt.Text);
                    cmd.Parameters.AddWithValue("@Date", Date_txt.SelectedDate);

                    cmd.ExecuteNonQuery();

                    // Retrieve the last inserted VehicleId
                    cmd.CommandText = "SELECT MAX(VehicleId) FROM VehicleInfo;";
                    int vehicleId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Insert into DeviceInfo table
                    cmd.CommandText = "INSERT INTO DeviceInfo (DeviceName, Weight, Unit, VehicleId) VALUES (@DeviceName, @Weight, @Unit, @VehicleId)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@DeviceName", deviceListBox.SelectedItem);
                    cmd.Parameters.AddWithValue("@Weight", 5); // Sample weight value
                    cmd.Parameters.AddWithValue("@Unit", "KG"); // Sample unit value
                    cmd.Parameters.AddWithValue("@VehicleId", vehicleId); // Use the retrieved VehicleId

                    cmd.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();
                    MessageBox.Show("Inserted Successfully");

                    // Restore the previous weight value
                    weightTextBox.Text = previousWeightValue;
                }
                catch (Exception ex)
                {
                    // Roll back the transaction if an exception occurs
                    MessageBox.Show("Error: " + ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Rollback Exception Type: " + ex2.GetType());
                        MessageBox.Show("Message: " + ex2.Message);
                    }
                }
                finally
                {
                    con.Close();
                    clearData();
                    LoadCombinedData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
          

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // You can access the selected date using the SelectedDate property of the DatePicker
            DateTime selectedDate = Date_txt.SelectedDate ?? DateTime.MinValue; // If no date is selected, default to minimum date value

            // Use the selected date as needed, such as displaying it in a MessageBox or performing other operations
            //MessageBox.Show("Selected Date: " + selectedDate.ToString("yyyy-MM-dd"));
        }


        private void LoadCombinedData()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                // SQL query to fetch combined data
                cmd.CommandText = @"SELECT vi.VehicleId, vi.VehicleNo, vi.ChalanNo, vi.Date, di.DeviceName, di.Weight, di.Unit
                            FROM VehicleInfo vi
                            JOIN DeviceInfo di ON vi.VehicleId = di.VehicleId";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Bind the DataTable to the DataGrid
                datagrid2.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading combined data: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void datagrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if an item is selected in the DataGrid
            if (datagrid1.SelectedItem != null)
            {
                // Get the selected item from the DataGrid
                var selectedDeviceInfo = datagrid1.SelectedItem as DeviceInfo;

                // Update the weightTextBox with the weight value from the selected item
                if (selectedDeviceInfo != null)
                {
                    weightTextBox.Text = selectedDeviceInfo.Weight.ToString();
                }
            }
        }
        private void datagrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void weightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

      
    }
}

