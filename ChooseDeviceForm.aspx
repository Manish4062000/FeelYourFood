<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="ChooseDeviceForm.aspx.cs" Inherits="FeelYourFood.ChooseDeviceForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <!-- page heading start-->
    <div class="page-heading">
        <h3>Choose Device
            </h3>
        <ul class="breadcrumb">
            <li>
                <a href="#">Dashboard</a>
            </li>
            <li class="active">Choose Device </li>
        </ul>

    </div>
    <!-- page heading end-->
    <div class="rest-details">

        <div class="row">
            <div class="col-md-12">
                <h3 class="sub-hrd">Select Kiosk and Server Type </h3>
                <hr style="margin-top: 10px; margin-bottom: 10px">
            </div>
            <div class="col-md-3"></div>
            <div class="col-md-3">
                <div class="choose-main">
                    <h5>Ordering Kiosk</h5>
                    <figure>
                        <img src="images/order-kiosk.jpg" class="img-responsive" alt="" srcset="">
                    </figure>

                    <label>How many ordering kiosks are required ?</label>
                    <asp:DropDownList ID="ddlKiosk" runat="server" CssClass="form-control">
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="6" Value="6" />
                        <asp:ListItem Text="7" Value="7" />
                        <asp:ListItem Text="8" Value="8" />
                        <asp:ListItem Text="9" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                        <asp:ListItem Text="12" Value="12" />
                    </asp:DropDownList>

                </div>
            </div>
            <div class="col-md-3">
                <div class="choose-main">
                    <h5>Server</h5>
                    <figure>
                        <img src="images/server.jpg" class="img-responsive" alt="" srcset="">
                    </figure>

                    <label>Which type of server do you require ?</label>
                    <asp:DropDownList ID="ddlServer" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Not Required" Value="0" />
                        <asp:ListItem Text="Android Server" Value="androidserver" />
                        <asp:ListItem Text="Desktop Server" Value="desktopserver" />
                    </asp:DropDownList>

                </div>
            </div>
            <div class="col-md-3"></div>
        </div>
        <div class="clearfix"></div>
        <h3 class="sub-hrd" style="margin-top: 10px">Optional </h3>
        <hr style="margin-top: 10px; margin-bottom: 10px">
        <div class="row ">
            <div class="col-md-12 server-main" align="center">
                <div class="col-md-3">
                    <div class="choose-main">
                        <h5>Kitchen Display<h5>
                        <figure>
                            <img src="images/kitchen.jpg" class="img-responsive" alt="" srcset="">
                        </figure>

                        <label>How many kitchen displays are required ?</label>
                        <asp:DropDownList ID="ddlKitchenDisplay" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Not Required" Value="0" />
                            <asp:ListItem Text="1" Value="1" />
                            <asp:ListItem Text="2" Value="2" />
                            <asp:ListItem Text="3" Value="3" />
                            <asp:ListItem Text="4" Value="4" />
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="col-md-3">
                    <div class="choose-main">
                        <h5>QMS (Order Display)</h5>
                        <figure>
                            <img src="images/QMS.jpg" class="img-responsive" alt="" srcset="">
                        </figure>

                        <label>How many QMS screens are required ?</label>
                        <asp:DropDownList ID="ddlQMS" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Not Required" Value="0" />
                            <asp:ListItem Text="1" Value="1" />
                            <asp:ListItem Text="2" Value="2" />
                            <asp:ListItem Text="3" Value="3" />
                            <asp:ListItem Text="4" Value="4" />
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="col-md-3">
                    <div class="choose-main">
                        <h5>Table Tablet</h5>
                        <figure>
                            <img src="images/tablet.jpg" class="img-responsive" alt="" srcset="">
                        </figure>

                        <label>How many table tablet are required ?</label>
                        <asp:DropDownList ID="ddlTableTablet" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Not Required" Value="0" />
                            <asp:ListItem Text="1" Value="1" />
                            <asp:ListItem Text="2" Value="2" />
                            <asp:ListItem Text="3" Value="3" />
                            <asp:ListItem Text="4" Value="4" />
                            <asp:ListItem Text="5" Value="5" />
                            <asp:ListItem Text="6" Value="6" />
                            <asp:ListItem Text="7" Value="7" />
                            <asp:ListItem Text="8" Value="8" />
                            <asp:ListItem Text="9" Value="9" />
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="11" Value="11" />
                            <asp:ListItem Text="12" Value="12" />
                        </asp:DropDownList>

                    </div>
                </div>

            </div>

        </div>
        <div align="center" class="form-group">
            <br />
            <asp:Button ID="btnsubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnsubmit_Click" />
        </div>
    </div>
</asp:Content>
