<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_AddonItemMaster.aspx.cs" Inherits="FeelYourFood.M_AddonItemMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Addon Item Master </div>
                        <div class="user-form">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Addon Menu</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:DropDownList ID="ddlmenulist" class="default-select wide form-control ms-0" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Addon Item Name</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="itemName" runat="server" placeholder="Addon Item Name" required="" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Description</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="description" runat="server" placeholder="Description" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Item Price</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="price" runat="server" placeholder=" Price" required="" class="form-control">0.00</asp:TextBox>
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
                                        <img id="imagePreview" runat="server"  class="img-fluid" style="display: block; width: 190px; height: 150px;" />
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
                    <div style="font-weight: bold">Addon Item List</div>
                    <hr style="margin-top: 10px;" />
                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvAddonItem" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No Addon Item Found." DataKeyNames="AddOnItemId" CssClass="table table-bordered table-striped table-responsive-md"
                            HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvAddonItem_PageIndexChanging">
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
                                <asp:BoundField DataField="AddonItemName" HeaderText="Addon Item Name" />
                                <asp:BoundField DataField="ItemDescription" HeaderText="Description" />
                                <asp:BoundField DataField="price" HeaderText="Price" />
                                <asp:TemplateField HeaderText="Image">
                                    <ItemTemplate>
                                        <img src='<%# ResolveUrl(Eval("AddOnItemPhoto")?.ToString() ?? "/images/NotAvailable.png") %>'
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
                                            CommandArgument='<%# Eval("AddOnItemId") %>' OnCommand="btnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("AddOnItemId")%>' OnClientClick="return confirm('Are you sure you want to delete this addon item ?');"
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
    <script type="text/javascript">
        // Handle image preview
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
