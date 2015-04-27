<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:ValidationSummary runat="server" />
        <asp:Label Text="This is a field:" AssociatedControlID="fieldTextBox" runat="server" />
        <asp:TextBox runat="server" id="fieldTextBox"/>
        <asp:RequiredFieldValidator ErrorMessage="Field is required!" ControlToValidate="fieldTextBox" runat="server" Display="none"  />
        <re:recaptchav2 SiteKey="6LfL3gUTAAAAAGI4JXTR_XsPIWwMcnmkYJNyFHfq" SecretKey="6LfL3gUTAAAAANRlzlgaAXePHcjY2cCHWAj8qOLu" errormessage="Human validation failed!" id="recaptchaControl" runat="server" /> 
        <asp:Button Text="Submit" runat="server" OnClick="Unnamed2_Click" />
        <asp:Literal id="resultLiteral" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
