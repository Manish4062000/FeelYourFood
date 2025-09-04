<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="FoodItemData.aspx.cs" Inherits="FeelYourFood.FoodItemData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="page-heading">
        <h3>Food Items</h3>
        <ul class="breadcrumb">
            <li><a href="DashBoardForm.aspx">Dashboard</a></li>
            <li class="">Configuration</li>
            <li class="active">Food Items</li>
        </ul>
    </div>
    <!-- page heading end-->

    <div class="rest-details">
        <div class="row">
            <div class="col-md-12">

                <%--<hr style="margin-top: 0;margin-bottom: 20px;"/>--%>
                <div class="row">

                    <div class="col-md-6">



                        <div class="row">

                            <div class="col-md-4">
                                <label>Select Menu</label>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlmenulist" runat="server" class="form-control" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlmenulist_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Select Food </label>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlitemlist" runat="server" class="form-control" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlitemlist_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Description</label>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:TextBox ID="ItemDescription" runat="server" placeholder="Description" required="" class="form-control"></asp:TextBox>

                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Price</label>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:TextBox ID="Price" runat="server" placeholder="Price" required="" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Active Status </label>
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
                        <div class="row">

                            <div class="col-md-4">
                                <label>Upload Image </label>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:FileUpload ID="myFile" runat="server" class="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-3">
                                <p>&nbsp;&nbsp;&nbsp;&nbsp;</p>
                            </div>
                            <img id="imagePreview" runat="server" src="#" alt="Image Preview" class="img-fluid" style="display: none; width: 190px; height: 150px" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">

            <div align="center">
                <asp:Button ID="btnSubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="theme-btn-clear"
                    OnClick="btnClear_Click" formnovalidate="formnovalidate" />

            </div>
        </div>

    </div>
    <div class="rest-details-main">
        <div class="row">
            <div class="col-md-12">

                <div class="rest-details-sub-main">
                    <div style="font-weight: bold; margin-bottom: 15px;">Food Item List</div>

                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No Food Item Found."
                            DataKeyNames="Id" CssClass="table table-bordered table-striped table-responsive-md"
                            HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvItem_PageIndexChanging">

                            <Columns>
                                <asp:TemplateField HeaderText="Sl.No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Updated">
                                    <ItemTemplate>
                                        <%# Eval("CreatedDate", "{0:dd/MM/yyyy}") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                                <asp:BoundField DataField="description" HeaderText="Description" />
                                <asp:BoundField DataField="Price" HeaderText="Price" />
                                <asp:TemplateField HeaderText="Image">
                                    <ItemTemplate>
                                        <img src='<%# ResolveUrl(Eval("ItemPhoto")?.ToString() ?? "/images/NotAvailable.png") %>'
                                            alt="Menu Image" style="width: 100px; height: 65px;"
                                            onerror="this.onerror=null; this.src='/images/NotAvailable.png';" />

                                    </ItemTemplate>
                                </asp:TemplateField>

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
                                            CommandArgument='<%# Eval("Id") %>' OnCommand="ImageBtnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("Id")%>' OnClientClick="return confirm('Are you sure you want to delete this Item ?');"
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
    <script type="text/javascript">
        document.getElementById('<%= myFile.ClientID %>').addEventListener('change', function (event) {
            const imagePreview = document.getElementById('<%= imagePreview.ClientID %>');
            const file = event.target.files[0];

            if (file) {
                const reader = new FileReader();

                reader.onload = function () {
                    imagePreview.src = reader.result;
                    imagePreview.style.display = 'block';
                };

                reader.readAsDataURL(file);
            }
        });

        function clearForm() {
            document.querySelector('.needs-validation')?.reset();

            const imagePreview = document.getElementById('<%= imagePreview.ClientID %>');
            if (imagePreview) {
                imagePreview.src = "#"; // or "", depending on your fallback
                imagePreview.style.display = "none";
            }
        }
    </script>
</asp:Content>
