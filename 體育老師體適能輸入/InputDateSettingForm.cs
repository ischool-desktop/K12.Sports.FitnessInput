using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using FISCA.LogAgent;
using K12.Data;
using FISCA.Presentation.Controls;

namespace K12.Sports.FitnessInput
{
    public partial class InputDateSettingForm : FISCA.Presentation.Controls.BaseForm
    {
        private const string myDateTimeFormat = "yyyy/MM/dd HH:mm";
        private List<int> Grades;
        Dictionary<int,FitnessInputDateRecord> dfidr;
        public InputDateSettingForm()
        {
            InitializeComponent();

            this.Text = "體適能輸入時間設定";
            Grades = K12.Data.Class.SelectAll().Select(x => (x.GradeYear.HasValue) ? x.GradeYear.Value : -1).Distinct().ToList();
            Grades.Remove(-1);
            Grades.Sort();
            lblSemester.Text = string.Format("{0}學年度", School.DefaultSchoolYear);
            dfidr = tool._A.Select<FitnessInputDateRecord>().ToDictionary(x => x.GradeYear, x => x);
            foreach (int g in Grades)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dgvTimes);
                dgvr.Cells[0].Value = g;
                if (dfidr.ContainsKey(g))
                {
                    dgvr.Cells[1].Value = DateTime.Parse("" + dfidr[g].StartTime).ToString(myDateTimeFormat);
                    dgvr.Cells[2].Value = DateTime.Parse("" + dfidr[g].EndTime).ToString(myDateTimeFormat);
                    dgvr.Tag = dfidr[g];
                }
                dgvTimes.Rows.Add(dgvr);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool valid = true;
            foreach (DataGridViewRow each in dgvTimes.Rows)
            {
                if (!string.IsNullOrEmpty(each.ErrorText))
                {
                    valid = false;
                }
                foreach (DataGridViewCell eachCell in each.Cells)
                {
                    if (!string.IsNullOrEmpty(eachCell.ErrorText))
                    {
                        valid = false;
                    }
                }
                if (!valid) break;
            }
            if (!valid)
            {
                MsgBox.Show("畫面中含有不正確資料。");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            foreach (DataGridViewRow each in dgvTimes.Rows)
            {
                FitnessInputDateRecord fidr;
                DateTime StartTime,EndTime ;
                bool bools,boole ;
                bools = DateTime.TryParse("" + each.Cells[chStartTime.Index].Value, out StartTime);
                boole = DateTime.TryParse("" + each.Cells[chEndTime.Index].Value, out EndTime);
                if (each.Tag != null && each.Tag is FitnessInputDateRecord)
                {
                    fidr = each.Tag as FitnessInputDateRecord;
                    fidr.StartTime = DateTime.Parse("" + each.Cells[chStartTime.Index].Value);
                    fidr.EndTime = DateTime.Parse("" + each.Cells[chEndTime.Index].Value);
                }
                else if (bools && boole)
                {
                    dfidr.Add(int.Parse("" + each.Cells[chGradeYear.Index].Value), new FitnessInputDateRecord()
                    {
                        GradeYear = int.Parse("" + each.Cells[chGradeYear.Index].Value),
                        StartTime = StartTime,
                        EndTime = EndTime

                    });
                }
            }
            dfidr.Values.SaveAll();
            //ApplicationLog.Log("日常生活表現輸入時間", "修改", sb1.ToString() + sb2.ToString());
            MsgBox.Show("儲存成功!!");
            DialogResult = DialogResult.OK;
        }
        private void dgvTimes_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = dgvTimes.Rows[e.RowIndex];
            string startTime = row.Cells[chStartTime.Index].Value + "";
            string endTime = row.Cells[chEndTime.Index].Value + "";

            row.ErrorText = "";
            if (string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                //這裡沒有程式。
            }
            else if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                DateTime? objStart = DateTimeHelper.Parse(startTime);
                DateTime? objEnd = DateTimeHelper.Parse(endTime);

                if (objStart.HasValue && objEnd.HasValue)
                {
                    if (objStart.Value >= objEnd.Value)
                        row.ErrorText = "截止時間必須在開始時間之後。";
                }
            }
            else
                row.ErrorText = "請輸入正確的時間限制資料(必需同時有資料或同時沒有資料)。";
        }
        private void dgvTimes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            string value = "" + dgvTimes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (columnIndex == chStartTime.Index || columnIndex == chEndTime.Index)
            {
                DataGridViewCell cell = dgvTimes.Rows[rowIndex].Cells[columnIndex];
                cell.ErrorText = "";
                if (string.IsNullOrEmpty(value)) //沒有資料就不作任何檢查。
                    return;

                DateTime dt;
                if (!DateTime.TryParse(value, out dt))
                {
                    cell.ErrorText = "日期格式錯誤。";
                }
                else
                {
                    cell.Value = dt.ToString(myDateTimeFormat);
                }
            }
        }
        private void TryToCorrectData(int columnIndex, int rowIndex)
        {
            //row.Cells[columnIndex].ErrorText = string.Empty;
            //DateTime? objStart = DateTimeHelper.ParseGregorian(time, PaddingMethod.First);
            //if (objStart.HasValue)
            //    row.Cells[columnIndex].Value = objStart.Value.ToString(myDateTimeFormat);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
        
}
