﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Méphistophélès
{
    public partial class Form1 : Form
    {
        int mov;
        int movX;
        int movY;
        bool nulledauthmode = false, forlaxmode = false, ssl = false, server_status_cracked = false, server_status_nulled = false, server_status_custom = false, forlaxmode2 = false;
        public Form1()
        {
            InitializeComponent();
        }

        //Quit Button
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //this is for moving the form with form border style to none
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Screen.AllScreens[0].WorkingArea.Location;
        }

        //this is for moving the form with form border style to none
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        //this is for moving the form with form border style to none
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX,MousePosition.Y - movY);
            }
        }
        //this is for moving the form with form border style to none
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void stopbutton_cracked_Click(object sender, EventArgs e)
        {
            //write the old host file.
            StreamReader streamReader = new StreamReader("oldhost.txt");
            StreamWriter writer = new StreamWriter("C:\\Windows\\System32\\drivers\\etc\\hosts");

            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                writer.WriteLine(line);
            }
            writer.Close();
            streamReader.Close();

            foreach (var process in Process.GetProcessesByName("python"))
            {
                process.Kill();
            }
            serverstatus.ForeColor = System.Drawing.Color.DarkRed;
            server_status_cracked = false;
            serverstatus.Text = "OFF";
            //set the server status to OFF
        }

        private void forlaxauthmode_check_CheckedChanged(object sender, EventArgs e)
        {
            forlaxmode = !forlaxmode;
        }

        private void startnulled_button_Click(object sender, EventArgs e)
        {
            if(server_status_nulled == true)
            {
                MessageBox.Show("You didn't stop the server,please stop the server before starting other mode.");
            }
            if(server_status_nulled == false)
            {
                StreamWriter optionWriter = new StreamWriter("server\\nulled\\config.ini");
                //if secret key textbox is not null write the secret key to secret.txt

                if (nulledauthmode == true)
                {
                    optionWriter.WriteLine("[options]");
                    optionWriter.WriteLine("nulledauthmode=True");
                }
                else
                {
                    optionWriter.WriteLine("[options]");
                    optionWriter.WriteLine("nulledauthmode=False");
                }

                if (secretkey_nulled.Text != null)
                {
                    optionWriter.WriteLine("[secret]");
                    optionWriter.WriteLine("secretkey=" + secretkey_nulled.Text);
                }

                optionWriter.Close();

                //starting the server
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c \"python server\\nulled\\server.py\"";
                p.StartInfo = startInfo;
                p.Start();

                //saving the host file to oldhost.txt for get less trouble that possible if you close without stopping the server.
                StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\drivers\\etc\\hosts");
                StreamWriter streamWriter = new StreamWriter("oldhost.txt");

                string line = streamReader.ReadLine();

                while (line != null)
                {
                    line = streamReader.ReadLine();
                    streamWriter.WriteLine(line);
                }

                streamReader.Close();
                streamWriter.Close();

                if (nulledauthmode == false)
                {
                    string payload = "\x0D" + "127.0.0.1 www.nulled.to";
                    //what this does ? all the traffic that will pass through nulled.to will actually go to our server
                    File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                    //set the server status to ON
                    serverstatus_nulled.ForeColor = System.Drawing.Color.LimeGreen;
                    serverstatus_nulled.Text = "ON";
                }
                else
                {
                    string payload = "\x0D" + "127.0.0.1 nulledauth.net";
                    //what this does ? all the traffic that will pass through nulledauth.net will actually go to our server
                    File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                    //set the server status to ON
                    serverstatus_nulled.ForeColor = System.Drawing.Color.LimeGreen;
                    server_status_nulled = true;
                    serverstatus_nulled.Text = "ON";
                }
            }
        }

        private void nulledauth_mode_CheckedChanged(object sender, EventArgs e)
        {
            nulledauthmode = !nulledauthmode;
        }

        private void stopnulled_button_Click(object sender, EventArgs e)
        {
            //write the old host file.
            StreamReader streamReader = new StreamReader("oldhost.txt");
            StreamWriter writer = new StreamWriter("C:\\Windows\\System32\\drivers\\etc\\hosts");
            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                writer.WriteLine(line);
            }

            writer.Close();
            streamReader.Close();

            //The program search through all process the python process once he found it he kill it
            foreach (var process in Process.GetProcessesByName("python"))
            {
                process.Kill();
            }
            //set the server status to OFF
            serverstatus_nulled.ForeColor = System.Drawing.Color.DarkRed;
            server_status_nulled = false;
            serverstatus_nulled.Text = "OFF";
        }

        private void ssl_check_CheckedChanged(object sender, EventArgs e)
        {
            ssl = !ssl;
        }

        private void startbutton_custom_Click(object sender, EventArgs e)
        {
            //check if the server is already started
            if(server_status_custom == true)
            {
                MessageBox.Show("You didn't stop the server, please stop the server before starting other mode.");
            }
            if(server_status_custom == false)
            {
                StreamWriter config = new StreamWriter("server\\custom\\config.ini");

                if (ssl == false)
                {
                    config.WriteLine("[options]");
                    config.WriteLine("port=80");
                }
                if (ssl == true)
                {
                    config.WriteLine("[options]");
                    config.WriteLine("port=443");
                }
                config.WriteLine("[response]");
                config.WriteLine("response=" + goodboy_textbox.Text);

                config.Close();

                //starting the server
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c \"python server\\custom\\server.py\"";
                p.StartInfo = startInfo;
                p.Start();

                //saving the host file to oldhost.txt for get less trouble that possible if you close without stopping the server.
                StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\drivers\\etc\\hosts");
                StreamWriter streamWriter = new StreamWriter("oldhost.txt");

                string line = streamReader.ReadLine();

                while (line != null)
                {
                    line = streamReader.ReadLine();
                    streamWriter.WriteLine(line);
                }

                streamReader.Close();
                streamWriter.Close();
                string payload = "\x0D" + "127.0.0.1 " + authname_textbox.Text;
                //what this does ? all the traffic that will pass through nulled.to will actually go to our server
                File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                //set the server status to ON
                serverstatus_custom.ForeColor = System.Drawing.Color.LimeGreen;
                server_status_custom = true;
                serverstatus_custom.Text = "ON";
            }
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=dcBDijs0Y84");
        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=YcO_dE0Z19g");
        }

        private void forlaxauthmode2_check_CheckedChanged(object sender, EventArgs e)
        {
            forlaxmode2 = !forlaxmode2;
        }

        //by cain button
        private void metroLabel8_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCN9SbyGOmm4cj_xzykyXJPQ?sub_confirmation=1"); // if you want to support me :p
        }

        private void lost_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=WAj08qj3kKw");
        }

        private void fromfile_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Txt Files (*.txt)|*.txt";
            openFileDialog.ShowDialog();
            string tmp = File.ReadAllText(openFileDialog.FileName);
            goodboy_textbox.Text = tmp;
        }

        private void stopbutton_custom_Click(object sender, EventArgs e)
        {
            StreamReader streamReader = new StreamReader("oldhost.txt");
            StreamWriter writer = new StreamWriter("C:\\Windows\\System32\\drivers\\etc\\hosts");
            string line = streamReader.ReadLine();

            while (line != null)
            {
                line = streamReader.ReadLine();
                writer.WriteLine(line);
            }

            writer.Close();
            streamReader.Close();

            //The program search through all process the python process once he found it he kill it
            foreach (var process in Process.GetProcessesByName("python"))
            {
                process.Kill();
            }
            //set the server status to OFF
            serverstatus_custom.ForeColor = System.Drawing.Color.DarkRed;
            server_status_custom = false;
            serverstatus_custom.Text = "OFF";
        }

        private void startbutton_cracked_Click(object sender, EventArgs e)
        {
            if(server_status_cracked == true)
            {
                MessageBox.Show("You didn't stop the server,please stop the server before starting other mode.");
            }
            if(server_status_cracked == false)
            {
                //write the username to the file
                StreamWriter streamWriter1 = new StreamWriter("server\\cracked\\crackedauth.txt");
                StreamWriter config = new StreamWriter("server\\cracked\\config.ini");

                if (forlaxmode == true && forlaxmode2 == false)
                {
                    config.WriteLine("[options]");
                    config.WriteLine("forlaxmode=True");
                    config.WriteLine("forlaxmode2=False");

                }
                else if(forlaxmode == false && forlaxmode2 == true)
                {
                    config.WriteLine("[options]");
                    config.WriteLine("forlaxmode=False");
                    config.WriteLine("forlaxmode2=True");
                }

                //if cracked.to authkey textbox is not empty
                if (secretkey_box.Text != null)
                {
                    config.WriteLine("[secret]");
                    config.WriteLine("secretkey=" + secretkey_box.Text);
                }

                config.Close();
                streamWriter1.WriteLine("{\"auth\":true,\"username\":\"" + cracked_username.Text + "\",\"group\":\"100\"");

                streamWriter1.Close();

                //starting the server
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c \"python server\\cracked\\server.py\"";
                p.StartInfo = startInfo;
                p.Start();

                //saving the host file to oldhost.txt for get less trouble that possible if you close without stoping the server.
                StreamReader streamReader = new StreamReader("C:\\Windows\\System32\\drivers\\etc\\hosts");
                StreamWriter streamWriter = new StreamWriter("oldhost.txt");

                string line = streamReader.ReadLine();

                while (line != null)
                {
                    line = streamReader.ReadLine();
                    streamWriter.WriteLine(line);
                }
                streamReader.Close();
                streamWriter.Close();

                string payload = "\x0D" + "127.0.0.1 cracked.to";
                //what this does ? all the traffic that will pass through cracked.to will actually go to our server
                File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", payload);
                serverstatus.ForeColor = System.Drawing.Color.LimeGreen;
                server_status_cracked = true;
                serverstatus.Text = "ON";
            }
        }
    }
}
