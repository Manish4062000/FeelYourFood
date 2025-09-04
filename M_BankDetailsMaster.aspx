<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_BankDetailsMaster.aspx.cs" Inherits="FeelYourFood.M_BankDetailsMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Bank Master </div>
                        <div class="user-form">

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <label>Bank Name</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="bankname" runat="server" placeholder="Enter Bank Name" required="" class="form-control"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label>Branch</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="branchname" runat="server" placeholder="Enter Branch Name" required="" class="form-control"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <label>IFSC Code</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="ifsc" runat="server" placeholder="Enter IFSC Code" required="" class="form-control"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <label>Account Number</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="accountnumber" runat="server" placeholder="Enter Account Number" required="" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <label>Account Holder Name</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="holdername" runat="server" placeholder="Account Holder Name" required="" class="form-control"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </div>
                </div>
                <div align="center">
                    <asp:Button ID="btnSubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnSubmit_Click" />

                </div>
            </div>
        </div>
    </div>

    </div>
</asp:Content>
