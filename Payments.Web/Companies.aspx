<%@ Page Language="C#"
    AutoEventWireup="true"
    Async="true"
    CodeBehind="Companies.aspx.cs"
    Inherits="Payments.Web.Companies" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Payment Companies</title>

    <link href="Content/Site.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">

        <asp:Panel ID="pnlMenu" runat="server" CssClass="navigation">
            <div class="navigation-content">
                <a href="Default.aspx">Home</a>
                <a href="Companies.aspx?canal=AB">Channel AB</a>
                <a href="Companies.aspx?canal=CJ">Channel CJ</a>
            </div>
        </asp:Panel>

        <main class="container">

            <header class="page-header">
                <h1>Payment Companies</h1>

                <p>
                    Select the company for which you want to make a payment.
                </p>
            </header>

            <asp:Label
                ID="lblMessage"
                runat="server"
                EnableViewState="false"
                CssClass="message"
                Visible="false" />

            <asp:Repeater
                ID="rptCompanies"
                runat="server"
                OnItemCommand="RptCompanies_ItemCommand"
                OnItemDataBound="RptCompanies_ItemDataBound">

                <HeaderTemplate>
                    <div class="companies-grid">
                </HeaderTemplate>

                <ItemTemplate>

                    <article class="company-card">

                        <asp:Image
                            ID="imgCompanyLogo"
                            runat="server"
                            CssClass="company-logo"
                            AlternateText="Company logo" />

                        <h3 class="company-name">
                            <%#: Eval("Name") %>
                        </h3>

                        <asp:LinkButton
                            ID="btnSelectCompany"
                            runat="server"
                            Text="Select company"
                            CssClass="btn-primary"
                            CommandName="SelectCompany"
                            CommandArgument='<%# Eval("Id") %>'
                            CausesValidation="false" />

                    </article>

                </ItemTemplate>

                <FooterTemplate>
                    </div>
                </FooterTemplate>

            </asp:Repeater>

        </main>

    </form>
</body>
</html>
