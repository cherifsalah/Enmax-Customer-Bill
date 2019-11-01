//author: Salah Hadj Cherif
//created on 22 june 2019
//Contain code that calculates a customer bill for the city power company


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Customer_bill
{

    public partial class frm_customerbill : Form
    {
        const decimal BASE_RESIDENTIAL = 6m;//flate rate of 6
        const decimal BASE_COMMERCIAL = 60m;//flat rate of 60 for the first 1000kwh
        const decimal FRPH_INDUSTRIAL = 76m;//flat rate peak hours for first 1000kwh
        const decimal FROFFPH_INDUSTRIAL = 40m;//flat rate Off peak hours for first 1000kwh
        const decimal RATE_RESIDENTIAL = 0.052m;//rate for residential by kwh
        const decimal RATE_COMMERCIAL = 0.045m;//rate for commercial by kwh
        const decimal RATEPH_INDUSTRIAL = 0.065m;//rate for industrial by kwh on peak hours
        const decimal RATEOFFPH_INDUSTRIAL = 0.028m;//rate for industrial by kwh on off peak hours
        const int INDEX_TAB_RESIDENTIAL = 0;//Index of tab Residential
        const int INDEX_TAB_COMMERCIAL = 1;// Index of tab Commercial
        const int INDEX_TAB_INDUSTRIAL = 2;// Index of tab Industrial

        //Initialise all component of frm_customerbill
        public frm_customerbill()
        {
            InitializeComponent();
        }
        //if user click on Exit Application end 
        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Calculate bill from KWH entered by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void btncalculate_Click(object sender, EventArgs e)
        {
            decimal amount=0;// Cost calculated 
            decimal Kwh;//Kwh consumed by customer
            decimal PHKwh;// Kwh consumed by industrial customer on Peak hours
            decimal OffPHKwh;//Kwh consumed by industrial customer on Off Peak hours
            decimal amount_PH=0;//cost calculated for Peak hours Kwh
            decimal amount_OffPH=0;//cost calculated for Off Peak hours Kwh

            //get currant tab
            //get parameter (kwh) needed to calculate amount 
            //calculate amount
            //display values
            int index = tab_customer.SelectedIndex; // index of the currant tab or selected tab 
            //currant tab is RESIDENTIAL
            try
            {
                if (index == INDEX_TAB_RESIDENTIAL)
                {
                    if (Validator.IsPresent(txtresidential_kwh, "KWH") &&
                    Validator.IsNonNegativeDecimal(txtresidential_kwh, "KWH"))
                    {
                        Kwh = Convert.ToDecimal(txtresidential_kwh.Text);
                        amount = GetAmount_Residential(Kwh);
                        txt_amount_residential.Text = amount.ToString("c");
                    }
                }
                else //currant tab is COMMERCIAL
                if (index == INDEX_TAB_COMMERCIAL)
                {

                    if (Validator.IsPresent(txt_commercial_kwh, "KWH") &&
                    Validator.IsNonNegativeDecimal(txt_commercial_kwh, "KWH"))
                    {
                        Kwh = Convert.ToDecimal(txt_commercial_kwh.Text);
                        amount = GetAmount_Commercial(Kwh);
                        txt_amount_commercial.Text = amount.ToString("c");
                    }
                }
                else//currant tab is INDUSTRIAL
                if (index == INDEX_TAB_INDUSTRIAL)
                {

                    //calculate peak hours amount
                    if (Validator.IsPresent(txt_peakhourskwh, "Peak Hours KWH") &&
                    Validator.IsNonNegativeDecimal(txt_peakhourskwh, "Peak Hours KWH"))
                    {
                        PHKwh = Convert.ToDecimal(txt_peakhourskwh.Text);
                        amount_PH = GetAmount_Industrial_PeakHours(PHKwh);
                        txt_peakhours_amount.Text = amount_PH.ToString("c");
                    }
                    //calculate Off peak hours amount
                    if (Validator.IsPresent(txt_offpeakhourskwh, "Off Peak Hours KWH") &&
                    Validator.IsNonNegativeDecimal(txt_offpeakhourskwh, "Off Peak Hours KWH"))
                    {
                        OffPHKwh = Convert.ToDecimal(txt_offpeakhourskwh.Text);
                        amount_OffPH = GetAmount_Industrial_OffPeakHours(OffPHKwh);
                        txt_Offpeakhours_amount.Text = amount_OffPH.ToString("c");
                    }

                    //calculate total amount
                    amount = amount_PH + amount_OffPH;
                    txt_amount_industrial.Text = amount.ToString("c");
                }
            }
            // on exception show this message 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" +
                    ex.GetType().ToString() + "\n" +
                    ex.StackTrace, "Exception");
            }

        }
        /// <summary>
        /// calculate charge Amount for residential 
        /// </summary>
        /// <param name="kwh"> kwh consumed </param>
        /// <returns>Amount</returns>
        private Decimal GetAmount_Residential(decimal kwh)
        {
            return BASE_RESIDENTIAL + (kwh * RATE_RESIDENTIAL);

        }
        /// <summary>
        /// calculate charge Amount for Commercial
        /// </summary>
        /// <param name="kwh"> kwh consumed </param>
        /// <returns>Amount</returns>
        private Decimal GetAmount_Commercial(decimal kwh)
        {
            decimal amount = BASE_COMMERCIAL;//contain cost for a commercial customer
            if (kwh > 1000)
            {
                amount = amount + ((kwh - 1000) * RATE_COMMERCIAL);
            }
            return amount;
        }
        /// <summary>
        /// calculate charge Amount for KWH consumed on Peak hours  
        /// </summary>
        /// <param name="PHkwh">Peak hours kwh consumed </param>
        /// <returns>Amount</returns>
        private Decimal GetAmount_Industrial_PeakHours(decimal PHkwh)
        {
            decimal amount = FRPH_INDUSTRIAL;//contain cost for a Indutrial customer On Peak hours
            if (PHkwh > 1000)
            {
                amount = amount + ((PHkwh - 1000) * RATEPH_INDUSTRIAL);
            }
            return amount;
        }
        /// <summary>
        /// calculate charge Amount for KWH consumed on off Peak hours  
        /// </summary>
        /// <param name="OffPHkwh">Off Peak hours kwh consumed </param>
        /// <returns>Amount</returns>
        private Decimal GetAmount_Industrial_OffPeakHours(decimal OffPHkwh)
        {
            decimal amount = FROFFPH_INDUSTRIAL;//contain cost for a Indutrial customer On Off Peak hours
            if (OffPHkwh > 1000)
            {
                amount = amount + ((OffPHkwh - 1000) * RATEOFFPH_INDUSTRIAL);
            }
            return amount;
        }
        /// <summary>
        /// calculate total charge Amount for Industrial   
        /// </summary>
        /// <param name="OffPHkwh">Off Peak hours kwh consumed </param>
        /// <param name="PHkwh">Peak hours kwh consumed </param>
        /// <returns>Amount</returns>
        private Decimal GettotalAmount_Industrial(decimal OffPHkwh, decimal PHkwh)
        {
            //amount will contain a total amount for an Industrial customer
            decimal amount= GetAmount_Industrial_PeakHours(PHkwh) + GetAmount_Industrial_OffPeakHours(OffPHkwh);
            return amount;
        }
        //If user click on Clear this method will clear all the Text Box in the form
        private void btn_clear_Click(object sender, EventArgs e)
        {
            //Clear all textbox controls in the form
            
            // Clear controls in tab  RESIDENTIAL
                txtresidential_kwh.Text = "";
                txt_amount_residential.Text = "";

            //Clear controls in tab COMMERCIAL
                txt_commercial_kwh.Text = "";
                txt_amount_commercial.Text = "";

            //Clear controls in tab Industrial
                txt_offpeakhourskwh.Text = "";
                txt_Offpeakhours_amount.Text = "";
                txt_peakhourskwh.Text = "";
                txt_peakhours_amount.Text = "";
                txt_amount_industrial.Text = "";
            
        }
        //when form load put in the center of screen
        
        private void frm_customerbill_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            
         }
        //set focus to txtresidential_kwh 
        private void frm_customerbill_Shown(object sender, EventArgs e)
        {
            txtresidential_kwh.Focus();
        }
       
    }
}
