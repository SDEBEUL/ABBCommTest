using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Controllers.EventLogDomain;
using ABB.Robotics.Controllers.ConfigurationDomain;
using ABB.Robotics.Controllers.FileSystemDomain;
using ABB.Robotics.Controllers.IOSystemDomain;

using System.Xml.Linq;
using System.Data.SqlClient;

namespace ABBCommTest
{
    public class debugger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Exeption(Exception ex)
        {
            log.Error(ex);
        }

        public void Message(string message)
        {
            log.Info(message);
            MessageBox.Show(message, "Sorry", MessageBoxButtons.OK);
        }

        public void Log(string message)
        {
            log.Debug(message);
        }
    }

    public partial class Form1 : Form
    {
        private NetworkScanner scanner = new NetworkScanner();
        private Controller controller = null;
        private DataTable dt_robots = new DataTable();
        private debugger debugger;

        public Form1()
        {
            InitializeComponent();
        }

        private void LinkControllertoList(ControllerInfo controllerInfo)
        {
            foreach (DataRow row in dt_robots.Rows)
            {
                if (row.Field<string>("IP") == controllerInfo.IPAddress.ToString())
                {
                    row["SystemId"] = controllerInfo.SystemId;
               //     row["Availability"] = controllerInfo.Availability.ToString();
               //     row["IsVirtual"] = controllerInfo.IsVirtual.ToString();
               //     row["SystemName"] = controllerInfo.SystemName;
               //     row["Version"] = controllerInfo.Version.ToString();
                    row["ControllerName"] = controllerInfo.ControllerName;
                }
            }
        }

        //expose a robot by IP to the networkscanner
        private void AddRobotByIp(string Ip)
        {
            try
            {
                System.Net.IPAddress ipAddress;
                try
                {
                    ipAddress = System.Net.IPAddress.Parse(Ip);
                    
                    NetworkScanner.AddRemoteController(ipAddress);
                }
                catch (FormatException ex)
                {
                    debugger.Exeption(ex);
                }

                this.scanner.Scan();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //add controllers that are exposed to the scanner
        private void HandleScanner()
        {
            scanner.Scan();
            ControllerInfoCollection controllers = scanner.Controllers;
            //populate the listview
            foreach (ControllerInfo controllerInfo in controllers)
            {
                LinkControllertoList(controllerInfo);
            }
            debugger.Message("Done with scan");
        }


        /*
    PROC InitUser()
    !=================================================
    ! Description : Initiation of equipment
    ! Info        : 
    ! Called from : LMainEvent / Init
    !=================================================
    bUseSocket:=FALSE;  ! Switch on or off the use of Socket communication
    bBodyIDActive:=FALSE; ! Switch on or off the use of Production BodyID
    nMaxNoOffAutoReDress := 1; !Allow robot to redress once (from HDRESS) SDEBEUL WO10854044
    InitTool ToolOnRobot();
    SendAlarm A_ClearAlarms,""\Reset;
  ENDPROC
         * */

        //add redress config in autorun. to robot (PFM) ONLY SE ! 
        private void PFMConfigureRobot(ControllerInfo ci, DataGridViewRow row)
        {
            string tempdir = @"c:\temp\debug\";
            string FilePathOnControler = @"/hd0a/TEMP/";

            try
            {
                if (ci.Availability != Availability.Available) { debugger.Log("controller busy: " + ci.Id); return; } //stop if controller is not available
                //
                controller = ControllerFactory.CreateFrom(ci); //get controller from factory
                if (controller.OperatingMode != ControllerOperatingMode.Auto) //controller must be on auto to take master 
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "NOK";
                    return;
                }
                else
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "OK";

                    //need to check if we have the Hdress var 
                    RapidData rd;
                    try {
                        rd = controller.Rapid.GetRapidData("T_ROB1", "HDress", "nMaxNoOffAutoReDress");
                        debugger.Message("nMaxNoOffAutoReDress found continue");
                        row.Cells[dataGridView1.Columns["HasRedress"].Index].Value = "OK";
                    }
                    catch (Exception ex)
                    {
                        //var not found exit
                        debugger.Message("nMaxNoOffAutoReDress not found EXIT");
                        row.Cells[dataGridView1.Columns["HasRedress"].Index].Value = "NOK";
                        return;
                    }

                    // get modules from controller task trob1
                    ABB.Robotics.Controllers.RapidDomain.Task tRob1 = controller.Rapid.GetTask("T_ROB1");
                    Module[] mx = tRob1.GetModules();
                    //find the one we need 
                    foreach (Module m in mx)
                    {
                        if (m.Name.StartsWith("LR", StringComparison.InvariantCulture))
                        {
                            Routine proc = m.GetRoutine("InitUser");
                            if (proc != null) //check if we have the right module
                            {
                                //find the module on the controller and get it **************************************
                                //save it on the controller
                                m.SaveToFile(FilePathOnControler);
                                System.Threading.Thread.Sleep(1000);
                                //get file from controler to pc 
                                FileSystem cntrlFileSystem;
                                cntrlFileSystem = controller.FileSystem;
                                controller.FileSystem.RemoteDirectory = FilePathOnControler;
                                controller.FileSystem.LocalDirectory = tempdir;
                                //move file to pc
                                controller.FileSystem.GetFile(m.Name + ".mod", m.Name + ".mod", true);
                                //process the file******************************************************************
                                string lrModule = File.ReadAllText(tempdir + m.Name + ".mod");
                                if (lrModule.Contains("nMaxNoOffAutoReDress"))
                                {
                                    debugger.Log("File already changed do nothing");
                                    return;
                                }
                  

                                lrModule = lrModule.Replace(@"Switch on or off the use of Production BodyID", @"Switch on or off the use of Production BodyID
    nMaxNoOffAutoReDress := 1; !Allow robot to redress once (from HDRESS) SDEBEUL WO10854044");
                                File.WriteAllText(tempdir + m.Name + ".mod", lrModule);
                                //put the file back*****************************************************************
                                try
                                {
                                    //get controller mastership
                                    using (Mastership master = Mastership.Request(controller))
                                    {

                                        //put the file back on the controller
                                        controller.FileSystem.PutFile(m.Name + ".mod", m.Name + ".mod", true);


                                        //check if controller if home for load.
                                        Signal O_Homepos = controller.IOSystem.GetSignal("O_Homepos");
                                        if (O_Homepos.Value == 1)
                                        {
                                            tRob1.Stop(StopMode.Immediate);
                                            System.Threading.Thread.Sleep(1000);
                                            tRob1.LoadModuleFromFile(FilePathOnControler + m.Name + ".mod", RapidLoadMode.Replace);
                                            row.Cells[dataGridView1.Columns["LoadOK"].Index].Value = "OK";
                                        }
                                        else
                                        {
                                            row.Cells[dataGridView1.Columns["LoadOK"].Index].Value = "NOK";
                                        }

                                        //release master
                                        master.Release();
                                    }
                                }
                                catch (System.InvalidOperationException ex)
                                {
                                    debugger.Exeption(ex);
                                    return;
                                }
                            }

                        }

                    }
                }
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "OK";
            }
            catch (Exception ex)
            {
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "NOK";
                debugger.Exeption(ex);
                return;
            }
        }

        //load new version of a module.
        private void LoadNewLrobotRobot(ControllerInfo ci, DataGridViewRow row, string workfolder,string modulename,string refVar, string refVarValueContains)
        {
            //   string workfolder = @"c:\temp\debug\";
            string FilePathOnControler = @"/hd0a/TEMP/";
           // string modulename = "LRobot.sys";
           // string refVar = "Version_LRobot";
           // string refVarValueContains = "ABB 6V99 - 2017-11-02";

            try
            {
                if (ci.Availability != Availability.Available) { debugger.Log("controller busy: " + ci.Id); return; } //stop if controller is not available
                //
                controller = ControllerFactory.CreateFrom(ci); //get controller from factory
                if (controller.OperatingMode != ControllerOperatingMode.Auto) //controller must be on auto to take master 
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "NOK";
                    return;
                }
                else
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "OK";

                    RapidData rd = null;
                    try
                    {
                        //check current version 
                         rd = controller.Rapid.GetRapidData("T_ROB1", modulename.Split('.')[0], refVar);
                         row.Cells[dataGridView1.Columns["Version"].Index].Value = rd.Value.ToString();
                    }
                    catch 
                    {
                        row.Cells[dataGridView1.Columns["VersionOK"].Index].Value = "NOT FOUND";
                        return;
                    }

                   // return; // break for testing .

                    if (rd.Value.ToString().Contains(refVarValueContains))
                    {
                        debugger.Log("rapid value: " + rd.Value.ToString());
                        debugger.Log("ref value: " + refVarValueContains);
                        row.Cells[dataGridView1.Columns["VersionOK"].Index].Value = "CanLoad";
                        //put the file on the controller*****************************************************************
                        FileSystem cntrlFileSystem;
                        cntrlFileSystem = controller.FileSystem;
                        controller.FileSystem.RemoteDirectory = FilePathOnControler;
                        controller.FileSystem.LocalDirectory = workfolder;

                        try
                        {

                            //get controller mastership
                            using (Mastership master = Mastership.Request(controller))
                            {
                                //check if controller if home for load.
                                Signal O_Homepos = controller.IOSystem.GetSignal("O_Homepos");
                                if (O_Homepos.Value == 1)
                                {
                                    row.Cells[dataGridView1.Columns["RobotHome"].Index].Value = "OK";
                                    //put the file  on the controller
                                    controller.FileSystem.PutFile(modulename, modulename, true);
                                    // get modules from controller task trob1
                                    ABB.Robotics.Controllers.RapidDomain.Task tRob1 = controller.Rapid.GetTask("T_ROB1");
                                    //stop trob 1 if running.
                                    bool bWasRunning = false;
                                    if (tRob1.ExecutionStatus == TaskExecutionStatus.Running)
                                    {
                                        bWasRunning = true;
                                        tRob1.Stop(StopMode.Immediate);
                                        System.Threading.Thread.Sleep(1000);

                                    }

                                    tRob1.LoadModuleFromFile(FilePathOnControler + modulename, RapidLoadMode.Replace);

                                    //restart if was running
                                    if (bWasRunning)
                                    {
                                        tRob1.ResetProgramPointer();
                                        tRob1.Start();
                                    }
                                }
                                else // robot not home
                                {
                                    row.Cells[dataGridView1.Columns["RobotHome"].Index].Value = "NOK";
                                }

                                //release master
                                master.Release();
                            }
                        }
                        catch (System.InvalidOperationException ex)
                        {
                            debugger.Exeption(ex);
                            debugger.Message("error in write to controller");
                            return;
                        }
                        
                    }
                    else
                    {
                        row.Cells[dataGridView1.Columns["VersionOK"].Index].Value = "NoLoadAllowed";
                    }

                    }
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "OK";
            }
            catch (Exception ex)
            {
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "NOK";
                debugger.Exeption(ex);
                debugger.Message("Error connecting to controller");
                return;
            }
            
        }

        //change tipdress parameter.
        private void ChangeDressParm(ControllerInfo ci, DataGridViewRow row)
        {

            try
            {
                if (ci.Availability != Availability.Available) { debugger.Message("controller busy: " + ci.Id); return; } //stop if controller is not available
                //
                controller = ControllerFactory.CreateFrom(ci); //get controller from factory
                if (controller.OperatingMode != ControllerOperatingMode.Auto) //controller must be on auto to take master 
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "NOK";
                    return;
                }
                else
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "OK";

                    RapidSymbolSearchProperties sProp = RapidSymbolSearchProperties.CreateDefault();
                    sProp.Recursive = true;
                    sProp.Types = SymbolTypes.Constant | SymbolTypes.Persistent;
                    sProp.SearchMethod = SymbolSearchMethod.Block;

                    ABB.Robotics.Controllers.RapidDomain.Task tRob1 = controller.Rapid.GetTask("T_ROB1");
                    RapidSymbol[] rsCol = tRob1.SearchRapidSymbol(sProp, "", string.Empty);

                    RapidDataType theDataType;

                    foreach (RapidSymbol rs in rsCol)
                    {
                        theDataType = RapidDataType.GetDataType(rs);

                        if (theDataType.Name.StartsWith("Tipdressdata") && rs.Scope[1].EndsWith("UserData")) //type tipdress data in a module that ends with user data
                        {
                        RapidData rd = controller.Rapid.GetRapidData(rs.Scope[0], rs.Scope[1], rs.Scope[2]);

                        RapidDataType rdt = controller.Rapid.GetRapidDataType(rs.Scope[0], rs.Scope[1], rs.Scope[2]);
                        UserDefined dressdata = new UserDefined(rdt);

                        dressdata = (UserDefined)rd.Value;
                        string totalcutter = dressdata.Components[13].ToString();

                        Console.WriteLine("{0}; {1}; {2}", controller.Name, rs.Scope[2], totalcutter);
                            //change it 
                            try
                            {
                                //get controller mastership
                                using (Mastership master = Mastership.Request(controller))
                                {
                                    dressdata.Components[13].FillFromString("True"); // set TRUE
                                    rd.Value = dressdata;
                                    row.Cells[dataGridView1.Columns["WriteOk"].Index].Value = "OK";
                                    //release master
                                    master.Release();
                                }
                            }
                            catch (System.InvalidOperationException ex)
                            {
                                debugger.Exeption(ex);
                                debugger.Message("error in write to controller");
                                return;
                            }
                        }
                    }

                }
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "OK";
            }
            catch (Exception ex)
            {
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "NOK";
                debugger.Exeption(ex);
                debugger.Message("Error connecting to controller");
                return;
            }

        }

        //change tipwearRatioInterval parameter.
        private void ChangeTipwearRatio(ControllerInfo ci, DataGridViewRow row)
        {

            try
            {
                if (ci.Availability != Availability.Available) { debugger.Message("controller busy: " + ci.Id); return; } //stop if controller is not available
                //
                controller = ControllerFactory.CreateFrom(ci); //get controller from factory
                if (controller.OperatingMode != ControllerOperatingMode.Auto) //controller must be on auto to take master 
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "NOK";
                    return;
                }
                else
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "OK";

                    RapidSymbolSearchProperties sProp = RapidSymbolSearchProperties.CreateDefault();
                    sProp.Recursive = true;
                    sProp.Types = SymbolTypes.Constant | SymbolTypes.Persistent;
                    sProp.SearchMethod = SymbolSearchMethod.Block;

                    ABB.Robotics.Controllers.RapidDomain.Task tRob1 = controller.Rapid.GetTask("T_ROB1");
                    RapidSymbol[] rsCol = tRob1.SearchRapidSymbol(sProp, "", string.Empty);

                    RapidDataType theDataType;

                    foreach (RapidSymbol rs in rsCol)
                    {
                        theDataType = RapidDataType.GetDataType(rs);

                        if (theDataType.Name.StartsWith("TipMeasdata") && rs.Scope[1].EndsWith("UserData")) 
                        {
                            RapidData rd = controller.Rapid.GetRapidData(rs.Scope[0], rs.Scope[1], rs.Scope[2]);

                            RapidDataType rdt = controller.Rapid.GetRapidDataType(rs.Scope[0], rs.Scope[1], rs.Scope[2]);
                            UserDefined TipMeasdata = new UserDefined(rdt);

                            TipMeasdata = (UserDefined)rd.Value;
                            string TipwearRatioInterval = TipMeasdata.Components[1].ToString(); //take 2nd element of data
                            Console.WriteLine("FOUND:{0}; {1}; {2}", controller.Name, rs.Scope[2], TipMeasdata);
                            debugger.Log(string.Format("FOUND:{0}; {1}; {2}", controller.Name, rs.Scope[2], TipMeasdata));
                            if (TipwearRatioInterval == "1")
                            {         
                                //change it 
                                try
                                {
                                
                                    //get controller mastership
                                    using (Mastership master = Mastership.Request(controller))
                                    {
                                        TipMeasdata.Components[1].FillFromString("2"); // set new value to 2
                                        rd.Value = TipMeasdata;
                                        //new value 
                                        string TipwearRatioIntervalNEW = TipMeasdata.Components[1].ToString();
                                        Console.WriteLine("SETVALUE:{0}; {1}; {2}", controller.Name, rs.Scope[2], TipwearRatioIntervalNEW);
                                        debugger.Log(string.Format("SETVALUE:{0}; {1}; {2}", controller.Name, rs.Scope[2], TipwearRatioIntervalNEW));
                                        row.Cells[dataGridView1.Columns["WriteOk"].Index].Value = "OK";
                                        //release master
                                        master.Release();
                                    }
                                
                                }
                                catch (System.InvalidOperationException ex)
                                {
                                    debugger.Log("Error in write to controller");
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Data not as expected did nothing");
                                debugger.Log("Data not as expected did nothing");
                            }
                        }
                    }

                }
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "OK";
            }
            catch (Exception ex)
            {
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "NOK";
                debugger.Exeption(ex);
                debugger.Message("Error connecting to controller");
                return;
            }

        }

        //change tipwearRatioInterval parameter.
        private void ChangeValue(ControllerInfo ci, DataGridViewRow row,string Modulename, string Varname, double newValue)
        {
            try
            {
                if (ci.Availability != Availability.Available) { debugger.Message("controller busy: " + ci.Id); return; } //stop if controller is not available
                //
                controller = ControllerFactory.CreateFrom(ci); //get controller from factory
                if (controller.OperatingMode != ControllerOperatingMode.Auto) //controller must be on auto to take master 
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "NOK";
                    return;
                }
                else
                {
                    row.Cells[dataGridView1.Columns["AutoOK"].Index].Value = "OK";
                    try
                    {
                        RapidData rd = controller.Rapid.GetRapidData("T_ROB1", Modulename, Varname);
                        debugger.Log($"{ci.ControllerName}; {Varname}  FOUND");
                        //test that data type is correct before cast
                        if (rd.Value is ABB.Robotics.Controllers.RapidDomain.Num)
                        {
                            ABB.Robotics.Controllers.RapidDomain.Num rapidNum = (ABB.Robotics.Controllers.RapidDomain.Num)rd.Value;
                            rapidNum.Value = newValue;
                            //get controller mastership
                            using (Mastership master = Mastership.Request(controller))
                            {
                                rd.Value = rapidNum;
                                row.Cells[dataGridView1.Columns["WriteOk"].Index].Value = "OK";
                                //release master
                                master.Release();
                            }
                            debugger.Log($"{ci.ControllerName}; {Varname} WRITE OK");
                        }
                    }
                    catch(Exception ex)
                    {
                        debugger.Log($"{ci.ControllerName}; {Varname} NOT FOUND");
                    }


                }
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "OK";
            }
            catch (Exception ex)
            {
                row.Cells[dataGridView1.Columns["ConnectOK"].Index].Value = "NOK";
                debugger.Exeption(ex);
                debugger.Message("Error connecting to controller");
                return;
            }

        }


        //buttons
        private void Btn_scanNetwork_Click(object sender, EventArgs e)
        {
            HandleScanner();
        }
        private void btn_addCtrl_Click(object sender, EventArgs e)
        {
            //adding for specifc ip 
            AddRobotByIp(tbox_ip.Text);
        }
        private void Btn_expose_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in dt_robots.Rows)
            {
                AddRobotByIp(row.Field<string>("IP"));

            }
            debugger.Message("done with expose");
        }

        private void BtnDoWork_Click(object sender, EventArgs e)
        {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    try
                    {
                    if (scanner.TryFind(new Guid(row.Cells[dataGridView1.Columns["SystemId"].Index].Value.ToString()), out ControllerInfo ci))
                    {
                        this.controller = ControllerFactory.CreateFrom(ci);
                        this.controller.Logon(UserInfo.DefaultUser);

                        // SocketConfigureRobot(ci, row);
                        //DoRobbieFupCheck(ci, row);
                        //DoTipneedcheckRobot(ci, row);
                        //ChangeMAxnoDress(ci, row);
                        LoadNewLrobotRobot(ci, row, tb_workfolder.Text.Trim(), tb_module.Text.Trim(), tbVerVarName.Text.Trim(), tb_verfileValue.Text.Trim());


                    }
                    else
                    {
                        debugger.Message("can not find controller: " + row.Cells[0].Value.ToString());
                    }
                }
                    catch (Exception ex)
                    {
                        debugger.Exeption(ex);
                    }
                }
            
            debugger.Message("done with controllers");
        }

        private void Btn_bckShortcuts_Click(object sender, EventArgs e)
        {
            RobotBckShortcuts bckShort = new RobotBckShortcuts();
            bckShort.searchForRobots();
            bckShort.buildShortcutdirectory();
            debugger.Message("done with dirbuild");
        }

        private void Btn_loadGrid_Click(object sender, EventArgs e)
        {
            //get greenfield list from GADATA
            string qry = string.Format(@"select controller_name, IP from gadata.ngac.c_controller where assetnum like 'URA%' AND CONTROLLER_NAME LIKE '{0}' and enable_bit <> 0",tbGridWhereClause.Text.Trim());
            SqlConnection Conn = new SqlConnection("Data Source=sqla001.gen.volvocars.net;Initial Catalog=GADATA;Integrated Security = true");        
            using (SqlCommand myCommand = new SqlCommand(qry, Conn))
            {
                Conn.Open();
                dt_robots.Load(myCommand.ExecuteReader());
                Conn.Close();         
            }

             //add colums for extra data
            dt_robots.Columns.Add("SystemId", System.Type.GetType("System.String"));
            dt_robots.Columns.Add("ControllerName", System.Type.GetType("System.String"));
            dt_robots.Columns.Add("autoOK", System.Type.GetType("System.String"));
            dt_robots.Columns.Add("Version", System.Type.GetType("System.String"));
            dt_robots.Columns.Add("VersionOK", System.Type.GetType("System.String"));
            dt_robots.Columns.Add("RobotHome", System.Type.GetType("System.String"));
            dt_robots.Columns.Add("ConnectOK", System.Type.GetType("System.String"));
       //     dt_robots.Columns.Add("HasRedress", System.Type.GetType("System.String"));
        //    dt_robots.Columns.Add("LoadOK", System.Type.GetType("System.String"));
      //      dt_robots.Columns.Add("ConfigOK", System.Type.GetType("System.String"));
      //      dt_robots.Columns.Add("restartOK", System.Type.GetType("System.String"));
           dt_robots.Columns.Add("WriteOk", System.Type.GetType("System.String"));
            //  dt_robots.Columns.Add("HasTipneed", System.Type.GetType("System.String"));
            // dt_robots.Columns.Add("HasTipneedComment", System.Type.GetType("System.String"));
            //  dt_robots.Columns.Add("Found", System.Type.GetType("System.String"));
            //  dt_robots.Columns.Add("Deleted", System.Type.GetType("System.String"));
            //  dt_robots.Columns.Add("Exeption", System.Type.GetType("System.String"));

            //link to datagrid
            dataGridView1.DataSource = dt_robots;
            //
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                try
                {
                    if (scanner.TryFind(new Guid(row.Cells[dataGridView1.Columns["SystemId"].Index].Value.ToString()), out ControllerInfo ci))
                    {
                        this.controller = ControllerFactory.CreateFrom(ci);
                        this.controller.Logon(UserInfo.DefaultUser);
                        //ChangeValue(ci, row, "HMeasurment", "nMaxRatioDiffFromAverage", 25);
                        //ChangeValue(ci, row, "HDress", "nMaxNoOffAutoReDress", 1);
                        ChangeDressParm(ci, row);
                    }
                    else
                    {
                        debugger.Message("can not find controller: " + row.Cells[0].Value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    debugger.Exeption(ex);
                }
            }

            debugger.Message("done with controllers");
        }
    }


}
