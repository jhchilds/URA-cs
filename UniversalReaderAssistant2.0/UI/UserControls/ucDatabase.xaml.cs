using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
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
using System.Web.Script.Serialization;
using System.Data;
using ThingMagic;
using ThingMagic.URA2.BL;
using RestSharp;
using Newtonsoft.Json;
using System.IO;

namespace ThingMagic.URA2
{
    /// <summary>
    /// Interaction logic for ucDatabase.xaml
    /// </summary>
    public partial class ucDatabase : UserControl
    {

        Reader objReader;
        uint startAddress = 0;
        int dataLength = 0;
        int antenna = 0;
        Gen2.Bank selectMemBank;
        /// <summary>
        /// Instances of asset and rfid objects
        /// </summary>
        RfidTags rfid = new RfidTags();
        Assets asset = new Assets();

        public ucDatabase()
        {
            InitializeComponent();
        }

        public void LoadEPC(Reader reader)
        {

            objReader = reader;

        }

        public void Load(Reader reader, uint address, int length, Gen2.Bank selectedBank, TagReadRecord selectedTagRed)
        {
            InitializeComponent();
            objReader = reader;
            startAddress = address;
            dataLength = length;
            selectMemBank = selectedBank;

            spDatabase.IsEnabled = true;
            rbSelectedTag.IsChecked = true;
            rbSelectedTag.IsEnabled = true;

            //Clear Leftover Data
            ClearRFID();
            ClearAsset();

            string[] stringData = selectedTagRed.Data.Split(' ');
            txtEpc.Text = selectedTagRed.EPC;
            txtData.Text = string.Join("", stringData);
            Window mainWindow = App.Current.MainWindow;
            ucTagResults tagResults = (ucTagResults)mainWindow.FindName("TagResults");
            switch (selectedBank)
            {
                case Gen2.Bank.EPC:
                    if (tagResults.txtSelectedCell.Text == "Data")
                    {
                        lblSelectFilter.Content = "EPC Memory, Decimal Address = " + address.ToString() + " and Data = " + txtData.Text;
                    }
                    else
                    {
                        lblSelectFilter.Content = "EPC ID = " + selectedTagRed.EPC;
                    }
                    break;
                case Gen2.Bank.TID:
                    if (tagResults.txtSelectedCell.Text == "Data")
                    {
                        lblSelectFilter.Content = "TID Memory, Decimal Address = " + address.ToString() + " and Data = " + txtData.Text;
                    }
                    else
                    {
                        lblSelectFilter.Content = "EPC ID = " + selectedTagRed.EPC;
                    }
                    break;
                case Gen2.Bank.USER:
                    if (tagResults.txtSelectedCell.Text == "Data")
                    {
                        lblSelectFilter.Content = "User Memory, Decimal Address = " + address.ToString() + " and Data = " + txtData.Text;
                    }
                    else
                    {
                        lblSelectFilter.Content = "EPC ID = " + selectedTagRed.EPC;
                    }
                    break;
            }
            txtCurrentEpc.Text = selectedTagRed.EPC;
            currentEpc = txtCurrentEpc.Text;
            antenna = selectedTagRed.Antenna;

            


        }

        /// <summary>
        /// Reset Database tab to default values
        /// </summary>
        public void ResetDatabaseTab()
        {
            if (null != objReader)
            {
                rbSelectedTag.IsEnabled = false;
                txtEpc.Text = "";
                txtData.Text = "";
                txtCurrentEpc.Text = "";
                lblError.Content = "";
                rbFirstTag.IsChecked = true;
                rbHexRep.IsChecked = true;
            }
        }


        private void rbFirstTag_Checked(object sender, RoutedEventArgs e)
        {
            if (null != objReader)
            {
                ResetDatabaseTab();
            }
        }

        string currentEpcRep = "Hex";
        string currentEpc = string.Empty;

