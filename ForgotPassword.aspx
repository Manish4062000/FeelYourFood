<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="FeelYourFood.ForgotPassword" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="ThemeBucket">
    <link rel="shortcut icon" href="images/icon.png" type="image/png">

    <title>Feel Your Food. </title>

    <link href="css/style.css" rel="stylesheet">
    <link href="css/style-responsive.css" rel="stylesheet">
</head>

<body class="login-body">
    <div class="container">
        <div class="row row-flex">
            <div class="col-md-3"></div>
            <div class="col-md-6">
                <div class="login-form-section">

                    <div style="text-align: center;">
                        <div>
                            <img src="images/alert1.png" style="height: 50px; margin: 0 auto;" class="img-responsive" alt="Alert Icon">
                        </div>
                        <h2>Forgot Password</h2>
                        <br />
                        <span>Enter your registered email and we'll send you a link to reset your password.</span>
                    </div>
                    <form class="login-formsec" runat="server">
                        <div class="login-btn-fa">
                            <i class="fa fa-user"></i>
                            <input name="email" placeholder="Enter Email Id" required="" type="text" class="mb-3 form-control" />
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnLogin" Style="margin-top: 20px; margin-bottom: 20px; width: 25%" runat="server" Text="Submit" class="btn-main-primary btn btn-primary sign-inbtn" OnClick="btnLogin_Click" />
                            <div style="text-align: center !important; ">
                                <a href="LoginForm.aspx"style="color: #829ba1;" >Back to Login</a>
                            </div>
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

</body>
</html>

