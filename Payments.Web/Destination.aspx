<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Destination.aspx.cs" Inherits="Payments.Web.Destination" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Payment Destination</title>

    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
            background: #f4f6f9;
            color: #263238;
        }

        .header {
            background: #17365d;
            color: white;
            padding: 22px 40px;
        }

        .header h1 {
            margin: 0;
            font-size: 23px;
        }

        .container {
            max-width: 950px;
            margin: 45px auto;
            padding: 0 20px;
        }

        .card {
            background: white;
            border-radius: 10px;
            padding: 35px;
            box-shadow: 0 3px 15px rgba(0,0,0,.06);
        }

        .breadcrumb {
            color: #64748b;
            font-size: 13px;
            margin-bottom: 25px;
        }

        .card h2 {
            margin-top: 0;
            color: #17365d;
            font-size: 25px;
        }

        .description {
            color: #64748b;
            line-height: 1.6;
        }

        .information {
            margin-top: 30px;
            padding: 20px;
            background: #f1f5f9;
            border-radius: 7px;
        }

        .information p {
            margin: 10px 0;
        }

        .actions {
            margin-top: 30px;
            display: flex;
            gap: 12px;
        }

        .button {
            display: inline-block;
            padding: 12px 22px;
            border-radius: 6px;
            text-decoration: none;
            font-weight: bold;
            font-size: 14px;
            border: none;
            cursor: pointer;
        }

        .primary {
            background: #17365d;
            color: white;
        }

        .secondary {
            background: #e2e8f0;
            color: #334155;
        }

        .error {
            display: block;
            margin-top: 20px;
            color: #dc2626;
        }

        @media (max-width: 600px) {
            .header {
                padding: 20px;
            }

            .card {
                padding: 22px;
            }

            .actions {
                flex-direction: column;
            }

            .button {
                text-align: center;
            }
        }
    </style>
    
</head>

<body>

    <form id="form1" runat="server">

        <header class="header">
            <h1>Payment Platform</h1>
        </header>

        <main class="container">

            <section class="card">

                <h2>Payment Destination</h2>

                <p class="description">
                    Welcome to the payment destination page.
                    Review the selected company and continue
                    with your payment process.
                </p>

                <div class="information">

                    <p>
                        <strong>Channel:</strong>
                        <asp:Label
                            ID="lblChannelCode"
                            runat="server"
                            Text="N/A" />
                    </p>

                    <p>
                        <strong>Company:</strong>
                        <asp:Label
                            ID="lblCompanyName"
                            runat="server"
                            Text="Not selected" />
                    </p>

                    <p>
                        <strong>Company Code:</strong>
                        <asp:Label
                            ID="lblCompanyCode"
                            runat="server"
                            Text="N/A" />
                    </p>

                    <p>
                        <strong>Status:</strong>
                        <asp:Label
                            ID="lblStatus"
                            runat="server"
                            Text="Ready" />
                    </p>

                </div>

                <asp:Label
                    ID="lblError"
                    runat="server"
                    CssClass="error"
                    Visible="false" />

                <div class="actions">

                    <asp:Button
                        ID="btnContinue"
                        runat="server"
                        Text="Continue Payment"
                        CssClass="button primary"
                        OnClick="BtnContinue_Click" />

                    <asp:Button
                        ID="btnBack"
                        runat="server"
                        Text="Back to Companies"
                        CssClass="button secondary"
                        CausesValidation="false"
                        OnClick="BtnBack_Click" />

                </div>

            </section>

        </main>

    </form>

</body>
</html>
