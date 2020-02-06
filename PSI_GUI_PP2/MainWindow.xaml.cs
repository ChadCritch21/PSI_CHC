/*
 * This file provides the backend C# code to calculate a Pneumonia Severity Index score, proivde a risk class and admission status based on that score, and appened the individuals data to a csv file.
 * GUI Programming Project #2 Xmal and C# combined. 
 * CS311 - App Devel in Visual Languages
 * Spring 2020
 * Author: Chad Critchelow
 * @version 1.0
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSI_GUI_PP2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
         * This is the function that will be ran once the "Calculate Pneumonia Severity Index" button is selected.
         * All the calculations and file writing/messages are tied to the button being clicked. 
         */
        private void calculatePSI_Click(object sender, RoutedEventArgs e)
        {
            // Here I have my various varibles used thoughout the program. I'm setting them to 0 initally and if sleclected will become "1"
            double PSI = 0;
            int NHR = 0;
            int ND = 0;
            int LD = 0;
            int CHF = 0;
            int CD = 0;
            int RD = 0;
            int AMS = 0;
            int PE = 0;
            double wtemp = 0;
            double wBUN = 0;
            double wGlucose = 0;
            double wPPO = 0;
            string sex = "";

            /*
             * Here I am doing a series of "if" checks to see if the user has selected a given checkbox.
             * If the checkbox has been selected, the PSI score will be increaed by its specfied ammount and the condition identifier will become "1".
             * I didn't have any trouble with this portion after I did a few conditional checks and made sure the checkbox were named appropriately.
             */
            if (NHRyes.IsChecked == true)
            {
                NHR = 1;
                PSI = PSI + 10;
            }

            if (NDyes.IsChecked == true)
            {
                ND = 1;
                PSI = PSI + 30;
            }

            if (LDyes.IsChecked == true)
            {
                LD = 1;
                PSI = PSI + 20;
            }

            if (CHFyes.IsChecked == true)
            {
                CHF = 1;
                PSI = PSI + 10;
            }

            if (CDyes.IsChecked == true)
            {
                CD = 1;
                PSI = PSI + 10;
            }

            if (RDyes.IsChecked == true)
            {
                RD = 1;
                PSI = PSI + 10;
            }

            if (AMSyes.IsChecked == true)
            {
                AMS = 1;
                PSI = PSI + 20;
            }

            if (PEyes.IsChecked == true)
            {
                PE = 1;
                PSI = PSI + 10;
            }

            /*
             * Here I am checking the see if the user enter a sex of female or male.
             * If they idenified as female, the PSI score is reduced by 10 and no change is made if they are male.
             * I've included the identifiers for sex on these conditions as well so that they made be read to the file.
             */

            if (Sex.Text == "Female" || Sex.Text == "F" || Sex.Text == "female")
            {
                sex = "F";
                PSI = PSI - 10;
            }

            if (Sex.Text == "Male" || Sex.Text == "M" || Sex.Text == "male")
            {
                sex = "M";
            }

            // Checking to make the Age field has a value and if it does, I'm adding that age directly to the PSI score.
            if (Age.Text != "")
            {
                PSI = PSI + Convert.ToDouble(Age.Text);
            }

            /*
             * These next "if" conditions are coming from direct data enter by or user or health provider.
             * The units for this data is already set as a standard so, no conversion of the data points will be made.
             * With the data for each variable, if the value is under or over (equal to over or under) the PSI score will increase
             * depeniding on what the reading for the biometric should be.
             */

            if (respiratoryRate.Text != "")
            {
                if (Convert.ToDouble(respiratoryRate.Text) >= 30)
                {
                    PSI = PSI + 20;
                }
            }

            if (sysBP.Text != "")
            {
                if (Convert.ToDouble(sysBP.Text) < 90)
                {
                    PSI = PSI + 20;
                }
            }

            if (pulseRate.Text != "")
            {
                if (Convert.ToDouble(pulseRate.Text) >= 125)
                {
                    PSI = PSI + 10;
                }
            }

            if (phLvl.Text != "")
            {
                if (Convert.ToDouble(phLvl.Text) < 7.35)
                {
                    PSI = PSI + 30;
                }
            }

            if (sodiumLvl.Text != "")
            {
                if (Convert.ToDouble(sodiumLvl.Text) < 130)
                {
                    PSI = PSI + 20;
                }
            }

            if (hematocritReading.Text != "")
            {
                if (Convert.ToDouble(hematocritReading.Text) < 30)
                {
                    PSI = PSI + 10;
                }
            }

            /*
             * These next "if" conditions are coming from direct data enter by or user or health provider.
             * The units for this data may be recorded with two varying types. Since that is the case, we must convert
             * the units to the desired unit if they're not already given in the desired units.
             * With the data for each variable, if the value is under or over (equal to over or under) the PSI score will increase
             * depeniding on what the reading for the biometric should be.
             * 
             * This is what I had trouble with the most at first by trying to get everything converted to the correct units if need be.
             * Once I figured out the structure of how the conditions needed to be written and the conversion equations, the process wasn't too hard.
             */

            if (ctempSelected.IsChecked == true)
            {
                wtemp = Convert.ToDouble(temp.Text);

                if (Convert.ToDouble(temp.Text) < 35 || Convert.ToDouble(temp.Text) > 39.9)
                {
                    
                    PSI = PSI + 15;
                }
            }

            if (ftempSelected.IsChecked == true)
            {
                double ctempr = Math.Round(((Convert.ToDouble(temp.Text) - 32) * 5/9), 1);
                wtemp = ctempr;

                if (ctempr < 35 || ctempr > 39.9)
                {
                    PSI = PSI + 15;
                }

            }

            if (BUNinDL.IsChecked == true)
            {
                wBUN = Convert.ToDouble(BUNvalue.Text);

                if (Convert.ToDouble(BUNvalue.Text) >= 30)
                {
                    
                    PSI = PSI + 20;
                }
            }

            if (BUNinL.IsChecked == true)
            {
                double BinDL = Math.Round(Convert.ToDouble(BUNvalue.Text) * 18);
                wBUN = BinDL;
                
                if (BinDL >= 30)
                {
                    PSI = PSI + 20; 
                }
            }

            if (glucoseIndL.IsChecked == true)
            {
                wGlucose = Convert.ToDouble(glucoseValue.Text);

                if (Convert.ToDouble(glucoseValue.Text) >= 250)
                {
                    PSI = PSI + 10;
                }
            }

            if (glucoseInL.IsChecked == true)
            {
                double gINdL = Math.Round(Convert.ToDouble(glucoseValue.Text) * 18);
                wGlucose = gINdL;

                if (gINdL >= 250)
                {
                    PSI = PSI + 10;
                }
            }

            if (OxygenINmmHg.IsChecked == true)
            {
                wPPO = Convert.ToDouble(partialOxygenValue.Text);

                if (Convert.ToDouble(partialOxygenValue.Text) < 60)
                {
                    PSI = PSI + 10;
                }
            }

            if (OxygenINkPa.IsChecked == true)
            {
                double oINmmHg = Math.Round(Convert.ToDouble(partialOxygenValue.Text) * 7.50062);
                wPPO = oINmmHg;

                if (oINmmHg < 60)
                {
                    PSI = PSI + 10;
                }
            }

            /*
            * Now we are checking to see what risk class the individual should be placed in based on the PSI score.
            * After the conditon is met, we are displaying a message box to show the risk class, risk level, PSI score, and admission status.
            * Note: if the Age field is the only field where points come from, the risk class will be "I" regardless of how old they are.
            
            * This portion of the project wasn't too hard once I figured out how to work with Age being risk class "I" if it contains all the points.
            */

            if (PSI <= Convert.ToDouble(Age.Text))
            {
                MessageBox.Show("Risk Class: I"+"\n"
                                +"Risk: Low"+"\n"
                                +"Pneumonia Severity Index: " +PSI+"\n"
                                +"Admisson Status: Outpatient Care");
            }

            if (PSI > Convert.ToDouble(Age.Text) && PSI <= 70 )
            {
                MessageBox.Show("Risk Class: II" + "\n"
                                + "Risk: Low" + "\n"
                                + "Pneumonia Severity Index: " + PSI + "\n"
                                + "Admisson Status: Outpatient Care");
            }

            if (PSI > Convert.ToDouble(Age.Text) && PSI >= 71 && PSI <= 90)
            {
                MessageBox.Show("Risk Class: III" + "\n"
                                + "Risk: Low" + "\n"
                                + "Pneumonia Severity Index: " + PSI + "\n"
                                + "Admisson Status: Outpatient or Observation Admission");
            }

            if (PSI > Convert.ToDouble(Age.Text) && PSI >= 91 && PSI <= 130)
            {
                MessageBox.Show("Risk Class: IV" + "\n"
                                + "Risk: Moderate" + "\n"
                                + "Pneumonia Severity Index: " + PSI + "\n"
                                + "Admisson Status: Inpatient Admission");
            }

            if (PSI > Convert.ToDouble(Age.Text) && PSI > 130)
            {
                MessageBox.Show("Risk Class: V" + "\n"
                                + "Risk: High" + "\n"
                                + "Pneumonia Severity Index: " + PSI + "\n"
                                + "Admisson Status: Inpatient Admission (check for sepsis)");
            }

            // Path of the csv file to attend to. It is locate in the Debug folder.
            string path = @"data.csv";

            // Reading the csv file to get the next avaliable line as the "ID" and appending the file with the new data submitted by the user.
            var lines = File.ReadAllLines(path);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(lines.Length+1+", " + Age.Text + ", " + sex + ", " + NHR + ", " + ND + ", " + LD + ", " + CHF + ", " + CD + ", " + RD + ", " + AMS + ", " + respiratoryRate.Text + ", " + sysBP.Text + ", "
                                           + wtemp.ToString() + ", " + pulseRate.Text + ", " + phLvl.Text + ", " + wBUN + ", " + sodiumLvl.Text + ", " + wGlucose + ", " + hematocritReading.Text + ", " + wPPO + ", " + PE); ;
            }

        }
    }
}
