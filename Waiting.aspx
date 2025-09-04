<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="Waiting.aspx.cs" Inherits="FeelYourFood.Waiting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
<div class="col-md-12">
    <div class="rest-details-sub">
        <div class="user-details">
            <div class="text-center" style="padding: 40px 60px;">
                <img src="images/1486.gif" style="width:120px;height:120px;" />
                <h2 style="margin-bottom: 20px; color:#ff6a00;">Thank you for submitting!</h2>
                <p style="font-size: 16px; color: #555;">
                    We have successfully received your payment transaction information and uploaded documents.
                    Your submission is currently under verification by our banking partner.
                </p>
                <p style="font-size: 16px; color: #555; margin-top: 10px;">
                    This verification process typically takes between <strong>24 to 48 hours</strong>.
                    You will receive a confirmation once your details are verified and approved.
                </p>
                <p style="font-size: 15px; color: #888; margin-top: 30px;">
                    If you have any questions or need assistance, feel free to reach out to our <strong><a href="https://www.addsofttech.com/support.html">support team</a></strong>.
                </p>
            </div>
        </div>
    </div>
</div>

</asp:Content>
