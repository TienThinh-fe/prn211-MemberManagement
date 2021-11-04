using System;
using System.Windows.Forms;
using BusinessObject;
using DataAccess.Repository;
using DataAccess;
using System.Data;

namespace MyStoreWinApp
{
    public partial class frmMemeberManagement : Form
    {
        public frmMemeberManagement()
        {
            InitializeComponent();
        }

        IMemberRepository memberRepository = new MemberRepository();

        private void frmMemeberManagement_Load(object sender, EventArgs e)
        {
            LoadTable();
            if (frmLogin.role == "Member")
            {
                btnInsert.Enabled = false;
            }else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void dgvMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMember.SelectedRows.Count > 0)
            {
                var selectedItem = dgvMember.SelectedRows[0];
                txtMemberId.Text = selectedItem.Cells[0].Value.ToString();
                txtMemberName.Text = selectedItem.Cells[1].Value.ToString();
                txtEmail.Text = selectedItem.Cells[2].Value.ToString();
                txtPassword.Text = selectedItem.Cells[3].Value.ToString();
                txtCity.Text = selectedItem.Cells[4].Value.ToString();
                txtCountry.Text = selectedItem.Cells[5].Value.ToString();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            CheckTextBox();

            MemberObject memAdd = new MemberObject {
                MemberID = int.Parse(txtMemberId.Text),
                MemberName = txtMemberName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                City = txtCity.Text,
                Country = txtCountry.Text
            };

            memberRepository.AddMember(memAdd);
            LoadTable();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Validation.ValidateString(txtMemberId.Text);

            memberRepository.DeleteMember(int.Parse(txtMemberId.Text));
            LoadTable();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckTextBox();

            foreach (MemberObject mem in memberRepository.GetMembers())
            {
                if (mem.MemberID == int.Parse(txtMemberId.Text))
                {
                    mem.MemberName = txtMemberName.Text;
                    mem.Email = txtEmail.Text;
                    mem.Password = txtPassword.Text;
                    mem.City = txtCity.Text;
                    mem.Country = txtCountry.Text;
                }
            }

            LoadTable();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);    
        }


        private void CheckTextBox()
        {
            Validation.ValidateString(txtMemberId.Text);
            Validation.ValidateString(txtMemberName.Text);
            Validation.ValidateEmail(txtEmail.Text);
            Validation.ValidateString(txtPassword.Text);
            Validation.ValidateString(txtCity.Text);
            Validation.ValidateString(txtCountry.Text);
        }

        private void LoadTable()
        {

            DataTable table = new DataTable();
            table.Columns.Add("Member ID", typeof(int));
            table.Columns.Add("Member Name", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Password", typeof(string));
            table.Columns.Add("City", typeof(string));
            table.Columns.Add("Country", typeof(string));


            if (frmLogin.role == "Admin")
            {
                foreach (MemberObject mem in memberRepository.GetMembers())
                {
                    table.Rows.Add(mem.MemberID, mem.MemberName, mem.Email, mem.Password, mem.City, mem.Country);
                }
            }
            else
            {
                foreach (MemberObject mem in memberRepository.GetMembers())
                {
                    if (mem.Email == frmLogin.emailUser)
                        table.Rows.Add(mem.MemberID, mem.MemberName, mem.Email, mem.Password, mem.City, mem.Country);
                }
            }

            dgvMember.DataSource = table;
        }

        
    }
}
