<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_SubscriptionMaster.aspx.cs" Inherits="FeelYourFood.M_SubscriptionMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Subscription Master </div>
                        <div class="user-form">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Select Device </label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:DropDownList ID="ddldevice" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Device Price</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="deviceprice" runat="server" placeholder=" Enter Device Price" type="number" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Subscription Price</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="subscriptionprice" runat="server" placeholder=" Enter Subscription Price" type="number" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4 ">
                                            &nbsp;&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 ">
                                            &nbsp;</div>
                                    </div>
                                    <div class="row ">
                                        <p><br/><br /><br /><br />(Quarterly Price)</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnSubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="theme-btn-clear" OnClick="btnClear_Click" formnovalidate="formnovalidate" />
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div style="font-weight: bold">Subscription List</div>
                    <hr style="margin-top: 10px;" />
                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvsubscription" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No Subscription Found." DataKeyNames="Id" CssClass="table table-bordered table-striped table-responsive-md"
                            HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvsubscription_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl.No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Updated">
                                    <ItemTemplate>
                                        <%# Eval("UpdatedDate", "{0:dd/MM/yyyy}") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DeviceName" HeaderText=" Device Name" />
                                <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                <asp:BoundField DataField="DevicePrice" HeaderText="Device Price" />
                                <asp:BoundField DataField="SubscriptionPrice" HeaderText="Subscription Price" />

                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="/images/edit.png" Width="30" Height="30"
                                            CommandArgument='<%# Eval("Id") %>' OnCommand="btnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                        </asp:GridView>

                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
