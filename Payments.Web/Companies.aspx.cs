using Payments.Business.Exceptions;
using Payments.Business.Interfaces;
using Payments.Business.Models;
using Payments.Business.Services;
using Payments.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Payments.Web
{
    public partial class Companies : Page
    {
        private IChannelRepository _channelRepository;
        private ICompanyService _companyService;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _channelRepository = new ChannelRepository();
            _companyService = new CompanyService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                RestoreCompaniesFromSession();
                return;
            }

            RegisterAsyncTask(
                new PageAsyncTask(LoadCompaniesAsync));
        }

        private async Task LoadCompaniesAsync()
        {
            string channelCode = Request.QueryString["canal"];

            if (!IsValidChannelCode(channelCode))
            {
                ShowMessage(
                    "The requested channel is invalid.");

                return;
            }

            try
            {
                ChannelConfiguration configuration =
                    _channelRepository.GetConfiguration(channelCode);

                if (configuration == null)
                {
                    ShowMessage(
                        "The requested channel is not configured.");

                    return;
                }

                pnlMenu.Visible = configuration.ShowMenu;

                List<Company> companies =
                    (List<Company>)await _companyService.GetCompaniesAsync(configuration);

                Session["Companies"] = companies;
                Session["ChannelConfiguration"] = configuration;

                BindCompanies(companies);

                if (companies.Count == 0)
                {
                    ShowMessage(
                        "There are no companies available for this channel.");
                }
            }
            catch (CompanyServiceException ex)
            {
                LogError("Company service error", ex);

                ShowMessage(
                    "The company service is temporarily unavailable. " +
                    "Please try again later.");
            }
            catch (Exception ex)
            {
                LogError("Unexpected error loading companies", ex);

                ShowMessage(
                    "An unexpected error occurred while loading companies.");
            }
        }

        private void RestoreCompaniesFromSession()
        {
            if (!(Session["Companies"] is List<Company> companies))
            {
                ShowMessage(
                    "Your session has expired. Please select the channel again.");

                return;
            }

            BindCompanies(companies);
        }

        private void BindCompanies(List<Company> companies)
        {
            rptCompanies.DataSource = companies;
            rptCompanies.DataBind();
        }

        protected void RptCompanies_ItemDataBound(
            object sender,
            RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            if (!(e.Item.FindControl("imgCompanyLogo") is Image companyLogo) || !(e.Item.DataItem is Company company))
                return;

            Uri logoUri;

            if (Uri.TryCreate(
                company.LogoUrl,
                UriKind.Absolute,
                out logoUri) &&
                (logoUri.Scheme == Uri.UriSchemeHttps ||
                 logoUri.Scheme == Uri.UriSchemeHttp))
            {
                companyLogo.ImageUrl = logoUri.AbsoluteUri;
            }
            else
            {
                companyLogo.Visible = false;
            }
        }

        protected void RptCompanies_ItemCommand(
            object source,
            RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "SelectCompany")
                return;

            try
            {
                int companyId;

                if (!int.TryParse(
                    Convert.ToString(e.CommandArgument),
                    out companyId) ||
                    companyId <= 0)
                {
                    ShowMessage(
                        "The selected company is invalid.");

                    return;
                }

                List<Company> companies =
                    Session["Companies"] as List<Company>;

                if (companies == null)
                {
                    ShowMessage(
                        "Your session has expired. Please select the channel again.");

                    return;
                }

                string channelCode = Request.QueryString["canal"];

                if (!IsValidChannelCode(channelCode))
                {
                    ShowMessage(
                        "The requested channel is invalid.");

                    return;
                }

                ChannelConfiguration configuration =
                    _channelRepository.GetConfiguration(channelCode);

                if (configuration == null)
                {
                    ShowMessage(
                        "The channel configuration could not be retrieved.");

                    return;
                }

                Company selectedCompany =
                    companies.FirstOrDefault(
                        company => company.Id == companyId);

                if (selectedCompany == null)
                {
                    ShowMessage(
                        "The selected company is no longer available.");

                    return;
                }

                string destinationUrl;

                if (!TryGetDestinationUrl(
                    configuration.DestinationPage,
                    out destinationUrl))
                {
                    LogError(
                        "Invalid destination page configuration",
                        new InvalidOperationException(
                            "Configured destination: " +
                            (configuration.DestinationPage ?? "NULL")));

                    ShowMessage(
                        "The destination page is not configured correctly.");

                    return;
                }

                Session["ChannelConfiguration"] = configuration;
                Session["SelectedCompany"] = selectedCompany;
                Session["SelectedChannel"] = configuration.ChannelCode;

                Response.Redirect(
                    ResolveUrl(destinationUrl),
                    false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                LogError(
                    "Error selecting company",
                    ex);

                ShowMessage(
                    "Unable to process the selected company.");
            }
        }


        private static bool TryGetDestinationUrl(
            string destinationPage,
            out string destinationUrl)
        {
            destinationUrl = null;

            if (string.IsNullOrWhiteSpace(destinationPage))
                return false;

            string normalizedPage = destinationPage.Trim();

            if (string.Equals(
                normalizedPage,
                "Destination.aspx",
                StringComparison.OrdinalIgnoreCase))
            {
                destinationUrl = "~/Destination.aspx";
                return true;
            }

            return false;
        }


        private static bool IsValidChannelCode(string channelCode)
        {
            return !string.IsNullOrWhiteSpace(channelCode) &&
                   channelCode.Length <= 10 &&
                   Regex.IsMatch(
                       channelCode,
                       @"^[A-Za-z0-9]+$");
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = HttpUtility.HtmlEncode(message);
            lblMessage.Visible = true;
        }

        private static void LogError(
            string message,
            Exception exception)
        {
            System.Diagnostics.Trace.TraceError(
                "{0}: {1}",
                message,
                exception);
        }
    }
}