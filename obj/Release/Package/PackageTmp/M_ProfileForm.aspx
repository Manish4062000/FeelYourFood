<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_ProfileForm.aspx.cs" Inherits="FeelYourFood.M_ProfileForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- page heading start-->
    <div class="page-heading">
        <h3>Profile</h3>

    </div>
    <!-- page heading end-->

    <div class="rest-details">
        <div class="row">
            <div class="col-md-2"></div>
        <div class="col-md-8">


            <div class="form-horizontal">
                <table class="table table-bordered">
                    <tr>
                        <th>Name</th>
                        <td>
                            <asp:Label ID="lblName" runat="server" /></td>
                    </tr>
                    <tr>
                        <th>Mobile No</th>
                        <td>
                            <asp:Label ID="lblMobile" runat="server" /></td>
                    </tr>
                    <tr>
                        <th>Email</th>
                        <td>
                            <asp:Label ID="lblEmail" runat="server" /></td>
                    </tr>

                </table>

            </div>

        </div>
        <div class="col-md-2"></div>
    </div>
    </div>

</asp:Content>

