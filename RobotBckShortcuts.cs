using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace ABBCommTest
{
	public class RobotBckShortcuts
	{
        DataTable dt_robots;
		List<string> ResultList;
		string backupBasePath = @"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\Backup_Volgens_Maximo_Locaties";
		
		public void buildShortcutdirectory()
		{
			//get robots from GADATA
			string qry = "select * from GADATA.equi.ASSETS where (CLassificationId like 'URA%' or CLassificationId like 'URC%') and LocationTree like 'VCG -> A%'";
            SqlConnection Conn = new SqlConnection("Data Source=sqla001.gen.volvocars.net;Initial Catalog=GADATA;Integrated Security = true");
            using (SqlCommand myCommand = new SqlCommand(qry, Conn))
            {
                Conn.Open();
                dt_robots.Load(myCommand.ExecuteReader());
                Conn.Close();
            }

            //select location tree
            List<string> robots = dt_robots.AsEnumerable()
						   .Select(r => r.Field<string>("LocationTree"))
						   .ToList();
			//loop robots
			foreach (string robot in robots)
			{
				//VCG -> A -> AAL -> A FLOOR L -> A PAFL4 -> A LIJN 26000 -> A STN26010 -> 26010P03 
				string[] Locationsplitstring = robot.Split(new string[] { "->" }, StringSplitOptions.None);
				StringBuilder activelocation = new StringBuilder();
				activelocation.Append(backupBasePath);
				//
				foreach (string sublocation in Locationsplitstring)
				{
					activelocation.Append(@"\").Append(sublocation.Trim());
					//
					if (Locationsplitstring.Last() == sublocation) //is last in array it must be a robot.
					{
						//string linktarget =  @"C:\temp";//must find real robot in old system here... 
						IEnumerable<string> linktargets = ResultList.Where(s => s.EndsWith(Locationsplitstring.Last().Trim()));
						if (linktargets == null) { Console.WriteLine("l:" + activelocation.ToString() + " ->" + "Missing"); }
						//
						foreach  (string linktarget in linktargets)
						{ 
							//
							object shDesktop = (object)"Desktop";
							WshShell shell = new WshShell();
							string shortcutAddress = activelocation.ToString() + @".lnk";
							//
							if (System.IO.File.Exists(shortcutAddress)) {System.IO.File.Delete(shortcutAddress);}
							//
							IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
							//shortcut.Description = "New shortcut for a Notepad";
							//shortcut.Hotkey = "Ctrl+Shift+N";
							shortcut.TargetPath = linktarget;
							shortcut.Save();
							//
							Console.WriteLine("l:" + activelocation.ToString() + " ->" + linktarget);
						}
					}
					else //directory mode
					{
						//
						if (!Directory.Exists(activelocation.ToString()))
						{
							Directory.CreateDirectory(activelocation.ToString());
							Console.WriteLine("c:" + activelocation.ToString());
						}
						else
						{
							Console.WriteLine("o:" + activelocation.ToString());
						}
					}
				}

			}


		}

		public void searchForRobots()
		{
			/*List<string> LOGSearchpathsOLD = new List<String>() 
			{ 
			@"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\P1X_FLOOR" ,
			@"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\P1X_SIBO" , 
			@"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\P1X_HOP" , 
			@"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\FLOOR" , 
			@"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\SIBO" , 
			@"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\BOSKI" , 
			@"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\ROBLAB" , 

			};
			ResultList = ReqSearchDir(LOGSearchpathsOLD, "*", @"(\d\d\d\d\d)R(\d\d)$");*/

            List<string> LOGSearchpathsNGAC = new List<String>()
            {
            @"\\gnlsnm0101\6308-APP-NASROBOTBCK0001\robot_ga\IRC5-NGAC" 
            };
            ResultList = ReqSearchDir(LOGSearchpathsNGAC, "*", @"(\d\d\d\d\d\d)R(\d\d)$");

        }

		List<string> ReqSearchDir(List<string> als_filepaths, string as_mask, string as_fileNameMask)
		{
			List<string> List = new List<string>();
			try
			{
				foreach (string filepath in als_filepaths)
				{
					Regex reg = new Regex(as_fileNameMask);

					var files = Directory.GetDirectories(filepath, as_mask,SearchOption.AllDirectories)
										 .Where(path => reg.IsMatch(path))
										 .ToList();

					List.AddRange(files);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return List;
		}
	}
}
