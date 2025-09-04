<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationForm.aspx.cs" Inherits="FeelYourFood.RegistrationForm" %>

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
                background: #fff;
                height: 42px;
                margin-right: 2px;
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
                    <div class="reg-form-section">
                        <h2 style="margin-top: 10px;">Register  </h2>
                        <span>Create your account</span>

                        <form class="login-formsec" runat="server">
                            <div class="login-btn-fa">
                                <i class="fa fa-user-circle"></i>
                                <input name="name" placeholder="Enter your name" required="" type="text" class="mb-3 form-control">
                            </div>
                            <div class="login-btn-fa">
                                <i class="fa fa-envelope"></i>
                                <input name="email" placeholder="Enter your email" required="" type="text" class="mb-3 form-control">
                            </div>
                            <div class="login-btn-fa ">
                                <i class="fa fa-key"></i>
                                <input name="password" id="password" placeholder="Enter your password" required type="password" class="mb-3 form-control " />
                                <i class="fa fa-eye toggle-password" toggle="#password"></i>
                            </div>

                            <div class="login-btn-fa ">

                                <i class="fa fa-unlock"></i>
                                <input name="confirmPassword" id="confirmPassword" placeholder="Confirm Password" required type="password" class="mb-3 form-control pr-5" />
                                <i class="fa fa-eye toggle-password" toggle="#confirmPassword"></i>
                            </div>

                            <div class="login-btn-fa">
                                <i class="fa fa-phone"></i>
                                <input name="phone" placeholder="Mobile Number" required="" type="text" class="mb-3 form-control" value="" maxlength="10">
                            </div>
                            <div>
                                <div class="forget-psw">
                                    Already have account ?
                                    <a href="LoginForm.aspx">Login</a>
                                </div>
                                <asp:Button ID="btnRegister" runat="server" Text="Create Account" class="btn-main-primary btn btn-primary sign-inbtn" OnClick="btnRegister_Click" />

                                <%--<button type="button" class="btn-main-primary btn btn-primary sign-inbtn">Create Account</button>--%>
                            </div>
                        </form>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 pdl pdr">
                    <div class="reg-form-section2">
                        <img src="images/reg-image.jpg" alt="" class="img-responsive" srcset="">
                    </div>
                </div>
            </div>

            <div class="col-md-2"></div>
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
                const input = $($(this).attr("toggle"));
                const type = input.attr("type") === "password" ? "text" : "password";
                input.attr("type", type);
                $(this).toggleClass("fa-eye fa-eye-slash");
            });
        });
    </script>

</body>
</html>


