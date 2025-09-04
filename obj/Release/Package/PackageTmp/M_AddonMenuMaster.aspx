<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_AddonMenuMaster.aspx.cs" Inherits="FeelYourFood.M_AddonMenuMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Addon Menu Master </div>
                        <div class="user-form">

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Addon Menu Name</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="menuName" runat="server" placeholder="Addon Menu Name" required="" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Active Status</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:RadioButtonList ID="rbStatus" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Active" Value="1" Selected="True" />
                                                    <asp:ListItem style="margin-left: 50px;" Text="Inactive" Value="0" />
                                                </asp:RadioButtonList>
                                            </div>
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
            </div>
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div style="font-weight: bold">Addon Menu List</div>
                    <hr style="margin-top: 10px;" />
                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvaddonmenu" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No Addon Menu Found." DataKeyNames="AddonMenuId" CssClass="table table-bordered table-striped table-responsive-md"
                            HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvaddonmenu_PageIndexChanging">
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

                                <asp:BoundField DataField="AddonMenuName" HeaderText="Addon Menu Name" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# Convert.ToBoolean(Eval("ActiveStatus")) ? "text-success" : "text-danger" %>'>
                                            <i class="fa fa-circle"></i><%# Convert.ToBoolean(Eval("ActiveStatus")) ? "Active" : "Inactive" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="/images/edit.png" Width="30" Height="30"
                                            CommandArgument='<%# Eval("AddonMenuId") %>' OnCommand="btnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("AddonMenuId")%>' OnClientClick="return confirm('Are you sure you want to delete this addon menu ?');"
                                            OnCommand="btnDelete_Command" formnovalidate="formnovalidate" />
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
