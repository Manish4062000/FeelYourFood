<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_ItemMaster.aspx.cs" Inherits="FeelYourFood.M_ItemMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style>
        .radio-button-list label {
            margin-right: 30px;
        }

        .radio-button-list input[type="checkbox"] {
            margin-right: 6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Item Master </div>
                        <div class="user-form">

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <label>Select Cuisine</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:DropDownList ID="ddlcuisine" class="default-select wide form-control ms-0" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcuisine_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label>Select Menu</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:DropDownList ID="ddlmenulist" class="default-select wide form-control ms-0" runat="server" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <label>Select Food Category</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:RadioButtonList ID="category" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="radio-button-list">
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">

                                            <div class="col-md-4">
                                                <label>Item Description</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="itemdescription" runat="server" placeholder="Description" required="" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label>Item Name</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="ItemName" runat="server" placeholder="Item Name" required="" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- <div class="col-md-12">
                                    
                                </div>--%>
                                <div class="col-md-12">
                                    <div class="col-md-6">

                                        <div class="row">
                                            <div class="col-md-4">
                                                <label>Item Price</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:TextBox ID="price" runat="server" placeholder="Item Price" required="" class="form-control">0.00</asp:TextBox>
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
                                                <label>Addon Available</label>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:RadioButtonList ID="addon" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="No" Value="0" Selected="True" />
                                                        <asp:ListItem style="margin-left: 50px;" Text="Yes" Value="1" />
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
                                            <img id="imagePreview" runat="server" class="img-fluid" style="display: block; width: 190px; height: 150px;" />
                                        </div>
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
        <div class="col-md-12">
            <div class="rest-details-sub">
                <div style="font-weight: bold">Food Item List</div>
                <hr style="margin-top: 10px;" />
                <div class="bg-wrapper table-responsive">
                    <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No Fodd Item Found." DataKeyNames="ItemId" CssClass="table table-bordered table-striped table-responsive-md"
                        HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvItem_PageIndexChanging">

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
                            <asp:TemplateField HeaderText="Cuisine">
                                <ItemTemplate>
                                    <%# Eval("CuisineName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Menu Name">
                                <ItemTemplate>
                                    <%# Eval("FoodMenuName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Food Category">
                                <ItemTemplate>
                                    <%# Eval("CategoryName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                            <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                            <asp:BoundField DataField="price" HeaderText="Price" />
                            <asp:TemplateField HeaderText="Image">
                                <ItemTemplate>
                                    <img src='<%# ResolveUrl(Eval("ItemPhoto")?.ToString() ?? "/images/NotAvailable.png") %>'
                                        alt="Menu Image" style="width: 100px; height: 65px;"
                                        onerror="this.onerror=null; this.src='/images/NotAvailable.png';" />

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Addon Available">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("AddOnAvailable")) ? "text-success" : "text-danger" %>'>
                                        <i class="fa fa-circle"></i><%# Convert.ToBoolean(Eval("AddOnAvailable")) ? "Yes" : "No" %>
                                    </span>
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
                                        CommandArgument='<%# Eval("ItemId") %>' OnCommand="ImageBtnEdit_Command" formnovalidate="formnovalidate" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                        CommandArgument='<%#Eval("ItemId")%>' OnClientClick="return confirm('Are you sure you want to delete this Item ?');"
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
