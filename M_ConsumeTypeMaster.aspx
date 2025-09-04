<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_ConsumeTypeMaster.aspx.cs" Inherits="FeelYourFood.M_ConsumeTypeMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--body wrapper start-->
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Consume Type Master </div>
                        <div class="user-form">

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">

                                        <div class="col-md-4">
                                            <label>Consume Type</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:TextBox ID="consumetype" runat="server" placeholder="Consume Type" required="" class="form-control"></asp:TextBox>
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

                            <div class="row">

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

            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div style="font-weight: bold">Food Consume Type List</div>
                    <hr style="margin-top: 10px;" />
                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvcred" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No Consume Type Found." DataKeyNames="ConsumeTypeId" OnPageIndexChanging="gvrestdetail_PageIndexChanging"
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

                                <asp:BoundField DataField="Type" HeaderText="Consume Type" SortExpression="type" />
                                <asp:TemplateField HeaderText="Photo">
                                    <ItemTemplate>
                                        <img src='<%# ResolveUrl(Eval("Photo")?.ToString() ?? "/images/NotAvailable.png") %>'
                                            alt="Menu Image" style="width: 100px; height: 65px;"
                                            onerror="this.onerror=null; this.src='/images/NotAvailable.png';" />

                                    </ItemTemplate>
                                </asp:TemplateField>

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
                                            CommandArgument='<%#Eval("ConsumeTypeId")%>' OnCommand="ImageBtnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageBtnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("ConsumeTypeId")%>' OnClientClick="return confirm('Are you sure you want to delete this consume Type?');"
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
