using Dapper;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Layout.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightChecking.Models.Entities;

namespace WeightChecking
{
    public partial class frmDeleteBox : DevExpress.XtraEditors.XtraForm
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string IdLabel { get; set; } = string.Empty;
        public string Oc { get; set; } = string.Empty;
        public string BoxId { get; set; } = string.Empty;
        public string PassFail { get; set; } = "0";

        private EnumUnit _unit = EnumUnit.L;

        public frmDeleteBox()
        {
            InitializeComponent();
            Load += FrmDeleteBox_Load;
        }

        private void FrmDeleteBox_Load(object sender, EventArgs e)
        {
            _btnDelete.Click += _btnDelete_Click;

            _txtOc.Text = Oc;
            _txtBoxId.Text = BoxId;

            _txtOc.TextChanged += (s, o) => { Oc = _txtOc.Text; };
            _txtBoxId.TextChanged += (s, o) => { BoxId = _txtBoxId.Text; };
            _txtUnit.TextChanged += (s, o) =>
            {
                if (Enum.TryParse<EnumUnit>(_txtUnit.Text, true, out var unit))
                {
                    _unit = unit;
                }
                else
                {
                    MessageBox.Show("Invalid unit. Please enter 'L' or 'P'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _txtUnit.Text = _unit.ToString(); // Reset to previous valid value
                }
            };
        }

        private async void _btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($"Are you sure delete this box?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {

                    using (var dbContext = new ApplicationDbContext(GlobalVariables.ConnectionString))
                    {
                        var boxInfo = await dbContext.TblScanDatas
                            .Where(x => x.OcNo == Oc && x.BoxNo == BoxId && x.Unit == _unit.ToString())
                            .ToListAsync();

                        if (boxInfo == null || (boxInfo != null && boxInfo.Count <= 0))
                        {
                            MessageBox.Show("The box could not be found in the data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        dbContext.TblScanDatas.RemoveRange(boxInfo);
                        await dbContext.SaveChangesAsync();

                        MessageBox.Show("Successfull", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Successfull", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}