        private void rbHexRep_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentEpcRep != "Hex")
                {
                    if (currentEpcRep == "Ascii")
                    {
                        if (txtCurrentEpc.Text != "")
                        {
                            txtCurrentEpc.Text = Utilities.AsciiStringToHexString(txtCurrentEpc.Text);
                        }
                        
                    }
                    else if (currentEpcRep == "Base36")
                    {
                        if (txtCurrentEpc.Text != "")
                        {
                            txtCurrentEpc.Text = Utilities.ConvertBase36ToHex(txtCurrentEpc.Text);
                        }
                       
                    }
                }
                currentEpcRep = "Hex";
            }
            catch (Exception ex)
            {
                rbHexRep.IsChecked = true;
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbASCIIRep_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentEpcRep == "Hex")
                {
                    if (txtCurrentEpc.Text != "")
                    {
                        txtCurrentEpc.Text = Utilities.HexStringToAsciiString(txtCurrentEpc.Text);
                    }
                   
                }
                else if (currentEpcRep == "Base36")
                {
                    if (txtCurrentEpc.Text != "")
                    {
                        txtCurrentEpc.Text = Utilities.HexStringToAsciiString(Utilities.ConvertBase36ToHex(txtCurrentEpc.Text));
                    }
                    
                }
                currentEpcRep = "Ascii";
            }
            catch (Exception ex)
            {
                if (currentEpcRep == "Hex")
                    rbHexRep.IsChecked = true;
                else if (currentEpcRep == "Base36")
                    rbReverseBase36Rep.IsChecked = true;
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbReverseBase36Rep_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentEpcRep == "Hex")
                {
                    if (txtCurrentEpc.Text != "")
                    {
                        txtCurrentEpc.Text = Utilities.ConvertHexToBase36(txtCurrentEpc.Text);
                    }
                    
                }
                else if (currentEpcRep == "Ascii")
                {
                    if (txtCurrentEpc.Text != "")
                    {
                        txtCurrentEpc.Text = Utilities.ConvertHexToBase36(Utilities.AsciiStringToHexString(txtCurrentEpc.Text));
                    }
                    
                }
                currentEpcRep = "Base36";
            }
            catch (Exception ex)
            {
                if (currentEpcRep == "Hex")
                    rbHexRep.IsChecked = true;
                else if (currentEpcRep == "Ascii")
                    rbASCIIRep.IsChecked = true;
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtCurrentEpc_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back) || (e.Key == Key.Delete))
            {
                e.Handled = true;
            }
        }

        private void txtCurrentEpc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }


        private void updateDatabaseFields()
        {

        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            Window mainWindow = App.Current.MainWindow;
            ComboBox CheckRegionCombobx = (ComboBox)mainWindow.FindName("regioncombo");
            if (CheckRegionCombobx.SelectedValue.ToString() == "Select")
            {
                MessageBox.Show("Please select region", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TagReadData[] tagReads;
            try
            {
                if ((bool)rbFirstTag.IsChecked)
                {
                    SimpleReadPlan srp = new SimpleReadPlan(((null != GetSelectedAntennaList()) ? (new int[] { GetSelectedAntennaList()[0] }) : null), TagProtocol.GEN2, null, 0);
                    objReader.ParamSet("/reader/read/plan", srp);
                    tagReads = objReader.Read(500);
                }
                else
                {
                    SetReadPlan();
                    tagReads = objReader.Read(500);
                }

                if ((null != tagReads) && (tagReads.Length > 0))
                {
                    if ((bool)rbASCIIRep.IsChecked)
                    {
                        txtCurrentEpc.Text = Utilities.HexStringToAsciiString(tagReads[0].EpcString);
                    }
                    else if ((bool)rbReverseBase36Rep.IsChecked)
                    {
                        txtCurrentEpc.Text = Utilities.ConvertHexToBase36(tagReads[0].EpcString);
                    }
                    else
                    {
                        txtCurrentEpc.Text = tagReads[0].EpcString;
                    }
                    if (tagReads.Length > 1)
                    {
                        lblError.Content = "Warning: More than one tag responded";
                        lblError.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        lblError.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    currentEpc = txtCurrentEpc.Text;
                }
                else
                {
                    txtCurrentEpc.Text = "";
                    MessageBox.Show("No tags found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.SetCursor(Cursors.Arrow);

                

            }
        }

        private void SetReadPlan()
        {

            TagFilter filter = null;


            if (selectMemBank == Gen2.Bank.EPC)
            {
                if (txtEpc.Text != "")
                {
                    filter = new TagData(txtEpc.Text);
                }
            }
            else
            {
                byte[] data = ByteFormat.FromHex(txtData.Text);

                if (null == data)
                {
                    dataLength = 0;
                }
                else
                {
                    dataLength = data.Length;
                }

                filter = new Gen2.Select(false, selectMemBank, Convert.ToUInt16(startAddress * 16), Convert.ToUInt16(dataLength * 8), data);
            }

            SimpleReadPlan srp = new SimpleReadPlan(new int[] { antenna }, TagProtocol.GEN2, filter, 1000);
            objReader.ParamSet("/reader/read/plan", srp);
        }

        /// <summary>
        /// Get selected antenna list
        /// </summary>
        /// <returns></returns>
        private List<int> GetSelectedAntennaList()
        {
            Window mainWindow = App.Current.MainWindow;
            CheckBox Ant1CheckBox = (CheckBox)mainWindow.FindName("Ant1CheckBox");
            CheckBox Ant2CheckBox = (CheckBox)mainWindow.FindName("Ant2CheckBox");
            CheckBox Ant3CheckBox = (CheckBox)mainWindow.FindName("Ant3CheckBox");
            CheckBox Ant4CheckBox = (CheckBox)mainWindow.FindName("Ant4CheckBox");
            CheckBox[] antennaBoxes = { Ant1CheckBox, Ant2CheckBox, Ant3CheckBox, Ant4CheckBox };
            List<int> ant = new List<int>();

            for (int antIdx = 0; antIdx < antennaBoxes.Length; antIdx++)
            {
                CheckBox antBox = antennaBoxes[antIdx];

                if ((bool)antBox.IsChecked)
                {
                    int antNum = antIdx + 1;
                    ant.Add(antNum);
                }
            }
            if (ant.Count > 0)
                return ant;
            else
                return null;
        }

        /// <summary>
        /// Retrieving Data from Data Table and placing in necessary textbox
        /// </summary>
        private bool retrieveData()
        {
            
            
            //IF we are using the VTrans REST API
            if (chkBoxREST.IsChecked == true)
            {
                //check internet connection for sync
                if (CheckForInternetConnection())
                {

                    //If there is internet connection create replica immediately
                    if (CreateReplica())
                    {
                        MessageBox.Show("Replica schema retrieved from VTrans Database for offline mode.", "Sync Workflow", MessageBoxButton.OK, MessageBoxImage.Information);

                    }

                    //NO Tags with specified EPC In Database
                    if (GetRequestVTransREST() == false)
                    {
                        return false;
                    }

                    
                        return true;
                }

                //When there is not Internet Connection Innitiate sync by creating a replica
                else
                {
                    MessageBox.Show("No internet connection. Offline mode only.", "No Internet Connection", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return false;  
                }    
            }

            else if (chkBoxOFFLINE.IsChecked == true)
            {

               


                string jsonReplicaString = System.IO.File.ReadAllText(@"C:\Users\Joshua Childs\Desktop\URA-cs\UniversalReaderAssistant2.0\bin\x64\Debug\replica.json");
                //Console.WriteLine(jsonReplicaString);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                dynamic responseDict = serializer.Deserialize<dynamic>(jsonReplicaString);


                foreach (KeyValuePair<string, object> kvp in responseDict)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);

                }


                return false;
            }
            else
            {


                //TESTING DATABASE ACCESS
                rfid.epc = txtCurrentEpc.Text;
                DataTable dt = rfid.Select(rfid);
                dgTagResults.DataContext = dt;

                //Clear Leftover Data
                ClearRFID();
                ClearAsset();

                //Putting data into datafields.
                foreach (DataRow dataRow in dt.Rows)
                {
                    //rfid table data
                    txtRFIDDatabaseID.Text = dataRow.ItemArray[0].ToString(); //rfid.id
                                                                              //txtCurrentEpc.Text = dataRow.ItemArray[1].ToString(); //rfid.epc here for reference not LIVE. Epc retrieved from Tag Results.
                    txtRFIDManufactureDate.Text = dataRow.ItemArray[2].ToString();//rfid.manufacture_date
                    txtRFIDInstallationDate.Text = dataRow.ItemArray[3].ToString();//rfid.installation_date
                    txtAssetID.Text = dataRow.ItemArray[4].ToString();//rfid.asset_id
                    txtRFIDComments.Text = dataRow.ItemArray[5].ToString();//rfid.comments
                                                                           //asset table data
                    txtAssetIDAsset.Text = dataRow.ItemArray[6].ToString();//asset.id
                    txtLaneDirection.Text = dataRow.ItemArray[7].ToString();//asset.lane_direction
                    txtPositionCode.Text = dataRow.ItemArray[8].ToString();//asset.position_code
                    txtRouteSuffix.Text = dataRow.ItemArray[9].ToString();//asset.route_suffix
                    txtMarker.Text = dataRow.ItemArray[10].ToString();//asset.marker
                    txtCity.Text = dataRow.ItemArray[11].ToString();//asset.city
                    txtCounty.Text = dataRow.ItemArray[12].ToString();//asset.county
                    txtDistrict.Text = dataRow.ItemArray[13].ToString();//asset.district
                    txtStreetName.Text = dataRow.ItemArray[14].ToString();//asset.streetname
                    txtMutcdCode.Text = dataRow.ItemArray[15].ToString();//asset.mutcd_code
                    txtRetired.Text = dataRow.ItemArray[16].ToString();//asset.retired
                    txtReplaced.Text = dataRow.ItemArray[17].ToString();//asset.replaced
                    txtSignAge.Text = dataRow.ItemArray[18].ToString();//asset.sign_age
                    txtTwnTid.Text = dataRow.ItemArray[19].ToString();//asset.twn_tid
                    txtTwnMi.Text = dataRow.ItemArray[20].ToString();//asset.twn_mi
                    txtQcFlag.Text = dataRow.ItemArray[21].ToString();//asset.qc_flag
                    txtMinTwnFm.Text = dataRow.ItemArray[22].ToString();//asset.min_twn_fm
                    txtMaxTwnTm.Text = dataRow.ItemArray[23].ToString();//asset.max_twn_tm
                    txtSrSid.Text = dataRow.ItemArray[24].ToString();//asset.sr_sid
                    txtSignHeight.Text = dataRow.ItemArray[25].ToString();//asset.sign_height
                    txtSignWidth.Text = dataRow.ItemArray[26].ToString();//asset.sign_width


                }

                if (txtRFIDDatabaseID.Text == "")
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Clearing RFID DATA textboxes
        /// </summary>
        public void ClearRFID()
        {
            //CLEARING DATA FIELDS
            txtRFIDDatabaseID.Text = "";
            txtAssetID.Text = "";
            txtRFIDManufactureDate.Text = "";
            txtRFIDInstallationDate.Text = "";
            
            txtRFIDComments.Text = "";
            
            

        }
        /// <summary>
        /// Clearing Asset Data textboxes
        /// </summary>
        public void ClearAsset()
        {
            
            txtAssetIDAsset.Text = ""; //Asset Data AssetID
            txtLaneDirection.Text = "";
            txtPositionCode.Text = "";
            txtRouteSuffix.Text = "";
            txtMarker.Text = "";
            txtCity.Text = "";
            txtCounty.Text = "";
            txtDistrict.Text = "";
            txtStreetName.Text = "";
            txtMutcdCode.Text = "";
            txtRetired.Text = "";
            txtReplaced.Text = "";
            txtSignAge.Text = "";
            txtTwnTid.Text = "";
            txtTwnMi.Text = "";
            txtQcFlag.Text = "";
            txtMinTwnFm.Text = "";
            txtMaxTwnTm.Text = "";
            txtSrSid.Text = "";
            txtSignHeight.Text = "";
            txtSignWidth.Text = "";
        }

       

        private void SpDatabase_Loaded(object sender, RoutedEventArgs e)
        {

        }
        
        private void BtnRetrieveData_Click(object sender, RoutedEventArgs e)
        {
            ///Put data into proper fields based on current EPC
            if (!retrieveData())
            {
                MessageBox.Show("Tag not found in database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        //***********************RFID Data Panel**********************************


        private void BtnRFIDInsert_Click(object sender, RoutedEventArgs e)
        {

            if (chkBoxREST.IsChecked == true)
            {
                if (InsertTagREST())
                {
                    MessageBox.Show("Tag Inserted Successfully with REST API");
                }

                else
                {
                    MessageBox.Show("ERROR: REST API Error");
                }
            }

            else
            {


                //Get the value from input fields 
                try
                {
                    rfid.epc = txtCurrentEpc.Text; //Text box has type String 
                    rfid.manufacture_date = txtRFIDManufactureDate.Text;
                    rfid.installation_date = txtRFIDInstallationDate.Text;
                    rfid.asset_id = int.Parse(txtAssetID.Text);
                    rfid.comments = txtRFIDComments.Text;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Incorrect format in data fields.");

                }



                //Insert Data into database
                bool success = rfid.Insert(rfid);
                if (success)
                {
                    //Successfully Inserted
                    MessageBox.Show("New Tag Inserted Successfully");

                }
                else
                {
                    //FAILED Insertion
                    MessageBox.Show("Failed to add new Tag. Try Again.");
                }
                //Load Data in Data Grid View
                DataTable dt = rfid.Select(rfid);
                dgTagResults.DataContext = dt;
            }

        }
        /// <summary>
        /// Update RFID Tag selected 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRFIDUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (chkBoxREST.IsChecked == true)
            {
                if (UpdateTagTableREST())
                {
                    MessageBox.Show("Tag Updated Successfull with REST API");
                }

                else
                {
                    MessageBox.Show("ERROR: REST API Error");
                }
            }
            else
            {


                try
                {
                    //Retrieve Data from the Fields
                    rfid.id = int.Parse(txtRFIDDatabaseID.Text);
                    rfid.epc = txtCurrentEpc.Text;
                    rfid.manufacture_date = txtRFIDManufactureDate.Text;
                    rfid.installation_date = txtRFIDInstallationDate.Text;
                    rfid.asset_id = int.Parse(txtAssetID.Text);
                    rfid.comments = txtRFIDComments.Text;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Incorrect format in data fields.");
                }


                //Update Data in Database
                bool success = rfid.Update(rfid);
                if (success)
                {
                    //Load Data in Data Grid View
                    DataTable dt = rfid.Select(rfid);
                    dgTagResults.DataContext = dt;
                    //Update was successful
                    MessageBox.Show("Tag has been updated successfully");

                }
                else
                {
                    //Update failed
                    MessageBox.Show("Failed to update tag.");

                }
            }

        }
        /// <summary>
        /// Clear textboxes from the RFID Data panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRFIDClear_Click(object sender, RoutedEventArgs e)
        {
            //Clear Leftover Data
            ClearRFID();
            
        }
        /// <summary> 
        /// Delete an RFID Tag from the Database 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRFIDDelete_Click(object sender, RoutedEventArgs e)
        {

            if (chkBoxREST.IsChecked == true)
            {
                if (DeleteTagREST())
                {
                    MessageBox.Show("Tag Deleted Successfully with REST API");
                }

                else
                {
                    MessageBox.Show("ERROR: REST API Error");
                }
            }

            else
            {



                try
                {
                    //Retrieve data from Fields
                    rfid.id = int.Parse(txtRFIDDatabaseID.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Data fields not in correct format.");

                }
                bool success = rfid.Delete(rfid);
                if (success)
                {

                    //Load Data in Data Grid View
                    DataTable dt = rfid.Select(rfid);
                    dgTagResults.DataContext = dt;
                    //Successful Deletion
                    MessageBox.Show("Tag has been deleted successfully");
                    ClearRFID();
                    ClearAsset();


                }
                else
                {
                    //Failed Deletion
                    MessageBox.Show("Failed deletion. Try again.");

                }
            }
        }

        //***********************Asset Data Panel**********************************

        private void BtnAssetInsert_Click(object sender, RoutedEventArgs e)
        {
            if (chkBoxREST.IsChecked == true)
            {
                if (InsertSignREST())
                {
                    MessageBox.Show("Asset Inserted Successfully with REST API");
                }

                else
                {
                    MessageBox.Show("ERROR: REST API Error");
                }
            }

            else
            {


                //Get the value from input fields 
                try
                {

                    asset.lane_direction = txtLaneDirection.Text; //Text box has type String 
                    asset.position_code = txtPositionCode.Text;
                    asset.route_suffix = txtRouteSuffix.Text;
                    asset.marker = float.Parse(txtMarker.Text);
                    asset.city = txtCity.Text;
                    asset.county = txtCounty.Text;
                    asset.district = int.Parse(txtDistrict.Text);
                    asset.streetname = txtStreetName.Text;
                    asset.mutcd_code = txtMutcdCode.Text;
                    asset.retired = int.Parse(txtRetired.Text);
                    asset.replaced = DateTime.Parse(txtReplaced.Text);
                    asset.sign_age = int.Parse(txtSignAge.Text);
                    asset.twn_tid = txtTwnTid.Text;
                    asset.twn_mi = float.Parse(txtTwnMi.Text);
                    asset.qc_flag = int.Parse(txtQcFlag.Text);
                    asset.min_twn_fm = float.Parse(txtMinTwnFm.Text);
                    asset.max_twn_tm = float.Parse(txtMaxTwnTm.Text);
                    asset.sr_sid = txtSrSid.Text;
                    asset.sign_height = int.Parse(txtSignHeight.Text);
                    asset.sign_width = int.Parse(txtSignWidth.Text);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Incorrect format in data fields.");

                }



                //Insert Data into database
                bool success = asset.Insert(asset);
                if (success)
                {
                    //Successfully Inserted
                    MessageBox.Show("New Asset Inserted Successfully into Database");

                }
                else
                {
                    //FAILED Insertion
                    MessageBox.Show("Failed to add new Asset to database.");
                }
            }
            

        }
        /// <summary>
        /// Update Assets Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAssetUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (chkBoxREST.IsChecked == true)
            {
                if (UpdateSignTableREST())
                {
                    MessageBox.Show("Sign Updated Successfult with REST API");
                }

                else
                {
                    MessageBox.Show("ERROR: REST API Error");
                }
            }
            else
            {
                try
                {
                    //Retrieve Data from the Fields
                    asset.id = int.Parse(txtAssetIDAsset.Text);
                    asset.lane_direction = txtLaneDirection.Text;
                    asset.position_code = txtPositionCode.Text;
                    asset.route_suffix = txtRouteSuffix.Text;
                    asset.marker = float.Parse(txtMarker.Text);
                    asset.city = txtCity.Text;
                    asset.county = txtCounty.Text;
                    asset.district = int.Parse(txtDistrict.Text);
                    asset.streetname = txtStreetName.Text;
                    asset.mutcd_code = txtMutcdCode.Text;
                    asset.retired = int.Parse(txtRetired.Text);
                    asset.replaced = DateTime.Parse(txtReplaced.Text);
                    asset.sign_age = int.Parse(txtSignAge.Text);
                    asset.twn_tid = txtTwnTid.Text;
                    asset.twn_mi = float.Parse(txtTwnMi.Text);
                    asset.qc_flag = int.Parse(txtQcFlag.Text);
                    asset.min_twn_fm = float.Parse(txtMinTwnFm.Text);
                    asset.max_twn_tm = float.Parse(txtMaxTwnTm.Text);
                    asset.sr_sid = txtSrSid.Text;
                    asset.sign_height = int.Parse(txtSignHeight.Text);
                    asset.sign_width = int.Parse(txtSignWidth.Text);



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Incorrect format in data fields.");
                }


                //Update Data in Database
                bool success = asset.Update(asset);
                if (success)
                {
                    //Load Data in Data Grid View
                    DataTable dt = rfid.Select(rfid);
                    dgTagResults.DataContext = dt;
                    //Update was successful
                    MessageBox.Show("Asset has been updated successfully in Database");

                }
                else
                {
                    //Update failed
                    MessageBox.Show("Failed to update Asset.");

                }
            }
        }
        /// <summary>
        /// Clear the textboxes in Asset Data Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAssetClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAsset();
        }

        private void BtnAssetDelete_Click(object sender, RoutedEventArgs e)
        {

            if (chkBoxREST.IsChecked == true)
            {
                if (DeleteAssetREST())
                {
                    MessageBox.Show("Asset Deleted Successfully with REST API");
                }

                else
                {
                    MessageBox.Show("ERROR: REST API Error");
                }
            }

            else
            {


                try
                {
                    //Retrieve data from Fields
                    asset.id = int.Parse(txtAssetIDAsset.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Data fields not in correct format.");

                }
                bool success = asset.Delete(asset);
                if (success)
                {

                    //Load Data in Data Grid View
                    //DataTable dt = asset.Select(asset);
                    //dgTagResults.DataContext = dt; //Uncomment if need to display Asset seperately from RFID, Currently Joined
                    //Successful Deletion
                    MessageBox.Show("Tag has been deleted successfully");
                    ClearRFID();
                    ClearAsset();


                }
                else
                {
                    //Failed Deletion
                    MessageBox.Show("Failed deletion. Try again.");

                }
            }
        }

        private bool GetRequestVTransREST()
        {
            try
            {
                HttpWebRequest requestTag = (HttpWebRequest)WebRequest.Create(
                "https://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/2/" +
                "query?where=epc='" + txtCurrentEpc.Text +
                "'&time=&geometry=&geometryType=esriGeometryEnvelope" +
                "&inSR=&spatialRel=esriSpatialRelIntersects&distance=" +
                "&units=esriSRUnit" +
                "_Foot&relationParam=&outFields=*" +
                "&returnGeometry" +
                "=true&maxAllowableOffset=&geometryPrecision=&outSR=&gdbVersion=&historicMoment" +
                "=&returnDistinctValues=false&returnIdsOnly=false&returnCountOnly=false&returnE" +
                "xtentOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&retur" +
                "nZ=false&returnM=false&multipatchOption=&resultOffset=&resultRecordCount=&retur" +
                "nTrueCurves=false&sqlFormat=none&f=json");

                requestTag.Method = "Get";
                requestTag.ContentType = "application/json";

                HttpWebResponse responseTag = (HttpWebResponse)requestTag.GetResponse();



                string responseStrTag = "";

                using (System.IO.StreamReader sr = new System.IO.StreamReader(responseTag.GetResponseStream()))
                {
                    responseStrTag = sr.ReadToEnd();

                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                dynamic responseDictTag = serializer.Deserialize<dynamic>(responseStrTag);

                dynamic attributeDictTag = responseDictTag["features"][0]["attributes"];

                //rfid table data
                txtboxRFIDObjectID.Text = attributeDictTag["OBJECTID"].ToString();
                txtRFIDDatabaseID.Text = attributeDictTag["id"].ToString(); //rfid.id
                txtRFIDManufactureDate.Text = attributeDictTag["manufacture_date"].ToString();//rfid.manufacture_date
                txtRFIDInstallationDate.Text = attributeDictTag["installation_date"].ToString();//rfid.installation_date
                txtAssetID.Text = attributeDictTag["asset_id"].ToString();//rfid.asset_id
                txtRFIDComments.Text = attributeDictTag["comments"].ToString();//rfid.comments
                
                ///<summary>
                ///If there is an asset associated with a tag, display information from Asset table as well.
                ///</summary>
                if(txtAssetID.Text != "null")
                {
                    HttpWebRequest requestSign = (HttpWebRequest)WebRequest.Create(
                    "https://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/1/" +
                    "query?where=OBJECTID=" + int.Parse(txtAssetID.Text) +
                    "&time=&geometry=&geometryType=esriGeometryEnvelope" +
                    "&inSR=&spatialRel=esriSpatialRelIntersects&distance=" +
                    "&units=esriSRUnit" +
                    "_Foot&relationParam=&outFields=*" +
                    "&returnGeometry" +
                    "=true&maxAllowableOffset=&geometryPrecision=&outSR=&gdbVersion=&historicMoment" +
                    "=&returnDistinctValues=false&returnIdsOnly=false&returnCountOnly=false&returnE" +
                    "xtentOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&retur" +
                    "nZ=false&returnM=false&multipatchOption=&resultOffset=&resultRecordCount=&retur" +
                    "nTrueCurves=false&sqlFormat=none&f=json");

                    requestSign.Method = "Get";
                    requestSign.ContentType = "application/json";

                    HttpWebResponse responseSign = (HttpWebResponse)requestSign.GetResponse();



                    string responseStrSign = "";

                    using (System.IO.StreamReader sr = new System.IO.StreamReader(responseSign.GetResponseStream()))
                    {
                        responseStrSign = sr.ReadToEnd();

                    }

                    dynamic responseDictSign = serializer.Deserialize<dynamic>(responseStrSign);

                    dynamic attributeDictSign = responseDictSign["features"][0]["attributes"];


                    //Populate Asset text fields appropriately
                    txtAssetIDAsset.Text = attributeDictSign["OBJECTID"]?.ToString() ?? "null";
                    txtLaneDirection.Text = attributeDictSign["LaneDirection"]?.ToString() ?? "null";
                    txtPositionCode.Text = attributeDictSign["PositionCode"]?.ToString() ?? "null";
                    txtRouteSuffix.Text = attributeDictSign["RouteSuffix"]?.ToString() ?? "null";
                    txtMarker.Text = attributeDictSign["Marker"]?.ToString() ?? "null";
                    txtCity.Text = attributeDictSign["City"]?.ToString() ?? "null";
                    txtCounty.Text = attributeDictSign["County"]?.ToString() ?? "null";
                    txtDistrict.Text = attributeDictSign["District"]?.ToString() ?? "null";
                    txtStreetName.Text = attributeDictSign["STREETNAME"]?.ToString() ?? "null";
                    txtMutcdCode.Text = attributeDictSign["MUTCDCode"]?.ToString() ?? "null";
                    txtRetired.Text = attributeDictSign["Retired"]?.ToString() ?? "null";
                    txtReplaced.Text = attributeDictSign["Replaced"]?.ToString() ?? "null";
                    txtSignAge.Text = attributeDictSign["SignAge"]?.ToString() ?? "null";
                    txtTwnTid.Text = attributeDictSign["TWN_TID"]?.ToString() ?? "null";
                    txtTwnMi.Text = attributeDictSign["TWN_MI"]?.ToString() ?? "null";
                    txtQcFlag.Text = attributeDictSign["QCFLAG"]?.ToString() ?? "null";
                    txtMinTwnFm.Text = attributeDictSign["MIN_TWN_FMI"]?.ToString() ?? "null";
                    txtMaxTwnTm.Text = attributeDictSign["MAX_TWN_TMI"]?.ToString() ?? "null";
                    txtSrSid.Text = attributeDictSign["SR_SID"]?.ToString() ?? "null";
                    txtSignHeight.Text = attributeDictSign["SignHeight"]?.ToString() ?? "null";
                    txtSignWidth.Text = attributeDictSign["SignWidth"]?.ToString() ?? "null";

                }

                return true;
            }
            ///<summary>
            /// If request does not return valid dictionary return false, tag does not exist
            ///</summary>
            catch(Exception ex)
            {

                return false;

            }
            

        }

        private void ChkBoxREST_Checked(object sender, RoutedEventArgs e)
        {
            chkBoxREST.IsChecked = true;
            chkBoxOFFLINE.IsChecked = false;

        }

        private bool UpdateSignTableREST()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/1/applyEdits");

            var postData = "adds=" + Uri.EscapeDataString("");
            postData += "&updates=" + Uri.EscapeDataString(" [{\"attributes\":{" +
                                        "\"OBJECTID\":"+ txtAssetIDAsset.Text +"," +
                                        "\"LaneDirection\":\""+ txtLaneDirection.Text +"\"," +
                                        "\"PositionCode\":\"" + txtPositionCode.Text + "\"," +
                                        "\"RouteSuffix\":" + txtRouteSuffix.Text + "," +
                                        "\"Marker\":"+ txtMarker.Text + "," +
                                        "\"City\":\""+ txtCity.Text +"\"," +
                                        "\"County\":\""+txtCounty.Text+"\"," +
                                        "\"District\":\""+txtDistrict.Text+"\"," +
                                        "\"STREETNAME\":\""+txtStreetName.Text+"\"," +
                                        "\"MUTCDCode\":\""+ txtMutcdCode.Text +"\"," +
                                        "\"Retired\":"+txtRetired.Text+"," +
                                        "\"Replaced\":"+txtReplaced.Text+"," +
                                        "\"SignAge\":"+txtSignAge.Text+"," +
                                        "\"TWN_TID\":\""+txtTwnTid.Text+"\"," +
                                        "\"TWN_MI\":"+ float.Parse(txtTwnMi.Text)+" ," +
                                        "\"QCFLAG\":"+txtQcFlag.Text+" ," +
                                        "\"MIN_TWN_FMI\":"+txtMinTwnFm.Text+" ," +
                                        "\"MAX_TWN_FMI\":"+txtMaxTwnTm.Text+" ," +
                                        "\"SR_SID\": \""+txtSrSid.Text+" \"," +
                                        "\"SignHeight\":"+txtSignHeight.Text+"," +
                                        "\"SignWidth\":"+txtSignWidth.Text+"}}]");

            postData += "&deletes=" + Uri.EscapeDataString("");
            postData += "&gdbVersion=" + Uri.EscapeDataString("");
            postData += "&rollbackOnFailure=" + Uri.EscapeDataString("true");
            postData += "&useGlobalIds=" + Uri.EscapeDataString("false");
            postData += "&returnEditMoment=" + Uri.EscapeDataString("false");
            postData += "&trueCurveClient=" + Uri.EscapeDataString("true");
            postData += "&attachments=" + Uri.EscapeDataString("");
            postData += "&f=" + Uri.EscapeDataString("json");


            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic responseDict = serializer.Deserialize<dynamic>(responseString);

            try
            {
                if (responseDict["updateResults"][0]["success"] == true)
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
                MessageBox.Show("POST Failed");
            }

            return false;

            
        }

        private bool UpdateTagTableREST()
        {

            var request = (HttpWebRequest)WebRequest.Create("http://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/2/applyEdits");

            var postData = "adds=" + Uri.EscapeDataString("");
            postData += "&updates=" + Uri.EscapeDataString(" [{\"attributes\":{" +
                                        "\"OBJECTID\": "+txtboxRFIDObjectID.Text+"," +
                                        "\"id\":\"" + txtRFIDDatabaseID.Text + "\"," +
                                        "\"epc\":\"" + txtCurrentEpc.Text + "\"," +
                                        "\"manufacture_date\":" + txtRFIDManufactureDate.Text + "," +
                                        "\"installation_date\":" + txtRFIDInstallationDate.Text + "," +
                                        "\"asset_id\":\"" + txtAssetID.Text + "\"," +
                                        "\"comments\":\"" + txtCounty.Text + "\"," +
                                         "}}]");

            postData += "&deletes=" + Uri.EscapeDataString("");
            postData += "&gdbVersion=" + Uri.EscapeDataString("");
            postData += "&rollbackOnFailure=" + Uri.EscapeDataString("true");
            postData += "&useGlobalIds=" + Uri.EscapeDataString("false");
            postData += "&returnEditMoment=" + Uri.EscapeDataString("false");
            postData += "&trueCurveClient=" + Uri.EscapeDataString("true");
            postData += "&attachments=" + Uri.EscapeDataString("");
            postData += "&f=" + Uri.EscapeDataString("json");


            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic responseDict = serializer.Deserialize<dynamic>(responseString);

            try
            {
                if (responseDict["updateResults"][0]["success"] == true)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                MessageBox.Show("POST Failed");
            }

            return false;
        }



        private bool DeleteAssetREST()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/1/applyEdits");

            var postData = "adds=" + Uri.EscapeDataString("");
            postData += "&deletes=" + Uri.EscapeDataString("[" + txtAssetIDAsset.Text + "]");
            postData += "&updates=" + Uri.EscapeDataString("");
            postData += "&gdbVersion=" + Uri.EscapeDataString("");
            postData += "&rollbackOnFailure=" + Uri.EscapeDataString("true");
            postData += "&useGlobalIds=" + Uri.EscapeDataString("false");
            postData += "&returnEditMoment=" + Uri.EscapeDataString("false");
            postData += "&trueCurveClient=" + Uri.EscapeDataString("true");
            postData += "&attachments=" + Uri.EscapeDataString("");
            postData += "&f=" + Uri.EscapeDataString("json");


            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic responseDict = serializer.Deserialize<dynamic>(responseString);

            try
            {
                if (responseDict["deleteResults"][0]["success"] == true)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                MessageBox.Show("POST Failed");
            }

            return false;
        }

        private bool DeleteTagREST()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/2/applyEdits");

            var postData = "adds=" + Uri.EscapeDataString("");
            postData += "&deletes=" + Uri.EscapeDataString("[" + txtboxRFIDObjectID.Text + "]");
            postData += "&updates=" + Uri.EscapeDataString("");
            postData += "&gdbVersion=" + Uri.EscapeDataString("");
            postData += "&rollbackOnFailure=" + Uri.EscapeDataString("true");
            postData += "&useGlobalIds=" + Uri.EscapeDataString("false");
            postData += "&returnEditMoment=" + Uri.EscapeDataString("false");
            postData += "&trueCurveClient=" + Uri.EscapeDataString("true");
            postData += "&attachments=" + Uri.EscapeDataString("");
            postData += "&f=" + Uri.EscapeDataString("json");


            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic responseDict = serializer.Deserialize<dynamic>(responseString);

            try
            {
                if (responseDict["deleteResults"][0]["success"] == true)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                MessageBox.Show("POST Failed");
            }

            return false;
        }


        private bool InsertSignREST()
        {

            var request = (HttpWebRequest)WebRequest.Create("http://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/1/applyEdits");

            var postData = "updates=" + Uri.EscapeDataString("");
            postData += "&adds=" + Uri.EscapeDataString("[{\"attributes\":{" +
                                      "\"SignMainGeneralOID\":\"" + "50000" + "\"," +
                                      "\"LaneDirection\":\"" + "NB" + "\"," +
                                      "\"PositionCode\":\"" + "M" + "\"," +
                                      "\"RouteSuffix\":" + "null" + "," +
                                      "\"Marker\":" + "1.53" + "," +
                                      "\"City\":\"" + "SO BURLINGTON, 0414" + "\"," +
                                      "\"County\":\"" + "04, CHITTENDEN" + "\"," +
                                      "\"District\":\"" + "5" + "\"," +
                                      "\"STREETNAME\":\"" + "0070, US7" + "\"," +
                                      "\"MUTCDCode\":\"" + "R10-10L" + "\"," +
                                      "\"Retired\":" + "null" + "," +
                                      "\"Replaced\":" + "null" + "," +
                                      "\"SignAge\":" + "null" + "," +
                                      "\"TWN_TID\":\"" + "U007-0414" + "\"," +
                                      "\"TWN_MI\":" + "1.53" + " ," +
                                      "\"QCFLAG\":" + "null" + " ," +
                                      "\"MIN_TWN_FMI\":" + "0" + " ," +
                                      "\"MAX_TWN_FMI\":" + "1.738" + " ," +
                                      "\"SR_SID\": \"" + "US-7" + " \"," +
                                      "\"SignHeight\":" + "30" + "," +
                                      "\"SignWidth\":" + "24" + "}}]");

            postData += "&deletes=" + Uri.EscapeDataString("");
            postData += "&gdbVersion=" + Uri.EscapeDataString("");
            postData += "&rollbackOnFailure=" + Uri.EscapeDataString("true");
            postData += "&useGlobalIds=" + Uri.EscapeDataString("false");
            postData += "&returnEditMoment=" + Uri.EscapeDataString("false");
            postData += "&trueCurveClient=" + Uri.EscapeDataString("true");
            postData += "&attachments=" + Uri.EscapeDataString("");
            postData += "&f=" + Uri.EscapeDataString("json");


            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic responseDict = serializer.Deserialize<dynamic>(responseString);
            Console.WriteLine(responseString);
            try
            {
                if (responseDict["addResults"][0]["success"] == true)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                MessageBox.Show("POST Failed");
            }

            return false;

        }

        private bool InsertTagREST()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/2/applyEdits");

            var postData = "updates=" + Uri.EscapeDataString("");
            postData += "&adds=" + Uri.EscapeDataString(" [{\"attributes\":{" +
                                        "\"id\":\"" + txtRFIDDatabaseID.Text + "\"," +
                                        "\"epc\":\"" + txtCurrentEpc.Text + "\"," +
                                        "\"manufacture_date\":" + txtRFIDManufactureDate.Text + "," +
                                        "\"installation_date\":" + txtRFIDInstallationDate.Text + "," +
                                        "\"asset_id\":\"" + txtAssetID.Text + "\"," +
                                        "\"comments\":\"" + txtCounty.Text + "\"," +
                                         "}}]");

            postData += "&deletes=" + Uri.EscapeDataString("");
            postData += "&gdbVersion=" + Uri.EscapeDataString("");
            postData += "&rollbackOnFailure=" + Uri.EscapeDataString("true");
            postData += "&useGlobalIds=" + Uri.EscapeDataString("false");
            postData += "&returnEditMoment=" + Uri.EscapeDataString("false");
            postData += "&trueCurveClient=" + Uri.EscapeDataString("true");
            postData += "&attachments=" + Uri.EscapeDataString("");
            postData += "&f=" + Uri.EscapeDataString("json");


            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic responseDict = serializer.Deserialize<dynamic>(responseString);

            try
            {
                if (responseDict["addResults"][0]["success"] == true)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                MessageBox.Show("POST Failed");
            }

            return false;
        }


        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        bool CreateReplica()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://maps.vtrans.vermont.gov/arcgis/rest/services/AMP/Asset_Signs_RFID/FeatureServer/createReplica");

            var postData = "replicaName=" + Uri.EscapeDataString("cs_replica");
            postData += "&layers=" + Uri.EscapeDataString("1,2");

            postData += "&layerQueries=" + Uri.EscapeDataString(" {\"1\":{\"queryOption\": \"all\"}," +
                                                                 " \"2\":{ \"queryOption\": \"all\"}} ");

            postData += "&geometry=" + Uri.EscapeDataString("{\"xmin\" : 443285.3024000004, " +
                                                            " \"ymin\" : 212412.71839999966, " +
                                                             " \"xmax\" : 449453.62770000007, " +
                                                              " \"ymax\" : 220714.47320000082, " +
                                                               " \"spatialReference\" : { \"wkid\" : 32145} }");

            postData += "&geometryType=" + Uri.EscapeDataString("esriGeometryEnvelope");
            postData += "&inSR=" + Uri.EscapeDataString("");
            postData += "&replicaSR=" + Uri.EscapeDataString("");
            postData += "&transportType=" + Uri.EscapeDataString("esriTransportTypeUrl");
            postData += "&returnAttachments=" + Uri.EscapeDataString("false");
            postData += "&returnAttachmentsDataByUrl=" + Uri.EscapeDataString("false");
            postData += "&attachmentsSyncDirection=" + Uri.EscapeDataString("none");
            postData += "&async=" + Uri.EscapeDataString("false");
            postData += "&syncModel=" + Uri.EscapeDataString("perReplica");
            postData += "&dataFormat=" + Uri.EscapeDataString("json");
            postData += "&replicaOptions=" + Uri.EscapeDataString("");
            postData += "&targetType=" + Uri.EscapeDataString("client");
            postData += "&syncDirection=" + Uri.EscapeDataString("");
            postData += "&f=" + Uri.EscapeDataString("json");


            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            string jsonReplicaUrl = "";

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            dynamic responseDict = serializer.Deserialize<dynamic>(responseString);

            foreach (KeyValuePair<string, object> kvp in responseDict)
            {
                //Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                if (kvp.Key == "URL")
                {
                    jsonReplicaUrl = kvp.Value.ToString(); //THIS is the URL of JSON replica
                }
            }
            Console.WriteLine(jsonReplicaUrl);

            try
            {

                if (responseDict["transportType"] == "esriTransportTypeUrl")

                {
                    Console.WriteLine("SUCCESS REPLICA");
                    GetJsonReplicaFromURL(jsonReplicaUrl);
                    return true;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("FAILED REPLICA");
                return false;
            }

            return false;
        }

        bool GetJsonReplicaFromURL(string jsonReplicaUrl)
        {
            try
            {


                HttpWebRequest replicaJsonSync = (HttpWebRequest)WebRequest.Create(jsonReplicaUrl);

                replicaJsonSync.Method = "Get";
                replicaJsonSync.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)replicaJsonSync.GetResponse();



                string responseStr = "";

                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseStr = sr.ReadToEnd();

                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                dynamic responseDict = serializer.Deserialize<dynamic>(responseStr);


                foreach (KeyValuePair<string, object> kvp in responseDict)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                   
                }

                string json = JsonConvert.SerializeObject(responseStr.ToArray(), Formatting.Indented);

                System.IO.File.WriteAllText("replica.json", responseStr);

                //dynamic attributeDictTag = responseDictTag["layers"][0]["attributes"];
                return true;
            }
            catch(Exception e)
            {
                return false;

            }
        }

        private void ChkBoxOFFLINE_Checked(object sender, RoutedEventArgs e)
        {
            
            chkBoxREST.IsChecked = false;
            chkBoxOFFLINE.IsChecked = true;
            
            
      

        }
    }
}
