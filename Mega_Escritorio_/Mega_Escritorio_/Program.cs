Skip to content
This repository
Search
Pull requests
Issues
Gist
 @tishparkinson
 Watch 1
  Star 0
  Fork 0 DielTenille/cit_301c
 Code  Issues 0  Pull requests 0  Projects 0  Wiki Pulse  Graphs
Branch: master Find file Copy pathcit_301c/TenilleDiel_Mega-Escritorio-App/TenilleDiel_Mega-Escritorio-App/Program.cs
11aa730  3 days ago
@DielTenille DielTenille Adding my Mega_Escritorio_Console_Application for Lesson 3
1 contributor
RawBlameHistory
221 lines(190 sloc)  8.47 KB
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TenilleDiel_Mega_Escritorio_App
{
    class Order
    {
        static void Main(string[] args)
        {
            double materialPrice = calculateMaterialPrice();
            double drawerPrice = calculateDrawerPrice();
            double deskSize = calculateDimension();
            double dimensionPrice = calculateDimensionPrice(deskSize);
            double rushOrderPrice = calculateRushOrderPrice(deskSize);
            createPriceQuote(materialPrice, drawerPrice, dimensionPrice, rushOrderPrice);
            Console.ReadLine();
        }
        public static void createPriceQuote(double materialPrice, double drawerPrice, double dimensionPrice, double rushOrderPrice)
        {
            Random rand = new Random();
            const double BASE_DESK_PRICE = 200;
            int orderID = rand.Next();

            Console.WriteLine("\n Order ID: #" + orderID);
            Console.WriteLine(" Base Desk Price: $" + BASE_DESK_PRICE);
            Console.WriteLine(" Desk Material Price: $" + materialPrice);
            Console.WriteLine(" Desk Drawer Price: $" + drawerPrice);
            Console.WriteLine(" Extra Large Surface Area Price: $" + dimensionPrice);
            Console.WriteLine(" Rush Order Price: $" + rushOrderPrice);

            double totalPrice = BASE_DESK_PRICE + materialPrice + drawerPrice + dimensionPrice + rushOrderPrice;
            Console.WriteLine("\n *****  Total Custom Desk Price: $" + totalPrice + "  *****");

            savePriceQuoteToFile(orderID, BASE_DESK_PRICE, materialPrice, drawerPrice, dimensionPrice, rushOrderPrice, totalPrice);


        }
        public static void savePriceQuoteToFile(double orderID, double baseDeskPrice, double materialPrice, double drawerPrice, double dimensionPrice, double rushOrderPrice, double totalPrice)
        {
            string json = JsonConvert.SerializeObject(new
            {
                OrderID = orderID,
                BaseDesk = baseDeskPrice,
                Material = materialPrice,
                Drawers = drawerPrice,
                Dimension = dimensionPrice,
                RushOrder = rushOrderPrice,
                TotalPrice = totalPrice
            });

            //write string to file
            System.IO.File.AppendAllText(@"C:\Users\t2alaska\cit301c\TenilleDiel_Mega-Escritorio-App\orders.txt", json);
        }

        public static double calculateMaterialPrice()
        {
            const int MIN_NUM = 0;
            const int MAX_NUM = 2;

            double[] materialTypePrice = new double[]
            {
                200, 100, 50
            };

            string[] materialType = new string[]
            {
                "Oak", "Laminate", "Pine"
            };

            Console.WriteLine("Please choose a desk material by typing the associated number. \n 0 = " + materialType[0] + ", 1 = " + materialType[1] + ", 2 = " + materialType[2]);
            int materialChosen;
            do
            {
                string materialChosenString = Console.ReadLine();
                materialChosen = int.Parse(materialChosenString);
                if (materialChosen < MIN_NUM || materialChosen > MAX_NUM)
                {
                    Console.WriteLine("Please enter a number between 0 and 2.");
                }
            } while (materialChosen < MIN_NUM || materialChosen > MAX_NUM);

            double materialSelectionPrice = materialTypePrice[materialChosen];

            return materialSelectionPrice;

        }
        public static int calculateDrawerPrice()
        {
            const int MIN_DRAWERS = 0;
            const int MAX_DRAWERS = 7;
            int numDrawers;

            Console.WriteLine("Please enter the number of drawers you want your desk to have.");
            do
            {
                string numDrawerString = Console.ReadLine();
                numDrawers = int.Parse(numDrawerString);
                if (numDrawers < MIN_DRAWERS || numDrawers > MAX_DRAWERS)
                {
                    Console.WriteLine("Please enter a number between 0 and 7.");
                }
            } while (numDrawers < MIN_DRAWERS || numDrawers > MAX_DRAWERS);

            int drawerPrice = 50;
            int drawerSelectionPrice = drawerPrice * numDrawers;

            return drawerSelectionPrice;
        }

        public static double calculateDimension()
        {
            Console.WriteLine("Please enter a desk top length in inches:");
            string lengthString = Console.ReadLine();
            double deskLength = double.Parse(lengthString);

            Console.WriteLine("Please enter a desk top width in inches:");
            string widthString = Console.ReadLine();
            double deskWidth = double.Parse(widthString);

            double deskDimensions = deskLength * deskWidth;
            Console.WriteLine("Total desk top area: " + deskDimensions);

            return deskDimensions;
        }
        public static double calculateDimensionPrice(double deskDimensions)
        {
            double dimensionPrice = 0;

            if (deskDimensions > 1000)
            {
                deskDimensions = deskDimensions - 1000;
                dimensionPrice = deskDimensions * 5;
            }

            return dimensionPrice;
        }

        private static int determineRushSize(double deskDimensions)
        {
            int[] rushSizeOptions = new int[]
            {
                1000, 1999, 2000
            };

            if (deskDimensions < rushSizeOptions[0])
            {
                return 1000;
            }
            else if (deskDimensions <= rushSizeOptions[1])
            {
                return 1999;
            }
            else
            {
                return 2000;
            }
        }

        public static double calculateRushOrderPrice(double deskDimensions)
        {
            double rushOrderSelectionPrice = 0;
            double price;
            int rushChosen;
            int[] rushOptions = new int[]
            {
                3, 5, 7
            };

            string priceFile = @"C:\Users\t2alaska\Dropbox\_College\CIT 301C\Lesson 3\price-table.txt";

            try
            {
                string[] rushOrderPrices = File.ReadAllLines(priceFile);

                double[][] rushOrderArray = new double[rushOrderPrices.Length][];
                for (int i = 0; i < rushOrderPrices.Length; i++)
                {
                    var columns = rushOrderPrices[i].Split(new char[] { ',' });
                    rushOrderArray[i] = new double[] {
                        Double.Parse(columns[0]),Double.Parse(columns[1]),Double.Parse(columns[2]),
                    };
                }

                //Ask the user if they want rush order shipping
                Console.WriteLine("Would you like Rush Order Shipping?");
                string feelingRushed = Console.ReadLine();
                if (feelingRushed == "n")
                {
                    return rushOrderSelectionPrice;
                }
                else
                {
                    //Ask the user if they want 3, 5, or 7 day rush shipping
                    Console.WriteLine("How fast do you want your order shipped? \n 0 = " + rushOptions[0] + "-Day" + ", 1 = " + rushOptions[1] + "-Day" + ", 2 = " + rushOptions[2] + "-Day");
                    do
                    {
                        string rushChosenString = Console.ReadLine();
                        rushChosen = int.Parse(rushChosenString);
                        if (rushChosen < 0 || rushChosen > 2)
                        {
                            Console.WriteLine("Please enter a number between 0 and 2.");
                        }
                    } while (rushChosen < 0 || rushChosen > 2);
                }
                //Decide what the price should be
                double rushSelected = rushOptions[rushChosen];
                double deskSize = determineRushSize(deskDimensions);
                price = rushOrderArray.Where(i => i[0] == rushSelected).Where(i => i[1] == deskSize).First()[2];
                Console.WriteLine("The rush order price will be: $" + price);
                return price;
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem with the file.");
            }
            return price = 1;
        }
    }
}
Contact GitHub API Training Shop Blog About
© 2017 GitHub, Inc.Terms Privacy Security Status Help