<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="ProfileForm.aspx.cs" Inherits="FeelYourFood.ProfileForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- page heading start-->
    <div class="page-heading">
        <h3>Profile</h3>
        
    </div>
    <!-- page heading end-->

    <div class="rest-details">
        <div class="row">
            <div class="col-md-12">


                <div class="form-horizontal">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-md-3 control-label">Name:</label>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-md-3 control-label">Mobile No:</label>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-md-3 control-label">Email:</label>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-md-3 control-label">Restaurant Name:</label>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtRestaurant" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-md-3 control-label">Address:</label>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" style="min-height: 98px;" Enabled="false" TextMode="MultiLine" Rows="2" />
                            </div>
                        </div>
                    </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="col-md-3 control-label">Restaurant Phone:</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtRestPhone" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-md-3 control-label">GST No:</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-md-3 control-label">Restaurant Logo:</label>
                        <div class="col-md-9">
                            <asp:Image ID="imgLogo" runat="server" Width="150px" Height="150px" class="img-fluid" />
                        </div>
                    </div>
                </div>

            </div>

            </div>
        </div>
    </div>
</asp:Content>
