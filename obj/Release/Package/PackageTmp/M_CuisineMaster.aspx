<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_CuisineMaster.aspx.cs" Inherits="FeelYourFood.M_CuisineMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--body wrapper start-->
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Cuisine Master </div>
                        <div class="user-form">

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">

                                        <div class="col-md-4">
                                            <label>Cuisine Name</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="cuisinename" runat="server" placeholder="Enter Cuisine Name" required="" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-6">
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
                            <div align="center">
                                <asp:Button ID="btnSubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="theme-btn-clear"
                                    OnClick="btnClear_Click" formnovalidate="formnovalidate" />
                            </div>
                        </div>


                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div style="font-weight: bold">Cuisine List</div>
                    <hr style="margin-top: 10px;" />
                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvcred" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No Cuisine Found." DataKeyNames="CuisineId" OnPageIndexChanging="gvrestdetail_PageIndexChanging"
                            CssClass="table table-responsive-md" BorderColor="#dddddd" HeaderStyle-CssClass="thead-dark"
                            PagerSettings-Mode="Numeric" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                            PagerSettings-NextPageText="Next" PagerSettings-PrevPageText="Previous">


                            <Columns>
                                <asp:TemplateField HeaderText="Sl.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Last Updated">
                                    <ItemTemplate>
                                        <%# Eval("UpdatedDate", "{0:dd/MM/yyyy}") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="CuisineName" HeaderText="Cuisine Name" SortExpression="CuisineName" />
                                <asp:TemplateField HeaderText="Active Status">
                                    <ItemTemplate>
                                        <div class="d-flex align-items-center">
                                            <i class="fa <%# Convert.ToBoolean(Eval("ActiveStatus")) ? "fa-circle text-success" : "fa-circle text-danger" %> me-1"></i>
                                            <%# Convert.ToBoolean(Eval("ActiveStatus")) ? "Active" : "Inactive" %>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageBtnEdit" runat="server" ImageUrl="/images/edit.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("CuisineId")%>' OnCommand="ImageBtnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageBtnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("CuisineId")%>' OnClientClick="return confirm('Are you sure you want to delete this cuisine?');"
                                            OnCommand="ImageBtnDelete_Command" formnovalidate="formnovalidate" />
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



    <!--body wrapper end-->


</asp:Content>
