<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="DiscountMaster.aspx.cs" Inherits="FeelYourFood.DiscountMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-heading">
        <h3>Discount
           </h3>
        <ul class="breadcrumb">
            <li><a href="DashBoardForm.aspx">Dashboard</a></li>
            <li class="">Configuration </li>
            <li class="active">Discount Configure</li>
        </ul>

    </div>
    <!-- page heading end-->
    <div class="rest-details">
        <div class="row">
            <div class="col-md-12">
                <div class="row">

                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Discount Name</label>
                            <asp:TextBox ID="discountname" runat="server" placeholder="Discount Name" required="" class="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Discount Percentage </label>
                             <asp:TextBox ID="discountper" runat="server" placeholder="Discount Percentage" type="number"  required="" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Status</label>
                            <div class="form-group">
                                <asp:RadioButtonList ID="rbStatus" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Value="1" Selected="True" />
                                    <asp:ListItem style="margin-left: 50px;" Text="Inactive" Value="0" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div align="center">
                            <asp:Button ID="btnSubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="theme-btn-clear"
                                OnClick="btnClear_Click" formnovalidate="formnovalidate" />
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>
    <div class="rest-details-main">
        <div class="row">
            <div class="col-md-12">

                <div class="rest-details-sub-main">
                    <div style="font-weight: bold; margin-bottom: 15px;">Discount List</div>
                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvdiscount" runat="server" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No Discount Found."
                            DataKeyNames="Id" CssClass="table table-bordered table-striped table-responsive-md"
                            HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvdiscount_PageIndexChanging">
                            
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

                                <asp:BoundField DataField="DiscountName" HeaderText="Discount Name" />
                                <asp:BoundField DataField="DiscountPercentage" HeaderText="Discount Percentage" />

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
                                            CommandArgument='<%# Eval("Id") %>' OnCommand="btnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("Id")%>' OnClientClick="return confirm('Are you sure you want to delete this discount ?');"
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
