﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace Trivago.Forms
{
    public partial class AccountSettings : Form
    {
        OracleConnection conn;
        string connST = "Data Source=ORCL;User Id=HR;Password=hr;";
        public AccountSettings()
        {
            InitializeComponent();
            conn = new OracleConnection(connST);
            conn.Open();
            loadUser();
        }
        void loadUser()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from users where id=:loggedID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("loggedID", Login.userID);
            OracleDataReader dr = cmd.ExecuteReader();
            string gender;
            string dateBirth;
            if (dr.Read())
            {
                first.Text = dr["first_name"].ToString();
                last.Text = dr["last_name"].ToString();
                email.Text = dr["email"].ToString();
                password.Text = dr["password"].ToString();
                mobile.Text = dr["mobile"].ToString();
                gender = dr["gender"].ToString();
                dateBirth = dr["date_of_birth"].ToString();
                dateofbirth.Value = DateTime.Parse(dateBirth);
                if (gender == "m")
                {
                    m.Checked = true;
                    f.Checked = false;
                }
                else { f.Checked = true; m.Checked = false; }
       
            }
            dr.Close();
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        Boolean validate()
        {
            if (email.Text == "" || first.Text == "" || last.Text == "" || mobile.Text == "" || password.Text == "")
            {
                MessageBox.Show("Enter Missing Data");
                return true;
            }
            if (mobile.Text.ToString().Length != 11)
            {
                MessageBox.Show("Enter Valid Mobile Number");
                return true;

            }
            if (password.Text.ToString().Length < 6)
            {
                MessageBox.Show("Password must contain at least 6 characters");
                return true;

            }
            if (!IsValidEmail(email.Text))
            {
                MessageBox.Show("Enter Valid E-mail");
                return true;
            }
            return false;


        }
        void update()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "update users set first_name=:nfirstname, last_name=:nlastname, email=:nemail, password=:npassword, gender=:ngender, date_of_birth=:ndateofbirth where id=:userid";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("nfirstname", first.Text);
            cmd.Parameters.Add("nlastname", last.Text);
            cmd.Parameters.Add("nemail", email.Text);
            cmd.Parameters.Add("npassword", password.Text);
            if (f.Checked)
                cmd.Parameters.Add("gender", 'f');
            else
                cmd.Parameters.Add("gender", 'm');
            cmd.Parameters.Add("ndateofbirth", dateofbirth.Value);
            cmd.Parameters.Add("userid", Login.userID);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Updated Successfully!");
                loadUser();
            }
            else MessageBox.Show("Something Wrong!");
        }
        private void updateBTN_Click(object sender, EventArgs e)
        {
            if (validate()) return;
            update();
        }

        private void dateofbirth_onValueChanged(object sender, EventArgs e)
        {

        }
    }
}