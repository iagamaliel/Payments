<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="Payments.Web.Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Payments</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="container">
            <h1>Payments</h1>
            <p>Seleccione el canal de acceso:</p>

            <a class="btn" href="Companies.aspx?canal=AB">
                Canal AB
            </a>

            <a class="btn" href="Companies.aspx?canal=CJ">
                Canal CJ
            </a>
        </main>
    </form>
</body>
</html>
