<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="LoginForm.aspx.cs" Inherits="FeelYourFood.LoginForm" %>

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
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet">

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
        height: 97%;
}

.login-btn-fa input.form-control {
    padding-right: 40px; /* space for eye icon */
}

    </style>
</head>

<body class="login-body">
    <div class="container">
        <div class="row row-flex">
            <div class="col-md-2"></div>
            <div class="col-md-8">
                <div class="col-md-6 col-sm-6 pdl pdr">
                    <div class="login-form-section">
                        <h2>Login  </h2>
                        <span>Sign in to your account</span>

                        <form class="login-formsec"  runat="server">
                            <div class="login-btn-fa">
                                <i class="fa fa-user"></i>
                                <input name="email" placeholder="Enter Email Id" required="" type="text" class="mb-3 form-control">
                            </div>
                            <div class="login-btn-fa position-relative">
                                <i class="fa fa-lock"></i>
                                <input  name="password" id="password"  placeholder="Enter your password" required type="password" class="mb-3 form-control pr-5" /><i class="fa fa-eye toggle-password" toggle="#password"></i>
                            </div>
                                 <%--<div class="g-recaptcha" data-sitekey="6LfKWWIrAAAAALXo5RqjYG_cEL_RF64Q_weAJj3M"></div>--%>
                            <div class="forget-psw">
                                <a href="ForgotPassword.aspx">Forgot Password?</a>
                            </div>
                            <asp:Button ID="btnLogin" style="margin-top: 35px;" runat="server" Text="Sign In" class="btn-main-primary btn btn-primary sign-inbtn" OnClick="btnLogin_Click" />
                            
                        </form>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 pdl pdr">
                    <div class="login-form-section2">
                        <img src="images/logo.png" class="img-responsive" alt="" srcset="">
                        <h2>Sign up </h2>

                        <p>Welcome back! Log in to your <strong>Feel Your Food</strong> account to access powerful tools for restaurant setup, configuration, and system management—all on our cloud-based platform. Don’t have an account yet? Sign up now to simplify your restaurant onboarding and unlock full access to all features.</p>



                        <a href="RegistrationForm.aspx" class="register-btn">Register Now!</a>

                    </div>
                </div>
            </div>

            <div class="col-md-2"></div>
        </div>

    </div>

    <!-- Placed js at the end of the document so the pages load faster -->
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>

    <!-- Placed js at the end of the document so the pages load faster -->
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/modernizr.min.js"></script>
    <script>
    $(document).ready(function () {
        $(".toggle-password").click(function () {
            const input = $($(this).attr("toggle"));
            const type = input.attr("type") === "password" ? "text" : "password";
            input.attr("type", type);
            $(this).toggleClass("fa-eye fa-eye-slash");
        });
    });
    </script>

</body>
</html>
