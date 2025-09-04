<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="ConfigureKioskForm.aspx.cs" Inherits="FeelYourFood.ConfigureKioskForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style>
        .radio-button-list label {
            margin-right: 30px;
        }

        .radio-button-list input[type="checkbox"] {
            margin-right: 6px;
        }

        .labelstyle {
            font-size: large;
            margin-bottom: 10px;
        }

        .table-responsive-md {
            min-width: 600px;
        }

        .table thead th {
            position: sticky;
            top: 0;
            background-color: #343a40;
            color: white;
            z-index: 1;
        }

        .grid-scroll-container {
            max-height: 400px;
            overflow: auto;
            border: 1px solid #ccc;
        }

        .preview-img {
            display: none;
            margin-top: 5px;
            max-width: 100px;
            max-height: 65px;
        }
        /*checkbox*/
        .control-group h1 {
            margin-top: 0;
            margin-bottom: 20px;
        }

        .control-group {
            vertical-align: top;
            background: #fff;
            text-align: left;
            box-shadow: 0 1px 2px rgba(0,0,0,0.1);
            padding: 20px;
            height: 220px;
        }

        .control {
            display: block;
            position: relative;
            padding-left: 30px;
            margin-bottom: 15px;
            cursor: pointer;
            font-weight: 200;
            letter-spacing: 0.4px;
        }

            .control input {
                position: absolute;
                z-index: -1;
                opacity: 0;
            }

        .control__indicator {
            position: absolute;
            top: 2px;
            left: 0;
            height: 20px;
            width: 20px;
            background: #ffffff;
            border: 1px solid #266774;
        }

        .control--radio .control__indicator {
            border-radius: 50%;
        }

        .control:hover input ~ .control__indicator,
        .control input:focus ~ .control__indicator {
            background: #ccc;
        }

        .control input:checked ~ .control__indicator {
            background: #2aa1c0;
        }

        .control:hover input:not([disabled]):checked ~ .control__indicator,
        .control input:checked:focus ~ .control__indicator {
            background: #0e647d;
        }

        .control input:disabled ~ .control__indicator {
            background: #e6e6e6;
            opacity: 0.6;
            pointer-events: none;
        }

        .control__indicator:after {
            content: '';
            position: absolute;
            display: none;
        }

        .control input:checked ~ .control__indicator:after {
            display: block;
        }

        .control--checkbox .control__indicator:after {
            left: 7px;
            top: 4px;
            width: 5px;
            height: 10px;
            border: solid #fff;
            border-width: 0 2px 2px 0;
            transform: rotate(45deg);
        }

        .control--checkbox input:disabled ~ .control__indicator:after {
            border-color: #7b7b7b;
        }

        .control--radio .control__indicator:after {
            left: 7px;
            top: 7px;
            height: 6px;
            width: 6px;
            border-radius: 50%;
            background: #fff;
        }

        .control--radio input:disabled ~ .control__indicator:after {
            background: #7b7b7b;
        }

        #style-3::-webkit-scrollbar {
            width: 6px;
            background-color: #F5F5F5;
        }

        #style-3::-webkit-scrollbar-thumb {
            background-color: #000000;
        }

        .scrollbar {
            height: 130px;
            width: auto;
            overflow-y: scroll;
            /*margin-bottom: 10px;*/
        }

        .force-overflow {
            min-height: 70px;
        }

        /*checkbox end*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <!-- page heading start-->
    <div class="page-heading">
        <h3>Configure Kiosk</h3>
        <ul class="breadcrumb">
            <li><a href="DashBoardForm.aspx">Dashboard</a></li>
            <li class="#">Configuration</li>
            <li class="active">Configure Kiosk</li>
        </ul>
    </div>
    <!-- page heading end-->

    <div class="rest-details">
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-3">
                        <div class="control-group" style="background: #e8fbff;">

                            <label class="labelstyle">Select Consume Types</label>
                            <hr style="margin-top: 0; margin-bottom: 10px; border-top: 1px dashed #bab4b4;" />
                            <asp:Repeater ID="rptConsumeType" runat="server">
                                <ItemTemplate>
                                    <label class="control control--checkbox">
                                        <%# Eval("Type") %>
                                        <input type="checkbox" id="chkConsume" runat="server" value='<%# Eval("ConsumeTypeId") %>' />
                                        <div class="control__indicator"></div>
                                    </label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="control-group" style="background: #efe39f96;">

                            <label class="labelstyle">Select Food Category</label>
                            <hr style="margin-top: 0; margin-bottom: 10px; border-top: 1px dashed #bab4b4;" />
                            <asp:Repeater ID="rptfoodcategory" runat="server">
                                <ItemTemplate>
                                    <label class="control control--checkbox">
                                        <%# Eval("CategoryName") %>
                                        <input type="checkbox" id="chkCategory" runat="server" value='<%# Eval("TypeId") %>' />
                                        <div class="control__indicator"></div>
                                    </label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="col-md-3">

                        <div class="control-group" style="background: #e2dae3;">

                            <label class="labelstyle">Select Cuisine Types</label>

                            <hr style="margin-top: 0; margin-bottom: 10px; border-top: 1px dashed #bab4b4;" />
                            <div class="scrollbar" id="style-3">
                                <div class="force-overflow">
                                    <asp:Repeater ID="rptCuisine" runat="server">
                                        <ItemTemplate>
                                            <label class="control control--checkbox">
                                                <%# Eval("CuisineName") %>
                                                <input type="checkbox" id="chkCuisine" runat="server" value='<%# Eval("CuisineId") %>' />
                                                <div class="control__indicator"></div>
                                            </label>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="control-group" style="background: #f5fbe7;">

                            <label class="labelstyle">Payment Mode Available</label>
                            <hr style="margin-top: 0; margin-bottom: 10px; border-top: 1px dashed #bab4b4;" />

                            <asp:Repeater ID="rptpayment" runat="server">
                                <ItemTemplate>
                                    <label class="control control--checkbox">
                                        <%# Eval("PaymentMode") %>
                                        <input type="checkbox" id="chkPayment" runat="server" value='<%# Eval("PaymentModeId") %>' />
                                        <div class="control__indicator"></div>
                                    </label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div align="center">
                        <br />
                        <asp:Button ID="Filter" Style="padding:15px"
                            runat="server" class="theme-btn" Text="Save" OnClick="Filter_Click" />
                    </div>
                </div>
                <div class="row">
                    <br />
                    <div class="col-md-12">
                        <label class="labelstyle">Menu Options</label>
                        <hr style="margin-top: 0; margin-bottom: 20px;" />
                    </div>
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
                    <div style="font-weight: bold; margin-bottom: 15px;">Food Menu List</div>

                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvMenu" runat="server" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No Food Menu Found."
                            DataKeyNames="KioskMenuDetailId" CssClass="table table-bordered table-striped table-responsive-md"
                            HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvMenu_PageIndexChanging">

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
                                <asp:BoundField DataField="FoodMenuName" HeaderText="Menu Name" />
                                <asp:TemplateField HeaderText="Image">
                                    <ItemTemplate>
                                        <img src='<%# ResolveUrl(Eval("MenuPhotoPath")?.ToString() ?? "/images/NotAvailable.png") %>'
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
                                            CommandArgument='<%# Eval("KioskMenuDetailId") %>' OnCommand="ImageBtnEdit_Command" formnovalidate="formnovalidate" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/del.png" Width="30" Height="30"
                                            CommandArgument='<%#Eval("KioskMenuDetailId")%>' OnClientClick="return confirm('Are you sure you want to delete this menu ?');"
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

        function triggerPostback() {
            document.forms[0].submit();
        }
    </script>
</asp:Content>
