using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniProj
{
    public partial class Form2 : Form

    {

        SqlCommand cmd;
        SqlConnection cn;
        SqlDataReader dr;

        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Username_Enter(object sender, EventArgs e)
        {
            if (Username.Text == "Username")
            {
                Username.Text = "";
            }
        }

        private void Username_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username.Text))
            {
                Username.Text = "Username";
            }
        }

        private void show_Click(object sender, EventArgs e)
        {
            Password.UseSystemPasswordChar = !Password.UseSystemPasswordChar;
        }


     

        private void Email_Enter_1(object sender, EventArgs e)
        {
            if (Email.Text == "Email")
            {
                Email.Text = "";
            }
        }

        private void Email_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Email.Text))
            {
                Email.Text = "Email";
            }
        }

        private void Password_Enter(object sender, EventArgs e)
        {
            if (Password.Text == "Password")
            {
                Password.Text = "";
                Password.UseSystemPasswordChar = true;
            }
        }

        private void Password_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password.Text))
            {
                Password.Text = "Password";
            }
        }

        private void rePassword_Enter(object sender, EventArgs e)
        {
            if (rePassword.Text == "Repeat Password")
            {
                rePassword.Text = "";
                rePassword.UseSystemPasswordChar = true;
            }
        }

        private void rePassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rePassword.Text))
            {
                rePassword.Text = "Repeat Password";
            }
        }

        private void Birthday_Validating(object sender, CancelEventArgs e)
        {
            int age = DateTime.Now.Year - Birthday.Value.Year - (DateTime.Now.DayOfYear < Birthday.Value.DayOfYear ? 1 : 0);
            if (age < 18) MessageBox.Show("You must be at least 18 to enter", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 start = new Form1();
            start.ShowDialog();
        }


        public static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; // REGEX pattern for acceptable email.

            if (string.IsNullOrEmpty(email)) // return false if email is empty.
                return false;

            Regex regex = new Regex(emailPattern); // create an instance of Regex class.
            return regex.IsMatch(email); // call ismatch method and return boolean value.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cn = new SqlConnection(@"Data Source=LAB109PC07\SQLEXPRESS; Initial Catalog=LogInfo; Integrated Security=True;");
            cn.Open();

            if (Password.Text != string.Empty || rePassword.Text != string.Empty)
            {
                if (Password.Text == rePassword.Text)
                {
                    SqlCommand cmd = new SqlCommand("select * from LoginInfo where Username='" + Username.Text + "'", cn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        MessageBox.Show("Username Already exist please try another ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        dr.Close();

                        int age = DateTime.Now.Year - Birthday.Value.Year - (DateTime.Now.DayOfYear < Birthday.Value.DayOfYear ? 1 : 0);

                        cmd = new SqlCommand("INSERT INTO LoginInfo (Username, Password, Email, Age) VALUES (@Username, @Password, @Email, @Age)", cn);
                        cmd.Parameters.AddWithValue("@Username", Username.Text);
                        cmd.Parameters.AddWithValue("@Password", Password.Text);
                        cmd.Parameters.AddWithValue("@Email", Email.Text);
                        cmd.Parameters.AddWithValue("@Age", age);
                        

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Your Account is created. Please login now.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Form4 login = new Form4();
                        login.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the same password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter value in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            length.Show(); // label for the password lenght requirement
            capital.Show(); // label for the uppercase letter requirement
            lowercase.Show();
            number.Show();

            if (Password.Text.Length >= 8)
                length.ForeColor = Color.Green;
            else
                length.ForeColor = Color.Red;

            if (Password.Text.Any(char.IsUpper))
                capital.ForeColor = Color.Green;
            else
                capital.ForeColor = Color.Red;

            if (Password.Text.Any(char.IsLower))
                lowercase.ForeColor = Color.Green;
            else
                lowercase.ForeColor = Color.Red;

            if (Password.Text.Any(char.IsNumber))
                number.ForeColor = Color.Green;
            else
                number.ForeColor = Color.Red;
        }

        private void Password_Validating(object sender, CancelEventArgs e)
        {
            if (!(Password.Text.Length >= 8 && Password.Text.Any(char.IsUpper) && Password.Text.Any(char.IsLower) && Password.Text.Any(char.IsDigit)))
            {
                MessageBox.Show("One or more password criteria(s) hasn't been met", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Password.SelectAll();
                e.Cancel = true;
            }
        }

        private void Email_Validating(object sender, CancelEventArgs e)
        {
            if (!IsValidEmail(Email.Text))
            {
                MessageBox.Show("Please imput valid email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Email.Focus();
                e.Cancel = true;
            }
        }

        private void Username_Validating(object sender, CancelEventArgs e)
        {
            if (Username.Text.Length < 5)
            {
                MessageBox.Show("Username should be at least 5 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Username.Focus();
                e.Cancel = true;
            }
        }


    }
}
