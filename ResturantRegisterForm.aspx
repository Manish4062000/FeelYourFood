<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="ResturantRegisterForm.aspx.cs" Inherits="FeelYourFood.ResturantRegisterForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="page-heading">
        <h3>Restaurant Details
            </h3>
        <ul class="breadcrumb">
            <li>
                <a href="#">Dashboard</a>
            </li>
            <li class="active">Restaurant Details </li>
        </ul>

    </div>
    <!-- page heading end-->

    <!--body wrapper start-->
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="rest-details">
                    <div class="user-details">

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Resturant Name</label>
                                    <asp:TextBox ID="restname" runat="server" placeholder="Resturant Name" required="" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Location </label>
                                    <asp:TextBox ID="location" runat="server" placeholder="Location" required="" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Resturant Phone Number </label>
                                    <asp:TextBox ID="restphone" runat="server" placeholder="Resturant Phone Number" required="" class="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>GST Number </label>
                                    <asp:TextBox ID="gstno" runat="server" placeholder="GST Number" required="" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Upload Logo </label>
                                    <asp:FileUpload ID="imageUpload" runat="server" class="form-control" />
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnsubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnsubmit_Click" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                 <img id="imagePreview" runat="server"  class="img-fluid" style="display: block; width: 203px; height: 175px;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
   <script type="text/javascript">
       // Handle image preview
       document.getElementById('<%= imageUpload.ClientID %>').addEventListener('change', function (event) {
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

        // Function to clear the form
        function clearForm() {
            document.querySelector('.needs-validation')?.reset();

            const imagePreview = document.getElementById('<%= imagePreview.ClientID %>');
            if (imagePreview) {
                imagePreview.src = "#"; // or "", depending on your fallback
                imagePreview.style.display = "none";
            }
        }
   </script>
    <!--body wrapper end-->
</asp:Content>
