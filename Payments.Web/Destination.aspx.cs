using Payments.Business.Models;
using System;
using System.Web;
using System.Web.UI;

namespace Payments.Web
{
    public partial class Destination : Page
    {
        private const string SelectedCompanySessionKey = "SelectedCompany";
        private const string SelectedChannelSessionKey = "SelectedChannel";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPaymentInformation();
            }
        }

        private void LoadPaymentInformation()
        {
            try
            {
                Company selectedCompany =
                    Session[SelectedCompanySessionKey] as Company;

                string channelCode =
                    Session[SelectedChannelSessionKey] as string;

                if (selectedCompany == null ||
                    string.IsNullOrWhiteSpace(channelCode))
                {
                    ReturnToCompanyList(channelCode);
                    return;
                }

                if (selectedCompany.Id <= 0 ||
                    string.IsNullOrWhiteSpace(selectedCompany.Name))
                {
                    ShowError(
                        "The selected company information is invalid.");

                    return;
                }

                lblChannelCode.Text =
                    HttpUtility.HtmlEncode(channelCode);

                lblCompanyName.Text =
                    HttpUtility.HtmlEncode(selectedCompany.Name);

                lblCompanyCode.Text =
                    selectedCompany.Id.ToString();

                lblStatus.Text = "Ready";

                btnContinue.Enabled = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "Error loading payment information: {0}",
                    ex);

                ShowError(
                    "An unexpected error occurred while loading payment information.");
            }
        }

        protected void BtnContinue_Click(object sender, EventArgs e)
        {
            Company selectedCompany =
                Session[SelectedCompanySessionKey] as Company;

            string channelCode =
                Session[SelectedChannelSessionKey] as string;

            if (selectedCompany == null ||
                string.IsNullOrWhiteSpace(channelCode))
            {
                ReturnToCompanyList(channelCode);
                return;
            }

            lblStatus.Text = "Payment workflow ready.";

            lblError.Text =
                "The payment processing functionality is pending implementation.";

            lblError.Visible = true;

            btnContinue.Enabled = false;
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            string channelCode =
                Session[SelectedChannelSessionKey] as string;

            ReturnToCompanyList(channelCode);
        }

        private void ReturnToCompanyList(string channelCode)
        {
            string destinationUrl = "~/Companies.aspx";

            if (!string.IsNullOrWhiteSpace(channelCode))
            {
                destinationUrl +=
                    "?canal=" + HttpUtility.UrlEncode(channelCode);
            }

            Response.Redirect(
                ResolveUrl(destinationUrl),
                false);

            Context.ApplicationInstance.CompleteRequest();
        }

        private void ShowError(string message)
        {
            lblError.Text = HttpUtility.HtmlEncode(message);
            lblError.Visible = true;

            lblStatus.Text = "Incomplete";

            btnContinue.Enabled = false;
        }
    }
}