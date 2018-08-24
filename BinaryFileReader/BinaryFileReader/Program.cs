using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryFileReader
{
    public class BinaryReadWriteClass
    {
        //Write to text file...
        public void LogWriter(TeamDetails fileStoredAlready, TeamDetails duplicateFile)
        {
            var fileName = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Log");
            fileName = fileName + "\\log.txt";

            //check if file exists
            if (!File.Exists(fileName))
            {
                using (StreamWriter userWriter = new StreamWriter(fileName))
                {
                    userWriter.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                    userWriter.WriteLine(fileStoredAlready.FileID + " is a dupluicate of file: " + duplicateFile.FileID);
                    userWriter.WriteLine();
                }
            }
            else
            {
                using (StreamWriter userWriter = File.AppendText(fileName))
                {
                    userWriter.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                    userWriter.WriteLine(fileStoredAlready.FileID + " is a dupluicate of file: " + duplicateFile.FileID);
                    userWriter.WriteLine();
                }
            }
        }

        //Write to Binnary file
        public void WriteBinary(TeamDetails teamDetails)
        {
            try
            {
                var fileName = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "BinaryFiles");
                var files = Directory.GetFiles(fileName).Length;
                fileName = fileName + "\\file00" + (files + 1) + ".dat";

                var readFile = fileName.Split('\\');
                var fileID = readFile.Last();

                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    teamDetails.FileID = fileID;
                    binaryWriter.Write(teamDetails.FileID);
                    binaryWriter.Write(teamDetails.TeamName);
                    binaryWriter.Write(teamDetails.Manager);
                    binaryWriter.Write(teamDetails.Players);

                }
                Console.WriteLine();
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Error: {0}", ioe.Message);

            }
        }

        public void ReadBinary()
        {
            try
            {
                var folderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "BinaryFiles");

                List<TeamDetails> teamList = new List<TeamDetails>();
                var files = Directory.GetFiles(folderPath).Length;
                var allFiles = Directory.GetFiles(folderPath);

                if (files > 0)
                {
                    foreach (var filePath in allFiles)
                    {
                        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                        {
                            TeamDetails teamDetails = new TeamDetails();
                            teamDetails.FileID = reader.ReadString();
                            teamDetails.TeamName = reader.ReadString();
                            teamDetails.Manager = reader.ReadString();
                            teamDetails.Players = reader.ReadInt32();

                            teamList.Add(teamDetails);

                            var readFile = filePath.Split('\\');
                            var listFile = readFile.Last();

                            var findTeam = teamList.FirstOrDefault(file => file.TeamName == teamDetails.TeamName && file.FileID.Equals(teamDetails.FileID) == false);

                            if(findTeam != null)
                            {
                                LogWriter(findTeam,teamDetails);
                            }

                        }
                    }
                }
                    
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Error: {0}", ioe.Message);

            }
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            string newTeam = "Y";

            do
            {
                TeamDetails teamDetails = new TeamDetails();
                System.Console.Clear();
            
                Console.WriteLine();
                Console.WriteLine("----------------------BINARY FILES: TEAMS -----------------------");
                Console.WriteLine("--------------------------------------------------------------------\n\n");
                Console.WriteLine("Please enter data to the file...\n\n");

                Console.WriteLine("Please enter Team Name: ");              //Team Name
                teamDetails.TeamName = Console.ReadLine();

                Console.WriteLine("Please enter Manager Name: ");           //Manager
                teamDetails.Manager = Console.ReadLine();

                Console.WriteLine("Please enter Number of players in the team: ");  //Players
                string players = Console.ReadLine();
                Int32 value;

                while(!Int32.TryParse(players, out value))
                {
                    Console.WriteLine("Please type in a valid number!!");
                    players = Console.ReadLine();
                }
                teamDetails.Players = Convert.ToInt32(players);

                BinaryReadWriteClass readWrite = new BinaryReadWriteClass();
                readWrite.WriteBinary(teamDetails);
                readWrite.ReadBinary();


                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Would you like to enter another team?(Y/N)");
                newTeam = Console.ReadLine();
                
                while(newTeam.Equals("Y", StringComparison.InvariantCultureIgnoreCase) == false && newTeam.Equals("N", StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    Console.WriteLine("Please type in Y for 'Yes' and N for 'No'");
                    newTeam = Console.ReadLine();
                }

            } while (newTeam.Equals("N", StringComparison.InvariantCultureIgnoreCase) == false);

            System.Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\n\n..........................................................................");
            Console.WriteLine("...         GOODBYE. THANKS FOR USING RESIDENCE TEAM APP                 .... ");
            Console.WriteLine("............................................................................\n\n");
            Console.WriteLine("Press any key to close!!");
            Console.ReadKey();
        }
    }
}
