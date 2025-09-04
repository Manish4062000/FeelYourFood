<%@ Page Title="Settings" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_SettingsForm.aspx.cs" Inherits="FeelYourFood.M_SettingsForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <style>
        .rest-details {
            padding-top: 0px !important;
            padding-bottom: 0px !important;
        }

        .buttonpart .btn {
            background: #05343e;
            color: white;
        }

        .login-btn-fa {
            position: relative;
        }

            .login-btn-fa .toggle-password {
                position: absolute;
                right: 35px;
                top: 50%;
                transform: translateY(-50%);
                cursor: pointer;
                color: #999;
                z-index: 2;
                background: #fff;
            }

            .login-btn-fa input.form-control {
                padding-right: 40px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-heading">
        <h3>Settings</h3>
    </div>

    <div class="rest-details">
        <div class="row">
            <div class="panel-heading"><strong>Edit Profile</strong></div>
            <div class="panel-body">
                <asp:Label ID="lblProfileMsg" runat="server" ForeColor="Green" />
                <div class="form-horizontal">
    <div class="col-md-8">
        <div class="form-group">
            <label class="col-md-3 control-label">Name:</label>
            <div class="col-md-9">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-md-3 control-label">Mobile No:</label>
            <div class="col-md-9">
                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-md-3 control-label">Email:</label>
            <div class="col-md-9">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group buttonpart">
            <div class="col-sm-offset-3 col-md-9">
                <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" CssClass="btn " OnClick="btnUpdateProfile_Click" />
            </div>
        </div>
    </div>
    <div class="col-md-4"></div>
</div>
            </div>
        </div>
    </div>
    <!-- Change Password Section -->
    <div class="rest-details">
        <div class="row">
            <div class="panel-heading"><strong>Change Password</strong></div>
            <div class="panel-body">
                <asp:Label ID="lblPasswordMsg" runat="server" ForeColor="Red" />
                <div class="form-horizontal">
                    <div class="col-md-8">
                        <div class="form-group">
                            <label class="col-md-3 control-label">Old Password:</label>
                            <div class="col-md-9 login-btn-fa">

                                <asp:TextBox ID="txtOldPassword" ClientIDMode="Static" runat="server" CssClass="form-control" TextMode="Password" />
                                <span toggle="#txtOldPassword" class="fa fa-fw fa-eye toggle-password"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-md-3 control-label">New Password:</label>
                            <div class="col-md-9 login-btn-fa">
                                <asp:TextBox ID="txtNewPassword" ClientIDMode="Static" runat="server" CssClass="form-control" TextMode="Password" />
                                <span toggle="#txtNewPassword" class="fa fa-fw fa-eye toggle-password"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-md-3 control-label">Confirm Password:</label>
                            <div class="col-md-9 login-btn-fa">
                                <asp:TextBox ID="txtConfirmPassword" ClientIDMode="Static" runat="server" CssClass="form-control" TextMode="Password" />
                                <span toggle="#txtConfirmPassword" class="fa fa-fw fa-eye toggle-password"></span>
                            </div>
                        </div>

                        <div class="form-group buttonpart">
                            <div class="col-sm-offset-3 col-md-9">
                                <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn" OnClick="btnChangePassword_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4"></div>
                </div>
            </div>
        </div>
    </div>
      <script>
      $(document).ready(function () {
          $(".toggle-password").each(function () {
              var target = $($(this).attr("toggle"));
              console.log("Targeting: ", target);
          });

          $(".toggle-password").click(function () {
              const input = $($(this).attr("toggle"));
              const type = input.attr("type") === "password" ? "text" : "password";
              input.attr("type", type);
              $(this).toggleClass("fa-eye fa-eye-slash");
          });
      });
      </script>
</asp:Content>
