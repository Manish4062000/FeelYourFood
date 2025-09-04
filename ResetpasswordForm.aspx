<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetpasswordForm.aspx.cs" Inherits="FeelYourFood.ResetpasswordForm" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="ThemeBucket">
    <link rel="shortcut icon" href="images/icon.png" type="image/png">

    <title>Feel Your Food. </title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet">

    <link href="css/style.css" rel="stylesheet">
    <link href="css/style-responsive.css" rel="stylesheet">
    <style>
        .login-btn-fa {
    position: relative;
}

.login-btn-fa .toggle-password {
    position: absolute;
    right: 0px;
    top: 50%;
    transform: translateY(-50%);
    cursor: pointer;
    color: #999;
    z-index: 2;
    background:#fff;
}

.login-btn-fa input.form-control {
    padding-right: 40px; /* space for eye icon */
}

    </style>
</head>

<body class="login-body">
    <div class="container">
        <div class="row row-flex">
            <div class="col-md-3"></div>
            <div class="col-md-6">
                <div class="login-form-section">


                    <form class="login-formsec" runat="server">

                        <div>
                            <img src="images/logo.png" style="height: 100px; margin: 0 auto;" class="img-responsive" alt="Alert Icon">
                        </div>
                        <h2 style="text-align: center !important;">Feel Your Food</h2>
                        <p style="margin-bottom: 0px">New Password</p>
                        <div class="login-btn-fa position-relative">
                            <i class="fa fa-lock"></i>
                            <input name="password" id="password" placeholder="Enter new password"
                                required type="password" class="mb-3 form-control pr-5" />
                            <i class="fa fa-eye toggle-password" toggle="#password"></i>
                        </div>


                        <p style="margin-bottom: 0px">Confirm Password</p>
                        <div class="login-btn-fa position-relative">
                            <i class="fa fa-lock"></i>
                            <input name="confirmPassword" id="confirmPassword" placeholder="Confirm password" required type="password" class="mb-3 form-control pr-5" />
                            <i class="fa fa-eye toggle-password" toggle="#confirmPassword"></i>
                        </div>

                        <div class="text-center">
                            <asp:Button ID="btnLogin" Style="margin-top: 20px; margin-bottom: 20px; width: 25%" runat="server" Text="Submit" class="btn-main-primary btn btn-primary sign-inbtn" OnClick="btnLogin_Click" />

                        </div>

                    </form>
                </div>


            </div>
            <div class="col-md-3"></div>
        </div>

    </div>



    <!-- Placed js at the end of the document so the pages load faster -->

    <!-- Placed js at the end of the document so the pages load faster -->
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/modernizr.min.js"></script>
    <script>
        $(document).ready(function () {
            $(".toggle-password").click(function () {
                var input = $($(this).attr("toggle"));
                var type = input.attr("type") === "password" ? "text" : "password";
                input.attr("type", type);

                $(this).toggleClass("fa-eye fa-eye-slash");
            });
        });
    </script>


</body>
</html>

