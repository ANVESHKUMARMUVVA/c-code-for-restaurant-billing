using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantConsole;
using System.IO;
using System.Text.RegularExpressions;


namespace RestaurantConsole
{
    class Program
    {
        static int orderno = 100;
        string customername = "", ContactNumber = "", y = "";          

        #region Array Declaration
        //array for Price      
        string[] PriceArray = new string[] { "20", "30", "40", "50", "60" };

        //Array For Food MEnu
        string[] FoodMenuArray = new string[] { "Iteam1", "Iteam2", "Iteam3", "Iteam4", "Iteam5" };

        //array used for storying user choice for Food
        string[] TempFoodArray = new string[10];

        //array used for storying user choice for Price
        string[] TempPriceArray = new string[10];

        //array used for storying user Quantity 
        string[] TempArrayQuantity = new string[10];

        #endregion Array Declaration

        static void Main(string[] args)
        {
            Program obj = new Program();
            string yes = string.Empty;
            do
            {
                long bill = 0, discount = 0, TotalAmount = 0;
                int quanty = 0;                
                orderno++;
                obj.Customer();
                bill = obj.PrinTFile(bill, quanty);
                obj.PrintBill();
                TotalAmount = obj.Discount(TotalAmount, discount, bill);
                obj.SaveFile(TotalAmount);//savefile method is call to save into file
                Console.Write("\n\tDo You Want to Continue Y/N--->");
                yes = Console.ReadLine();
            } while (yes == "y" || yes == "Y");

        }

        //CUSTOMER CLASS
        public void Customer()
        {
            ContactNumber = ""; customername = "";
            Console.Clear();
            customername = null; ContactNumber = null;
            Console.WriteLine("_______________________________ RESTURANT _____________________________________\n");
            //Accept Customer Name From User
            Console.Write("\n\tEnter Customer Name--->");
            customername = Console.ReadLine();
            //Accept MObile NUmber From User
            Console.Write("\n\tContact Number--->");
            ContactNumber = Console.ReadLine();
        }

        //Read Menu From File And Print It on Console
        public long PrinTFile(long bill, int quanty)
        {
            String line, y;
            int choice;
            int i = 1;
            try
            {
                do
                {
                    Console.WriteLine("________________________ RESTURANT _______________________\n");
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader("C:\\Menu.txt");

                    //Read the first line of text
                    line = sr.ReadLine();
                    //Continue to read until you reach end of file
                    while (line != null)
                    {
                        //write the lie to console window
                        Console.WriteLine("\t"+line);
                        //Read the next line
                        line = sr.ReadLine();
                    }
                    //close the file
                    sr.Close();
                    //Enter Your Choice for Iteams
                    Console.Write("\n\tEnter Your Choice-->");
                    choice = Convert.ToInt32(Console.ReadLine());
                    //Enter Quantity
                    Console.Write("\n\tEnter Your Quantity(Number Of Plates)-->");
                    quanty = Convert.ToInt32(Console.ReadLine());

                    //Total amount Stored IN bIll
                    bill = bill + quanty * Convert.ToInt32(PriceArray[choice - 1]);

                    //User Choice Stored IN array
                    TempFoodArray[i] = FoodMenuArray[choice - 1];
                    TempPriceArray[i] = PriceArray[choice - 1];
                    TempArrayQuantity[i] = quanty.ToString();

                    Console.Write("\n\tDo You Want to Place More Order Y/N--->");
                    y = Console.ReadLine();
                    i++;
                } while (y == "y" || y == "Y");
            }
            catch (Exception ex)
            {
                Program obj = new Program();
                obj.LogError(ex);
            }
            return bill;
        }

        //Bill will get print
        void PrintBill()
        {
            Console.WriteLine("________________________ Bill _______________________\n");
            Console.WriteLine(" \t Order Number Is--->" + orderno);
            Console.WriteLine(" \n\t MEALS\t\t\tPRICE\t\t\n");

            var Arraylist = TempFoodArray.Zip(TempPriceArray, (first, second) => first + "\t\t\t " + second);

            foreach (var item in Arraylist)
            {
                Console.WriteLine("\t" + item);
            }           
            Console.WriteLine("___________________________________________________\n");
        }

        //Discount will be added 
        private long Discount(long TotalAmount, long discount, long bill)
        {
            string y;
            Console.Write("\n\tDo You Have Any coupen Y/N--->");
            y = Console.ReadLine();
            if (y == "y" || y == "Y")
            {
                PrintBill();//to print bill to for discount
                discount = Convert.ToInt64(0.1 * bill);
                Console.Write("\n\tTotal Discount is--->\t" + discount);
                TotalAmount = bill - discount;
                Console.Write("\n\tTotal--->" + TotalAmount);
                // SaveFile();//savefile method is call to save into file
            }
            else
            {
                TotalAmount = bill;
                Console.WriteLine("\n\tTotal--->" + TotalAmount);
            }
            return TotalAmount;
        }

        //Method For Data Into File
        void SaveFile(long TotalAmount)
        {
            string filename = "c:\\bill.txt"; //Path where you want to create file

            //create or append new file
            //Add Customer name contact no order no into file
            File.AppendAllText(filename, Environment.NewLine + orderno + "\t" + customername + "\t" + ContactNumber + "\t");
            foreach (var item in TempFoodArray)
            {
                //add menu items into file
                File.AppendAllText(filename, item + "\t");
            }
            foreach (var item in TempArrayQuantity)
            {
                //add menu items into file
                File.AppendAllText(filename, item + "\t");
            }
            //add total items and discount inot file
            File.AppendAllText(filename, TotalAmount + "\t"  + Environment.NewLine);
            Array.Clear(TempFoodArray, 0, TempFoodArray.Length);//Clear Temp Array Values
            Array.Clear(TempPriceArray, 0, TempPriceArray.Length);//Clear Temp Array Values
            Array.Clear(TempArrayQuantity, 0, TempArrayQuantity.Length);//Clear Temp Array Values          
        }

        //Error Class For Exception Handling What Ever occurred it will store in  file in errorlog.txt
        public void LogError(Exception ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            using (StreamWriter writer = new StreamWriter("F:\\ErrorLog.txt", true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

    }
}





